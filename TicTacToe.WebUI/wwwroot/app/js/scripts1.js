var interval;

function emailConfirmation(email) {
    interval = setInterval(() => {
        checkEmailConfirmationStatus(email)
    }, 5000);    
}