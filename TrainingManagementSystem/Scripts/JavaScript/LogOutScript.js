function LogOutUser() {
    $.ajax({
        type: 'POST',
        url: "/Account/LogUserOut",
        success: function (result) {
            if (result.message == "Success") {
                window.location.href = '/Account/LogInPage';
            }
            else { ShowNotification("Error", "Failed to log out"); }
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    });
}