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

const accountButton = document.getElementById('account-button');
accountButton.addEventListener('click',() => {
    const url = '/Account/Account'
    if(window.location.pathname != url)
        window.location.href = url;
});