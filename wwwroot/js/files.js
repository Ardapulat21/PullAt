import { GET, POST , AJAX } from "./Api/api.js";
import { addIfNotExist} from "./Utils/Utils.js";

//#region Expandable Button
var button = document.getElementById("expandable-button");
var contents = document.getElementsByClassName("content");
var icon = document.getElementById("icon");
button.addEventListener("click",function(){
    icon.classList.contains("bi-caret-down") ? 
    icon.classList.toggle("bi-caret-up") :
    icon.classList.toggle("bi-caret-down");

    for(var i = 0;i < contents.length; i++){
        var content = contents[i];
        if(content.style.maxHeight){
            content.style.maxHeight = null;
            content.style.padding = "0px 15px";
        }
        else{
            content.style.padding = "5px 15px";
            content.style.maxHeight = content.scrollHeight + "px";
        }
    }
});

const fileInput = document.getElementById("fileInput");
fileInput.addEventListener("change",async function (event) {
    const file = event.target.files[0];
    if(!file) return;
    const formData = new FormData();
    formData.append("file", file);
    await POST(
        "/File/UploadFile",formData)
        .then(GetFiles);
    event.target.value = '';
});
//#endregion
let GetFiles = () => {
    const fileGrid = document.querySelector(".file-grid");
    fileGrid.innerHTML = ""; 
    AJAX('/File/GetFiles','GET',null,(response) => {
        const json = response.response;
        let obj = JSON.parse(json);
        obj.forEach(file =>{
            const fileItem = document.createElement("div");
            fileItem.classList.add("file-item");

            const img = document.createElement("img");
            img.name = file.filename;
            img.src = file.filePath;
            img.classList.add("gallery-item");

            fileItem.appendChild(img);
            fileGrid.appendChild(fileItem);
        })
    });
}
//#endregion AJAX
//#region Overlay
let displayImage = (event) => {
    const clickedImg = event.target.closest('.file-item');
    if (clickedImg) {
        const childImage = clickedImg.querySelector('.gallery-item');
        const imageName = childImage.getAttribute('name');
        if(selectionMode){
            addIfNotExist(selectedImages,imageName);
            return;
        }
        const imagePath = childImage.getAttribute('src');
        imageOverlay.src = imagePath;
        filename = childImage.getAttribute('name');
        overlayTitle.textContent =  filename;
        overlayContainer.style.display = 'block';
    }
};
var filename;
const overlayContainer = document.querySelector('.overlay-container');
const imageOverlay = document.querySelector('.image-overlay');
const overlayTitle = document.querySelector('.overlay-title');

const fileGrid = document.querySelector('.file-grid');
fileGrid.addEventListener('click',displayImage);

let selectImage = (event) => {
    if(!selectionMode)
        return;

    const clickedImg = event.target.closest('.file-item');
    if(clickedImg){
        const indicator = document.createElement('div');
        indicator.classList.add('selection-indicator');

        const indicatorElement = clickedImg.querySelector(".selection-indicator");
        indicatorElement 
        ? clickedImg.removeChild(indicatorElement) 
        : clickedImg.appendChild(indicator);
    }
};

fileGrid.addEventListener('click',selectImage);

const imageContainer = document.getElementById('imageContainer');
imageContainer.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
});
//#endregion Overlay
//#region Buttons
var selectedImages = new Array();
let selectionMode = false;
let select = () => {
    if(selectionMode){
        selectedImages = [];
        selectButton.style.backgroundColor = "rgba(249, 249, 249, 0.3)";

        const elements = document.querySelectorAll(`.selection-indicator`);
        elements.forEach(element => {
            element.parentNode.removeChild(element);
        });
        console.log(elements);
    }
    else{
        selectButton.style.backgroundColor = "rgba(249, 249, 249, 1)";
    }
    selectionMode = !selectionMode;
}

const selectButton = document.querySelector(".select-button");
selectButton.addEventListener('click',select);

let download = () => {
    fetch(`/File/DownloadFile/${filename}`)
    .then(response => response.blob())
    .then(blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
    })
    .catch(err => console.error('Error downloading file:', err));
};
const downloadButton = document.getElementById('downloadButton');
downloadButton.addEventListener('click',download);

const exitButton = document.getElementById('exitButton');
exitButton.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
});

function deleteImage() {
    try {
        selectedImages.map(image => fetch(`/File/DeleteFileAsync/${image}`));
        GetFiles();

    } catch (error) {
        console.error('Error fetching URLs:', error.message);
    }
};

async function saveImage() {
    try {
        selectedImages.map(image => fetch(`/File/DownloadFile/${image}`));
        await GetFiles();
    } catch (error) {
        console.error('Error fetching URLs:', error.message);
    }
};
// const deleteButton = document.getElementById('deleteButton');
// deleteButton.addEventListener('click',deleteImage);

// const saveButton = document.getElementById('saveButton');
// saveButton.addEventListener('click',saveImage);

// const refreshButton = document.getElementById('Refresh');
// refreshButton.addEventListener('click',GetFiles);

//#endregion 
