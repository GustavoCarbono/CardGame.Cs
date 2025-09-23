const gameData = {
    jogador1: "player1",
    jogador2: "player2",
    turnoAtual: 1,
    statusP1: {
        movRestante: 3,
        habilidadeRestante: 2
    },
    statusP2: {
        movRestante: 3,
        habilidadeRestante: 2
    },
    unidades: [
        {
            id: 1,
            nome: "personagem",
            dono: "jogador1",
            cartaId: "0x01",
            posicao: {
                x: 3,
                y: 4
            },
            habilidade : ["0x01"],
            status: "normal",
            dano: 15,
            combate: 10,
            hpAtual: 40,
            gastoMov: 1,
            passos: 2,
            img: "https://www.pokemon.com/static-assets/content-assets/cms2/img/pokedex/full/004.png",
            jaAtacou: false,
            jaMoveu: false
        },
        {
            id: 2,
            nome: "personagem",
            dono: "jogador1",
            cartaId: "0x03",
            posicao: {
                x: 2,
                y: 3
            },
            habilidade : ["0x03"],
            status: "normal",
            dano: 1,
            combate: 2,
            hpAtual: 15,
            gastoMov: 1,
            passos: 1,
            img: "https://www.pokemon.com/static-assets/content-assets/cms2/img/pokedex/full/004.png",
            jaAtacou: false,
            jaMoveu: false
        },
        {
            id: 3,
            nome: "personagem",
            dono: "jogador2",
            cartaId: "0x02",
            posicao: {
                x: 8,
                y: 4
            },
            habilidade : ["0x02"],
            status: "normal",
            dano: 10,
            combate: 5,
            hpAtual: 40,
            gastoMov: 1,
            passos: 1,
            img: "https://www.pokemon.com/static-assets/content-assets/cms2/img/pokedex/full/025.png",
            jaAtacou: false,
            jaMoveu: false
        },
        {
            id: 4,
            nome: "personagem",
            dono: "jogador2",
            cartaId: "0x04",
            posicao: {
                x: 7,
                y: 3
            },
            habilidade : ["0x04"],
            status: "normal",
            dano: 2,
            combate: 5,
            hpAtual: 15,
            gastoMov: 1,
            passos: 1,
            img: "https://static.vecteezy.com/system/resources/thumbnails/022/025/119/small/gray-stone-isolated-on-a-transparent-background-png.png",
            jaAtacou: false,
            jaMoveu: false
        }
    ],
    habilidade: [
        {
            id: "0x01",
            nome: "lança chamas",
            descricao: "a descrição",
            cooldown: 2,
            distancia: 4
        },
        {
            id: "0x02",
            nome: "lança chamas",
            descricao: "a descrição",
            cooldown: 2,
            distancia: 4
        },
        
        {
            id: "0x03",
            nome: "lança chamas",
            descricao: "a descrição",
            cooldown: 2,
            distancia: 4
        },
        
        {
            id: "0x04",
            nome: "lança chamas",
            descricao: "a descrição",
            cooldown: 2,
            distancia: 4
        },
    ],
    log: [
        {
            turno: 1,
            tipo: "mover",
            unidadeId: 1,
            de: {x: 1, y: 3},
            para: {x: 2, y: 3}
        },
        {
            turno: 2,
            tipo: "habilidade",

        }
    ]
}

export default gameData;