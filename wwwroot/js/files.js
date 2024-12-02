// var button = document.getElementById("expandableButton");
// var contents = document.getElementsByClassName("content");
// var icon = document.getElementById("icon");
// button.addEventListener("click",function(){
//     if(icon.classList.contains("bi-caret-down")){
//         icon.classList.remove("bi-caret-down");
//         icon.classList.add("bi-caret-up");
//     }
//     else{
//         icon.classList.remove("bi-caret-up");
//         icon.classList.add("bi-caret-down");
//     }
//     for(var i = 0;i < contents.length; i++){
//         var content = contents[i];
//         if(content.style.maxHeight){
//             content.style.maxHeight = null;
//             expandableButton.style.borderRadius = "30px";
//             content.style.minHeight = null;
//         }
//         else{
//             expandableButton.style.borderRadius = "0px";
//             content.style.padding = "5px 15px";
//             content.style.maxHeight = content.scrollHeight + "px";
//             content.style.borderWidth = "1px";
//         }
//     }
// });

// var uploadButton = document.getElementById("uploadButton");
// var deleteButton = document.getElementById("deleteButton");
// uploadButton.addEventListener("click",function(){

//     alert("Upload button was clicked");
// });
// deleteButton.addEventListener("click",function(){
//     alert("Delete button was clicked");
// });