using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;

using QRCodeTracker.Models;

namespace QRCodeTracker.Service
{
	public class GoogleSheetsUploader : IGoogleSheetsUploader
	{
		private const string DateTimeFormatSpecifier = "DATE_TIME";

		private ISheetsServiceFactory SheetsServiceFactory { get; init; }

		private GoogleSheetsUploaderOptions Options { get; init; }

		private ILogger<GoogleSheetsUploader> Logger { get; init; }

		public GoogleSheetsUploader(IOptions<GoogleSheetsUploaderOptions> options, ISheetsServiceFactory sheetsServiceFactory, ILogger<GoogleSheetsUploader> logger)
		{
			SheetsServiceFactory = sheetsServiceFactory;
			Options = options.Value;
			Logger = logger;
		}

		public void AddDataToSheet(Checkin checkin)
		{
			var addCellsRequest = new AppendCellsRequest
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
				SheetsService service = SheetsServiceFactory.CreateSheetsService();

				SpreadsheetsResource.BatchUpdateRequest? update = service.Spreadsheets.BatchUpdate(batchContainer, Options.SpreadsheetId);

				update.Execute();
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
		internal static CellData ConvertDateToDateTimeCell(DateTime datetime)
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

		internal static CellData CreateStringCell(string value)
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
