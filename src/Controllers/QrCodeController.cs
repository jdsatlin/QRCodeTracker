using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using QRCodeTracker.Models;
using QRCodeTracker.Service;

namespace QRCodeTracker.Controllers
{
    public class QrCodeController : Controller
    {
        private ILogger<QrCodeController> Logger { get; init; }

        private IGoogleSheetsUploader GoogleSheetsUploader { get; init; }

		public QrCodeController(ILogger<QrCodeController> logger, IGoogleSheetsUploader googleSheetsUploader)
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

			DateTime currentTime = DateTime.UtcNow;

			var checkinInfo = new Checkin
            {
                Location = location,
                Time = currentTime
			};
            
            Logger.Log(LogLevel.Information, "Check in at {location}, time is {currentTime}", location, currentTime);

            try
            {
	            GoogleSheetsUploader.AddDataToSheet(checkinInfo);
	            
	            return View(checkinInfo);
            }
            catch (Exception exception)
            {
                Logger.LogError("Something went wrong attempting to save to the spreadsheet", exception);
                return Problem(title: "Something went wrong", detail: "An error was encountered processing this request.", statusCode: 500);
            }
        }
    }
}
