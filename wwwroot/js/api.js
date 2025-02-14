async function POST(url,data) {
    await fetch(url, {
            method: "POST", 
            body: data
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response;
        })
        .catch(error => {
            console.log("An error occurred while sending the data.");
        });
}

function AJAX(url, method, data, callback) {
    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            if (typeof callback == 'function') {
                callback(this);
            }
        }
    }

    xhr.open(method, url);
    if (method == 'POST' && !(data instanceof FormData)) {
        xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
    }
    xhr.send(data);
}

export { POST, AJAX };