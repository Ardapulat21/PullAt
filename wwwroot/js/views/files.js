import { toggleMenu, displayImage , mouseOverImage , mouseOutImage , selectImage} from "../dom.js";
import { download, refreshGallery, uploadFile} from "../filehandler.js";

const selectionData = {
    filename: '',
    mode: false,
    images: [],
};

var button = document.getElementById("file-menu-button");
button.addEventListener("click",toggleMenu);

const fileInput = document.getElementById("fileInput");
fileInput.addEventListener("change",(event) => {
    const file = event.target.files[0];
    uploadFile(file, '/File/UploadFile', (err, result) => {
        if (err) {
            console.error("Upload error:", err.message);
        } else {
            console.log("Upload successful:", result);
            refreshGallery(); 
        }
    });
});

const uploadButton = document.getElementById("uploadButton");
uploadButton.addEventListener("click", () => {
    fileInput.click();
});

const fileGrid = document.querySelector('.file-grid');

fileGrid.addEventListener('click',(event) => {
    if (event.target.classList.contains('gallery-item')) {
        displayImage(event);
    }
});

fileGrid.addEventListener('mouseover',(event) => {
    if (event.target.classList.contains('gallery-item')) {
        mouseOverImage(event);
    }
});

fileGrid.addEventListener('mouseout',(event) => {
    if (event.target.classList.contains('file-item')) {
        mouseOutImage(event);
    }
});

document.querySelector('.gallery').addEventListener('click', (event) => {selectImage(event,selectionData)});

const overlayContainer = document.querySelector('.overlay-container');

const imageContainer = document.getElementById('imageContainer');
imageContainer.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
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
            await fetch(`/File/DeleteFileAsync/${image}`,{
                method: 'DELETE'
            });
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
