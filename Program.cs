var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();


app.UseStaticFiles();
app.MapRazorPages();

// adiciona a rota do hub
app.MapHub<GameHub>("/gameHub");

app.Run();