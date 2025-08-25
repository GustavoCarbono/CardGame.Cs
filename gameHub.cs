using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;

//Habilidades e Personagens
public class Habilidade
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string TipoDeHabilidade { get; set; } = string.Empty;

    public Dictionary<string, object> Efeito { get; set; } = new();
}
public class Personagem
{
    public string nome { get; set; } = string.Empty;
    public string codigo { get; set; } = string.Empty;
    public int dano { get; set; }
    public int combate { get; set; }
    public int hp { get; set; }

    public string[]? Habilidade { get; set; }
    public Dictionary<string, object> Extras { get; set; } = new();
}

public class GameData
{
    public List<Personagem> Personagem { get; set; } = new();
    public List<Habilidade> Habilidade { get; set; } = new();
}

//Tabuleiro e Celula
public class Celula
{
    public int X { get; set; }
    public int Y { get; set; }
    public Personagem? ocupante { get; set; }
    public string? personagemPlayer { get; set; }
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

//log partida
public class TurnoLog
{
    public string jogador1 { get; set; } = string.Empty;
    public string jogador2 { get; set; } = string.Empty;
    public int turnoAtual { get; set; }
    public List<LogPlayer1> logPlayer1 { get; set; } = new();
    public List<LogPlayer2> logPlayer2 { get; set; } = new();
}

public class LogPlayer1
{
    public int logId { get; set; }
    public string tipo { get; set; } = string.Empty;
    public string cartaId { get; set; } = string.Empty;
    public string? habilidadeId { get; set; }
    public Posicao? de { get; set; } = new();
    public Posicao? para { get; set; } = new();
    public Alvo? alvo { get; set; } = new();
}

public class LogPlayer2
{
    public int logId { get; set; }
    public string tipo { get; set; } = string.Empty;
    public string cartaId { get; set; } = string.Empty;
    public string? habilidadeId { get; set; }
    public Posicao? de { get; set; } = new();
    public Posicao? para { get; set; } = new();
    public Alvo? alvo { get; set; } = new();
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


//GameHub
public class GameHub : Hub
{
    public static void main(String[] args)
    {
        Tabuleiro tabuleiro = new Tabuleiro();

        var celula = tabuleiro.Grid[2, 4];

    }
}