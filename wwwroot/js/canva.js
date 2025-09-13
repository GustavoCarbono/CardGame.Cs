const canvaTabuleiro = document.getElementById("tabuleiro");
const ctx = canvaTabuleiro.getContext("2d");

const canvasPersonagem = document.getElementById("personagem");
const ctxPersonagem = canvasPersonagem.getContext("2d");

ctxPersonagem.clearRect(0, 0, ctxPersonagem.canvas.width, ctxPersonagem.canvas.height);

var blocoLargura, blocoAltura;

const margem = 16;
const espaco = 10;

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
}

const img = new Image();
img.src = 'img/concreto.png';
img.onload = function(){
    ajustarCanvas();
    desenharTabuleiro();
    ctx.beginPath();
    ctx.strokeStyle = "red";
    ctx.lineWidth = 11;
    const meioX = canvaTabuleiro.clientWidth / 2;
    ctx.moveTo(meioX, 0);
    ctx.lineTo(meioX, canvaTabuleiro.clientHeight);
    ctx.stroke();
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
}

document.querySelectorAll(".personagem").forEach(p => {
    p.addEventListener("dragstart", (e) => {
        e.dataTransfer.setData("id", p.dataset.id);
        const img = p.querySelector("img");
        if (img) {
            e.dataTransfer.setData("src", img.src);
        }

    });
});

canvasPersonagem.addEventListener("dragover", (e) => e.preventDefault());

canvasPersonagem.addEventListener("drop", (e) => {
    e.preventDefault();

    const personagemId = e.dataTransfer.getData("id");
    const personagemSrc = e.dataTransfer.getData("src");

    if (!personagemSrc) return;

    const rect = canvaTabuleiro.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    ajeitarPosicao(x, y, personagemSrc);
    console.log(`Soltou personagem ${personagemId} em (${x}, ${y})`);
});

function ajeitarPosicao(x, y, personagemSrc) {
    console.log(personagemSrc)

    if(x<=blocoLargura*4+margem+espaco*3){
        if(!(x>margem && y>margem && y<=margem+blocoAltura*5+espaco*4)) {
            alert("espaço inválido")
            return
        }
    } else {
        alert("espaço inválido")
        return
    }

    const img = new Image();
    img.src = personagemSrc;
    img.onload = () => {
        var posicaoX, posicaoY;
        for(let i=1; i<6; i++) {
            for(let j=1; j<5; j++) {
                if(x<margem+blocoLargura*j+espaco*(j-1) && x>margem+blocoLargura*(j-1)+espaco*(j-2)) {
                    if(y<margem+blocoAltura*i+espaco*(i-1) && y>margem+blocoAltura*(i-1)+espaco*(i-2)){
                        posicaoY = margem+blocoAltura*(i-1)+espaco*(i-1);
                        posicaoX = margem+blocoLargura*(j-1)+espaco*(j-1);
                    }
                }
            }
        }
        ctxPersonagem.drawImage(img, posicaoX+12, posicaoY, blocoLargura-24, blocoAltura);
    };
}