import { POST } from "./api.js";
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

let uploadFile = async (event) => {
    const file = event.target.files[0];
    if(!file) return;

    const formData = new FormData();
    formData.append("file", file);
    await POST(
        "/File/UploadFile",formData)
        .then(refresh);
    event.target.value = '';
}
const fileGrid = document.getElementById('file-grid');
const refresh = () => {
    fetch('/File/GetFiles')
    .then(response => response.json())
    .then(data => {
        const fileGrid = document.querySelector(".file-grid");
        fileGrid.innerHTML = "";
        data.forEach(item => {
            console.log(item);
            appendImageElement(fileGrid,item);
            // const fileItem = document.createElement('div');
            // fileItem.className = "file-item";
            // fileItem.innerHTML = `<img class="gallery-item" name="${item.filename}" src="${item.filePath}"/>`;
            // fileGrid.appendChild(fileItem);
        });
    });
}

export {download ,uploadFile ,refresh };