import { addIfNotExist } from "./utils.js";
var contents = document.getElementsByClassName("content");
var fileMenuIcon = document.getElementById("file-menu-icon");

let toggleMenu = () => {
    fileMenuIcon.classList.toggle("bi-caret-up");
    for(const content of contents){
        content.classList.toggle("expanded");
    }
};

let select = (image) => {
    const indicator = document.createElement('div');
    indicator.classList.add('selection-indicator');

    const indicatorElement = image.querySelector(".selection-indicator");
    indicatorElement 
    ? image.removeChild(indicatorElement) 
    : image.appendChild(indicator);
};

const overlayContainer = document.querySelector('.overlay-container');
const imageOverlay = document.querySelector('.image-overlay');
const overlayTitle = document.querySelector('.overlay-title');

let setOverlayImage = (imagePath,imageName) => {
    imageOverlay.src = imagePath;
    overlayTitle.textContent =  imageName;
    overlayContainer.style.display = 'block';
}

let appendImageElement = (fileGrid,file) => {
    const fileItem = document.createElement("div");
    fileItem.classList.add("file-item");

    const img = document.createElement("img");
    img.name = file.filename;
    img.src = file.filePath;
    img.classList.add("gallery-item");

    fileItem.appendChild(img);
    fileGrid.appendChild(fileItem);
}

let displayImage = (event,selectionData) => {
    const clickedImg = event.target.closest('.file-item');
    if (!clickedImg) return;

    const childImage = clickedImg.querySelector('.gallery-item');
    selectionData.filename = childImage.getAttribute('name');
    if(selectionData.mode){
        addIfNotExist(selectionData.images,selectionData.filename);
    }
    else{
        const imagePath = childImage.getAttribute('src');
        setOverlayImage(imagePath,selectionData.filename);
    }
};

let selectImage = (event,selectionData) => {
    if(!selectionData.mode) return;
    
    const clickedImg = event.target.closest('.file-item');
    if(!clickedImg) return;

    select(clickedImg);
};

const selectButton = document.querySelector(".select-button");

let selectionModeToggle = (selectionData) => {
    if(selectionData.mode){
        selectionData.images = [];
        selectButton.style.backgroundColor = "rgba(249, 249, 249, 0.3)";
        const elements = document.querySelectorAll(`.selection-indicator`);
        elements.forEach(element => {
            element.parentNode.removeChild(element);
        });
    }
    else{
        selectButton.style.backgroundColor = "rgba(249, 249, 249, 1)";
    }
    selectionData.mode = !selectionData.mode;
};

export { toggleMenu, select, displayImage, selectImage, selectionModeToggle, appendImageElement };