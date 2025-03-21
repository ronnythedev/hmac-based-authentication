# HMAC Authentication Demo in C#
Welcome to my HMAC-based authentication demo! 

This project shows how to secure service-to-service communication using HMAC-SHA256 in C# 9. 

I built this to mimic two services, a sender and a receiver, talking securely with signatures, replay protection, and some real-world tweaks. 

It’s a companion to my [post](https://www.ronnydelgado.com/my-blog/security-hmac-authentication-demo), where I dive deeper into the why and how.

## What’s This About?
HMAC (Hash-based Message Authentication Code) is a lightweight way to ensure authenticity and integrity between services. 

Here, the sender signs a message with a shared secret, and the receiver verifies it. 

Think of it like a secret handshake for APIs! 

Companies like **Stripe** and **AWS** use similar patterns, and this project brings that concept to life in a simple console app.

## Features
* **HMAC-SHA256 Signing**: Securely signs requests with a shared key.
* **Replay Protection**: Uses timestamps and nonces to block replay attacks.
* **Middleware**: Verifies HMAC in an ASP.NET Core middleware.
* **Body Buffering**: Rewinds the request body for downstream use.
* **Key Versioning**: Preps for future key rotation (not implemented yet).
  
## Prerequisites
* .NET 9
* Visual Studio, Rider, VSCode, etc.
* Basic familiarity with C# and ASP.NET Core.

## Project Structure
* **SenderService.cs**: Crafts and sends a signed HTTP request.
* **ReceiverService.cs**: Hosts a minimal API.
* **HmacAuthMiddleware.cs**: Verifies the HMAC.
* **Program.cs**: Ties it together, launching both services.

## Try It Out!
Feel free to tweak the code, change the payload, adjust the tolerance, change the secret, or add your own features. 

If you’ve got questions or ideas, open an issue or ping me! 

I’d love to hear how you use it.

## License
MIT License—use it however you like, just give a nod if it helps!
