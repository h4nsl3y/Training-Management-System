$(document).ready(
    GetDepartmentList(),
    GetTrainingList(),
    GetPrerequisiteDataList()
)
// GLOBAL VARIABLES
let rowCount = 0
function AddDisplayPrerequisite() {
    let body = document.getElementById("prerequisiteRowId");

    let comboBoxElement = document.createElement("select");
    comboBoxElement.setAttribute("id", "trainingPrerequisiteDetailId-" + rowCount);
    comboBoxElement.setAttribute("name", "PrerequisiteField");
    comboBoxElement.setAttribute("class", "input-combo-box");

    let buttonElement = document.createElement("button");
    buttonElement.setAttribute("id", "removePrerequisiteBtnId-" + rowCount);
    buttonElement.setAttribute("name", "PrerequisiteField");
    buttonElement.setAttribute("class", "item-button");
    buttonElement.setAttribute("type", "button");
    buttonElement.setAttribute("onclick", "RemoveDisplayPrerequisite(" + rowCount + ");");
    buttonElement.textContent = "Remove";

    body.appendChild(comboBoxElement);
    body.appendChild(buttonElement);
    GetPrerequisiteList(rowCount);
    rowCount += 1;
}

//#region ComboboxList
function GetDepartmentList() {
    $.ajax({
        type: "GET",
        url: "/Department/GetDepartmentList",
        success: function (result) {
            if (result.message == "success") {
                let combobox = document.getElementById("trainingDepartmentId");
                result.data.forEach(function (row) {
                    let option = document.createElement("option");
                    option.value = row.DepartmentId;
                    option.text = row.DepartmentName;
                    combobox.add(option);
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
function GetPrerequisiteList(rowId) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetAllPrerequisite",
        success: function (result) {
            if (result.message == "success") {
                let combobox = document.getElementById("trainingPrerequisiteDetailId-" + rowId);
                result.data.forEach(function (row) {
                    let option = document.createElement("option");
                    option.value = row.PrerequisiteId;
                    option.text = row.PrerequisiteDescription;
                    combobox.add(option);
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
    const elements = document.querySelectorAll('[name="PrerequisiteField"]');
    elements.forEach(function (element) {
        element.remove();
    });
}
function FillTrainingDetail(trainingId) {
    $.ajax({
        type: "GET",
        url: "/Training/GetTraining",
        data: { trainingId: trainingId },
        dataType: 'json',
        success: function (result) {
            let training = result.data;
            if (message = 'success') {

                document.getElementById("submitTrainingDetailsBtn").setAttribute("onclick", "UpdateTraining(" + training.TrainingId + ");");
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
                ShowNotification('Error', "Some error has been encountered while loading the training details");
            }
        },
        error: function () {
            ShowNotification('Error', "Some error has been encountered")
        }
    });
};
function RegisterTraining() {
    let data = {
        Title: document.getElementById("trainingTitleId").value,
        DepartmentId: document.getElementById("trainingDepartmentId").value,
        SeatNumber: document.getElementById("trainingSeatAvailableId").value,
        Deadline: document.getElementById("trainingDeadLineId").value,
        StartDate: document.getElementById("trainingStartDateId").value,
        EndDate: document.getElementById("trainingEndDateId").value,
        ShortDescription: document.getElementById("trainingShortDescriptionId").value,
        LongDescription: document.getElementById("trainingLongDescriptionId").value
    };
    setTrainingPrerequisite(document.getElementById("trainingTitleId").value);
    $.ajax({
        type: "POST",
        url: "/Training/RegisterTraining",
        data: data,
        dataType: 'json',
        success: function () {
            if (mesage = 'success') {
                ShowNotification('Success', "Training has been successfully registered");
            }
            else {
                ShowNotification('Error', "Some error has been encountered while registering the training");
            }
        },
        error: function () {
            ShowNotification('Error', "Some error has been encountered")
        }
    });
    CloseTrainingCreationForm();
    GetTrainingList();
};
function UpdateTraining(trainingId) {
    let data = {
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
    $.ajax({
        type: "POST",
        url: "/Training/UpdateTraining",
        data: data,
        dataType: 'json',
        success: function () {
            if (mesage = 'success') {
                ShowNotification('Success', "Training has been successfully updated");
            }
            else {
                ShowNotification('Error', "Some error has been encountered while registering the training");
            }
        },
        error: function () {
            ShowNotification('Error', "Some error has been encountered")
        }
    });
    CloseTrainingCreationForm();
    GetTrainingList();
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
        document.getElementById("trainingFormId").textContent = "Training update";
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
            if (result.message == "success") {

                result.data.forEach(function (row) {
                    AddDisplayPrerequisite()
                    let combobox = document.getElementById("trainingPrerequisiteDetailId-" + (rowCount - 1));
                    combobox.value = row.PrerequisiteId;
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
}
function RegisterPrerequisite() {
    let data = { prerequisiteDescription: document.getElementById("prerequisiteDescriptionId").value}
    $.ajax({
        type: "POST",
        url: "/Prerequisite/AddPrerequisite",
        data: data,
        dataType: 'json',
        success: function () {
            if (mesage = 'success') {
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
    GetTrainingList();
}
//#endregion

//#endregion

//#region DataTable
function GetTrainingList() {
    $.ajax({
        type: "GET",
        url: "/Training/GetAllTraining",
        success: function (result) {
            if (result.message == "success") {
                if ($.fn.DataTable.isDataTable('#TrainingTableId')) {
                    $('#TrainingTableId').DataTable().destroy();
                }
                $('#TrainingTableId').DataTable({
                    "data": result.data,
                    "columns": [
                        { "data": "Title" },
                        { "data": "DepartmentId" },
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
                            "data": "TrainingId",
                            render: function (data) {
                                return "<button class='item-button' id='detailBtn' onclick='DisplayTrainingForm(" + false + "," + data + ")'>Edit</button>";
                            },
                        }
                    ],
                });
            }
            else {
                ShowNotification("Error", result.data);
            };
        },
        error: function () {
            ShowNotification("Error", "Communication has been interupted");
        },
    });
};

function GetPrerequisiteDataList() {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetAllPrerequisite",
        success: function (result) {
            if (result.message == "success") {
                if ($.fn.DataTable.isDataTable('#PrerequisiteTableId')) {
                    $('#PrerequisiteTableId').DataTable().destroy();
                }
                $('#PrerequisiteTableId').DataTable({
                    "data": result.data,
                    "columns": [
                        { "data": "PrerequisiteDescription" },
                    ],
                });
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

//#region TableToggle

function TrainingTableToggle() {
    table = document.getElementById("TrainingTableId_wrapper");
    image = document.getElementById("trainingTableArrowId");
    if (table.style.display == 'none') {
        table.style.display = '';
        image.style.transform = "scaleY(-1)";
    } else {
        table.style.display = 'none';
        image.style.transform = "scaleY(1)";
    }
}

function PrerequisitTeableToggle() {
    table = document.getElementById("PrerequisiteTableId_wrapper");
    image = document.getElementById("prerequisiteTableArrowId");
    if (table.style.display == 'none') {
        table.style.display = '';
        image.style.transform = "scaleY(-1)";
    } else {
        table.style.display = 'none';
        image.style.transform = "scaleY(1)";
    }
}

function DepartmentTableToggle() {
    table = document.getElementById("trainingTableArrowId_wrapper");
    image = document.getElementById("departmentTableArrowId");
    if (table.style.display == 'none') {
        table.style.display = '';
        image.style.transform = "scaleY(-1)";
    } else {
        table.style.display = 'none';
        image.style.transform = "scaleY(1)";
    }
}
//#endregion

//#endregion


function setTrainingPrerequisite(trainingTitle) {
    let prerequisiteIds = document.querySelectorAll('select[name="PrerequisiteField"]');
    prerequisiteIds.forEach(prerequisiteId => {
        console.log(prerequisiteId.value); 

        data = { prerequisiteId: prerequisiteId.value, title: trainingTitle };


        $.ajax({
            type: "GET",
            url: "/Training/SetPrerequisite",
            data: data,
            success: function (result) {
                if (result.message == "success") {
                    console.log("success");
                }
                else {
                    ShowNotification("Error", result.data);
                }
            },
            error: function (error) {
                ShowNotification("Error", "Communication has been interupted");
            }
        });

    });
}