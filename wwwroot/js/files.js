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
    var deleteButton = document.getElementById("deleteButton");
    deleteButton.addEventListener("click",function(){
        alert("Delete button was clicked");
    });
     document.getElementById("fileInput").addEventListener("change", function (event) {
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