﻿function checkEmailConfirmationStatus(email) {
    $.get("../CheckEmailConfirmationStatus?email=" + email, function (data) {
        if (data === "OK") {
            if (interval !== null) 
                clearInterval(interval);
            window.location.href = "../GameInvitation/Index?email=" + email;            
        }
    })
}