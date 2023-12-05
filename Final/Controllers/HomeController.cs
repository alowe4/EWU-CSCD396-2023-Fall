using Final.Models;
using Final.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QueueService _queueService;

        public HomeController(ILogger<HomeController> logger, QueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        public async Task<IActionResult> SendMessage()
        {
            var message = new { Content = "Hello, Azure Queue!" };
            await _queueService.SendMessageAsync(message);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReceiveMessage()
        {
            var queueMessage = await _queueService.ReceiveMessageAsync();

            if (queueMessage != null)
            {
                var content = queueMessage.MessageText;
                _logger.LogInformation($"Received message: {content}");

                // Add the received message to the cookie
                var receivedMessages = Request.Cookies.ContainsKey("ReceivedMessages")
                    ? Request.Cookies["ReceivedMessages"]
                    : string.Empty;

                receivedMessages += $"{content}|";

                Response.Cookies.Append("ReceivedMessages", receivedMessages);

                await _queueService.DeleteMessageAsync(queueMessage.MessageId, queueMessage.PopReceipt);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            // Retrieve received messages from the cookie
            var receivedMessages = Request.Cookies.ContainsKey("ReceivedMessages")
                ? new List<string>(Request.Cookies["ReceivedMessages"].Split('|', System.StringSplitOptions.RemoveEmptyEntries))
                : new List<string>();

            return View(receivedMessages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
