import { GET, POST , AJAX } from "./Api/api.js";
import { addIfNotExist} from "./Utils/Utils.js";

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
//#region Buttons

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
//#endregion 
