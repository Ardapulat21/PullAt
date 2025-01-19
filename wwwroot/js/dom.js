var contents = document.getElementsByClassName("content");
var icon = document.getElementById("icon");

let toggleMenu = () => {
    icon.classList.contains("bi-caret-down") ? 
    icon.classList.toggle("bi-caret-up") :
    icon.classList.toggle("bi-caret-down");

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

export { toggleMenu, select, setOverlayImage, appendImageElement };