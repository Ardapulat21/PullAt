import { GET, POST, AJAX } from "./Api/api.js";
import { toggleMenu, select, setOverlayImage } from "./dom.js";
import { download, refreshGallery} from "./filehandler.js";
import { addIfNotExist } from "./Utils/Utils.js";

var button = document.getElementById("expandable-button");
button.addEventListener("click",toggleMenu);

const fileInput = document.getElementById("fileInput");
fileInput.addEventListener("change",async function (event) {
    const file = event.target.files[0];
    if(!file) return;

    const formData = new FormData();
    formData.append("file", file);
    await POST(
        "/File/UploadFile",formData)
        .then(refreshGallery);
    event.target.value = '';
});
var filename;
let displayImage = (event) => {
    const clickedImg = event.target.closest('.file-item');
    if (!clickedImg) return;

    const childImage = clickedImg.querySelector('.gallery-item');
    filename = childImage.getAttribute('name');
    if(selectionMode){
        addIfNotExist(selectedImages,filename);
    }
    else{
        const imagePath = childImage.getAttribute('src');
        setOverlayImage(imagePath,filename);
    }
};

const fileGrid = document.querySelector('.file-grid');
fileGrid.addEventListener('click',displayImage);

let selectImage = (event) => {
    if(!selectionMode) return;

    const clickedImg = event.target.closest('.file-item');
    if(!clickedImg) return;

    select(clickedImg);
};

fileGrid.addEventListener('click',selectImage);

const overlayContainer = document.querySelector('.overlay-container');

const imageContainer = document.getElementById('imageContainer');
imageContainer.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
});

var selectedImages = new Array();
let selectionMode = false;

let selectionModeToggle = () => {
    if(selectionMode){
        selectedImages = [];
        selectButton.style.backgroundColor = "rgba(249, 249, 249, 0.3)";
        const elements = document.querySelectorAll(`.selection-indicator`);
        elements.forEach(element => {
            element.parentNode.removeChild(element);
        });
    }
    else{
        selectButton.style.backgroundColor = "rgba(249, 249, 249, 1)";
    }
    selectionMode = !selectionMode;
}

const selectButton = document.querySelector(".select-button");
selectButton.addEventListener('click',selectionModeToggle);


const downloadButton = document.getElementById('downloadButton');
downloadButton.addEventListener('click',() => {
    download(filename);
});

const exitButton = document.getElementById('exitButton');
exitButton.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
});