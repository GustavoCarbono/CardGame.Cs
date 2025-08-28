using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

//GameHub
public class GameHub : Hub
{
    private Partida partida = new Partida();

    public string ExecutarPartida(string json)
    {
        try
        {
            var doc = JsonSerializer.Deserialize<Turno>(json);
            if (doc == null || string.IsNullOrEmpty(doc.tipo))
                return "JSON inválido";
            switch (doc.tipo)
            {
                case "criarPartida":
                    var partidaObj = JsonSerializer.Deserialize<Partida>(json);
                    if (partidaObj != null)
                        CriarPartida(partidaObj);
                    break;
                case "turno":
                    var turnoObj = JsonSerializer.Deserialize<Turno>(json);
                    if (turnoObj != null)
                        ProcessarTurno(turnoObj);
                    break;
                default:
                    return "Tipo não reconhecido";
            }
            return "Sucesso";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return "Erro 500";
        }

    }
    public void CriarPartida(Partida? criar)
    {
        Tabuleiro tabuleiro = new Tabuleiro();

        // Inicializa partida
        if (criar == null) return;
        criar.tabuleiro = tabuleiro;
        partida.jogador1 = criar.jogador1;
        partida.jogador2 = criar.jogador2;
        partida.turnoAtual = 1;
        partida.statusPlayer1 = new StatusPlayer();
        partida.statusPlayer2 = new StatusPlayer();

        var qtdCartasP1 = 0;
        var qtdCartasP2 = 0;
        var novasUnidades = new List<Unidades>();
        // Adiciona unidades
        foreach (var u in criar.unidades)
        {
            if (u.cartaId != null)
            {
                var carta = GameData.getPersonagem(u.cartaId);
                if (carta != null)
                {
                    if ((u.dono == criar.jogador1 && qtdCartasP1 < 8) ||
                        (u.dono == criar.jogador2 && qtdCartasP2 < 8))
                    {
                        if (u.posicao.x >= 0 && u.posicao.x < 8 && u.posicao.y >= 0 && u.posicao.y < 5)
                        {
                            var posicao = partida.tabuleiro.Grid[u.posicao.x, u.posicao.y];
                            if (posicao.ocupante == false)
                            {
                                posicao.ocupante = true;

                                novasUnidades.Add(new Unidades
                                {
                                    id = Guid.NewGuid().GetHashCode(),
                                    dono = u.dono,
                                    cartaId = u.cartaId,
                                    posicao = u.posicao,
                                    habilidade = u.habilidade,
                                    dano = carta.dano,
                                    combate = carta.combate,
                                    hpMaximo = carta.hp,
                                    hpAtual = carta.hp,
                                    passos = carta.passos,
                                });
                                if (u.dono == criar.jogador1) qtdCartasP1++;
                                else if (u.dono == criar.jogador2) qtdCartasP2++;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Limite de cartas atingido");
                    }
                }
            }
            else
            {
                Console.WriteLine("CartaId é nulo");
            }
        }
        partida.unidades.AddRange(novasUnidades);

    }

    public void ProcessarTurno(Turno turno)
    {

        Movimentacao movimentacao = new();
        UsarHabilidade habilidade = new();

        if (turno.jogador == partida.jogadorTurno) return;

        foreach (var unidade in partida.unidades)
        {
            var pos = unidade.posicao;
            var celula = partida.tabuleiro.Grid[pos.x, pos.y];

            if (celula.obstaculo && celula.idObstaculo != null)
            {
                var obstaculo = partida.getUnidadeById(celula.idObstaculo.Value);
                if (obstaculo != null)
                {
                    unidade.hpAtual -= obstaculo.dano;//dano do obstaculo
                }
            }
        }

        foreach (var acao in turno.acoes)
            {
                switch (acao.tipo.ToLower())
                {
                    case "mover":
                        if (acao.para != null)
                            if (movimentacao.movimento(partida, partida.tabuleiro, acao.cartaId.GetHashCode(), turno.jogador, acao.para) != 200) return;
                        break;
                    case "habilidade":
                        if (acao.habilidadeId != null && acao.alvo != null)
                            if (habilidade.habilidade(partida, partida.tabuleiro, turno.jogador, acao.cartaId.GetHashCode(), acao.habilidadeId, acao.alvo) != 200) return;
                        break;
                    default:
                        Console.WriteLine("Ação não reconhecida");
                        break;
                }
            }

        var jogadorAtualStatus = partida.jogadorTurno == partida.jogador1 ? partida.statusPlayer1 : partida.statusPlayer2;
        jogadorAtualStatus.movRestante = jogadorAtualStatus.movTotal;
        jogadorAtualStatus.habilidadeRestante = jogadorAtualStatus.totalUsoHabilidade;
        foreach (var unidade in partida.unidades)
        {
            unidade.jaMoveu = false;
            unidade.jaAtacou = false;
        }
        partida.turnoAtual++;
    }
}