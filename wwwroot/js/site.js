// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById('showPassword').addEventListener('change', function () {
    const passwordInput = document.getElementById('Password');
    if (this.checked) {
        passwordInput.type = 'text'; // Change to text
    } else {
        passwordInput.type = 'password'; // Change back to password
    }
});

function navigateToHome() {
    var url = '/Home/Index';
    window.location.href = url;
}