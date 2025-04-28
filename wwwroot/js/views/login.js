const passwordField = document.getElementById('password');
const showPassword = document.getElementById('showPassword');
showPassword.addEventListener('change', function() {
    passwordField.type = this.checked ? 'text' : 'password';
});

const loginButton = document.getElementById('login-button');
loginButton.addEventListener('click',() => {
    window.location.href = "/User/Login";
});

const registerButton = document.getElementById('register-button');
registerButton.addEventListener('click',() => {
    window.location.href = "/User/Register";
});