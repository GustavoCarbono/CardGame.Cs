document.querySelectorAll('.personagem-header').forEach(header => {
    header.addEventListener('click', () => {
        const container = header.parentElement;
        const content = container.querySelector('.personagem-descricao');
        const arrow = container.querySelector('.arrow');

        if (content.style.height === '' || content.style.height === '0px') {
            content.style.height = content.scrollHeight + 'px';
            content.style.margin.bottom = '10px';
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
})

const equipeBox = document.getElementById('checkbox-equipe');
const statusBox = document.getElementById('checkbox-status');
const logBox = document.getElementById('checkbox-log');

document.getElementById('equipe').addEventListener('click', (e) => {
    if(equipeBox.checked) {
        e.preventDefault();
        equipeBox.checked = false;
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