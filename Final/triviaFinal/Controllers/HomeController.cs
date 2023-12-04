using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using triviaFinal.Models;
using System.Threading.Tasks;
using triviaFinal.Services;
using System.Text.Json;


namespace triviaFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly QueueService _queueService;

        public HomeController(QueueService queueService)
        {
            _queueService = queueService;
        }

        public IActionResult Index()
        {
            return View();
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
                // Process the message as needed
                var content = JsonSerializer.Deserialize<string>(queueMessage.MessageText);

                // Delete the message after processing
                await _queueService.DeleteMessageAsync(queueMessage.MessageId, queueMessage.PopReceipt);
            }

            return RedirectToAction("Index");
        }
    }
}