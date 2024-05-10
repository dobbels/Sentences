using Sentences.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(); 
builder.Services.AddSingleton<SentenceStemService>();
builder.Services.AddSingleton<NotesService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var sentenceStemService = services.GetRequiredService<SentenceStemService>();
    await sentenceStemService.InitializeAsync();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var sentenceStemService = services.GetRequiredService<NotesService>();
    await sentenceStemService.InitializeAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
