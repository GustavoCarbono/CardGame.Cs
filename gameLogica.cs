public class Movimentacao
{

    public int movimento(Partida partida, Tabuleiro tabuleiro, int id, string dono, Posicao novaPosicao)
    {

        ValidarMovimento validar = new();
        ValidarHabilidade validarHabilidade = new();
        ContextoHabilidade contexto = new();
        contexto.partida = partida;

        var unidade = partida.getUnidadeById(id);
        if (unidade != null)
        {
            contexto.unidadeOriginal = unidade;
            contexto.unidadeAlterada = contexto.cloneUnidade(unidade);
            //logar no partida.log
            if (validar.validarMovimentoUnidade(unidade, tabuleiro, novaPosicao, partida, dono) == 200)
            {
                if (unidade.jaMoveu == false)
                {
                    if (tabuleiro.Grid[novaPosicao.x, novaPosicao.y].obstaculo)
                    {
                        var obstaculo = partida.getUnidadeById(tabuleiro.Grid[novaPosicao.x, novaPosicao.y].idObstaculo ?? 0);
                        if (obstaculo != null && !(obstaculo.hitbox ?? false))
                        {
                            unidade.posicao = novaPosicao;
                            var movimento = dono == "jogador1" ? partida.statusPlayer1 : partida.statusPlayer2;
                            movimento.movRestante--;
                            unidade.jaMoveu = true;
                            contexto.unidadeAlterada.hpAtual -= obstaculo.dano;
                            validarHabilidade.validarHabilidadePassiva(null, dono, contexto, null);
                            tabuleiro.Grid[novaPosicao.x, novaPosicao.y].ocupante = true;
                            tabuleiro.Grid[novaPosicao.x, novaPosicao.y].idOcupante = id;
                            return 200;
                        }
                        else
                        {
                            return 400;
                        }
                    }
                    else
                    {
                        unidade.posicao = novaPosicao;
                        var movimento = dono == "jogador1" ? partida.statusPlayer1 : partida.statusPlayer2;
                        movimento.movRestante--;
                        unidade.jaMoveu = true;
                        tabuleiro.Grid[novaPosicao.x, novaPosicao.y].ocupante = true;
                        tabuleiro.Grid[novaPosicao.x, novaPosicao.y].idOcupante = id;
                        return 200;
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
        else
        {
            return 404;
        }
    }
}

public class UsarHabilidade
{
    public int habilidade(Partida partida, Tabuleiro tabuleiro, string dono, int id, string habilidadeId, List<Alvo> alvos)
    {

        ValidarHabilidade validar = new();
        AplicarHabilidade aplicar = new();
        var unidade = partida.getUnidadeById(id);
        var habilidade = GameData.getHabilidade(habilidadeId);
        if (unidade != null && habilidade != null)
        {   //separar o jaAtacou
            if (validar.validarUsoHabilidade(partida, tabuleiro, unidade, dono, habilidade, alvos) == 200 && unidade.jaAtacou == false)
            {
                if (aplicar.aplicarHabilidadeAtiva(partida, habilidade, unidade, dono, alvos) == 200)
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
            var target = alvo.id.HasValue ? partida.getUnidadeById(alvo.id.Value) : null;

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
                        validar.validarHabilidadePassiva(habilidade, dono, contexto, null);
                    }
                    else
                    {
                        contexto.alvoAlterado.hpAtual -= unidade.dano;
                        validar.validarHabilidadePassiva(habilidade, dono, contexto, null);
                    }
                    break;
                case "feixeMagia":
                    var obstaculo = GameData.getObstaculo("0x01");
                    if (obstaculo != null)
                    {
                        partida.unidades.Add(new Unidades
                        {
                            id = Guid.NewGuid().GetHashCode(),
                            dono = dono,
                            obstaculoId = "0x01",
                            posicao = alvo.posicao,
                            duracao = obstaculo.duracao,
                            dano = obstaculo.dano,
                            hitbox = obstaculo.hitbox,
                            efeito = obstaculo.tipo
                        });
                    }
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
                    if (rand.NextDouble() <= chance && ctx.alvoOriginal.hpAtual != null &&
                        ctx.alvoAlterado.hpAtual != null)
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