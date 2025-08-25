using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Lê o conteúdo do arquivo
string json = File.ReadAllText("GameData/gameData.json");

// Converte para objeto GameData
GameData data = JsonSerializer.Deserialize<GameData>(json);

// Acessando personagens
foreach (var p in data.Personagem)
{
    Console.WriteLine($"Personagem: {p.nome}, HP: {p.hp}, Dano: {p.dano}");

    // Acessando propriedades extras
    foreach (var extra in p.Extras)
    {
        Console.WriteLine($"  {extra.Key}: {extra.Value}");
    }
}
foreach (var h in data.Habilidade)
{
    Console.WriteLine($"Habilidade: {h.Nome}");
    foreach (var kv in h.Efeito)
    {
        Console.WriteLine($"  {kv.Key}: {kv.Value}");
    }
}

app.UseStaticFiles();
app.MapRazorPages();

// adiciona a rota do hub
app.MapHub<GameHub>("/gameHub");

app.Run();