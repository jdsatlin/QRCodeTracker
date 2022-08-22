using System.ComponentModel.DataAnnotations;

namespace QRCodeTracker.Models
{
	public class Checkin
	{
		private const string PacificTimeZone = "America/Los_Angeles"; // IANA Time zones account for DST appropriately.
		public string Location { get; init; } = string.Empty;
		public DateTime Time { get; init; } = DateTime.UtcNow;

		[DisplayFormat(DataFormatString = "{0:f}")]
		public DateTime PacificTime => TimeZoneInfo.ConvertTimeFromUtc(Time, TimeZoneInfo.FindSystemTimeZoneById(PacificTimeZone));
	}
}
