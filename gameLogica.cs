public class Movimentacao
{

    public int movimento(Partida partida, Tabuleiro tabuleiro, string id, string dono, Posicao novaPosicao)
    {

        ValidarMovimento validar = new();

        var unidade = partida.unidades.FirstOrDefault(u => u.id == id);
        if (unidade != null)
        {
            //verificar se ele já moveu, colocar que moveu e logar no partida.log
            if (validar.validarMovimentoUnidade(unidade, tabuleiro, novaPosicao, partida, dono, unidade.passos) == 200)
            {
                unidade.posicao = novaPosicao;
                var movimento = dono == "jogador1" ? partida.statusPlayer1 : partida.statusPlayer2;
                movimento.movRestante--;
                tabuleiro.Grid[novaPosicao.x, novaPosicao.y].ocupante = true;
                tabuleiro.Grid[novaPosicao.x, novaPosicao.y].idOcupante = id;
                return 200;
            }
            else
            {
                return 401;
            }
        }
        else
        {
            return 404;
        }
    }
}

public class UsarHabilidade
{
    public int habilidade(Partida partida, string dono, string id, string habilidadeId, List<Alvo> alvos)
    {

        ValidarHabilidade validar = new();
        var unidade = partida.unidades.FirstOrDefault(u => u.id == id);
        var habilidade = GameData.getHabilidade(habilidadeId);
        if (unidade != null && habilidade != null)
        {
            if (validar.validarUsoHabilidade(partida, unidade.posicao, dono, habilidade, alvos) == 200 && unidade.jaAtacou == false)
            {
                if (dono == "Chico Diabo")
                {
                    return 200;
                }
                else
                {
                    return 400;
                }
            }
            else
            {
                return 400;
            }
        }
        else
        {
            return 401;
        }
    }
}

public class AplicarHabilidade
{
    public int aplicarHabilidadeAtiva(Partida partida, Habilidade habilidade, Unidades unidade, string dono, List<Alvo> alvos)
    {

        unidade.jaAtacou = true;
        var status = dono == partida.jogador1 ? partida.statusPlayer1 : partida.statusPlayer2;
        status.habilidadeRestante--;

        foreach (var alvo in alvos)
        {
            var target = partida.unidades.FirstOrDefault(u => u.cartaId == alvo.cartaId && u.dono == alvo.cartaPlayer);
            if (target != null)
            {

                switch (habilidade.tipoDeHabilidade)
                {
                    case "ataqueCorpoACorpo":
                        bool custaHp = Convert.ToBoolean(getEfeito(habilidade, "custaHp"));
                        break;
                    case "feixeMagia":

                        break;
                    case "invocacaoFraca":

                        break;
                    case "tiro":

                        break;
                    case "debuffRInimigo":

                        break;
                    default:
                        return 404;
                }


            }
            else
            {
                return 404;
            }
        }
        return 200;
    }

    public object getEfeito(Habilidade habilidade, string Chave)
    {
        if (habilidade.efeito.TryGetValue("custaHp", out object? valor) && valor != null)
        {
            return valor;
        }
        else
        {
            Console.WriteLine("Não foi possivo pegar valor");
            return new object();
        }        
    }
}