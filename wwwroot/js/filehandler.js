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

function uploadFile(file, endpoint, callback) {
    const xhr = new XMLHttpRequest();
    const formData = new FormData();
    formData.append("file", file);

    xhr.onload = function () {
        if (xhr.status >= 200 && xhr.status < 300) {
            try {
                const response = JSON.parse(xhr.responseText);
                callback(null, response); // success
            } catch (e) {
                callback(null, xhr.responseText); // fallback to raw text
            }
        } else {
            callback(new Error("Upload failed with status: " + xhr.status));
        }
    };

    xhr.onerror = function () {
        callback(new Error("Upload failed due to a network error"));
    };

    xhr.timeout = 5000;
    xhr.ontimeout = function () {
        callback(new Error("Upload timed out"));
    };

    xhr.open("POST", endpoint);
    xhr.send(formData);
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