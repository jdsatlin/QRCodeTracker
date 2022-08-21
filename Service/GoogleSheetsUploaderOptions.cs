namespace QRCodeTracker.Service
{
    public record GoogleSheetsUploaderOptions
    {
		// Spreadsheet ID, e.g. https://docs.google.com/spreadsheets/d/SpreadsheetId/edit#gid=0
		public string SpreadsheetId { get; init; } = string.Empty;

		// Specific Sheet (tab) ID, e.g. https://docs.google.com/spreadsheets/d/aBC-123_xYz/edit#gid=SheetId
		public int SheetId { get; init; } = 0;
    }
}
