using System.Collections.Generic;

public class Movimentacao
{

    public string movimento(Partida partida, string dono, string cartaId, Posicao novaPosicao)
    {
        
        validarMovimento validar = new();

        var unidade = partida.unidades.FirstOrDefault(u => u.dono == dono && u.cartaId == cartaId);
        if (unidade != null)
        {

            if (validar.validarMovimentoUnidade(unidade.posicao, novaPosicao, partida.statusPlayer1.movRestante, unidade.gastoMov) != "sucesso")
            {
                unidade.posicao = novaPosicao;
            return "sucesso";
            } 
            else
            {
                return "movimento inválido";
            }
        }
        else
        {
            return "unidade não encontrada";
        }
        
    }

}

public class Aplicacao
{

    public string aplicarTurno(Partida partida, int turno)
    {
        try
        {
            partida.turnoAtual = turno;
            return "sucesso";
        }
        catch (System.Exception)
        {
            return "erro";
            throw;
        }
    }

    public string aplicarStatus(Partida partida, StatusPlayer status, string jogador)
    {
        try
        {
            if (jogador == "player1")
            {
                partida.statusPlayer1 = status;
                return "sucesso";
            }
            else
            {
                partida.statusPlayer2 = status;
                return "sucesso";
            }
        }
        catch (System.Exception)
        {
            return "erro";
            throw;
        }
    }

    public string aplicarUnidades(Partida partida, string dono, string cartaId, Unidades unidadeNova)
    {
        try
        {
            var unidade = partida.unidades.FirstOrDefault(u => u.dono == dono && u.cartaId == cartaId);
            if (unidade != null)
            {
                unidade.posicao = unidadeNova.posicao;
                unidade.hpAtual = unidadeNova.hpAtual;
                unidade.jaAtacou = unidadeNova.jaAtacou;
                unidade.gastoMov = unidadeNova.gastoMov;
                unidade.habilidade = unidadeNova.habilidade;
                unidade.posicao = unidadeNova.posicao;
                unidade.dano = unidadeNova.dano;
                unidade.combate = unidadeNova.combate;
                unidade.hpMaximo = unidadeNova.hpMaximo;
                return "sucesso";
            }
            else
            {
                return "unidade não encontrada";
            }
            
        }
        catch (System.Exception)
        {
            return "erro";
            throw;
        }
    }

    public string aplicarLogPlayer(Partida partida, LogPlayer logPlayer, string jogador)
    {
        try
        {
            if (jogador == "player1")
            {
                partida.logPlayer1.Add(logPlayer);
                return "sucesso";
            }
            else
            {
                partida.logPlayer2.Add(logPlayer);
                return "sucesso";
            }
        }
        catch (System.Exception)
        {
            return "erro";
            throw;
        }
    }

    public string aplicarLog(Partida partida, Log log)
    {
        try
        {
            partida.log.Add(log);
            return "sucesso";
        }
        catch (System.Exception)
        {
            return "erro";
            throw;
        }
    }
}