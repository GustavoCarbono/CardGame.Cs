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
    public int habilidade(Partida partida, Tabuleiro tabuleiro, string dono, string id, string habilidadeId, List<Alvo> alvos)
    {

        ValidarHabilidade validar = new();
        var unidade = partida.unidades.FirstOrDefault(u => u.id == id);
        var habilidade = GameData.getHabilidade(habilidadeId);
        if (unidade != null && habilidade != null)
        {   //separar o jaAtacou
            if (validar.validarUsoHabilidade(partida, tabuleiro, unidade, dono, habilidade, alvos) == 200 && unidade.jaAtacou == false)
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

        ValidarHabilidade validar = new();
        unidade.jaAtacou = true;
        var status = dono == partida.jogador1 ? partida.statusPlayer1 : partida.statusPlayer2;
        status.habilidadeRestante--;

        ContextoHabilidade contexto = new();
        contexto.partida = partida;
        contexto.unidadeOriginal = unidade;
        contexto.unidadeAlterada = contexto.cloneUnidade(unidade);
        
        foreach (var alvo in alvos)
        {
            var target = alvo.id != null ? partida.getUnidadeById(alvo.id) : null;

            contexto.alvoOriginal = target;
            contexto.alvoAlterado = target == null ? null : contexto.cloneUnidade(target);
            switch (habilidade.tipoDeHabilidade)
            {
                case "ataqueCorpoACorpo":
                    if (contexto.alvoAlterado == null) return 404;
                    bool custaHp = Convert.ToBoolean(getEfeito(habilidade, "custaHp"));
                    if (custaHp)
                    {
                        contexto.unidadeAlterada.hpAtual -= 2;
                        contexto.alvoAlterado.hpAtual -= unidade.dano;
                        validar.validarHabilidadePassiva(habilidade, dono, contexto);
                    }
                    else
                    {
                        contexto.alvoAlterado.hpAtual -= unidade.dano;
                        validar.validarHabilidadePassiva(habilidade, dono, contexto);
                    }
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
        return 200;
    }

    public int aplicarHabilidadePassiva(Habilidade ativa, string dono, ContextoHabilidade ctx, Habilidade passiva)
    {
        if (ctx.alvoAlterado != null && ctx.alvoOriginal != null)
        {
            switch (passiva.tipoDeHabilidade)
            {
                case "refletirDano":
                    Random rand = new();
                    double chance = Convert.ToDouble(getEfeito(passiva, "porcentagem"));
                    double porcRefletida = Convert.ToDouble(getEfeito(passiva, "porcentagemDano"));
                    if (rand.Next(1, 101) <= (chance * 100))
                    {
                        ctx.alvoOriginal.hpAtual -= (int)((ctx.alvoOriginal.hpAtual - ctx.alvoAlterado.hpAtual) * porcRefletida);
                    }
                    break;
                default:
                    return 400;
            }
            return 200;
        }
        else
        {
            switch (passiva.tipoDeHabilidade)
            {
                case "":
                    break;
                default:
                    return 400;
            }
            return 200;
        }
    }

    public object getEfeito(Habilidade habilidade, string Chave)
    {
        if (habilidade.efeito.TryGetValue(Chave, out object? valor) && valor != null)
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