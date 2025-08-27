public class ValidarMovimento
{
    public int validarMovimentoUnidade(Unidades unidade, Tabuleiro tabuleiro, Posicao novaPosicao, Partida partida, string dono, int passos)
    {
        if (novaPosicao.x < 0 || novaPosicao.x > 8 || novaPosicao.y < 0 || novaPosicao.y > 5)
        {
            int distancia = System.Math.Abs(novaPosicao.x - unidade.posicao.x) + System.Math.Abs(novaPosicao.y - unidade.posicao.y);

            if (distancia <= passos && (dono == partida.jogador1 ? partida.statusPlayer1 : partida.statusPlayer2)
                .movRestante >= 1)
            {
                if (!tabuleiro.Grid[novaPosicao.x, novaPosicao.y].ocupante)
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
            return 400;
        }
    }
}

public class ValidarHabilidade
{
    public int validarDistancia(Posicao origem, Posicao alvo, string? tipoDistancia, int? areaDeAtaque)
    {
        if (alvo.x < 0 || alvo.x > 8 || alvo.y < 0 || alvo.y > 5)
        {
            try
            {
                int dx = System.Math.Abs(alvo.x - origem.x);
                int dy = System.Math.Abs(alvo.y - origem.y);
                if (areaDeAtaque == null) return 200;
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


    public int validarUsoHabilidade(Partida partida, Tabuleiro tabuleiro, Unidades unidade, string dono, Habilidade habilidade, List<Alvo> alvos)
    {
        if (habilidade.inimigoMaximo <= alvos.Count || habilidade.inimigoMaximo == null)
        {
            if ((dono == partida.jogador1 ? partida.statusPlayer1 : partida.statusPlayer2)
                .habilidadeRestante >= 1)
            {
                foreach (var alvo in alvos)
                {

                    if (validarDistancia(unidade.posicao, alvo.posicao, null, habilidade.areaDeAtaque) != 200)
                    {
                        return 401;
                    }
                    if (!tabuleiro.Grid[alvo.posicao.x, alvo.posicao.y].ocupante)
                    {
                        return 400;
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

    public int validarHabilidadePassiva(Habilidade habilidade, string dono, ContextoHabilidade ctx)
    {
        AplicarHabilidade aplicar = new();
        if (ctx.alvoOriginal == null)
        {
            if (ctx.unidadeOriginal.habilidade.Any())
            {
                foreach (var idPassiva in ctx.unidadeOriginal.habilidade)
                {
                    var passiva = GameData.getHabilidade(idPassiva);
                    if (passiva != null)
                    {
                        if (!passiva.ativo)
                        {
                            switch (passiva.gatilho)
                            {
                                case "": //ainda não definido
                                    break;
                                //especificações de passivas sem alvo
                                default:
                                    break;
                            }
                        }
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
            if (ctx.unidadeOriginal.habilidade.Any())
            {
                foreach (var idPassiva in ctx.unidadeOriginal.habilidade)
                {
                    var passiva = GameData.getHabilidade(idPassiva);
                    if (passiva != null)
                    {
                        if (!passiva.ativo)
                        {
                            switch (passiva.gatilho)
                            {
                                case "receberDano":
                                    if (ctx.alvoOriginal.hpAtual < ctx.alvoAlterado!.hpAtual)
                                    {
                                        aplicar.aplicarHabilidadePassiva(habilidade, dono, ctx, passiva);
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
                return 200;
            }
            else
            {
                return 400;
            }
        }
    }
}