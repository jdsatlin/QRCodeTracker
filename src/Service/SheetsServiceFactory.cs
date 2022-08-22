using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace QRCodeTracker.Service;

public class SheetsServiceFactory : ISheetsServiceFactory
{
	private const string ApplicationName = "QR Code Checkin";

	private ServiceAccountCredential Credentials { get; init; }

	public SheetsServiceFactory(IGoogleSheetsCredentialLoader credentialLoader)
	{
		Credentials = credentialLoader.LoadServiceCredentials();
	}
	public SheetsService CreateSheetsService()
	{
		return new SheetsService(new BaseClientService.Initializer
		{
			HttpClientInitializer = Credentials,
			ApplicationName = ApplicationName
		});
	}
}

