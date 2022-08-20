
using Microsoft.AspNetCore.Mvc;
using QRCodeTracker.Models;
using QRCodeTracker.Service;
using System.Diagnostics;

namespace QRCodeTracker.Controllers
{
    public class QrCodeController : Controller
    {
        private ILogger<QrCodeController> Logger { get; init; }

        private GoogleSheetsUploader GoogleSheetsUploader { get; init; }

		public QrCodeController(ILogger<QrCodeController> logger, GoogleSheetsUploader googleSheetsUploader)
		{
			Logger = logger;
            GoogleSheetsUploader = googleSheetsUploader;
		}

        public IActionResult Checkin(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                Logger.Log(LogLevel.Error, $"Call to Checkin provided with invalid location");
                return BadRequest("Invalid location");
            }

			var currentTime = DateTime.UtcNow;

			var checkinInfo = new Checkin
            {
                Location = location,
                Time = currentTime
			};
            
            Logger.Log(LogLevel.Information, "Check in at {location}, time is {currentTime}", location, currentTime);

            GoogleSheetsUploader.AddDataToSheet(checkinInfo);

			return View(checkinInfo);
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
    }
}
