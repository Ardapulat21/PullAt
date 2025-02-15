import { toggleMenu, displayImage, selectImage, selectionModeToggle } from "./dom.js";
import { download, uploadFile ,refresh} from "./filehandler.js";

const selectionData = {
    filename: '',
    mode: false,
    images: [],
};
var button = document.getElementById("expandable-button");
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

const deleteFiles = selectionData.images.map(v => {
    return fetch(`/File/DeleteFileAsync/${v}`)
        .catch(e => {
            console.log(e);
        });
});

const deleteButton = document.getElementById('deleteButton');
deleteButton.addEventListener('click',() => {
    console.log('deletebutton');
    Promise.all(deleteFiles)
    .then(() => {
        refresh();
        selectionData.images=[];
    })
    .catch(error => {
        console.error('An error occurred during file deletion:', error);
    });
});

const refreshButton = document.getElementById('refreshButton');
refreshButton.addEventListener('click',() => {
    refresh();
})
function sayhi(){
    console.log('hi');
}