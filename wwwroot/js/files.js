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

const galleryImages = document.querySelectorAll('.gallery-item');
const imageOverlay = document.querySelector('.image-overlay');
const img = document.querySelector('.img');
const title = document.querySelector('.title');
var filename;
galleryImages.forEach(function(image) {
    image.addEventListener('click', function() {
        const imagePath = this.getAttribute('src');
        img.src = imagePath;
        filename = this.getAttribute('name');
        title.textContent =  filename;
        imageOverlay.style.display = 'block';
    });
});

const imageContainer = document.getElementById('imageContainer');
imageContainer.addEventListener('click',function() {
    imageOverlay.style.display = 'none';
});

const downloadButton = document.getElementById('downloadButton');
downloadButton.addEventListener('click',function() {
    console.log(`${filename}`);
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
});

const exitButton = document.getElementById('exitButton');
exitButton.addEventListener('click',function() {
    imageOverlay.style.display = 'none';
});