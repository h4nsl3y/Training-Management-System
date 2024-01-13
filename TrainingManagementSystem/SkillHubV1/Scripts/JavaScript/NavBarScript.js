function LogOutUser() {
    $.ajax({
        type: 'POST',
        url: "/Account/LogUserOut",
        success: function (result) {
            if (result.Success == true) {
                window.location.href = '/Account/LogInPage';
            }
            else { ShowNotification(false, "Error", "Failed to log out"); }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
}
function CreateNotification(accountId, enrollmentState, trainingTitle, comment, email) {
    switch (enrollmentState) {
        case 3:
            subject = "Approval";
            body = `Your request for the training :${trainingTitle} has been approved by your manager`;
            break;
        case 4:
            subject = "Cancelled";
            body = `Your request for the training :${trainingTitle} has been cancelled`;
            break;
        case 5:
            subject = "Confirmation";
            body = `Your enrollment reguarding the training :${trainingTitle} has been confirmed.`;
            break;
        default:
            subject = "Rejection";
            body = `Your request for the training :${trainingTitle} has been rejected due to :\n '${comment}'`;
            break;
    }
    let notification = { AccountId: accountId, Subject: subject, Body: body};
    $.ajax({
        type: "POST",
        url: "/Notification/AddNotification",
        data: { notification: notification, email: email},
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    })
}
function GetMessage() {
    let messageBox = document.getElementById("messageBoxId")
    if (messageBox.style.visibility == "visible") {
        messageBox.style.visibility = "hidden"
        document.getElementsByName("messageContainer").forEach((element) => element.remove())
    }
    else {
        refreshMessageBox();
    }
}

function refreshMessageBox() {
    let messageBox = document.getElementById("messageBoxId")
    document.getElementsByName("messageContainer").forEach((element) => element.remove())
    $.ajax({
        type: "GET",
        url: "/Notification/GetNotification",
        success: function (result) {
            if (result.Success == true) {
                if (result.Data.length == 0) {
                    result.Data.push({ NotificationId: 0, Subject: "-", Body: "No message", HasRead: true })
                }
                result.Data.forEach((message) => {
                    messageContainer = document.createElement("div");
                    messageContainer.setAttribute("id", message.NotificationId)
                    messageContainer.setAttribute("name", "messageContainer")
                    messageContainer.setAttribute("class", "message-container")
                    messageContainer.setAttribute("onclick", `UpdateMessageState(${message.NotificationId})`)

                    messageTitle = document.createElement("p");
                    messageTitle.classList.add("message-title");
                    messageTitle.id = "messageTitle";
                    messageTitle.textContent = message.Subject;

                    messageBody = document.createElement("p");
                    messageBody.classList.add("message-text");
                    messageBody.id = "MessageBody";
                    messageBody.textContent = message.Body;

                    if (message.HasRead == false) {
                        messageTitle.classList.add("bold-text");
                        messageBody.classList.add("bold-text");
                    }
                    messageContainer.appendChild(messageTitle);
                    messageContainer.appendChild(messageBody);
                    messageBox.appendChild(messageContainer);
                })
                messageBox.style.visibility = "visible";
            }
            else { ShowNotification(false, "Error", "Failed to retrieve messages"); }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
}
function UpdateMessageState(notificationId) {
    $.ajax({
        type: "POST",
        url: "/Notification/UpdateNotificationState",
        data: { notificationId: notificationId },
        success: function (result) {
            refreshMessageBox();
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    })
}