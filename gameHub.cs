using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;

//GameHub
public class GameHub : Hub
{
    public static void main(String[] args)
    {
        Tabuleiro tabuleiro = new Tabuleiro();

        var celula = tabuleiro.Grid[2, 4];

    }
}