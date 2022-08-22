using Google.Apis.Auth.OAuth2;

namespace QRCodeTracker.Service
{
	public interface IGoogleSheetsCredentialLoader
	{
		ServiceAccountCredential LoadServiceCredentials();
	}
}
