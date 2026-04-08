const apiUrls = {
    conta: 'http://localhost:5001',
    transferencia: 'http://localhost:5002'
};

function getToken() {
    return localStorage.getItem('token');
}

function requireAuth() {
    const token = getToken();
    if (!token) {
        window.location.href = 'login.html';
        return false;
    }
    return true;
}

function getAuthHeaders() {
    return {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + getToken()
    };
}

function parseJwt(token) {
    if (!token) {
        return {};
    }
    const payload = token.split('.')[1];
    const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map((c) => {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
}

function getAccountInfo() {
    const token = getToken();
    if (!token) {
        return { number: '---', name: '---' };
    }
    const payload = parseJwt(token);
    return {
        number: payload.contaOrigemNumero || payload.ContaOrigemNumero || '---',
        name: payload.Nome || payload.nome || '---'
    };
}

function showMessage(id, text, type = 'negative') {
    const message = document.getElementById(id);
    if (!message) {
        return;
    }
    message.className = `ui ${type} message`;
    message.innerText = text;
    message.style.display = 'block';
    setTimeout(() => {
        message.style.display = 'none';
    }, 5000);
}

function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('accountNumber');
    window.location.href = 'login.html';
}

function setupPage(title) {
    if (!requireAuth()) {
        return;
    }
    const info = getAccountInfo();
    const accountNumber = document.getElementById('accountNumber');
    const accountName = document.getElementById('accountName');
    const pageTitle = document.getElementById('pageTitle');
    if (accountNumber) {
        accountNumber.innerText = info.number;
    }
    if (accountName) {
        accountName.innerText = info.name;
    }
    if (pageTitle) {
        pageTitle.innerText = title;
    }
}
