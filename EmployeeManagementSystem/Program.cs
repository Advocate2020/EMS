using EmployeeManagementSystem.BusinessLogic;
using EmployeeManagementSystem.Network;
using EmployeeManagementSystem.Services;
using Firebase.Auth;
using Firebase.Auth.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Read Firebase config from appsettings.json
var firebaseConfig = builder.Configuration.GetSection("Firebase");
var apiKey = firebaseConfig["ApiKey"];
var authDomain = firebaseConfig["AuthDomain"];

// Register FirebaseAuthClient
builder.Services.AddSingleton(sp =>
{
    var config = new FirebaseAuthConfig
    {
        ApiKey = apiKey,
        AuthDomain = authDomain,
        Providers = new FirebaseAuthProvider[]
        {
            new EmailProvider(),
        },
    };
    return new FirebaseAuthClient(config);
});

// Register your service
builder.Services.AddHttpClient<AuthNetworkService>();
builder.Services.AddScoped<FirebaseAuthService>();
builder.Services.AddScoped<AuthBL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();