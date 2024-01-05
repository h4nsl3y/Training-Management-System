$(window).on('load', function () {
    let roleId = document.getElementById("roleSelectorContainer").attributes.value.value;
    GetActualRole(roleId);
});

function GetRoleList(roleId) {
    $.ajax({
        type: "GET",
        url: "/Role/GetRoleList",
        success: function (result) {
            if (result.Success == true) {
                let roleSelector = document.getElementById("roleSelectorId");
                result.Data.forEach(function (role) {
                    if (role.RoleId <= roleId) {

                        let optionArea = document.createElement("div")

                        let option = document.createElement("input");
                        option.type = 'radio';
                        option.name = 'Role';
                        option.value = role.RoleId;

                        let optionLabel = document.createElement("label");
                        optionLabel.innerHTML = role.RoleName

                        optionArea.appendChild(option);
                        optionArea.appendChild(optionLabel);
                        roleSelector.appendChild(optionArea);
                    }
                })
            }
            else {
                ShowNotification(false, "Error", result.Message);
            }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
};
function GetActualRole(roleId) {
    if (roleId == 1) { window.location.href = '/Home/EmployeeViewPage'; }
    else { GetRoleList(roleId) }
}
function SetRole() {
    let roleId = document.querySelector('input[name="Role"]:checked').value;
    $.ajax({
        type: "POST",
        url: "/Account/SetRole",
        data: { roleId: roleId},
        success: function (result) {
            if (result.Success == true) {
                ShowNotification(true, "Success", "Role has been set");
                if (roleId == "1") { window.location.href = "/Home/EmployeeViewPage" }
                if (roleId == "2") { window.location.href = "/Home/ManagerViewPage" }
                if (roleId == "3") { window.location.href = "/Home/AdministratorViewPage" }
            }
            else {
                ShowNotification(false, "Error", result.Message);
            }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
}


