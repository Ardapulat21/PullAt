import { uploadFile} from "../filehandler.js";

const input = document.getElementById('imageInput');
input.addEventListener('change',(event) => {
    uploadFile(event,'/Account/ChangeProfilePhoto')
});

const form = document.getElementById('imageForm');

const changeProfilePhoto = document.getElementById('change-profile-photo-anchor');
changeProfilePhoto.addEventListener('click',(event) => {
    event.preventDefault();
    input.click();
});