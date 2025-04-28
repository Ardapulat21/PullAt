// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function navigateToHome() {
    var url = '/Home/Index';
    window.location.href = url;
}

const homeMenu = document.getElementById('home-menu');
const homeMenuContent = document.getElementById('home-menu-content');
const homeMenuIcon = document.getElementById('home-menu-icon-menu');
const homeMenuButton = document.getElementById('home-menu-button-layout');
var isHomeMenuExpanded = true;
homeMenuButton.addEventListener('click',() => {
    if(isHomeMenuExpanded){
        homeMenu.style.width = '40px';
    }
    else{
        homeMenu.style.width = '140px';
    }
    homeMenuIcon.classList.toggle("bi-caret-right");
    isHomeMenuExpanded = !isHomeMenuExpanded;
});


