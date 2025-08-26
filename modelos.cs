using System.Text.Json;

//Habilidades e Personagens

public static class GameData
{
    public static List<Personagem> Personagem { get; private set; } = new();
    public static List<Habilidade> Habilidade { get; private set; } = new();
    public static List<Obstaculo> Obstaculo { get; private set; } = new();

    static GameData()
    {
        string caminho = "GameData/gameData.json";

        string json = File.ReadAllText(caminho);
        var data = JsonSerializer.Deserialize<GameDataArquivo>(json);

        if (data != null)
        {
            if (data.Personagem != null) Personagem = data.Personagem;
            if (data.Habilidade != null) Habilidade = data.Habilidade;
            if (data.Obstaculo != null) Obstaculo = data.Obstaculo;
        }
    }

    public static Personagem? getPersonagem(string codigo)
    {
        return Personagem.FirstOrDefault(p => p.codigo == codigo);
    }

    public static Habilidade? getHabilidade(string codigo)
    {
        return Habilidade.FirstOrDefault(h => h.codigo == codigo);
    }
    
    public static Obstaculo? getObstaculo(string codigo)
    {
        return Obstaculo.FirstOrDefault(o => o.codigo == codigo);
    }
}

public class GameDataArquivo
{
public List<Personagem> Personagem { get; set; } = new();
public List<Habilidade> Habilidade { get; set; } = new();
public List<Obstaculo> Obstaculo { get; set; } = new();
}

public class Habilidade
{
    public string codigo { get; set; } = string.Empty;
    public string nome { get; set; } = string.Empty;
    public string tescricao { get; set; } = string.Empty;
    public string tipo { get; set; } = string.Empty;
    public string tipoDeHabilidade { get; set; } = string.Empty;
    public int? inimigoMaximo { get; set; }
    public int areaDeAtaque { get; set; }
    public int duracao { get; set; }
    public int coodown { get; set; }

    public Dictionary<string, object> efeito { get; set; } = new();
}
public class Personagem
{
    public string nome { get; set; } = string.Empty;
    public string codigo { get; set; } = string.Empty;
    public int dano { get; set; }
    public int combate { get; set; }
    public int hp { get; set; }
    public int gastoMov { get; set; }
    public int passos { get; set; }

    public string[]? habilidade { get; set; }
    public Dictionary<string, object> extras { get; set; } = new();
}

public class Obstaculo
{
    public string nome { get; set; } = string.Empty;
    public string codigo { get; set; } = string.Empty;
    public int duracao { get; set; }
    public int dano { get; set; }
    public bool hitbox { get; set; }
    public string tipo { get; set; } = string.Empty;
}

//Tabuleiro e Celula
public class Celula
{
    public int X { get; set; }
    public int Y { get; set; }
    public Personagem? ocupante { get; set; }
    public string? personagemPlayer { get; set; }
    public Obstaculo? obstaculo { get; set; }
    public string? obstaculoPlayer { get; set; }
}

public class Tabuleiro
{
    public Celula[,] Grid { get; set; }
    public Tabuleiro()
    {
        Grid = new Celula[5, 8];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Grid[i, j] = new Celula { X = i, Y = j };
            }
        }
    }
}

//log da partida
public class Partida
{
    public string jogador1 { get; set; } = string.Empty;
    public string jogador2 { get; set; } = string.Empty;
    public int turnoAtual { get; set; }
    public StatusPlayer statusPlayer1 { get; set; } = new();
    public StatusPlayer statusPlayer2 { get; set; } = new();
    public List<LogPlayer> logPlayer1 { get; set; } = new();
    public List<LogPlayer> logPlayer2 { get; set; } = new();
    public List<Unidades> unidades { get; set; } = new();
    public List<Log> log { get; set; } = new();
}

public class StatusPlayer
{
    public int movTotal { get; set; }
    public int movRestante { get; set; }
    public int totalUsoHabilidade { get; set; }
    public int habilidadeRestante { get; set; }
}

public class Unidades
{
    public string dono { get; set; } = string.Empty;
    public string cartaId { get; set; } = string.Empty;
    public Posicao posicao { get; set; } = new();
    public int dano { get; set; }
    public int combate { get; set; }
    public int hpMaximo { get; set; }
    public int hpAtual { get; set; }
    public int passos { get; set; }
    public bool jaAtacou { get; set; }

    public string[]? habilidade { get; set; }
}

public class LogPlayer
{
    public int logId { get; set; }
    public string tipo { get; set; } = string.Empty;
    public string cartaId { get; set; } = string.Empty;
    public string? habilidadeId { get; set; }
    public Posicao? de { get; set; }
    public Posicao? para { get; set; }
    public Alvo? alvo { get; set; }
}

public class Alvo
{
    public string cartaId { get; set; } = string.Empty;
    public string cartaPlayer { get; set; } = string.Empty;
    public Posicao posicao { get; set; } = new();
}

public class Posicao
{
    public int x { get; set; }
    public int y { get; set; }
}

public class Log
{
    public int turno { get; set; }
    public List<int> player1 { get; set; } = new();
    public List<int> player2 { get; set; } = new();
}