var interval;

function emailConfirmation(email) {
    if (window.WebSocket) {
        alert("Websockets are enabled");
        openSocket(email, "Email");
    }
    else {
        alert("Websockets are not enabled");
        interval = setInterval(() => {
            checkEmailConfirmationStatus(email)
        }, 5000);  
    } 
}