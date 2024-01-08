function ShowNotification(isPositive, title ,message) {  

    notificationDiv = document.createElement("div");
    notificationDiv.classList.add("notification-container");
    notificationDiv.id = "notification";

    notificationTitle = document.createElement("p");
    notificationTitle.classList.add("notification-title");
    notificationTitle.id = "notificationTitle";
    notificationTitle.textContent = title;

    notificationMessage = document.createElement("p"); 
    notificationMessage.classList.add("notification-text");
    notificationMessage.id = "notificationMessage";
    notificationMessage.textContent = message

    if (isPositive == true) { notificationDiv.style.backgroundColor = "#3982e8" }
    else { notificationDiv.style.backgroundColor = "#e83939" }

    notificationDiv.appendChild(notificationTitle);
    notificationDiv.appendChild(notificationMessage);

    var body = document.getElementsByTagName("BODY")[0];
    body.appendChild(notificationDiv);
    
    setTimeout(function () {
        notificationDiv.remove();
    }, 5000);
}