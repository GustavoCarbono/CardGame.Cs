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
    public bool ativo { get; set; }
    public string tipoDeHabilidade { get; set; } = string.Empty;
    public int? inimigoMaximo { get; set; }
    public int? areaDeAtaque { get; set; }
    public int? duracao { get; set; }
    public int? coodown { get; set; }
    public string? gatilho { get; set; }

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
    public bool ocupante { get; set; }
    public int? idOcupante { get; set; }
    public bool obstaculo { get; set; }
    public int? idObstaculo { get; set; }
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
    public string jogadorTurno => turnoAtual % 2 != 0 ? jogador1 : jogador2;
    public StatusPlayer statusPlayer1 { get; set; } = new();
    public StatusPlayer statusPlayer2 { get; set; } = new();
    public List<LogPlayer> logPlayer1 { get; set; } = new();
    public List<LogPlayer> logPlayer2 { get; set; } = new();
    public List<Unidades> unidades { get; set; } = new();
    public List<Log> log { get; set; } = new();
    public Tabuleiro tabuleiro { get; set; } = new();

    public Unidades? getUnidadeById(int id)
    {
        return unidades.FirstOrDefault(u => u.id == id);
    }
}

public class StatusPlayer
{
    public int movTotal { get; set; }
    public int movRestante { get; set; }
    public int totalUsoHabilidade { get; set; }
    public int habilidadeRestante { get; set; }

    public StatusPlayer()
    {
        movTotal = 3;
        movRestante = 3;
        totalUsoHabilidade = 2;
        habilidadeRestante = 2;
    }
}

public class Unidades
{
    public int id { get; set; } = Guid.NewGuid().GetHashCode();
    public string dono { get; set; } = string.Empty;
    public string? cartaId { get; set; } = string.Empty;
    public string? obstaculoId { get; set; } = string.Empty;
    public Posicao posicao { get; set; } = new();
    public int dano { get; set; }
    public int? combate { get; set; }
    public int? hpMaximo { get; set; }
    public int? hpAtual { get; set; }
    public int? passos { get; set; }
    public int? duracao { get; set; }
    public bool? hitbox { get; set; }
    public string? efeito { get; set; }
    public bool? jaMoveu { get; set; }
    public bool? jaAtacou { get; set; }

    public string[] habilidade { get; set; } = Array.Empty<string>();
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
    public int? id { get; set; }
    public string? cartaPlayer { get; set; } = string.Empty;
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


public class ContextoHabilidade
{
    public Partida partida { get; set; } = new();

    //unidade
    public Unidades unidadeOriginal { get; set; } = new();
    public Unidades unidadeAlterada { get; set; } = new();

    //alvo
    public Unidades? alvoOriginal { get; set; } = new();
    public Unidades? alvoAlterado { get; set; } = new();

    public Unidades cloneUnidade(Unidades original)
    {
        return new Unidades
        {
            id = original.id,
            dono = original.dono,
            cartaId = original.cartaId,
            posicao = new Posicao { x = original.posicao.x, y = original.posicao.y },
            habilidade = original.habilidade != null ? (string[])original.habilidade.Clone() : Array.Empty<string>(),
            dano = original.dano,
            combate = original.combate,
            hpMaximo = original.hpMaximo,
            hpAtual = original.hpAtual,
            passos = original.passos,
            jaMoveu = original.jaMoveu,
            jaAtacou = original.jaAtacou
        };
    }
}

//Turno do jogador
public class Turno
{
    public string tipo { get; set; } = string.Empty;
    public string jogador { get; set; } = string.Empty;
    public List<Acao> acoes { get; set; } = new();
}

public class Acao
{
    public string tipo { get; set; } = string.Empty; // "mover" ou "habilidade"
    public string cartaId { get; set; } = string.Empty;

    // Campos para movimento
    public Posicao? de { get; set; }
    public Posicao? para { get; set; }

    // Campos para habilidade
    public string? habilidadeId { get; set; }
    public List<Alvo>? alvo { get; set; }
}

public class CriarPartidaJson
{
    public string jogador1 { get; set; } = string.Empty;
    public string jogador2 { get; set; } = string.Empty;
    public List<Unidades> unidades { get; set; } = new();
}