 async function GET(url) {
    await fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response;
        })
        .catch(error => {
            console.log(`There was a problem with the fetch operation: ${error}`);
        });
};

async function POST(url,data) {
    await fetch(url, {
            method: "POST", 
            body: data
        })
        .then(response => {
            if (response.ok) {
                return response;
            }
        })
        .catch(error => {
            console.error("Error:", error);
            console.log("An error occurred while sending the data.");
        });
}

export { GET, POST };