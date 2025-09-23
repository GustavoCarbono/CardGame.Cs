import gameData from "./gameData.js"

const descricaoPersonagem = document.getElementById("descricao-personagem")

const canvaTabuleiro = document.getElementById("tabuleiro");
const ctx = canvaTabuleiro.getContext("2d");

const canvasPersonagem = document.getElementById("personagem");
const ctxPersonagem = canvasPersonagem.getContext("2d");

ctxPersonagem.clearRect(0, 0, ctxPersonagem.canvas.width, ctxPersonagem.canvas.height);

var blocoLargura, blocoAltura;
var descricao = false;
var modoHabilidade = false;
var cancelar;

const margem = 16;
const espaco = 10;

let tabuleiro = Array.from({ length: 8 }, () => Array(5).fill(null));
let menu = false;

let selecionado = null;
let origem = null;

function ajustarCanvas() {
    const scale = window.devicePixelRatio || 1;

    const widthT = canvaTabuleiro.clientWidth;
    const heightT = canvaTabuleiro.clientHeight;
    const widthP = canvasPersonagem.clientWidth;
    const heightP = canvasPersonagem.clientHeight;

    canvaTabuleiro.width = widthT*scale;
    canvaTabuleiro.height = heightT*scale;
    canvasPersonagem.width = widthP*scale;
    canvasPersonagem.height = heightP*scale;

    ctx.setTransform(1, 0, 0, 1, 0, 0);
    ctx.scale(scale, scale);
    ctxPersonagem.setTransform(1, 0, 0, 1, 0, 0);
    ctxPersonagem.scale(scale, scale);

    ctx.imageSmoothingEnabled = false;
    ctxPersonagem.imageSmoothingEnabled = false;

    const larguraDisponivel = widthT - margem * 2 - espaco * (8 - 1);
    const alturaDisponivel = heightT - margem * 2 - espaco * (5 - 1);

    blocoLargura = larguraDisponivel / 8;
    blocoAltura = alturaDisponivel / 5;

    cancelar = document.createElement("button");
    cancelar.innerHTML = '<i class="bi bi-x-lg"></i> Cancelar habilidade';
    cancelar.className = "cancelar";

    cancelar.addEventListener('click', cancelarHabilidade);

    document.querySelector('body').appendChild(cancelar);
}

const img = new Image();
img.src = 'img/concreto.png';
img.onload = function(){
    ajustarCanvas();
    desenharTabuleiro();
}

function desenharTabuleiro(){
    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    for(let j=0; j<5; j++){
        for(let i=0; i<8; i++){
            const x = margem + i * (blocoLargura + espaco);
            const y = margem + j * (blocoAltura + espaco);

            ctx.fillStyle = "black";
            ctx.fillRect(x, y, blocoLargura, blocoAltura);
            ctx.drawImage(img, x+1, y+1, blocoLargura, blocoAltura);

            ctx.strokeStyle = "black";
            ctx.lineWidth = 2;
            ctx.strokeRect(x, y, blocoLargura, blocoAltura);
        }
    }
    ctx.beginPath();
    ctx.strokeStyle = "red";
    ctx.lineWidth = 11;
    const meioX = canvaTabuleiro.clientWidth / 2;
    ctx.moveTo(meioX, 0);
    ctx.lineTo(meioX, canvaTabuleiro.clientHeight);
    ctx.stroke();
}

function desenharPersonagem() {
    ctxPersonagem.clearRect(0, 0, canvasPersonagem.width, canvasPersonagem.height);
    for(let j=0; j<5; j++){
        for(let i=0; i<8; i++){
            if(tabuleiro[i][j]!=null){
                const x = margem + i * (blocoLargura + espaco);
                const y = margem + j * (blocoAltura + espaco);

                const img = new Image();
                img.src = tabuleiro[i][j].src;
                img.onload = () => {
                    ctxPersonagem.drawImage(img, x+12, y, blocoLargura-24, blocoAltura);
                }
            }
        }
    }
}

