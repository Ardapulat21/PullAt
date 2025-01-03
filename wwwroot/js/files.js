//#region Expandable Button
var button = document.getElementById("expandableButton");
var contents = document.getElementsByClassName("content");
var icon = document.getElementById("icon");
button.addEventListener("click",function(){
    if(icon.classList.contains("bi-caret-down")){
        icon.classList.remove("bi-caret-down");
        icon.classList.add("bi-caret-up");
    }
    else{
        icon.classList.remove("bi-caret-up");
        icon.classList.add("bi-caret-down");
    }
    for(var i = 0;i < contents.length; i++){
        var content = contents[i];
        if(content.style.maxHeight){
            content.style.maxHeight = null;
            expandableButton.style.borderRadius = "30px";
            content.style.padding = "0px 15px";
        }
        else{
            expandableButton.style.borderRadius = "0px";
            content.style.padding = "5px 15px";
            content.style.maxHeight = content.scrollHeight + "px";
            content.style.borderWidth = "1px";
        }
    }
});
const fileInput = document.getElementById("fileInput");
fileInput.addEventListener("change", function (event) {
    const file = event.target.files[0];
    if (file) {
        const formData = new FormData();
        formData.append("file", file);
        fetch("/File/UploadFile", {
            method: "POST", 
            body: formData
        })
        .then(response => {
            if (response.ok) {
                console.log("File uploaded successfully.");
                GetFiles();
            } else {
                console.log("File upload failed.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            console.log("An error occurred while uploading the file.");
        });
    }
});
//#endregion
//#region AJAX
let GetFiles = () => {
    const fileGrid = document.querySelector(".file-grid");
    fileGrid.innerHTML = ""; 
    $.ajax({
        type: "GET",
        url: "/File/GetFiles",
        dataType: "json",
        success: (response) => {
            Object.keys(response).forEach(key => {
                var file = response[key];
                const fileItem = document.createElement("div");
                fileItem.classList.add("file-item");
                
                const img = document.createElement("img");
                img.name = file.filename;
                img.src = file.filePath;
                img.classList.add("gallery-item");

                fileItem.appendChild(img);
                fileGrid.appendChild(fileItem);
            });
        },
        error: (xhr,status,error) => {
            console.log(error);
        }
    });
}
//#endregion AJAX
//#region Overlay
let displayImage = (event) => {
    const clickedImg = event.target.closest('.file-item');
    if (clickedImg) {
        const childImage = clickedImg.querySelector('.gallery-item');
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

const imageContainer = document.getElementById('imageContainer');
imageContainer.addEventListener('click',() => {
    overlayContainer.style.display = 'none';
});
//#endregion Overlay
//#region Buttons
let selectButton = () => {
    alert("hr");
}

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
//#endregion 
