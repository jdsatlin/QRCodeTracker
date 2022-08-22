using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using QRCodeTracker.Service;

namespace QRCodeTrackerTests.Service;

public class GoogleSheetsCredentialLoaderTests
{
	private const string PrivateKeyPem =
		"-----BEGIN PRIVATE KEY-----\r\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCv4r1DIACB/4Ca\r\nafhvrxX3z8NoUlDniqmBXHlXRoU/b0Cf7y2Nngi8QVzdMGx4O6DBpsp2yOh/gKTd\r\n2YyVCMKqwJMlC5t+7Zn892cimF1MkOUUzNB3/ha8t6iuuq/PkpKOfZQCVJaAQmoD\r\nEXd9usokZLtVsI8xvyy1IgLdNMC5LaxGvpcep5X21ZFFIbd+pPxPGAt00CiC49mC\r\nGfSjuw44OWcNV586KkvSyge+EkeCsEZnlscS7pyva0y/oJ+07stN7x5Buz75gf+c\r\na7vPvI4TpMMSJAgECRmw85lNM+oi+4d/AWe808+GXP1sJdLW+773OH/F+tj5K/rM\r\nUT2AVFOfAgMBAAECggEAPKRspOwvwKKbcKIA6mDvrhWQRjO7eVmxv8xmedTytEBj\r\ng8Evb5iBYocWwZykiX/lmV8sh3AV2YA9V9BeTDRNChqDoU4zOrwpT7LteFwYLwH2\r\nOgm0BuswY7jfC/+KBoN+zGo30eXgTjCX5J7tGAiDEbE5df3+ISTNAVbMrgA2QqIO\r\nBfMUy3syGmJuvX31ywDP83aF42Ji4mKWN0aUXeovdMXfXUKrGuCLaJA5Rk7mveie\r\nkUzoA9cfIjO+UTQR+a25B52rtQr9wrvrk/PTQm9GeyCptB+qGMsbExL6B1wOvbDq\r\nopzZEZx++2ow6aaH4lGswobxqaYaC5rMi5l7D4dewQKBgQDj20+OtHGehdxIn8Hi\r\nrXTieOGuE9jz6EfVU40ARznKovItRiDdVSpTJPxl9W0FaSJTORzQ7CKJM5DTDhFN\r\noOewqPEctJuTadb10jJp2nhI0ITRpMezVdKH/GnEqUTelXwllsAHZxaIrbevN144\r\nyLW2mCiWHWmHDqG2KiiE+mWJSQKBgQDFnCNGm20OFRAzqiuK8DN3Xpc9IuCQ8+oA\r\nWqLBpSprbeHNkbdGQAwXNeWJ9piDE4jS2SYncuU9NNpslgZhMc4QTAC7mGCUKF6y\r\nazMHqBtty1wSAHJUfnrdUNyF5Z7x4hB8eAJXyY3G6k5teirgJxcMcpjzk4r6nWZU\r\nVN6NAbadpwKBgHhz1lHVN/8aOoI12hQPqCuUxZD1swn1cAvo2DAedwGVNDsUyIjY\r\n3cAIXFk17cUfd+LQ3VfgjL1FAjlgRWtpNTHhjVykaV37ZISc8sOcl1u+x4ff+SXn\r\np5z1paAt71dPZMqmL6t6JqJoWyMIPQn9qHn3Y/U/ZSrdZEVdcgAiMozpAoGAYPO8\r\nMvYqyU/4RQDmoCI9fgGAvlQQTc3+LvJJDC0W5OJieNNXUjY2OGUPG2NLbHP3G0vC\r\n9nPemOsHh2ML+j/PVBuV+HRIXih4XZ1OyiDmZDX0FDj33xC3A0KbD26bTx2U7RyM\r\nObn/v2pR1FCuHI/RNSA8frUS6Oa0wAdrsU73490CgYEA2AdvhAhBFAHjOChx3dyj\r\naNpUARIW/xR+qEKaPfrMCYipkzERt30ETWuw8QCW9r6+1hGV3syZ2PLY11BY7IpV\r\nvZfF8JpnXHMe9wyJeIeT/EKt0wp4ywouqd4A+ZFvrqKhB+rll1hJrjd/T7v3p6w0\r\noY/uuJjYK8EwJziRXpYNCmk=\r\n-----END PRIVATE KEY-----";

	private const string ServiceEmail = "ServiceEmail@Unit.Test";

	[Test]
	public void LoadServiceCredentials_Loads_Successfully_When_Using_Private_Key()
	{
		// Arrange
		IOptions<GoogleSheetsCredentialLoaderOptions>? options = Options.Create(new GoogleSheetsCredentialLoaderOptions
		{
			PrivateKey = PrivateKeyPem,
			ServiceEmail = ServiceEmail
		});

		var logger = Mock.Of<ILogger<GoogleSheetsCredentialLoader>>();

		var service = new GoogleSheetsCredentialLoader(options, logger);

		// Act
		var credentials = service.LoadServiceCredentials();

		// Assert
		Assert.That(credentials.Id, Is.EqualTo(ServiceEmail));
		Assert.That(credentials.Scopes, Is.EqualTo(new [] {SheetsService.Scope.Spreadsheets}));
	}

	[Test]
	public void LoadServiceCredentials_Loads_Via_Certificate_File_When_No_Key_Is_Included_In_Options()
	{
		// Arrange
		IOptions<GoogleSheetsCredentialLoaderOptions>? options = Options.Create(new GoogleSheetsCredentialLoaderOptions
		{
			CertificateLocation = Path.Combine(Directory.GetCurrentDirectory(), $"Service{Path.DirectorySeparatorChar}qrcodecheckinunittest.p12"),
			CertificatePassword = "unittest",
			ServiceEmail = ServiceEmail
		});

		var logger = Mock.Of<ILogger<GoogleSheetsCredentialLoader>>();

		var service = new GoogleSheetsCredentialLoader(options, logger);

		// Act
		var credentials = service.LoadServiceCredentials();

		// Assert
		Assert.That(credentials.Id, Is.EqualTo(ServiceEmail));
		Assert.That(credentials.Scopes, Is.EqualTo(new[] { SheetsService.Scope.Spreadsheets }));
	}
}