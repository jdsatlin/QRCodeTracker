using QRCodeTracker.Models;

namespace QRCodeTracker.Service;

public interface IGoogleSheetsUploader
{
	void AddDataToSheet(Checkin checkin);
}

