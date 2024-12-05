var passwordCheckbox = document.getElementById('passwordCheckbox');
passwordCheckbox.addEventListener('change', function () {
    const passwordInput = document.getElementById('Password');
    if (this.checked) {
        passwordInput.type = 'text'; // Change to text
    } else {
        passwordInput.type = 'password'; // Change back to password
    }
});
