import { uploadFile} from "../filehandler.js";

const input = document.getElementById('imageInput');
input.addEventListener('change',(event) => {
    uploadFile(event,'/Account/ChangeProfilePhoto',
        (data) => {
            if (data.success) {
                document.getElementById('profile-picture').src = data.relativePath + "?t=" + new Date().getTime();
            } else {
                console.log("Error updating profile photo.");
            }
        }
);
});

const changeProfilePhoto = document.getElementById('change-profile-photo-anchor');
changeProfilePhoto.addEventListener('click',(event) => {
    event.preventDefault();
    input.click();
});