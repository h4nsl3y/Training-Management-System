$(window).on('load', function () {
    HideTab();
    GetDepartmentList();
    GetAvailableTrainingList();
    GetStateList();
    GetPrerequisiteFiles();
});
//GLOBAL VARIABLE
var index = 0;
var departmentList = [] ;

//#region DataTable
function GetAvailableTrainingList() {
    DisplaySpinner()
    $.ajax({
        type: 'GET',
        url: "/Training/GetAvailableTraining",
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                if ($.fn.DataTable.isDataTable('#trainingTableId')) {
                    $('#trainingTableId').DataTable().destroy();
                }
                $('#trainingTableId').DataTable({
                    "data": result.Data,
                    "columns": [
                        { "data": "Title" },
                        {
                            "data": "StartDate",
                            render: function (data) {
                                return new Date(Number((data).match(/\d+/)[0]));
                            },
                        },
                        { "data": "SeatNumber" },
                        { "data": "ShortDescription" },
                        {
                            "data": "TrainingId",
                            render: function (data) {
                                return "<button class= 'item-button' id = 'detailBtn' onclick = 'GetTrainingDetail(" + data + ", true)'> Details</button>";
                            }
                        }
                    ],
                });
               
            }
            else {
                ShowNotification(false, "Error", result.Message);
            }
            RemoveSpinner();
        },
        error: function (error) {
            RemoveSpinner();
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
}
function GetStateList() {
    $.ajax({
        type: "GET",
        url: "/State/GetStateList",
        success: function (result) {
            GetEnrolledTrainingList(result.Data);
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
};
function GetDepartmentList() {
    $.ajax({
        type: "GET",
        url: "/Department/GetDepartmentList",
        success: function (result) {
            result.Data.forEach((department) =>  departmentList.push(department));
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
};
function GetEnrolledTrainingList(stateList) {
    let waitForApprovalState = GetStateId(stateList, "Waiting for approval");
    let rejectedState = GetStateId(stateList, "Rejected");
    let approveState =  GetStateId(stateList, "Approved");
    let cancelState = GetStateId(stateList, "Cancelled");
    let confirmedState = GetStateId(stateList, "Confirmed");
    $.ajax({
        type: 'GET',
        url: "/Enrollment/GetAllEnrollmentByEmployeeId",
        data: { accountId: "0" },
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                if ($.fn.DataTable.isDataTable('#enrollmentTableId')) {
                    $('#enrollmentTableId').DataTable().destroy();
                }
                $('#enrollmentTableId').DataTable({
                    "data": result.Data,
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
                            render: function (data, type, row) {
                                let enrollmentParameter = {
                                    EnrollmentId: row.EnrollmentId
                                    , AccountId: row.AccountId
                                    , TrainingId: row.TrainingId
                                    , StateId: cancelState
                                    , SubmissionDate: row.SubmissionDate
                                };
                                enrollmentParameter = JSON.stringify(enrollmentParameter);
                                let buttons =
                                    `<button class= 'item-button' id = 'detailBtn' onclick = 'GetTrainingDetail(${row.TrainingId}, false)'> Details </button>`;
                                if (row.StateId == confirmedState || row.StateId == approveState || row.StateId == waitForApprovalState) {
                                    buttons =
                                        `<div class='split-Area'> 
                                            ${buttons}
                                            <button class= 'item-button' id = 'cancelBtn' onclick = 'UpdateStateToCancel(${enrollmentParameter})'> Cancel </button >
                                        </div>`;
                                }  
                                return buttons;
                            }
                        }
                    ],
                });
                var table = $('#enrollmentTableId').DataTable();
                table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                    var data = this.data();
                    if (data.StateId == confirmedState) {
                        $(this.nodes()).css('background-color', '#ababff');
                    }
                    else if (data.StateId == cancelState) {
                        $(this.node()).css('background-color', '#ffffab');
                    }
                    else if (data.StateId == approveState) {
                        $(this.node()).css('background-color', '#abffac');
                    }
                    else if (data.StateId == rejectedState) {
                        $(this.node()).css('background-color', '#ffabab');
                    }
                });
            }
            else { ShowNotification(false, "Error", result.data); }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
};
function GetPrerequisiteFiles() {
    DisplaySpinner();
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisiteFile",
        success: function (result) {
            if (result.Success == true) {
                GetAllPrerequisite(result.Data)
            }
            else {
                ShowNotification(false, "Error", result.Data);
            }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    })
    RemoveSpinner();
}
function GetAllPrerequisite(setPrerequisite) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetAllPrerequisite",
        success: function (result) {
            if (result.Success == true) {
                if ($.fn.DataTable.isDataTable('#prerequisiteTableId')) {
                    $('#prerequisiteTableId').DataTable().destroy();
                }
                $('#prerequisiteTableId').DataTable({
                    "data": result.Data,
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
                ShowNotification(false, "Error", result.Message);
            }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
}
function UpdateStateToCancel(enrollmentParameter) {
    let data = { enrollment: enrollmentParameter };
    $.ajax({
        type: "POST",
        url: "/Enrollment/UpdateState",
        data: data,
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                ShowNotification(true, "Success", "Enrollment request has been cancelled"),
                GetStateList()
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
        },
        error: function () {
            ShowNotification(false, "Error", "Communication has been interupted");
        },
    });
}
function GetStateId(list, stateDefinition) {
    let state = _.find(list, { StateDefinition: stateDefinition });
    return state.StateId
}
//#endregion

//#region FormTraining
function GetTrainingDetail(id, displayButton) {
    $.ajax({
        type: 'GET',
        url: "/Training/GetTraining",
        data: { trainingid: id },
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) { DisplayTrainingDetails(result.Data[0], displayButton); }
            else { ShowNotification(false, "Error",result.Message); }
        },
        error: function (error) {
            console.log(error);
        }
    });
};
function DisplayTrainingDetails(training, displayButton) {
    let overlay = document.getElementById("screenOverlay");
    let trainingTitle = document.getElementById("detailTitle");

    let trainingDepartment = document.getElementById("detailDepartmentPriority")
    let trainingDescription = document.getElementById("detailDescription");
    let trainingDate = document.getElementById("detailDate");
    let trainingDeadline = new Date(Number((training.Deadline).match(/\d+/)[0]));

    trainingTitle.textContent = "Title : " + training.Title;

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
            if (result.Success == true) {
                let fileForm = $('#uploadFormId');
                fileForm.empty();
                let prerequisites = result.Data;
                prerequisites.forEach(function (prerequisite) {
                    let row = "<div>" + prerequisite.PrerequisiteDescription + "<div><br>";
                    fileForm.append(row);
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
function CheckValidEnrollment(trainingId) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisiteByTraining",
        data: { trainingId: trainingId, accountId: 0 },
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                if (result.Data.length == 0) {
                    Enroll(trainingId);
                }
                else {
                    let prerequisites = result.Data
                    let prerequisiteIdList = []
                    prerequisites.forEach(function (prerequisite) {
                        prerequisiteIdList.push(prerequisite.PrerequisiteId)
                    })
                    CheckPrerequisiteFiles(prerequisiteIdList, trainingId)
                }
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
function CheckPrerequisiteFiles(prerequisiteIdList, trainingId) {
    let IsValid = true;
    $.ajax({
        type: "GET",
        url: "/RequiredFile/CoutPresentFile",
        data: { trainingId: trainingId },
        success: function (result) {
            if (result.Success == true) {
                if (result.Data == prerequisiteIdList.length) {
                    console.log(result.Data,prerequisiteIdList.length)
                    Enroll(trainingId);
                } else {
                    ShowNotification(false, "Error", "File(s) are missing for this training");
                }
            }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Communication has been interupted");
        }
    });
}
function Enroll(trainingId) {
    $.ajax({
        type: "POST",
        url: "/Enrollment/RegisterEnrollment",
        data: { trainingId: trainingId },
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                GetAvailableTrainingList();
                GetStateList();
                ShowNotification(true, "Success", "Successfully enrolled")
                let overlay = document.getElementById("screenOverlay");
                let enrollButton = document.getElementById("enrollBtn");
                let uploadForm = document.getElementById("upload-form-container");
                uploadForm.style.visibility = "hidden";
                enrollButton.style.visibility = "hidden";
                overlay.style.visibility = "hidden";
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
            else { ShowNotification(false, "Error", "File size is not supported ! maximum size is 10 MB"); uploadFlag = false; }
        }
        else { ShowNotification(false, "Error", "File type is not supported ! please select a file from the following format : PDF"); uploadFlag = false; };
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
            if (result.Success == true) {
                ShowNotification(true, "Success", "File update successfully");
                document.getElementById('uploadBtn').style.visibility = 'hidden';
                GetPrerequisiteFiles();
                HideDetail()
            }
            else { ShowNotification(false, "Error", result.Message); }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Failed to upload file");
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
            if (result.Success == true) {
                ShowNotification(true, "Success", "File uploaded successfully");
                document.getElementById('uploadFormId').style.backgroundColor = '#a1ffa4';
                document.getElementById('uploadBtn').style.visibility = 'hidden';
                GetPrerequisiteFiles();
                HideDetail()
            }
            else { ShowNotification(false, "Error", result.Message); }
        },
        error: function (error) {
            ShowNotification(false, "Error", "Failed to upload file");
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
            ShowNotification(false, "Error, File could not be load");
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
            if (result.Success == true) {
                let fileForm = $('#uploadFormId');
                fileForm.empty();
                let prerequisites = result.Data

                let overlay = document.getElementById("screenOverlay");
                let trainingTitle = document.getElementById("detailTitle");
                let trainingDepartment = document.getElementById("detailDepartmentPriority");
                let trainingDescription = document.getElementById("detailDescription");
                let trainingDate = document.getElementById("detailDate");

                trainingTitle.textContent = "";
                trainingDepartment.textContent = "";
                trainingDescription.textContent = "";
                trainingDate.textContent = "";

                let enrollButton = document.getElementById("enrollBtn");
                let uploadForm = document.getElementById("upload-form-container");
                enrollButton.style.visibility = "hidden";
                uploadForm.style.visibility = "visible";
                overlay.style.visibility = "visible";

                prerequisites.forEach(function (prerequisite) {
                    let row = `<label for='uploadFileId${prerequisite.PrerequisiteId}'> ${prerequisite.PrerequisiteDescription} </label><br>
                                <input type='file' name='file' id='uploadFileId${prerequisite.PrerequisiteId}' />
                                <input type='button' id='uploadBtn' value='Upload' onclick='UploadFile(${prerequisite.PrerequisiteId},true)'><br>`;
                    fileForm.append(row);
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
function Upload(prerequisiteId) {
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisite",
        data: { prerequisiteId: prerequisiteId },
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                let fileForm = $('#uploadFormId');
                fileForm.empty();
                let prerequisites = result.Data;
                let overlay = document.getElementById("screenOverlay");
                let trainingTitle = document.getElementById("detailTitle");
                let trainingDepartment = document.getElementById("detailDepartmentPriority");
                let trainingDescription = document.getElementById("detailDescription");
                let trainingDate = document.getElementById("detailDate");

                trainingTitle.textContent = "";
                trainingDepartment.textContent = "";
                trainingDescription.textContent = "";
                trainingDate.textContent = "";

                let enrollButton = document.getElementById("enrollBtn");
                let uploadForm = document.getElementById("upload-form-container");
                enrollButton.style.visibility = "hidden";
                uploadForm.style.visibility = "visible";

                overlay.style.visibility = "visible";

                prerequisites.forEach(function (prerequisite) {
                    let row = `<label for='uploadFileId${prerequisite.PrerequisiteId}'> ${prerequisite.PrerequisiteDescription} </label><br>
                                <input type='file' name='file' id='uploadFileId${prerequisite.PrerequisiteId}' />
                                <input type='button' id='uploadBtn' value='Upload' onclick='UploadFile(${prerequisite.PrerequisiteId},false)'><br>`;
                    fileForm.append(row);
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
//#endregion

//#region function
function DisplayTab(event, tabId) {
    let tabs = document.getElementsByName("tabArea")
    tabs.forEach((tab) => tab.style.display = 'none')

    let tabButtons = document.getElementsByName("tabButton")
    tabButtons.forEach((tabButton) => tabButton.style.backgroundColor = "#d2d2d2")

    let displayTab = document.getElementById(tabId);
    displayTab.style.display = "inherit";

    console.log(displayTab.style.display)

    event.currentTarget.style.backgroundColor = "#ffffff";
}
function HideTab() {
    let tabs = document.getElementsByName("tabArea")
    tabs.forEach((tab) => tab.style.display = 'none')

    tabs[0].style.display = 'inherit';

    let tabButtons = document.getElementsByName("tabButton")
    tabButtons[0].style.backgroundColor = "#ffffff";
}
//#endregion

