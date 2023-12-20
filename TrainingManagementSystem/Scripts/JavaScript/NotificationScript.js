function ShowNotification(title ,message) {  
    let notification = document.getElementById("notification");
    let notifTitle = document.getElementById("notificationTitle");
    let notifMessage = document.getElementById("notificationMessage");
    notifMessage.textContent = message;
    notifTitle.textContent = title;
    if (title == "Success") { notification.style.backgroundColor = "rgb(127,255,0)" }
    else { notification.style.backgroundColor = "rgb(240, 240, 125)" }
    notification.style.visibility = "visible";
    setTimeout(function () {
        notification.style.visibility = "hidden";
        notifMessage = null;
    }, 5000);
}