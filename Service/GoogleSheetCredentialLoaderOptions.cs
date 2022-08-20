namespace QRCodeTracker.Service
{
    public record GoogleSheetCredentialLoaderOptions
    {
        public string PrivateKey { get; init; } = string.Empty;

        public string ServiceEmail { get; init; } = string.Empty;

        public string CertificateLocation { get; init; } = string.Empty;

        public string CertificatePassword { get; init; } = string.Empty;
    }
}
