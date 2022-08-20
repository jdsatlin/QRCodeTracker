using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace QRCodeTracker.Service
{
    public class GoogleSheetCredentialLoader
    {
		private GoogleSheetCredentialLoaderOptions Options { get; init; }

		private ILogger<GoogleSheetCredentialLoader> Logger { get; init; }

		public GoogleSheetCredentialLoader(IOptions<GoogleSheetCredentialLoaderOptions> options, ILogger<GoogleSheetCredentialLoader> logger)
		{
			Options = options.Value;
			Logger = logger;
		}

		public ServiceAccountCredential LoadServiceCredentials()
		{
			if (!string.IsNullOrEmpty(Options.PrivateKey))
			{
				Logger.LogInformation("Attempting to retrieve Service Account Credentials via Private Key");
				return new ServiceAccountCredential(
					new ServiceAccountCredential.Initializer(Options.ServiceEmail)
					{
						Scopes = new[] { SheetsService.Scope.Spreadsheets }
					}.FromPrivateKey(Options.PrivateKey));
			}
			else
			{
				Logger.LogInformation("Attempting to retrieve Service Account Credentials via Certificate file");
				var certificate = new X509Certificate2(Options.CertificateLocation, Options.CertificatePassword, X509KeyStorageFlags.Exportable);

				return new ServiceAccountCredential(
				new ServiceAccountCredential.Initializer(Options.ServiceEmail)
				{
					Scopes = new[] { SheetsService.Scope.Spreadsheets }
				}.FromCertificate(certificate));

			}
			
		}
	}
}
