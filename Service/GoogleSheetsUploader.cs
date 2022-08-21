using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;

using QRCodeTracker.Models;

namespace QRCodeTracker.Service
{
	public class GoogleSheetsUploader
	{
		private const string DateTimeFormatSpecifier = "DATE_TIME";

		private ServiceAccountCredential Credentials { get; init; }

		private GoogleSheetsUploaderOptions Options { get; init; }

		private ILogger<GoogleSheetsUploader> Logger { get; init; }

		private const string ApplicationName = "QR Code Checkin";

		public GoogleSheetsUploader(GoogleSheetsCredentialLoader credentialLoader, IOptions<GoogleSheetsUploaderOptions> options, ILogger<GoogleSheetsUploader> logger)
		{
			Credentials = credentialLoader.LoadServiceCredentials();
			Options = options.Value;
			Logger = logger;
		}

		public void AddDataToSheet(Checkin checkin)
		{
			var service = new SheetsService(new BaseClientService.Initializer
			{
				HttpClientInitializer = Credentials,
				ApplicationName = ApplicationName
			});

			AppendCellsRequest addCellsRequest = new AppendCellsRequest
			{
				SheetId = Options.SheetId,
				Fields = "*",
				Rows = new List<RowData>
				{
					ConvertCheckinToRow(checkin)
				}
			};

			var batchContainer = new BatchUpdateSpreadsheetRequest
			{
				Requests = new List<Request>
				{
					new Request
					{
						AppendCells = addCellsRequest
					}
				},
				IncludeSpreadsheetInResponse = false
			};

			try
			{
				var update = service.Spreadsheets.BatchUpdate(batchContainer, Options.SpreadsheetId);

				var result = update.Execute();
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "Failed to update spreadsheet with checkin: {checkin}", checkin);
			}

		}

		private static RowData ConvertCheckinToRow(Checkin checkin)
		{
			return new RowData
			{
				Values = new List<CellData>
				{
					ConvertDateToDateTimeCell(checkin.PacificTime),
					CreateStringCell(checkin.Location)
				}
			};
		}

		/**
		 * <summary>
		 * For the underlying format Google Sheets uses what they call SERIAL_NUMBER format and .NET calls OLE Automation date.
		 * </summary>
		 * <see cref="https://www.ablebits.com/office-addins-blog/google-sheets-change-date-format/"/>
		 */
		private static CellData ConvertDateToDateTimeCell(DateTime datetime)
		{
			return new CellData
			{
				UserEnteredValue = new ExtendedValue
				{
					NumberValue = datetime.ToOADate()
				},
				UserEnteredFormat = new CellFormat
				{
					NumberFormat = new NumberFormat
					{
						Type = DateTimeFormatSpecifier
					}
				}
			};
		}

		private static CellData CreateStringCell(string value)
		{
			return new CellData
			{
				UserEnteredValue = new ExtendedValue
				{
					StringValue = value
				}
			};
		}
	}
}
