$(document).ready(
    GetDepartmentList(),
    GetUnenrolledTrainingList(),
    GetStateList(),
    GetPrerequisiteFiles()
)
//GLOBAL VARIABLE
var index = 0;
var departmentList = [] ;

//#region DataTable
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
function GetStateList() {
    $.ajax({
        type: "GET",
        url: "/State/GetStateList",
        success: function (result) {
            GetEnrolledTrainingList(result.data);
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    });
};
function GetDepartmentList() {
    $.ajax({
        type: "GET",
        url: "/Department/GetDepartmentList",
        success: function (result) {
            result.data.forEach((department) =>  departmentList.push(department));
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    });
};
function GetEnrolledTrainingList(stateList) {
    $.ajax({
        type: 'GET',
        url: "/Enrollment/GetAllEnrollmentByEmployee",
        data: { email: "none" },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                if ($.fn.DataTable.isDataTable('#enrollmentTableId')) {
                    $('#enrollmentTableId').DataTable().destroy();
                }
                $('#enrollmentTableId').DataTable({
                    "data": result.data,
                    "columns": [
                        {
                            "data": "SubmissionDate",
                            render: function (data) {
                                return new Date(Number((data).match(/\d+/)[0]));
                            }
                        },
                        {
                            "data": "StateId",
                            render: function (data) {
                                let state = _.find(stateList, { StateId: data })
                                return state.StateDefinition;
                            }
                        },
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


function GetPrerequisiteFiles() {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisiteFile",
        success: function (result) {
            if (result.message = "success") {
                GetAllPrerequisite(result.data)
            }
            else {
                ShowNotification("Error", result.data);
            }
        },
        error: function (error) {
            ShowNotification("Error", "Communication has been interupted");
        }
    })
}
function GetAllPrerequisite(setPrerequisite) {
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
                            "data": "PrerequisiteId",
                            render: function (data) {
                                if (setPrerequisite.includes(data)) {
                                    return "<button class='item-button' id='detailBtn' onclick='GetDocument(" + data + ")'>View</button>";
                                }
                                else {
                                    return "no file";
                                }
                            }
                        },
                        {
                            "data": "PrerequisiteId",
                            render: function (data) {
                                if (setPrerequisite.includes(data)) {
                                    return "<button class='item-button' id='detailBtn' onclick='Update(" + data + ")'>Update</button>";
                                }
                                else {
                                    return "<button class='item-button' id='detailBtn' onclick='Upload(" + data + ")'>Upload</button>";
                                }
                            }
                        },
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

//#region TableToggle
function TrainingTableToggle() {
    let table = document.getElementById("trainingTableId_wrapper");
    let image = document.getElementById("trainingTableArrowId");
    if (table.style.display == 'none') {
        table.style.display = '';
        table.style.width = '80 % !important';
        table.style.padding = '0px 10 % !important';
        image.style.transform = "scaleY(-1)";
    } else {
        console.log(table.style.display);
        table.style.display = 'none';
        image.style.transform = "scaleY(1)";
    }
}
function EnrollmentTableToggle() {
    table = document.getElementById("enrollmentTableId_wrapper");
    image = document.getElementById("enrollmentTableArrowId");
    if (table.style.display == 'none') {
        table.style.display = '';
        table.style.width = '80 % !important';
        table.style.padding = '0px 10 % !important';
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
        table.style.display = '';
        table.style.width = '80 % !important';
        table.style.padding = '0px 10 % !important';
        image.style.transform = "scaleY(-1)";
    } else {
        table.style.display = 'none';
        image.style.transform = "scaleY(1)";
    }
}
//#endregion

//#endregion

//#region FormTraining
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
function DisplayTrainingDetails(training, displayButton) {
    let overlay = document.getElementById("screenOverlay");
    let trainingTitle = document.getElementById("detailTitle");
    let trainingId = document.getElementById("detailId")

    let trainingDepartment = document.getElementById("detailDepartmentPriority")
    let trainingDescription = document.getElementById("detailDescription");
    let trainingDate = document.getElementById("detailDate");
    let trainingDeadline = new Date(Number((training.Deadline).match(/\d+/)[0]));

    trainingTitle.textContent = "Title : " + training.Title;
//    trainingId.textContent = "Id : " + training.TrainingId;

    let department = _.find(departmentList, { DepartmentId: training.DepartmentId })
    trainingDepartment.textContent = "Priority to department : " + department.DepartmentName;
    trainingDescription.textContent = "Description : " + training.LongDescription;
    trainingDate.textContent = "Application deadline : " + trainingDeadline;

    btnElement = document.getElementById("enrollBtn");
    btnElement.setAttribute("onclick", "CheckValidEnrollment(" + training.TrainingId + ")");

    GetTrainingPrerequisite(training.TrainingId)
    let uploadForm = document.getElementById("upload-form-container");

    if (displayButton) {
        btnElement.style.visibility = "visible";
        uploadForm.style.visibility = "visible";
    }
    else {
        btnElement.style.visibility = "hidden";
        uploadForm.style.visibility = "hidden";
    }
    overlay.style.visibility = "visible";
}
function GetTrainingPrerequisite(trainingId) { 
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisiteByTraining",
        data: { trainingId: trainingId, accountId: 0 },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                let fileForm = $('#uploadFormId');
                fileForm.empty();
                let prerequisites = result.data
                prerequisites.forEach(function (prerequisite) {
                    let row = "<div>" + prerequisite.PrerequisiteDescription + "<div><br>";
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
function CheckValidEnrollment(trainingId) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisiteByTraining",
        data: { trainingId: trainingId, accountId: 0 },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                if (result.data.length == 0) {
                    Enroll(trainingId);
                }
                else {
                    let prerequisites = result.data
                    let prerequisiteIdList = []
                    prerequisites.forEach(function (prerequisite) {
                        prerequisiteIdList.push(prerequisite.PrerequisiteId)
                    })
                    CheckPrerequisiteFiles(prerequisiteIdList, trainingId)
                }
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
function CheckPrerequisiteFiles(prerequisiteIdList, trainingId) {
    let IsValid = true;
    prerequisiteIdList.forEach(function (prerequisiteId) {
        $.ajax({
            type: "GET",
            url: "/RequiredFile/IsFilePresent",
            data: { prerequisiteId: prerequisiteId },
            success: function (result) {
                if (result.message == "success") {
                    if (result.data != true) {
                        IsValid = false;
                    }
                    if (IsValid) {
                        Enroll(trainingId);
                    } else {
                        ShowNotification("Error", "File(s) are missing for this training");
                    }
                }
            },
            error: function (error) {
                ShowNotification("Error", "Communication has been interupted");
            }
        });
    });
}
function Enroll(trainingId) {
    let textId = document.getElementById("detailId").textContent
    let Id = textId.split(" ")[2]
    $.ajax({
        type: "POST",
        url: "/Enrollment/RegisterEnrollment",
        data: { trainingId: trainingId },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                GetUnenrolledTrainingList();
                GetStateList();
                ShowNotification("Success", "Successfully enrolled")
                let overlay = document.getElementById("screenOverlay");
                let enrollButton = document.getElementById("enrollBtn");
                let uploadForm = document.getElementById("upload-form-container");
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
//#endregion FormTraining

//#region commonForm
function HideDetail() {
    let overlay = document.getElementById("screenOverlay");
    let uploadForm = document.getElementById("upload-form-container");
    let enrollButton = document.getElementById("enrollBtn");
    overlay.style.visibility = "hidden";
    uploadForm.style.visibility = "hidden";
    enrollButton.style.visibility = "hidden";
};
function UploadFile(prerequisiteId , update) {
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
                formData.append('prerequisiteId', prerequisiteId);

                GetPrerequisiteFiles();
            }
            else { ShowNotification("Error", "File size is not supported ! maximum size is 10 MB"); uploadFlag = false; }
        }
        else { ShowNotification("Error", "File type is not supported ! please select a file from the following format : PDF"); uploadFlag = false; };
    });
    if (uploadFlag) {
        if (update) {
            FileUpdate(formData);
        }
        else {
            FileUpload(formData);
        } 
    }
}
//#endregion

//#region FileManagement
function FileUpload(fileData) {
    $.ajax({
        type: 'POST',
        url: "/RequiredFile/UploadFile",
        data: fileData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.message == "success") {
                ShowNotification("Success", "File update successfully");
                document.getElementById('uploadFormId').style.backgroundColor = '#a1ffa4';
                document.getElementById('uploadBtn').style.visibility = 'hidden';
                GetPrerequisiteFiles();
                HideDetail()
            }
            else { ShowNotification("Error", result.data); }
        },
        error: function (error) {
            ShowNotification("Error", "Failed to upload file");
        }
    });
}
function FileUpdate(fileData) {
    $.ajax({
        type: 'POST',
        url: "/RequiredFile/UpdateFile",
        data: fileData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.message == "success") {
                ShowNotification("Success", "File uploaded successfully");
                document.getElementById('uploadFormId').style.backgroundColor = '#a1ffa4';
                document.getElementById('uploadBtn').style.visibility = 'hidden';
                GetPrerequisiteFiles();
                HideDetail()
            }
            else { ShowNotification("Error", result.data); }
        },
        error: function (error) {
            ShowNotification("Error", "Failed to upload file");
        }
    });
}
function GetDocument(prerequisiteId) {
    $.ajax({
        type: "GET",
        url: "/RequiredFile/GetFile",
        data: { prerequisiteId: prerequisiteId},
        xhrFields: {
            responseType: 'blob'
        },
        success: function (result) {
            let url = URL.createObjectURL(result);
            window.open(url, '_blank');
        },
        error: function () {
            ShowNotification("Error, File could not be load");
        }
    });
}
function Update(prerequisiteId) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisite",
        data: { prerequisiteId: prerequisiteId },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                let fileForm = $('#uploadFormId');
                fileForm.empty();
                let prerequisites = result.data

                let overlay = document.getElementById("screenOverlay");
                let trainingTitle = document.getElementById("detailTitle");
                let trainingId = document.getElementById("detailId")
                let trainingDepartment = document.getElementById("detailDepartmentPriority");
                let trainingDescription = document.getElementById("detailDescription");
                let trainingDate = document.getElementById("detailDate");

                trainingTitle.textContent = "";
                trainingId.textContent = "";
                trainingDepartment.textContent = "";
                trainingDescription.textContent = "";
                trainingDate.textContent = "";

                let enrollButton = document.getElementById("enrollBtn");
                let uploadForm = document.getElementById("upload-form-container");
                enrollButton.style.visibility = "hidden";
                uploadForm.style.visibility = "visible";

                overlay.style.visibility = "visible";


                prerequisites.forEach(function (prerequisite) {
                    let row = "<label for='uploadFileId" + prerequisite.PrerequisiteId + "'> " + prerequisite.PrerequisiteDescription + " </label><br>";
                    row += "<input type='file' name='file' id='uploadFileId" + prerequisite.PrerequisiteId + "' />";
                    row += "<input type='button' id='uploadBtn' value='Upload' onclick='UploadFile(" + prerequisite.PrerequisiteId + ",true)'><br>";
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
function Upload(prerequisiteId) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisite",
        data: { prerequisiteId: prerequisiteId },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                let fileForm = $('#uploadFormId');
                fileForm.empty();
                let prerequisites = result.data

                let overlay = document.getElementById("screenOverlay");
                let trainingTitle = document.getElementById("detailTitle");
                let trainingId = document.getElementById("detailId")
                let trainingDepartment = document.getElementById("detailDepartmentPriority");
                let trainingDescription = document.getElementById("detailDescription");
                let trainingDate = document.getElementById("detailDate");

                trainingTitle.textContent = "";
                trainingId.textContent = "";
                trainingDepartment.textContent = "";
                trainingDescription.textContent = "";
                trainingDate.textContent = "";

                let enrollButton = document.getElementById("enrollBtn");
                let uploadForm = document.getElementById("upload-form-container");
                enrollButton.style.visibility = "hidden";
                uploadForm.style.visibility = "visible";

                overlay.style.visibility = "visible";


                prerequisites.forEach(function (prerequisite) {
                    let row = "<label for='uploadFileId" + prerequisite.PrerequisiteId + "'> " + prerequisite.PrerequisiteDescription + " </label><br>";
                    row += "<input type='file' name='file' id='uploadFileId" + prerequisite.PrerequisiteId + "' />";
                    row += "<input type='button' id='uploadBtn' value='Upload' onclick='UploadFile(" + prerequisite.PrerequisiteId + ", false)'><br>";
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
//#endregion