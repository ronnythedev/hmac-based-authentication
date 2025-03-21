namespace hmac_based_authentication.Receiver;

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

public class HmacAuthMiddleware
{
    private const string SecretKey = "XRpjBq&G&68KWd#9TCxGmxzJbn7vNdKHPJV4&&R";  // <== IMPORTANT: in real life, this MUST be stored in appsettings.json, KeyVault, or similar
    private const long TimeTolerance = 300; // 5 minutes
    private readonly RequestDelegate _next;
    
    public HmacAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-API-Key", out var apiKey) ||
            !context.Request.Headers.TryGetValue("X-Timestamp", out var timestamp) ||
            !context.Request.Headers.TryGetValue("X-Nonce", out var nonce) ||
            !context.Request.Headers.TryGetValue("X-HMAC-Signature", out var signature))
        {
            await WriteUnauthorized(context, "Missing headers");
            return;
        }

        if (!long.TryParse(timestamp, out long requestTime) ||
            Math.Abs(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - requestTime) > TimeTolerance)
        {
            await WriteUnauthorized(context, "Invalid timestamp");
            return;
        }

        // Enable buffering for body rewind
        context.Request.EnableBuffering();
        var body = await new StreamReader(context.Request.Body, Encoding.UTF8).ReadToEndAsync();
        context.Request.Body.Position = 0; // Reset for downstream

        var computedSignature = ComputeSignature(body, timestamp!, nonce!);
        
        if (computedSignature != signature)
        {
            await WriteUnauthorized(context, "Invalid signature");
            return;
        }

        await _next(context);
    }

    private static string ComputeSignature(string data, string timestamp, string nonce)
    {
        var message = $"{timestamp}:{nonce}:{data}";
        
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
        
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        
        return Convert.ToBase64String(hash);
    }

    private static async Task WriteUnauthorized(HttpContext context, string message)
    {
        context.Response.StatusCode = 401;
        
        await context.Response.WriteAsync(message);
    }
}
