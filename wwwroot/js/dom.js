var contents = document.getElementsByClassName("content");
var fileMenuIcon = document.getElementById("file-menu-icon");

let toggleMenu = () => {
    fileMenuIcon.classList.toggle("bi-caret-up");
    for(const content of contents){
        content.classList.toggle("expanded");
    }
};

const overlayContainer = document.querySelector('.overlay-container');
const imageOverlay = document.querySelector('.image-overlay');
const overlayTitle = document.querySelector('.overlay-title');

let setOverlayImage = (imagePath,imageName) => {
    imageOverlay.src = imagePath;
    overlayTitle.textContent =  imageName;
    overlayContainer.style.display = 'block';
};

let appendImageElement = (fileGrid,file) => {
    const fileItem = document.createElement("div");
    fileItem.classList.add("file-item");

    const img = document.createElement("img");
    img.name = file.filename;
    img.src = file.filePath;
    img.classList.add("gallery-item");

    fileItem.appendChild(img);
    fileGrid.appendChild(fileItem);
};

let mouseOverImage = (event) => {
    const image = event.target.closest('.file-item');
    if(!image) return;

    if (image.querySelector('.select-indicator')) return;

    const indicator = document.createElement('div');
    indicator.classList.add('select-indicator');

    image.appendChild(indicator);
};

let mouseOutImage = (event) => {
    const indicators = document.querySelectorAll('.select-indicator');

    indicators.forEach(indicator => {
      indicator.remove();
    });
}

let selectImage = (event,selectionData) => {
    if (event.target.classList.contains('select-indicator')) {
        const parentImage = event.target.closest('.file-item');
        if (!parentImage) return;

        const selectIndicator = parentImage.querySelector('.select-indicator');
        selectIndicator.remove();
        
        const isSelected = parentImage.querySelector('.selected-indicator');
        if(isSelected){
            isSelected.remove();
        }
        else{
            const selectedIndicator = document.createElement('div');
            selectedIndicator.classList.add('selected-indicator');
            parentImage.appendChild(selectedIndicator);
        }
        const img = parentImage.querySelector('.gallery-item');
        toggleArrayItem(selectionData.images,img.getAttribute('name'));
    }
};

let displayImage = (event) => {
    const clickedImg = event.target.closest('.file-item');
    if (!clickedImg) return;

    const img = clickedImg.querySelector('.gallery-item');
    let imageName = img.getAttribute('name');

    const imagePath = img.getAttribute('src');
    setOverlayImage(imagePath,imageName);
};

let toggleArrayItem = (array, value) => {
    const index = array.indexOf(value);
    if (index !== -1) {
        array.splice(index, 1);
    } else {
        array.push(value);
    }
    return array;
}

export { toggleMenu, displayImage, appendImageElement , mouseOverImage , mouseOutImage , selectImage};