document.querySelectorAll(".personagem").forEach(p => {
    p.addEventListener("dragstart", (e) => {
        e.dataTransfer.setData("id", p.dataset.id);
        menu = true
        const img = p.querySelector("img");
        if (img) {
            e.dataTransfer.setData("src", img.src);
        }

    });
});

canvasPersonagem.addEventListener("dragover", (e) => e.preventDefault());

canvasPersonagem.addEventListener("drop", (e) => {
    e.preventDefault();

    selecionado = null;
    origem = null;

    desenharTabuleiro();
    
    const personagemId = e.dataTransfer.getData("id");
    const personagemSrc = e.dataTransfer.getData("src");

    if (!personagemSrc) return;

    const rect = canvaTabuleiro.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    ajeitarPosicao(x, y, personagemSrc, personagemId);
    console.log(`Soltou personagem ${personagemId} em (${x}, ${y})`);
});

function ajeitarPosicao(x, y, personagemSrc, id) {
    let coluna, linha;
    if(menu) {
        menu = false;
        if(x<=blocoLargura*4+margem+espaco*3){
            for(let i = 0; i<8; i++) {
                for(let j = 0; j<5; j++) {
                    const cellX = margem + i * (blocoLargura + espaco);
                    const cellY = margem + j * (blocoAltura + espaco);

                    if (x >= cellX && x <= cellX + blocoLargura && y >= cellY && y <= cellY + blocoAltura) {
                        coluna = i;
                        linha = j;
                    }
                }
            }
        } else {
            alert("espaço inválido")
            return
        }
    } else {

    }

    if(tabuleiro[coluna][linha]!==null) {
        alert("Espaço ocupado")
        return;
    }

    tabuleiro[coluna][linha] = { src: personagemSrc, id: id }

    desenharPersonagem();
}

function pegarCelula(x, y) {
    for (let i = 0; i < 8; i++) {
        for (let j = 0; j < 5; j++) {
            const cellX = margem + i * (blocoLargura + espaco);
            const cellY = margem + j * (blocoAltura + espaco);

            if (x >= cellX && x <= cellX + blocoLargura &&
                y >= cellY && y <= cellY + blocoAltura) {
                return { coluna: i, linha: j };
            }
        }
    }
    return { coluna: null, linha: null };
}

