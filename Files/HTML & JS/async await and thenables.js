// aysnc await code (begins)

// async function getData() {
//   const response = await fetch('https://jsonplaceholder.typicode.com/todos/1');
//   const json = await response.json();
//   console.log(json);
// }

// async function test() {
//   await getData();
//   console.log("done");
// }

// test();


// aysnc await code (ends)

// thenables code method 1 (begins)

// function getData() {
//   return fetch('https://jsonplaceholder.typicode.com/todos/1')
//     .then(response => response.json())
//     .then(json => console.log(json));
// }

// function test() {
//   getData().then(() => {
//     console.log("done");
//   });
// }

// test();

// thenables code method 1 (ends)

// thenables code method 2 (begins)

function getData() {
  fetch('https://jsonplaceholder.typicode.com/todos/1')
    .then(function (response) {
        if (!response.ok) {
            stringResponseErrorMessage = "Response was not OK" + "<br>"
            /*stringResponseErrorMessage += "Status Code : " + response.status + "<br>"*/

            if (response.status == 301)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Redirection Responses </b><br> Moved Permanently: The requested resource has been permanently moved to a new location." + "<br>"

            if (response.status == 302)
                stringResponseErrorMessage += "Status : " + response.status + "<br ><b> Redirection Responses </b><br> Found (or 303 See Other): The requested resource is temporarily located at a different URI." + "<br>"

            if (response.status == 400)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Client Error Responses </b> <br> Bad Request: The request cannot be processed due to a client error (e.g., malformed request)." + "<br>"

            if (response.status == 401)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Client Error Responses </b> <br> Unauthorized: The client needs to authenticate to get access." + "<br>"

            if (response.status == 403)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Client Error Responses </b> <br> Forbidden: The client does not have permission to access the requested resource." + "<br>"

            if (response.status == 404)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Client Error Responses </b> <br> Not Found: The requested resource could not be found on the server." + "<br>"

            if (response.status == 500)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Server Error Responses </b> <br> Internal Server Error: The server encountered an error and could not fulfill the request." + "<br>"

            if (response.status == 502)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Server Error Responses </b> <br> Bad Gateway: The server, while acting as a gateway or proxy, received an invalid response from an upstream server." + "<br>"

            if (response.status == 503)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Server Error Responses </b> <br> Service Unavailable: The server is currently unable to handle the request due to temporary overloading or maintenance of the server." + "<br>"

            if (response.statusText.length > 0)
                stringResponseErrorMessage += "Status : " + response.status + "<br> <b> Server Error Responses </b> <br>" + "<br>"

            stringResponseErrorMessage += "API URL : " + response.url + "<br>"
            stringResponseErrorMessage += "<br><br>"

            throw new Error(stringResponseErrorMessage)
        }
        return response.json();
    })
    .then(function (objResult) {
        console.log(objResult);
    }).catch(objError =>
        alert(objError.stack)
    );
}

getData()
// thenables code method 2 (ends)