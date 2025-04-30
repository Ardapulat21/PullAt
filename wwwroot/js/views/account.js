import { uploadFile} from "../filehandler.js";

const input = document.getElementById('imageInput');
input.addEventListener('change',(event) => {
    const file = event.target.files[0];
    uploadFile(file,'/Account/ChangeProfilePhoto',
        (err, result) => {
            if (err) {
                console.error("Upload error:", err.message);
            } else {
                document.getElementById('profile-picture').src = result.relativePath + "?t=" + new Date().getTime();
                console.log("Upload successful:", result);
            }
        }
);
});

const changeProfilePhoto = document.getElementById('change-profile-photo-anchor');
changeProfilePhoto.addEventListener('click',(event) => {
    event.preventDefault();
    input.click();
});