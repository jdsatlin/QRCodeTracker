using Google.Apis.Sheets.v4;

namespace QRCodeTracker.Service;

public interface ISheetsServiceFactory
{
	SheetsService CreateSheetsService();
}

