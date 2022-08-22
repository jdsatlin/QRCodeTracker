Simple MVC App for recording QR code check ins to a Google Sheets spreadsheet.

The app expects to be deployed where either the base url or /QrCode/Checkin endpoints are accessible. Either endpoint expects a query parameter of location=somevalue. 
For example, if you hosted this site at https://www.myqrcodecheckinsite.com you could access the check in functionality via either 
the https://www.myqrcodecheckinsite.com?location=baseurllocation 
or the https://www.myqrcodecheckinsite.com/QrCode/Checkin?location=longerurllocation addresses.




## Setting up Authentication with Google Cloud Platform:
This app requires a Service Account with access to the spreadsheet you intend to add data to. See [Google documentation on how to create a service account here](https://developers.google.com/workspace/guides/create-credentials)

You must
1. Create a project
2. Activate the Google Sheets API on [the API Library page](https://console.cloud.google.com/apis/library?project=_). [Further documentation here](https://cloud.google.com/apis/docs/getting-started).
3. Create a service account for the project
4. Create keys for your service account
5. Share access to the spreadsheet with the service account, using the "Share" button in the Google Sheets UI, and share it with the service account email address as an editor.

## Configuring the application:
 
The configuration required for this app requires information on about the service account set up above, as well information about Google Sheets spreadsheet.

The configuration values can be set either in a appsettings.Production.json file added into the container running the application, or via environment variables.

Example appsettings.Production.json:
```
{
	"GoogleSheetsUploaderOptions" : {
		"SpreadsheetId": "someSpreadSheetIdHere",
		"SheetId": 1234
	},
	"GoogleSheetsCredentialLoaderOptions": {
		"PrivateKey": "privateKeyFromGoogleServiceAccountHere",
		"ServiceEmail": "emailGeneratedByGoogleServiceAccount@domain.tld"
	}
}
```

The following values are required:
`GoogleSheetsUploaderOptions.SpreadsheetId`
`GoogleSheetsCredentialLoaderOptions.ServiceEmail`

As well as either of:
`GoogleSheetsCredentialLoaderOptions.PrivateKey`
OR BOTH
`GoogleSheetsCredentialLoaderOptions.CertificateLocation` AND `GoogleSheetsCredentialLoaderOptions.CertificatePassword`

You can find your SpreadsheetId and SheetId for a given spreadsheet using the following patterns:
SpreasheetId: https://docs.google.com/spreadsheets/d/SpreadsheetId/edit#gid=0
SheetId: https://docs.google.com/spreadsheets/d/aBC-123_xYz/edit#gid=SheetId

The key for the PrivateKey value can be found in the secrets.json provided when you create a key via Google's dashboard, and will be the full value including the `-----BEGIN PRIVATE KEY-----` and `-----END PRIVATE KEY-----` portions. 
The certificate for Certificate location is the .p12 certificate that can be generated via Google's dashboard, as well as the key displayed on their site (or your own certificate if you chose to upload one to Google). The value for the location parameter is a complete filepath to the certificate file.   

All remaining values are optional and will default as follows:
`GoogleSheetsUploaderOptions.SheetId` will default to SheetId 0, which should be the first sheet (tab) of the spreadsheet.

CellRange is the cell range to append into, it will look in the range for the first empty cell. Uses A1 notation from https://developers.google.com/sheets/api/guides/concepts

You can also fill these options out using environment variables with the keys of:
`GoogleSheetsUploaderOptions.SpreadsheetId=someSpreadSheetIdHere`
`GoogleSheetsUploaderOptions.SheetId=1234`
`GoogleSheetsCredentialLoaderOptions.ServiceEmail=emailGeneratedByGoogleServiceAccount@domain.tld`
`GoogleSheetsCredentialLoaderOptions.PrivateKey=privateKeyFromGoogleServiceAccountHere`
`GoogleSheetsCredentialLoaderOptions.CertificateLocation=/path/to/cert`
`GoogleSheetsCredentialLoaderOptions.CertificatePassword=passwordForCert`