canvasPersonagem.addEventListener('click', (e) => {
    const rect = canvaTabuleiro.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    descricaoPersonagem.style.display = "none";
    descricao = false;

    const { coluna, linha } = pegarCelula(x, y);

    if(coluna == null || linha == null) return;
    if (!modoHabilidade) {
        const personagem = tabuleiro[coluna][linha];
        if(selecionado === null) {
            if(personagem !== null) {
                if(!pegarPersonagem(personagem.id).jaMoveu) {
                    desenharTabuleiro();
                    selecionado = personagem;
                    origem = { coluna, linha };
                    desenharQuadrados(coluna, linha, pegarPersonagem(personagem.id).passos);
                    console.log("Personagem selecionado em:", coluna, linha);
                }
            }
        } else {
            if (tabuleiro[coluna][linha] === null) {
                console.log(tabuleiro[origem.coluna][origem.linha].id)
                if(validarMovimento(coluna, linha, origem.coluna, origem.linha, pegarPersonagem(tabuleiro[origem.coluna][origem.linha].id).passos)) {
                    tabuleiro[coluna][linha] = selecionado;

                    tabuleiro[origem.coluna][origem.linha] = null;
                    selecionado = null;
                    origem = null;

                    desenharPersonagem();
                    desenharTabuleiro();
                    console.log("Personagem moveu para:", coluna, linha);
                } else {
                    selecionado = null;
                    origem = null;

                    desenharTabuleiro();
                }
            } else {
                if(origem.coluna == coluna && origem.linha == linha) {
                    if(!descricao) {
                        const cellX = margem + coluna * (blocoLargura + espaco);
                        const cellY = margem + linha * (blocoAltura + espaco);

                        const rect = canvaTabuleiro.getBoundingClientRect();

                        descricaoPersonagem.style.left = (rect.left + cellX-30) + "px";

                        if (linha < 3) {
                            descricaoPersonagem.style.top = (rect.top + cellY + blocoAltura + 5) + "px";
                        } else {
                            descricaoPersonagem.style.top = (rect.top + cellY - blocoAltura*2.25) + "px";
                        }

                        let texto = `
                            <h4>ATK ${pegarPersonagem(personagem.id).dano} HP ${pegarPersonagem(personagem.id).hpAtual} COMB. ${pegarPersonagem(personagem.id).combate}</h4>
                            <img class="personagem-img" src="${pegarPersonagem(personagem.id).img}" alt="">
                            <div class="habilidades"></div>`;

                        descricaoPersonagem.innerHTML = texto;

                        const container = document.querySelector('.habilidades')
                        container.innerHTML = '';

                        pegarPersonagem(personagem.id).habilidade.forEach(id => {
                            const hab = pegarHabilidade(id);
                            const div = document.createElement('div');
                            div.className = 'habilidade';
                            div.dataset.habilidade = hab.id;
                            div.dataset.col = coluna;
                            div.dataset.lin = linha;
                            div.innerHTML = `<strong>${hab.nome}</strong>: ${hab.descricao}`

                            div.addEventListener('click', () => {
                                usarHabilidade(hab.id, Number(div.dataset.col), Number(div.dataset.lin));
                            })

                            container.appendChild(div);
                        })

                        descricaoPersonagem.style.display = "flex";

                        selecionado = null;
                        desenharTabuleiro();
                        descricao = true;
                    }
                } else {
                    console.log("celula ocupada")
                    selecionado = null;
                    origem = null;
                    desenharTabuleiro(); 
                }
            }
        }
    } else {
        //envia informação para o servidor
    }
});

function desenharQuadrados(coluna, linha, tamanho) {
    for (let i = 0; i < 8; i++) {
        for (let j = 0; j < 5; j++) {
            let dx = Math.abs(i - coluna);
            let dy = Math.abs(j - linha);
            if (dx+dy <= tamanho || tamanho == null) {
                const x = margem + i * (blocoLargura + espaco);
                const y = margem + j * (blocoAltura + espaco);
                if(tabuleiro[i][j] == null) {
                    ctx.fillStyle = "#fffb0046";
                } else {
                    ctx.fillStyle = (i == coluna && j == linha) ? "#ffffff00" : "#ff000046";
                }
                ctx.fillRect(x, y, blocoLargura, blocoAltura);
            }
        }
    }
}

function validarMovimento(coluna, linha, colunaAlvo, linhaAlvo, tamanho) {
    let dx = Math.abs(colunaAlvo - coluna);
    let dy = Math.abs(linhaAlvo - linha);
    if (dx+dy <= tamanho || tamanho == null) {
        return true;
    } else {
        return false;
    }
}

function pegarPersonagem(id) {
    let persona = null;
    gameData.unidades.forEach(a => {
        if(parseInt(a.id) == parseInt(id)) {
            persona = a;
        }
    });
    return persona;
}

function pegarHabilidade(id) {
    let habilidade = null;
    gameData.habilidade.forEach(a => {
        if(a.id == id) {
            habilidade = a;
        }
    });
    return habilidade;
}

function usarHabilidade(id, x, y) {
    modoHabilidade = true;
    descricaoPersonagem.style.display = "none";
    cancelar.style.display = "flex"
    desenharQuadrados(x, y, pegarHabilidade(id).distancia);
}

window.usarHabilidade = usarHabilidade;

function cancelarHabilidade() {
    modoHabilidade = false;
    cancelar.style.display = "none";
    descricaoPersonagem.style.display = "none";
    desenharPersonagem();
    desenharTabuleiro();
}