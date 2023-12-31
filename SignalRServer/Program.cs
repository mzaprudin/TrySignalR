using SignalRServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var origins = new string[] { "https://tradingview.com", "https://www.tradingview.com", "https://in.tradingview.com", "https://es.tradingview.com", "https://*.tradingview.com" };

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policyBuilder =>
        {
            policyBuilder.WithOrigins(origins: origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                // .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

foreach (var origin in origins)
    webSocketOptions.AllowedOrigins.Add(origin);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors();
app.UseWebSockets(webSocketOptions);

app.MapHub<LearningHub>("/hub");
app.UseBlazorFrameworkFiles();

app.Run();
