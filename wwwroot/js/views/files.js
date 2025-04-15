import { toggleMenu, displayImage, selectionModeToggle , mouseOverImage , mouseOutImage , selectImage} from "../dom.js";
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

    uploadFile(file, '/File/UploadFile/', (err, result) => {
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

const galleryItems = document.getElementsByClassName('gallery-item');

[...galleryItems].forEach(item => {
    item.addEventListener('click',(event) => {
        displayImage(event,selectionData);
    });
});

const fileItems = document.getElementsByClassName('file-item');

[...fileItems].forEach(item => {
    item.addEventListener('mouseover',(event) => {
        mouseOverImage(event);
    });
    item.addEventListener('mouseleave',mouseOutImage);
});

document.querySelector('.gallery').addEventListener('click', (event) => {selectImage(event,selectionData)});

const overlayContainer = document.querySelector('.overlay-container');

const imageContainer = document.getElementById('imageContainer');
imageContainer.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
});

// const selectButton = document.querySelector(".select-button");
// selectButton.addEventListener('click',(event) => {
//     selectionModeToggle(selectionData);
// });

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
