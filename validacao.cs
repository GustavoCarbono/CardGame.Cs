public class validarMovimento
{
    public string validarMovimentoUnidade(Posicao posicaoAtual, Posicao novaPosicao, int movRestante, int gastoMov)
    {
        if (novaPosicao.x < 0 || novaPosicao.x > 9 || novaPosicao.y < 0 || novaPosicao.y > 9)
        {
            int distancia = System.Math.Abs(novaPosicao.x - posicaoAtual.x) + System.Math.Abs(novaPosicao.y - posicaoAtual.y);
            int movNecessario = distancia * gastoMov;
            if (movNecessario <= movRestante)
            {
                return "sucesso";
            }
            else
            {
                return "movimento inválido";
            }
        } else
        {
            return "posição inválida";
        }
    }
}