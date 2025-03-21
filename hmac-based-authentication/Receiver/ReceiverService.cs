using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace hmac_based_authentication.Receiver;

public class ReceiverService
{
    public static Task Main() =>
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseMiddleware<HmacAuthMiddleware>();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapPost("/api/verify", async context =>
                        {
                            await context.Response.WriteAsync("Request verified!");
                        });
                    });
                }).UseUrls("http://localhost:5000");
            }).Build().RunAsync();
}

