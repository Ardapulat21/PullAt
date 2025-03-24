import { appendImageElement } from "./dom.js";
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

let uploadFile = async (event,endpoint) => {
    const file = event.target.files[0];
    if(!file) return;

    const formData = new FormData();
    formData.append("file", file);
    await fetch(
        path,
        {
            method: 'POST', 
            body: formData
        })
        .then(refreshGallery);

    event.target.value = '';
}

const fileGrid = document.querySelector(".file-grid");
async function clearGallery() {
    fileGrid.innerHTML = ""; 
}

async function refreshGallery() {
    await clearGallery();
    await fetch('/File/GetFiles')
    .then(response => response.json())
    .then(data => {
        data.map(item => {
            appendImageElement(fileGrid,item);
        })
    })
    .catch(error => console.log(error));
}

export {download ,uploadFile ,refreshGallery};