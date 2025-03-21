using hmac_based_authentication.Receiver;
using hmac_based_authentication.Sender;

// Let's start the service that will receive the request first
var receiverService = ReceiverService.Main();

// Wait for the receiver to start
await Task.Delay(1000);

// Now, let's send a request!
await SenderService.SendRequestAsync();

await receiverService;
