using System.Security.Cryptography;
using System.Text;

namespace hmac_based_authentication.Sender;

public class SenderService
{
    private const string SecretKey = "XRpjBq&G&68KWd#9TCxGmxzJbn7vNdKHPJV4&&R"; // <== IMPORTANT: in real=life, this must be stored in appsettings.json, KeyVault, or similar
    private const string ApiKey = "4f7a49a7-6ce9-4a2f-82f0-a14e3954617e"; // <== IMPORTANT: same as above
    private const string ReceiverUrl = "http://localhost:5000/api/verify";

    public static async Task SendRequestAsync()
    {
        using var client = new HttpClient();
        
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        
        const string body = "{\"message\":\"Hello, Receiver!\"}";
        
        var nonce = Guid.NewGuid().ToString("N");

        var signature = ComputeSignature(body, timestamp, nonce);

        var request = new HttpRequestMessage(HttpMethod.Post, ReceiverUrl)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        
        request.Headers.Add("X-API-Key", ApiKey);
        request.Headers.Add("X-Timestamp", timestamp);
        request.Headers.Add("X-Nonce", nonce);
        request.Headers.Add("X-HMAC-Signature", signature);
        request.Headers.Add("X-Key-Version", "1"); // For future key rotation

        // The request is sent here, you might want to debug the middleware in the receiver area
        var response = await client.SendAsync(request);
        Console.WriteLine($"Sender got: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
    }

    private static string ComputeSignature(string data, string timestamp, string nonce)
    {
        var message = $"{timestamp}:{nonce}:{data}";
        
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
        
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        
        return Convert.ToBase64String(hash);
    }
}
