﻿$(document).ready(
    GetUnenrolledTrainingList(),
    GetEnrolledTrainingList(),
    GetPrerequisiteFiles()
)
function GetPrerequisite(trainingId) { // TO REMOVE
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisite",
        data: { trainingId: trainingId },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                let fileForm = $('#uploadFormId');
                fileForm.empty();
                let prerequisites = result.data
                prerequisites.forEach(function (prerequisite) {
                    let row = "<label for='uploadFileId" + prerequisite.PrerequisiteId + "'> " + prerequisite.PrerequisiteDescription + " </label><br>";
                    row += "<input type='file' name='file' id='uploadFileId" + prerequisite.PrerequisiteId + "' />";
                    row += "<input type='button' id='uploadBtn' value='Upload' onclick='UploadFile(" + prerequisite.PrerequisiteId + ")'><br>";
                    fileForm.append(row);
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
function DisplayTrainingDetails(training, displayButton) {
    let overlay = document.getElementById("screenOverlay");
    let trainingTitle = document.getElementById("detailTitle");
    let trainingId = document.getElementById("detailId")
    let trainingDepartment = document.getElementById("detailDepartmentPriority");
    let trainingDescription = document.getElementById("detailDescription");
    let trainingDate = document.getElementById("detailDate");
    let trainingDeadline = new Date(Number((training.Deadline).match(/\d+/)[0]));

    trainingTitle.textContent = "Title : " + trainingTitle;
    trainingId.textContent = "Id : " + training.TrainingId;
    trainingDepartment.textContent = "Priority to department : " + training.DepartmentId;
    trainingDescription.textContent = "Description : " + training.LongDescription;
    trainingDate.textContent = "Application deadline : " + trainingDeadline;

    GetPrerequisite(training.TrainingId)
    let enrollButton = document.getElementById("enrollBtn");
    let uploadForm = document.getElementById("upload-form-container");

    if (displayButton) {
        enrollButton.style.visibility = "visible";
        uploadForm.style.visibility = "visible";
    }
    else {
        enrollButton.style.visibility = "hidden";
        uploadForm.style.visibility = "hidden";
    }
    overlay.style.visibility = "visible";
}

function GetUnenrolledTrainingList() {
    $.ajax({
        type: 'GET',
        url: "/Training/GetUnenrolledTraining",
        dataType: 'json',
        success: function (result) {
            if (result.message = "success") {
                if ($.fn.DataTable.isDataTable('#trainingTableId')) {
                    $('#trainingTableId').DataTable().destroy();
                }
                $('#trainingTableId').DataTable({
                    "data": result.data,
                    "columns": [
                        { "data": "TrainingId" },
                        { "data": "Title" },
                        { "data": "Title" },
                        { "data": "SeatNumber" },
                        { "data": "ShortDescription" },
                        {
                            "data": "TrainingId",
                            render: function (data) {
                                return "<button class= 'item-button' id = 'detailBtn' onclick = 'GetTrainingDetail(" + data + ", true)'> Details</button>";
                            },
                            targets: -1
                        }
                    ],
                });
            }
            else { ShowNotification("Error", result.data); }
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    });
}
function Enroll() {
    let textId = document.getElementById("detailId").textContent
    let Id = textId.split(" ")[2]

    $.ajax({
        type: "POST",
        url: "/Enrollment/RegisterEnrollment",
        data: { trainingId: Id },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                ShowNotification("Success", "Successfully enrolled")
                let overlay = document.getElementById("screenOverlay");
                let enrollButton = document.getElementById("enrollBtn");
                let uploadForm = document.getElementById("upload-form-container");
                GetUnenrolledTrainingList();
                GetEnrolledTrainingList();
                uploadForm.style.visibility = "hidden";
                enrollButton.style.visibility = "hidden";
                overlay.style.visibility = "hidden";
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
function GetEnrolledTrainingList() {
    $.ajax({
        type: 'GET',
        url: "/Enrollment/GetAllEnrollmentByEmployee",
        dataType: 'json',
        success: function (result) {
            if (result.message = "success") {
                if ($.fn.DataTable.isDataTable('#enrollmentTableId')) {
                    $('#enrollmentTableId').DataTable().destroy();
                }

                $('#enrollmentTableId').DataTable({
                    "data": result.data,
                    "columns": [
                        { "data": "TrainingId" },
                        {
                            "data": "SubmissionDate",
                            render: function (data) {
                                return new Date(Number((data).match(/\d+/)[0]));
                            }
                        },
                        { "data": "StateId" },
                        {
                            "data": "TrainingId",
                            render: function (data) {
                                return "<button class= 'item-button' id = 'detailBtn' onclick = 'GetTrainingDetail(" + data + ", false)'> Details</button>";
                            }
                        }
                    ],
                });
                var table = $('#enrollmentTableId').DataTable();
                table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                    var data = this.data();
                    if (data.StateId == 3) {
                        $(this.node()).css('background-color', '#abffac');
                    }
                    else if (data.StateId == 2) {
                        $(this.node()).css('background-color', '#ffabab');
                    }
                });
            }
            else { ShowNotification("Error", result.data); }
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    });
};
function GetTrainingDetail(id, displayButton) {
    $.ajax({
        type: 'GET',
        url: "/Training/GetTraining",
        data: { trainingid: id },
        dataType: 'json',
        success: function (result) {
            if (result.message = "success") { DisplayTrainingDetails(result.data, displayButton); }
            else { ShowNotification(result.data); }
        },
        error: function (error) {
            console.log(error);
        }
    });
};
function HideDetail() {
    let overlay = document.getElementById("screenOverlay");
    let uploadForm = document.getElementById("upload-form-container");
    let enrollButton = document.getElementById("enrollBtn");
    overlay.style.visibility = "hidden";
    uploadForm.style.visibility = "hidden";
    enrollButton.style.visibility = "hidden";
};
function UploadFile(prerequisiteId) {
    let uploadFlag = true;
    let allowedFileType = "application/pdf";
    let allowedFileSize = 10;

    let fieldUpload = document.getElementById('uploadFileId' + prerequisiteId);
    let fileObjects = Array.from(fieldUpload.files);
    let formData = new FormData();
    fileObjects.forEach(fileObject => {
        if (allowedFileType.includes(fileObject.type)) {
            if (fileObject.size <= (allowedFileSize * 1024 * 1024)) {
                fileObject.name = prerequisiteId
                formData.append('file', fileObject);
                formData.append('prerequisiteId', prerequisiteId)
            }
            else { ShowNotification("Error", "File size is not supported ! maximum size is 10 MB"); uploadFlag = false; }
        }
        else { ShowNotification("Error", "File type is not supported ! please select a file from the following format : PDF"); uploadFlag = false; };
    });
    if (uploadFlag) {
        FileUpload(formData);
    }
}
function FileUpload(fileData) {

    $.ajax({
        type: 'POST',
        url: "/RequiredFile/UploadFile",
        data: fileData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.message == "success") {
                ShowNotification("Success", "File uploaded successfully");
                document.getElementById('uploadFormId').style.backgroundColor = '#a1ffa4';
                document.getElementById('uploadBtn').style.visibility = 'hidden';
            }
            else { ShowNotification("Error", result.data); }
        },
        error: function (error) {
            ShowNotification("Error", "Failed to upload file");
        }
    });
}
function TrainingTableToggle() {
    let table = document.getElementById("trainingTableId_wrapper");
    let image = document.getElementById("trainingTableArrowId");
    if (table.style.display == 'none') {
        table.style.display = 'table';
        image.style.transform = "scaleY(-1)";
    } else {
        table.style.display = 'none';
        image.style.transform = "scaleY(1)";
    }
}
function EnrollmentTableToggle() {
    table = document.getElementById("enrollmentTableId_wrapper");
    image = document.getElementById("enrollmentTableArrowId");
    if (table.style.display == 'none') {
        table.style.display = 'table';
        image.style.transform = "scaleY(-1)";
    } else {
        table.style.display = 'none';
        image.style.transform = "scaleY(1)";
    }
}
function PrerequisiteTableToggle() {
    table = document.getElementById("prerequisiteTableId_wrapper");
    image = document.getElementById("prerequisiteTableArrowId");
    if (table.style.display == 'none') {
        table.style.display = 'table';
        image.style.transform = "scaleY(-1)";
    } else {
        table.style.display = 'none';
        image.style.transform = "scaleY(1)";
    }
}
function GetPrerequisiteFiles() {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetAllPrerequisite",
        success: function (result) {
            if (result.message = "success") {
                if ($.fn.DataTable.isDataTable('#prerequisiteTableId')) {
                    $('#prerequisiteTableId').DataTable().destroy();
                }
                $('#prerequisiteTableId').DataTable({
                    "data": result.data,
                    "columns": [
                        { "data": "PrerequisiteDescription" },
                        {
                            "data": " PrerequisiteId",
                            render: function (data) {
                                return "<button class='item-button' id='detailBtn' onclick='DisplayFile(" + data + ")'>View</button>";
                            }
                        },
/*                        {
                            "data": " PrerequisiteId",
                            render: function (data) {
                                return "<button class='item-button' id='detailBtn' onclick='Update(" + data + ")'>View</button>";
                            }
                        },*/
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
}




        



    