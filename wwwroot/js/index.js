import gameData from "./gameData.js"

const time = document.getElementById('personagem-container')
const status = document.getElementById('status-container')

status.addEventListener('click', (e) => {
    const header = e.target.closest('.personagem-header');
    if (!header) return;

    const container = header.parentElement;
    const content = container.querySelector('.personagem-descricao');
    const arrow = container.querySelector('.arrow');

    if (content.style.height === '' || content.style.height === '0px') {
        content.style.height = content.scrollHeight + 'px';
        arrow.style.transform = 'rotate(180deg)';

        content.addEventListener('transitionend', function handler() {
        content.style.height = 'auto';
        content.removeEventListener('transitionend', handler);
        });
    } else {
        content.style.height = content.scrollHeight + 'px'; 
        requestAnimationFrame(() => {
        content.style.height = '0px';
        });
        arrow.style.transform = 'rotate(0deg)';
    }
})

function pegarHabilidade(id) {
    let habilidade = null;
    gameData.habilidade.forEach(a => {
        if(a.id == id) {
            habilidade = a;
        }
    });
    return habilidade;
}

const equipeBox = document.getElementById('checkbox-equipe');
const statusBox = document.getElementById('checkbox-status');
const logBox = document.getElementById('checkbox-log');

document.getElementById('equipe').addEventListener('click', (e) => {
    if(equipeBox.checked) {
        e.preventDefault();
        equipeBox.checked = false;
        atualizarStatus()
    } else {
        statusBox.checked = false;
        logBox.checked = false;
    }
})

document.getElementById('status-label').addEventListener('click', (e) => {
    if (statusBox.checked) {
        e.preventDefault();
        statusBox.checked = false;

    } else {
        equipeBox.checked = false;
        logBox.checked = false;
    }
});

document.getElementById('log-label').addEventListener('click', (e) => {
    if (logBox.checked) {
        e.preventDefault();
       logBox.checked = false;

    } else {
        equipeBox.checked = false;
        statusBox.checked = false;
    }
});

function atualizarStatus(jogador) {
    let statusJogador = (gameData.jogador1 == jogador) ? gameData.statusP1 : gameData.statusP2;
    status.innerHTML = '';
    let texto = `<div class="info-jogador">
                    <h3 class="habilidade-restante">Habilidade restante ${statusJogador.habilidadeRestante}</h3>
                    <h3 class="Movimento-restante">Movimento restante ${statusJogador.movRestante}</h3>
                </div>`;
    gameData.unidades.forEach(unidade => {
        texto += `<div class="personagem-info-toggle">
                        <div class="personagem-header">
                            <h3>${unidade.nome}</h5>
                            <i class="bi bi-caret-down-fill arrow"></i>
                        </div>
                        <div class="personagem-descricao">
                            <p class="p-descricao">
                                <strong>ATTACK ${unidade.dano} HP ${unidade.hpAtual} COMBATE ${unidade.combate}</strong><br>
                                <strong>Status:</strong> ${unidade.status}<br>
                                <strong>Habilidades:</strong> `
        unidade.habilidade.forEach(hab => {
            texto += pegarHabilidade(hab).nome + " " + pegarHabilidade(hab).descricao + "<br> Com cooldown de " + pegarHabilidade(hab).cooldown;
        })
        texto += '</p></div></div>'
        status.innerHTML = texto;
    })
}

function atualizarTime(jogador) {
    time.innerHTML = '';
    let texto = '';

    const unidades = gameData.unidades.filter(u => u.dono === jogador);

    for (let i = 0; i < unidades.length; i += 2) {
        texto += `<div class="personagem-div">`;
        texto += gerarPersonagem(unidades[i]);

        if (unidades[i + 1]) {
            texto += gerarPersonagem(unidades[i + 1]);
        }
        texto += `</div>`;
    }
    time.innerHTML = texto;
}

function gerarPersonagem(unidade) {
    let html = `
        <div class="personagem" draggable="true" data-id="${unidade.id}">
            <h4>ATK ${unidade.dano} HP ${unidade.hpAtual} COMB. ${unidade.combate}</h4>
            <img class="personagem-img" src="${unidade.img || 'img/default.png'}" alt="${unidade.nome}">
    `;

    unidade.habilidade.forEach(hab => {
        const h = pegarHabilidade(hab);
        html += `<p class="descricao"><strong>${h.nome}:</strong> ${h.descricao}</p>`;
    });

    html += `</div>`;
    return html;
}


atualizarTime("jogador1");
atualizarStatus("jogador1")