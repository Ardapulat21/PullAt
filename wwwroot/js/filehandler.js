import { appendImageElement } from "./dom.js";
import { POST ,AJAX } from "./api.js";

let download = (filename) => {
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

let uploadFile = async (event) => {
    const file = event.target.files[0];
    if(!file) return;

    const formData = new FormData();
    formData.append("file", file);
    await POST(
        "/File/UploadFile",formData)
        .then(refreshGallery);
    event.target.value = '';
}

let refreshGallery = () => {
    const fileGrid = document.querySelector(".file-grid");
    fileGrid.innerHTML = ""; 
    AJAX('/File/GetFiles','GET',null,(response) => {
        const json = response.response;
        let obj = JSON.parse(json);
        obj.forEach(file => {
            appendImageElement(fileGrid,file);
        });
    });
}

export {download ,uploadFile ,refreshGallery};