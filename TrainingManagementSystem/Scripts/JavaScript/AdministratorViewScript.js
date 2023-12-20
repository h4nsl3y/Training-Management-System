$(document).ready(
    GetDepartmentList(),
    GetTrainingList()
)
function CloseTrainingCreationForm() {
    document.getElementById("screenOverlay").style.visibility = "hidden";
    document.getElementById("submitTrainingDetailsBtn").style.visibility = "hidden";
}
function DisplayTrainingForm(isAdding, trainingId) {
    if (isAdding) {
        document.getElementById("trainingFormId").textContent = "Training creation";
        document.getElementById("trainingTitleId").value = null;
        document.getElementById("trainingDepartmentId").value = null;
        document.getElementById("trainingStartDateId").value = null;
        document.getElementById("trainingEndDateId").value = null;
        document.getElementById("trainingDeadLineId").value = null;
        document.getElementById("trainingSeatAvailableId").value = null;
        document.getElementById("trainingShortDescriptionId").value = null;
        document.getElementById("trainingLongDescriptionId").value = null;
        document.getElementById("submitTrainingDetailsBtn").onclick = function () {
            RegisterTraining();
        };
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
function GetDepartmentList() {
    $.ajax({
        type: "GET",
        url: "/Department/GetDepartmentList",
        success: function (result) {
            if (result.message == "success") {
                let managerCombobox = document.getElementById("trainingDepartmentId");
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
function OpenTrainingCreationForm() {
    let overlay = document.getElementById("screenOverlay");
    overlay.style.visibility = "visible";
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
function FillTrainingDetail(trainingId) {
    $.ajax({
        type: "GET",
        url: "/Training/GetTraining",
        data: { trainingId: trainingId },
        dataType: 'json',
        success: function (result) {
            let training = result.data;
            if (message = 'success') {
                document.getElementById("submitTrainingDetailsBtn").onclick = function () {
                    UpdateTraining(training.TrainingId);
                    GetTrainingList();
                };

                document.getElementById("trainingTitleId").value = training.Title;
                document.getElementById("trainingDepartmentId").value = training.DepartmentId;
                document.getElementById("trainingStartDateId").value = ConvertToDateString(training.StartDate);
                document.getElementById("trainingEndDateId").value = ConvertToDateString(training.EndDate);
                document.getElementById("trainingDeadLineId").value = ConvertToDateString(training.Deadline).split(" ")[0];
                document.getElementById("trainingSeatAvailableId").value = training.SeatNumber;
                document.getElementById("trainingShortDescriptionId").value = training.ShortDescription;
                document.getElementById("trainingLongDescriptionId").value = training.LongDescription;
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