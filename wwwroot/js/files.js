import { GET, POST , AJAX } from "./Api/api.js";

function deleteImage() {
    try {
        selectedImages.map(image => fetch(`/File/DeleteFileAsync/${image}`));
        GetFiles();

    } catch (error) {
        console.error('Error fetching URLs:', error.message);
    }
};
async function saveImage() {
    try {
        selectedImages.map(image => fetch(`/File/DownloadFile/${image}`));
        await GetFiles();
    } catch (error) {
        console.error('Error fetching URLs:', error.message);
    }
};