import { toggleMenu, displayImage, selectImage, selectionModeToggle } from "../dom.js";
import { download, refreshGallery, uploadFile} from "../filehandler.js";

const selectionData = {
    filename: '',
    mode: false,
    images: [],
};
var button = document.getElementById("file-menu-button");
button.addEventListener("click",toggleMenu);

const fileInput = document.getElementById("fileInput");
fileInput.addEventListener("change",uploadFile);

const uploadButton = document.getElementById("uploadButton");
uploadButton.addEventListener("click", () => {
    fileInput.click();
});

const fileGrid = document.querySelector('.file-grid');
fileGrid.addEventListener('click',(event) => {
    displayImage(event,selectionData);
});

fileGrid.addEventListener('click',(event) => {
    selectImage(event,selectionData);
});

const overlayContainer = document.querySelector('.overlay-container');

const imageContainer = document.getElementById('imageContainer');
imageContainer.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
});

const selectButton = document.querySelector(".select-button");
selectButton.addEventListener('click',(event) => {
    selectionModeToggle(selectionData);

});

const downloadButton = document.getElementById('downloadButton');
downloadButton.addEventListener('click',() => {
    download(selectionData.filename);
});

const exitButton = document.getElementById('exitButton');
exitButton.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
});

const saveButton = document.getElementById('saveButton');
saveButton.addEventListener('click',async () => {
    try{
        await Promise.all(selectionData.images.map(async (image) => {
            await download(image);
        }));
    } catch(error){
        console.log(error);
    }
});

const deleteButton = document.getElementById('deleteButton');
deleteButton.addEventListener('click',async () => {
    try{
        await Promise.all(selectionData.images.map(async (image) => {
            await fetch(`DeleteFileAsync/${image}`);
        }));
        await refreshGallery();
        selectionData.images = [];
    } catch(error){
        console.log(error);
    }
})

const refreshButton = document.getElementById('refreshButton');
refreshButton.addEventListener('click',() =>{
    refreshGallery();
});


const homeMenu = document.getElementById('home-menu')
const menuButton = document.getElementById('home-menu-button');
menuButton.addEventListener('click',() => {
    console.log('hey');
    homeMenu.style.width = '0px';
});