using QRCodeTracker.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GoogleSheetsUploaderOptions>(builder.Configuration.GetSection(nameof(GoogleSheetsUploaderOptions)));
builder.Services.Configure<GoogleSheetsCredentialLoaderOptions>(builder.Configuration.GetSection(nameof(GoogleSheetsCredentialLoaderOptions)));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IGoogleSheetsCredentialLoader, GoogleSheetsCredentialLoader>();
builder.Services.AddSingleton<ISheetsServiceFactory, SheetsServiceFactory>();
builder.Services.AddSingleton<IGoogleSheetsUploader, GoogleSheetsUploader>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=QrCode}/{action=Checkin}");

app.Run();
