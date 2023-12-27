$(document).ready(function () {
    GetPrefix();
    GetManagerList();
    GetDepartmentList();
    GetRoleList()
});
function CheckTextField() {
    let EmptyNotificationText = "Field(s) : ";
    let InvalidNotificationText = "Field(s) : ";
    let emptyFlag = false;
    let invalidFlag = false;
    let passwordFlag = false;
    let notificationText = document.getElementById("notificationText").innerHTML;
    let firstName = document.getElementById("FirstNameFieldId").value;
    let lastName = document.getElementById("LastNameFieldId").value;
    let nationalIdentificationNumber = document.getElementById("NationalIdentificationNumberFieldId").value;
    let mobileNumber = document.getElementById("MobileNumberFieldId").value;
    let email = document.getElementById("EmailFieldId").value;

    let department = document.getElementById("DepartmentComboBoxId").value;
    let manager = document.getElementById("ManagerComboBoxId").value;
    let role = document.getElementById("RoleComboBoxId").value;
    let password = document.getElementById("PasswordFieldId").value;
    let confirmPassword = document.getElementById("ConfirmPasswordFieldId").value;

    if (firstName == "") { EmptyNotificationText += "First name ,"; emptyFlag = true };
    if (lastName == "") { EmptyNotificationText += "Last name ,"; emptyFlag = true };
    if (nationalIdentificationNumber == "") { EmptyNotificationText += "National identification number ,"; flag = true };
    if (mobileNumber.toString().split(" ")[1].length == 0) { EmptyNotificationText += "Mobile number ,"; emptyFlag = true };
    if (email == "") { EmptyNotificationText += "email ,"; emptyFlag = true };
    if (department == "none") { EmptyNotificationText += "department ,"; emptyFlag = true };
    if (role == "none") { EmptyNotificationText += "role ,"; emptyFlag = true }
    if (password == "") { EmptyNotificationText += "password ,"; emptyFlag = true };
    

    if (nationalIdentificationNumber.toString().length != 14)
    { InvalidNotificationText += "National identification number ,"; invalidFlag = true };
    let mobileNumberlenght = mobileNumber.toString().split(" ")[1].length;
    if (mobileNumberlenght != 8)
    { InvalidNotificationText += "Mobile number ,"; invalidFlag = true };
    if (!email.includes("@") && !email.includes(".com"))
    { InvalidNotificationText += "email ,"; invalidFlag = true };
    
    if (confirmPassword != password) { passwordFlag = true };

    if (emptyFlag) {
        EmptyNotificationText += "are mandatory";
        document.getElementById("notificationText").innerHTML = EmptyNotificationText;
        document.getElementById("notificationText").style.visibility = "visible";
    }
    else if (invalidFlag) {
        InvalidNotificationText += "are invalid";
        document.getElementById("notificationText").innerHTML = InvalidNotificationText;
        document.getElementById("notificationText").style.visibility = "visible";
    }
    else if (passwordFlag) {
        document.getElementById("notificationText").innerHTML = "Passwords does not correspond!";
    }
    else if (!emptyFlag && !invalidFlag && !passwordFlag) { Register() }
}
function GetDepartmentList() {
    $.ajax({
        type: "GET",
        url: "/Department/GetDepartmentList",
        success: function (result) {
            if (result.message == "success") {
                let managerCombobox = document.getElementById("DepartmentComboBoxId");
                result.data.forEach(function (row) {
                    let option = document.createElement("option");
                    option.value = row.DepartmentId;
                    option.text = row.DepartmentName;
                    managerCombobox.add(option);
                })
            }
            else {
                ShowNotification("Error", result.data);
            }
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    });
};
function GetManagerList() {
    $.ajax({
        type: "GET",
        url: "/Account/GetManagerList",
        success: function (result) {
            if (result.message == "success") {
                let managerCombobox = document.getElementById("ManagerComboBoxId");
                result.data.forEach(function (managerData) {
                    let option = document.createElement("option");
                    option.value = managerData.Value;
                    option.text = managerData.Fullname;
                    managerCombobox.add(option);
                })
            }
            else {
                ShowNotification("Error", result.data);
            }
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    });
};
function GetRoleList() {
    $.ajax({
        type: "GET",
        url: "/Role/GetRoleList",
        success: function (result) {
            if (result.message == "success") {
                let managerCombobox = document.getElementById("RoleComboBoxId");
                result.data.forEach(function (row) {
                    let option = document.createElement("option");
                    option.value = row.RoleId;
                    option.text = row.RoleName;
                    managerCombobox.add(option);
                })
            }
            else {
                ShowNotification("Error", result.data);
            }
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    });
};

function GetPrefix(){
    var countryData = window.intlTelInputGlobals.getCountryData(),
        input = document.querySelector("#MobileNumberFieldId"),
        addressDropdown = document.querySelector("#country");

    var iti = window.intlTelInput(input, {
        hiddenInput: "full_phone",
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js"
    });
    for (var i = 0; i < countryData.length; i++) {
        var country = countryData[i];
        var optionNode = document.createElement("option");
        optionNode.value = country.iso2;
        var textNode = document.createTextNode(country.name);
        optionNode.appendChild(textNode);
        addressDropdown.appendChild(optionNode);
    }
    addressDropdown.value = iti.getSelectedCountryData().iso2;
    input.addEventListener('countrychange', function (e) {
        addressDropdown.value = iti.getSelectedCountryData().iso2;
    });
    addressDropdown.addEventListener('change', function () {
        iti.setCountry(this.value);
    });
    input.value = '+1 ';
}
function Register() {
    var userDetails = {
        FirstName: document.getElementById("FirstNameFieldId").value,
        OtherName: document.getElementById("OtherNameFieldId").value,
        LastName: document.getElementById("LastNameFieldId").value,
        NationalIdentificationNumber: document.getElementById("NationalIdentificationNumberFieldId").value,
        MobileNumber: document.getElementById("MobileNumberFieldId").value,
        Email: document.getElementById("EmailFieldId").value,
        DepartmentId: document.getElementById("DepartmentComboBoxId").value,
        ManagerId: document.getElementById("ManagerComboBoxId").value,
        RoleId: document.getElementById("RoleComboBoxId").value,
        Password: document.getElementById("PasswordFieldId").value
    }
    $.ajax({
        type: 'POST',
        url: "/Account/RegisterUser",
        data: userDetails,
        success: function (result) {
            if (result.message == "Success") {
                ShowNotification("Success", "Successfully registered user")
                window.location.href = '/Account/RedirectToView';
            }
            else {
                document.getElementById("notificationText").innerHTML = result.data;
            }
        },
        error: function (error) {
            document.getElementById("notificationId").textContext = "Communication has been interupted";
        }
    });
}