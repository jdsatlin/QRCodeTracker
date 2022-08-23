using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using QRCodeTracker.Models;
using QRCodeTracker.Service;

namespace QRCodeTrackerTests.Service;

public class GoogleSheetsUploaderTests
{
	[Test]
	public void ConvertDateToDateTimeCell_Creates_Compatible_DateTime_Representation()
	{
		// Arrange
		var testTime = new DateTime(2022, 01, 01, 23, 59, 59);

		// Act
		CellData dateTimeCell = GoogleSheetsUploader.ConvertDateToDateTimeCell(testTime);

		// Assert
		Assert.That(dateTimeCell, Is.Not.Null);

		Assert.That(dateTimeCell.UserEnteredValue, Is.Not.Null);
		Assert.That(dateTimeCell.UserEnteredValue.NumberValue, Is.EqualTo(44562.999988425923d));

		Assert.That(dateTimeCell.UserEnteredFormat, Is.Not.Null);
		Assert.That(dateTimeCell.UserEnteredFormat.NumberFormat, Is.Not.Null);
		Assert.That(dateTimeCell.UserEnteredFormat.NumberFormat.Type, Is.EqualTo("DATE_TIME"));
	}

	[Test]
	public void CreateStringCell_Creates_Compatible_String_Cell_Representation()
	{
		// Arrange
		const string input = "doot doot";
		// Act
		CellData stringCell = GoogleSheetsUploader.CreateStringCell(input);

		// Assert
		Assert.That(stringCell, Is.Not.Null);

		Assert.That(stringCell.UserEnteredValue, Is.Not.Null);
		Assert.That(stringCell.UserEnteredValue.StringValue, Is.EqualTo(input));
	}

	[Test]
	public void AddDataToSheet_Calls_Batch_Update_With_Append_Request()
	{
		// Arrange

		const string spreadSheetId = "Unit Test Spreadsheet ID";
		const int sheetId = 3131;

		IOptions<GoogleSheetsUploaderOptions> options = Options.Create(new GoogleSheetsUploaderOptions
		{ 
			SpreadsheetId = spreadSheetId,
			SheetId = sheetId
		});

		var sheetsService = new Mock<SheetsService>();
		var spreadSheet = new Mock<SpreadsheetsResource>(MockBehavior.Loose, sheetsService.Object);

		BatchUpdateSpreadsheetRequest? capturedRequest = null;
		string? capturedSpreadSheetId = string.Empty;
		spreadSheet
			.Setup(sheet => sheet.BatchUpdate(It.IsAny<BatchUpdateSpreadsheetRequest>(), It.IsAny<string>()))
			.Callback((BatchUpdateSpreadsheetRequest request, string providedSpreadSheetId) =>
			{
				capturedRequest = request;
				capturedSpreadSheetId = providedSpreadSheetId;
			});

		sheetsService.Setup(service => service.Spreadsheets).Returns(spreadSheet.Object);

		var serviceFactory = new Mock<ISheetsServiceFactory>();
		serviceFactory.Setup(factory => factory.CreateSheetsService()).Returns(sheetsService.Object);

		var input = new Checkin
		{
			Location = "Unit test location",
			Time = DateTime.UtcNow
		};

		var service = new GoogleSheetsUploader(options, serviceFactory.Object, Mock.Of<ILogger<GoogleSheetsUploader>>());

		// Act
		service.AddDataToSheet(input);

		// Assert
		Assert.That(capturedSpreadSheetId, Is.EqualTo(spreadSheetId));

		Assert.That(capturedRequest, Is.Not.Null);
		Assert.That(capturedRequest.IncludeSpreadsheetInResponse, Is.False);
		Assert.That(capturedRequest.Requests, Is.Not.Null);
		Assert.That(capturedRequest.Requests.Count, Is.EqualTo(1));

		Request? request = capturedRequest.Requests[0];
		Assert.That(request.AppendCells, Is.Not.Null);
		Assert.That(request.AppendCells.SheetId, Is.EqualTo(sheetId));
		Assert.That(request.AppendCells.Rows.Count, Is.EqualTo(1));

		RowData? row = request.AppendCells.Rows[0];
		Assert.That(row.Values.Count, Is.EqualTo(2));
	}
}

