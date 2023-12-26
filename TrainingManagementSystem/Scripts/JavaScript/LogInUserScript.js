function CheckTextField() {
    var notificationText = "Field(s) (";
    var flag = true;
    if (document.getElementById("employeeEmailId").value == "") { notificationText = notificationText.concat("Email ,"); flag = false };
    if (document.getElementById("employeePasswordId").value == "") { notificationText = notificationText.concat("Password "); flag = false };
    notificationText = notificationText.concat(") are mandatory")
    if (flag) { LoginUser() }
    else { ShowNotification("Error", notificationText); }
}
function LoginUser() {
    var data = {
        Email: document.getElementById("employeeEmailId").value,
        Password: document.getElementById("employeePasswordId").value
    }
    $.ajax({
        type: 'POST',
        url: "/Account/AuthenticateUser",
        data: data,
        success: function (result) {
            if (result.message == "Success") {
                ShowNotification("Success", "sucessfully log in user")
                window.location.href = '/Account/RedirectToView'; 
            }
            else {
                document.getElementById("notificationText").innerHTML = result.data;
            }
        },
        error: function (error) {
            console.log(error)
            ShowNotification("Error", "Communication has been interupted") ;
        }
    });
}
function RegisterUser() { window.location.href = '/Account/RegisterPage'; }