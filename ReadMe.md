Simple MVC App for recording QR code check ins to a Google Sheets spreadsheet.

The app expects to be deployed where either the base url or /QrCode/Checkin endpoints are accessible. Either endpoint expects a query parameter of location=somevalue. 
For example, if you hosted this site at https://www.myqrcodecheckinsite.com you could access the check in functionality via either 
the https://www.myqrcodecheckinsite.com?location=baseurllocation 
or the https://www.myqrcodecheckinsite.com/QrCode/Checkin?location=longerurllocation addresses.

The configuration required for this app is a secrets.json file deployed with the app (see:https://developers.google.com/workspace/guides/create-project and https://developers.google.com/workspace/guides/create-credentials#desktop-app) as well as configuration values for where to upload within Google Sheets set either in a package up appsettings-prod.json or via environment variables.

Example appsettings-prod.json:
```
{
	"GoogleSheetsUploaderOptions" : {
		"SpreadsheetId": "someSpreadSheetIdHere",
		"SheetId": 1234,
		"CellRange": "1:2"
	}
}
```
You can find your SpreadsheetId and SheetId for a given spreadsheet using the following patterns:
SpreasheetId: https://docs.google.com/spreadsheets/d/SpreadsheetId/edit#gid=0
SheetId: https://docs.google.com/spreadsheets/d/aBC-123_xYz/edit#gid=SheetId

CellRange is the cell range to append into, it will look in the range for the first empty cell. Uses A1 notation from https://developers.google.com/sheets/api/guides/concepts

You can also fill these options out using environment variables with the keys of:
`GoogleSheetsUploaderOptions.SpreadsheetId=someSpreadSheetIdHere`
`GoogleSheetsUploaderOptions.SheetId=1234`
`GoogleSheetsUploaderOptions.CellRange=1:2`