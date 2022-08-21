namespace QRCodeTracker.Service
{
    public record GoogleSheetsCredentialLoaderOptions
    {
        public string PrivateKey { get; init; } = string.Empty;

        public string ServiceEmail { get; init; } = string.Empty;

        public string CertificateLocation { get; init; } = string.Empty;

        public string CertificatePassword { get; init; } = string.Empty;
    }
}
