const logoutButton = document.getElementById('logout-button');
logoutButton.addEventListener('click',async () => {
    await fetch('/User/Logout', {
        method: 'GET',
    })
    .catch(error => console.error('Error:', error));
    window.location.href = '/User/Login'
});

const filesButton = document.getElementById('files-button');
filesButton.addEventListener('click',() => {
    if(window.location.pathname != '/File/Files')
        window.location.href = '/File/Files'

});