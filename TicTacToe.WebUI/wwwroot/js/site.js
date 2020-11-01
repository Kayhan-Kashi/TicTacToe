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
var openSocket = function (parameter, action) {
    if (interval !== null) clearInterval(interval);

    var protocol = location.protocol === "https:" ? "wss:" : "ws:";
    var wsUri ="";
    var operation = "";

    if (action === "Email") {
        wsUri = protocol + "//" + window.location.host + "/Registration/CheckEmailConfirmationStatus";
        operation = "CheckEmailConfirmationStatus";
    }

    var socket = new WebSocket(wsUri);

    socket.onmessage = function (response) {
        console.log(response);
        if (action === "Email" && response.data === "OK") {
            window.location.href = "../GameInvitation?email=" + parameter
        }
    };

    socket.onopen = function () {
        var json = JSON.stringify({ "Operation": operation, "Parameters": parameter });
        socket.send(json);
    }
};


function checkEmailConfirmationStatus(email) {
    $.get("../CheckEmailConfirmationStatus?email=" + email, function (data) {
        if (data === "OK") {
            if (interval !== null) 
                clearInterval(interval);
            window.location.href = "../GameInvitation/Index?email=" + email;            
        }
    })
}