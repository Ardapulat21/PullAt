let getRequest = (url,callback) => {
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json(); // Parse JSON data
        })
        .then(callback)
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
};

let postRequest = (url,data,callback) => {
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(callback)
    .catch(error => {
        console.error('Error:', error);
    });
};

export { getRequest, postRequest};