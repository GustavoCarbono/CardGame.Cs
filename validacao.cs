public class ValidarMovimento
{
    public int validarMovimentoUnidade(Unidades unidade, Posicao novaPosicao, Partida partida, string dono, int passos)
    {
        if (novaPosicao.x < 0 || novaPosicao.x > 8 || novaPosicao.y < 0 || novaPosicao.y > 5)
        {
            int distancia = System.Math.Abs(novaPosicao.x - unidade.posicao.x) + System.Math.Abs(novaPosicao.y - unidade.posicao.y);

            if (distancia <= passos && (dono == partida.jogador1 ? partida.statusPlayer1 : partida.statusPlayer2)
                .movRestante >= 1)
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
}

public class ValidarHabilidade
{
    public int validarDistancia(Posicao origem, Posicao alvo, string? tipoDistancia, int areaDeAtaque)
    {
        if (alvo.x < 0 || alvo.x > 8 || alvo.y < 0 || alvo.y > 5)
        {
            try
            {
                int dx = System.Math.Abs(alvo.x - origem.x);
                int dy = System.Math.Abs(alvo.y - origem.y);

                return (dx + dy) <= areaDeAtaque ? 200 : 400;
            }
            catch
            {
                return 500;
            }
        }
        else
        {
            return 400;
        }
    }


    public int validarUsoHabilidade(Partida partida, Posicao origem, string dono, Habilidade habilidade, List<Alvo> alvos)
    {
        if (habilidade.inimigoMaximo <= alvos.Count || habilidade.inimigoMaximo == null)
        {
            if ((dono == partida.jogador1 ? partida.statusPlayer1 : partida.statusPlayer2)
                .habilidadeRestante >= 1)
            {
                foreach (var alvo in alvos)
                {
                    if (validarDistancia(origem, alvo.posicao, null, habilidade.areaDeAtaque) != 200)
                    {
                        return 401;
                    }
                }
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
}