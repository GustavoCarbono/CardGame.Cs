using System.Collections.Generic;

public class Movimentacao
{

    public string movimento(Partida partida, string dono, string cartaId, Posicao novaPosicao)
    {

        validarMovimento validar = new();

        var unidade = partida.unidades.FirstOrDefault(u => u.dono == dono && u.cartaId == cartaId);
        if (unidade != null)
        {

            if (validar.validarMovimentoUnidade(unidade.posicao, novaPosicao, partida.statusPlayer1.movRestante, unidade.gastoMov) == "sucesso")
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