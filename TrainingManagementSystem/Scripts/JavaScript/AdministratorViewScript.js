$(window).on('load', function () {
    HideTab(),
    GetDepartmentList(),
    GetPrerequisiteDataList()
});
// GLOBAL VARIABLES
let rowCount = 0
let departmentList = [];

//#region ComboboxList
function AddDisplayPrerequisite() {
    let body = document.getElementById("prerequisiteRowId");

    let comboBoxElement = document.createElement("select");
    comboBoxElement.setAttribute("id", "trainingPrerequisiteDetailId-" + rowCount);
    comboBoxElement.setAttribute("name", "PrerequisiteField");
    comboBoxElement.setAttribute("class", "input-combo-box");

    let buttonElement = document.createElement("button");
    buttonElement.setAttribute("id", "removePrerequisiteBtnId-" + rowCount);
    buttonElement.setAttribute("name", "PrerequisiteFieldButton");
    buttonElement.setAttribute("class", "item-button");
    buttonElement.setAttribute("type", "button");
    buttonElement.setAttribute("onclick", "RemoveDisplayPrerequisite(" + rowCount + ");");
    buttonElement.textContent = "Remove";

    body.appendChild(comboBoxElement);
    body.appendChild(buttonElement);
    GetPrerequisiteList(rowCount);
    rowCount += 1;
}
function GetDepartmentList() {
    $.ajax({
        type: "GET",
        url: "/Department/GetDepartmentList",
        success: function (result) {
            if (result.Success == true) {
                let combobox = document.getElementById("trainingDepartmentId");
                result.Data.forEach(function (row) {
                    let option = document.createElement("option");
                    option.value = row.DepartmentId;
                    option.text = row.DepartmentName;
                    combobox.add(option);
                })
                GetTrainingList(result.Data)
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
function GetPrerequisiteList(rowId) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetAllPrerequisite",
        success: function (result) {
            if (result.Success == true) {
                let combobox = document.getElementById("trainingPrerequisiteDetailId-" + rowId);
                result.Data.forEach(function (row) {
                    let option = document.createElement("option");
                    option.value = row.PrerequisiteId;
                    option.text = row.PrerequisiteDescription;
                    combobox.add(option);
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
//#endregion

//#region Form

//#region Training
function OpenTrainingCreationForm() {
    let overlay = document.getElementById("screenOverlay");
    overlay.style.visibility = "visible";
};
function CloseTrainingCreationForm() {
    document.getElementById("screenOverlay").style.visibility = "hidden";
    document.getElementById("submitTrainingDetailsBtn").style.visibility = "hidden";
    document.getElementById("notificationText").style.visibility = "hidden";
    document.getElementById("notificationText").innerHTML = "";
    const elements = document.querySelectorAll('[name="PrerequisiteField"]');
    elements.forEach(function (element) {
        element.remove();
    });
    const elementButtons = document.querySelectorAll('[name="PrerequisiteFieldButton"]');
    elementButtons.forEach(function (element) {
        element.remove();
    })
}
function FillTrainingDetail(trainingId) {
    $.ajax({
        type: "GET",
        url: "/Training/GetTraining",
        data: { trainingId: trainingId },
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                let training = result.Data[0];

                document.getElementById("submitTrainingDetailsBtn").style.visibility = "visible";
                document.getElementById("submitTrainingDetailsBtn").textContent = "Update";

                document.getElementById("submitTrainingDetailsBtn").setAttribute("onclick", `UpdateTraining( ${training.TrainingId});`);
                document.getElementById("trainingTitleId").value = training.Title;
                document.getElementById("trainingDepartmentId").value = training.DepartmentId;
                document.getElementById("trainingStartDateId").value = ConvertToDateString(training.StartDate);
                document.getElementById("trainingEndDateId").value = ConvertToDateString(training.EndDate);
                document.getElementById("trainingDeadLineId").value = ConvertToDateString(training.Deadline).split(" ")[0];
                document.getElementById("trainingSeatAvailableId").value = training.SeatNumber;
                document.getElementById("trainingShortDescriptionId").value = training.ShortDescription;
                document.getElementById("trainingLongDescriptionId").value = training.LongDescription;
                UpdateDisplayPrerequisite(training.TrainingId);
            }
            else {
                ShowNotification(false ,'Error', "Some error has been encountered while loading the training details");
            }
        },
        error: function () {
            ShowNotification(false ,'Error', "Some error has been encountered")
        }
    });
};
function RegisterTraining() {
    let index = 0;
    let prerequisiteList = {};
    let prerequisiteIds = document.getElementsByName("PrerequisiteField");
    prerequisiteIds.forEach((prerequisiteId) => {
        prerequisiteList[`${index}`] = prerequisiteId.value.toString();
        index += 1
    });
    let training = {
        Title: document.getElementById("trainingTitleId").value,
        DepartmentId: document.getElementById("trainingDepartmentId").value,
        SeatNumber: document.getElementById("trainingSeatAvailableId").value,
        Deadline: document.getElementById("trainingDeadLineId").value,
        StartDate: document.getElementById("trainingStartDateId").value,
        EndDate: document.getElementById("trainingEndDateId").value,
        ShortDescription: document.getElementById("trainingShortDescriptionId").value,
        LongDescription: document.getElementById("trainingLongDescriptionId").value
    };
    if (TrainingFormValidation() == true) {
        $.ajax({
            type: "POST",
            url: "/Training/RegisterTraining",
            data: { training:training, prerequisiteList: prerequisiteList },
            dataType: 'json',
            success: function (result) {
                if (result.Success == true) {
                    ShowNotification(true ,'Success', "Training has been successfully registered");
                }
                else {
                    ShowNotification(false ,'Error', "Some error has been encountered while registering the training");
                }
            },
            error: function () {
                ShowNotification(false ,'Error', "Some error has been encountered")
            }
        });
        CloseTrainingCreationForm();
        GetTrainingList(result);
    }
};
function UpdateTraining(trainingId) {
    let index = 0;
    let prerequisiteList = {};
    let prerequisiteIds = document.getElementsByName("PrerequisiteField");
    prerequisiteIds.forEach((prerequisiteId) => {
        prerequisiteList[`${index}`] = prerequisiteId.value.toString();
        index += 1
    });
    let training = {
        TrainingId: trainingId,
        Title: document.getElementById("trainingTitleId").value,
        DepartmentId: document.getElementById("trainingDepartmentId").value,
        SeatNumber: document.getElementById("trainingSeatAvailableId").value,
        Deadline: document.getElementById("trainingDeadLineId").value,
        StartDate: document.getElementById("trainingStartDateId").value,
        EndDate: document.getElementById("trainingEndDateId").value,
        ShortDescription: document.getElementById("trainingShortDescriptionId").value,
        LongDescription: document.getElementById("trainingLongDescriptionId").value
    };
    if (TrainingFormValidation() == true) {
        $.ajax({
            type: "POST",
            url: "/Training/UpdateTraining",
            data: {training: training, prerequisiteList: prerequisiteList},
            dataType: 'json',
            success: function (result) {
                if (result.Success == true) {
                    ShowNotification(true, 'Success', "Training has been successfully updated");
                }
                else {
                    ShowNotification(false, 'Error', result.Message);
                }
            },
            error: function () {
                ShowNotification(false, 'Error', "Some error has been encountered")
            }
        });
        CloseTrainingCreationForm();
        GetTrainingList(result);
    }
}
function DisplayTrainingForm(isAdding, trainingId) {

    let trainingForm = document.getElementById("trainingFormId");
    let prerequisiteForm = document.getElementById("prerequisiteFormId");
    trainingForm.style.display = "";
    prerequisiteForm.style.display = "none";

    if (isAdding) {
        document.getElementById("trainingFormTitleId").textContent = "Training creation";
        document.getElementById("trainingTitleId").value = null;
        document.getElementById("trainingDepartmentId").value = null;
        document.getElementById("trainingStartDateId").value = null;
        document.getElementById("trainingEndDateId").value = null;
        document.getElementById("trainingDeadLineId").value = null;
        document.getElementById("trainingSeatAvailableId").value = null;
        document.getElementById("trainingShortDescriptionId").value = null;
        document.getElementById("trainingLongDescriptionId").value = null;
        document.getElementById("submitTrainingDetailsBtn").setAttribute("onclick", "RegisterTraining()");
        document.getElementById("submitTrainingDetailsBtn").style.visibility = "visible";
        document.getElementById("submitTrainingDetailsBtn").textContent = "Register";
    }
    else {
        document.getElementById("trainingFormTitleId").textContent = "Training update";
        document.getElementById("submitTrainingDetailsBtn").style.visibility = "visible";
        document.getElementById("submitTrainingDetailsBtn").textContent = "Update";
        FillTrainingDetail(trainingId);
    }
    document.getElementById("screenOverlay").style.visibility = "visible";
}
function ConvertToDateString(timestamp) {
    let dateTimeObject = new Date(Number((timestamp).match(/\d+/)[0]));
    return dateTimeObject.toISOString().slice(0, 19).replace("T", " ");
}
function DeleteTraining(trainingId) {
    $.ajax({
        type: "POST",
        url: "/Training/DeleteTraining",
        data: { trainingId: trainingId },
        success: function (result) {
            if (result.Success == true) {
                GetTrainingList(result),
                ShowNotification(true, "Success", "Training has successfully deleted");
            }
            else {
                ShowNotification(false, "Error", result.Message);
            }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    })
}
function TrainingFormValidation() {
    let title = document.getElementById("trainingTitleId").value ;
    let department = document.getElementById("trainingDepartmentId").value ;
    let startDate = document.getElementById("trainingStartDateId").value ;
    let endDate = document.getElementById("trainingEndDateId").value ;
    let deadline = document.getElementById("trainingDeadLineId").value ;
    let seatNumber = document.getElementById("trainingSeatAvailableId").value ;
    let shortDescrition = document.getElementById("trainingShortDescriptionId").value ;
    let longDescription = document.getElementById("trainingLongDescriptionId").value;

    let emptyNotification = "";
    if (!title) { emptyNotification += "title " };
    if (!department) { emptyNotification += "department " };
    if (!startDate) { emptyNotification += "starting-date " };
    if (!endDate) { emptyNotification += "ending-date " };
    if (!deadline) { emptyNotification += "deadline " };
    if (!seatNumber) { emptyNotification += "Seat-number " };
    if (!shortDescrition) { emptyNotification += "shortDescrition " };
    if (!longDescription) { emptyNotification += "longDescription " };

    let notification = "";

    if (emptyNotification) {
        notification = "The following fields are empty :"
        let emptyFields = emptyNotification.split(" ");
        emptyFields.forEach((emptyField) => {
            notification += ` ${emptyField} ,`;
        });
        notification = notification.substring(0, notification.length - 1);

        document.getElementById("notificationText").innerHTML = notification;
        document.getElementById("notificationText").style.visibility = "visible";
        return false;
    }
    else if (new Date(deadline) > new Date(startDate).setHours(0, 0, 0, 0)) {
        notification = "Starting date is before the deadline !";
        document.getElementById("notificationText").innerHTML = notification;
        document.getElementById("notificationText").style.visibility = "visible";
        return false;
    }
    else if (startDate > endDate) {
        notification = "Ending date is before the StartingDate !";
        document.getElementById("notificationText").innerHTML = notification;
        document.getElementById("notificationText").style.visibility = "visible";
        return false;
    }
    else { return true }
}
//#endregion

//#region Prerequisite
function DisplayPrerequisiteForm() {
    let trainingForm = document.getElementById("trainingFormId");
    let prerequisiteForm = document.getElementById("prerequisiteFormId");
    trainingForm.style.display = "none";
    prerequisiteForm.style.display = "";

    document.getElementById("submitTrainingDetailsBtn").setAttribute("onclick", "RegisterPrerequisite()");
    document.getElementById("submitTrainingDetailsBtn").style.visibility = "visible";
    document.getElementById("submitTrainingDetailsBtn").textContent = "Register";
    document.getElementById("screenOverlay").style.visibility = "visible";
}

function RemoveDisplayPrerequisite(rowId) {
    document.getElementById("trainingPrerequisiteDetailId-" + rowId).remove();
    document.getElementById("removePrerequisiteBtnId-" + rowId).remove();
}
function UpdateDisplayPrerequisite(trainingId) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisiteByTraining",
        data: { trainingId: trainingId },
        success: function (result) {
            if (result.Success == true) {
                result.Data.forEach(function (row) {
                    AddDisplayPrerequisite()
                    let combobox = document.getElementById("trainingPrerequisiteDetailId-" + (rowCount - 1));
                    combobox.value = row.PrerequisiteId;
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
}
function RegisterPrerequisite() {
    let data = { prerequisiteDescription: document.getElementById("prerequisiteDescriptionId").value}
    $.ajax({
        type: "POST",
        url: "/Prerequisite/AddPrerequisite",
        data: data,
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                ShowNotification('Success', "Prerequisite has been successfully registered");
                GetPrerequisiteDataList();
            }
            else {
                ShowNotification('Error', "Some error has been encountered while registering the prerequisite");
            }
        },
        error: function () {
            ShowNotification('Error', "Some error has been encountered")
        }
    });
    CloseTrainingCreationForm();
    GetTrainingList(result);
}
//#endregion

//#endregion

//#region DataTable
function GetTrainingList(departmentList) {
    let buttons = "";
    $.ajax({
        type: "GET",
        url: "/Training/GetAllTraining",
        success: function (result) {
            if (result.Success == true) {
                if ($.fn.DataTable.isDataTable('#TrainingTableId')) {
                    $('#TrainingTableId').DataTable().destroy();
                }
                $('#TrainingTableId').DataTable({
                    "data": result.Data,
                    "columns": [
                        { "data": "Title" },
                        {
                            "data": "DepartmentId",
                            render: function (data) {
                                let department = _.find(departmentList, function (d) 
                                { return d.DepartmentId == data; });
                                return department.DepartmentName;
                            }
                        },
                        { "data": "SeatNumber" },
                        {
                            "data": "StartDate",
                            render: function (data) {
                                let startDateTime = new Date(Number((data).match(/\d+/)[0]));
                                return startDateTime.toLocaleString();
                            }
                        },
                        {
                            "data": "Deadline",
                            render: function (data) {
                                let dateTime = new Date(Number((data).match(/\d+/)[0]));
                                return dateTime.getDate() + "/" + dateTime.getMonth() + "/" + dateTime.getFullYear();
                            }
                        },
                        { "data": "ShortDescription" },
                        {
                            render: function (data, type, row) {
                                let deadline = new Date(Number((row.Deadline).match(/\d+/)[0]));
                                let today = new Date();
                                if (deadline < today) {
                                    buttons =
                                        `<div class='three-split-Area'>
                                            <button class='item-button' id='detailBtn' onclick='DisplayTrainingForm(false,${row.TrainingId})'>Edit</button>
                                            <button class='item-button' id='detailBtn' onclick='DeleteTraining(${row.TrainingId})'>Delete</button>
                                            <button class='item-button' id='detailBtn' onclick='GenerateCSVFile(${row.TrainingId})'>CSV</button>
                                        </div>`;
                                }
                                else {
                                    buttons =
                                        `<div class='split-Area'>
                                            <button class='item-button' id='detailBtn' onclick='DisplayTrainingForm(false,${row.TrainingId})'>Edit</button>
                                            <button class='item-button' id='detailBtn' onclick='DeleteTraining(${row.TrainingId})'>Delete</button>
                                        </div>`;
                                }

                                return buttons;
                            }
                        },
                    ],
                });
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
        },
        error: function () {
            ShowNotification(false, "Error", "Communication has been interupted");
        },
    });
};
function GetPrerequisiteDataList() {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetAllPrerequisite",
        success: function (result) {
            if (result.Success == true) {
                if ($.fn.DataTable.isDataTable('#PrerequisiteTableId')) {
                    $('#PrerequisiteTableId').DataTable().destroy();
                }
                $('#PrerequisiteTableId').DataTable({
                    "data": result.Data,
                    "columns": [
                        { "data": "PrerequisiteDescription" },
                    ],
                });
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
//#endregion


function DisplayTab(event, tabId) {
    let tabs = document.getElementsByName("tabArea")
    tabs.forEach((tab) => tab.style.display = 'none')

    let tabButtons = document.getElementsByName("tabButton")
    tabButtons.forEach((tabButton) => tabButton.style.backgroundColor = "#d2d2d2")

    let displayTab = document.getElementById(tabId);
    displayTab.style.display = "inherit";

    table1 = document.getElementById("TrainingTableId_wrapper");
    table2 = document.getElementById("PrerequisiteTableId_wrapper");
    table1.style.display = '';
    table2.style.display = 'initial';

    event.currentTarget.style.backgroundColor = "#ffffff";
}
function GenerateCSVFile(trainingId) {
    $.ajax({
        type: "GET",
        url: "/ApplicationProcess/GenerateCSVFile",
        data: { trainingId: trainingId },
        xhrFields: {
            responseType: 'blob' 
        },
        success: function (result) {
            let url = URL.createObjectURL(result);
            window.open(url, '_blank');
        },
        error: function (error) {
            ShowNotification(false, "Error", "File could not be load");
        }
    });
}

function HideTab() {
    let tabs = document.getElementsByName("tabArea")
    tabs.forEach((tab) => tab.style.display = 'none')

    tabs[0].style.display = 'inherit';

    let tabButtons = document.getElementsByName("tabButton")
    tabButtons[0].style.backgroundColor = "#ffffff";
}
