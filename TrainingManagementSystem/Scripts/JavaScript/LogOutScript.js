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