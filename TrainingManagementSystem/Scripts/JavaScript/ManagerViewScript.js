$(document).ready(
    GetEnrollment()
)
//GLOBAL VARIABLE
let index = 0

//#region DataTable
function GetEnrollment() {
    DisplaySpinner()
    $.ajax({
        type: "GET",
        url: "/Account/GetEmployeeEnrolled",
        success: function (result) {
            if (result.Success == true) {
                if ($.fn.DataTable.isDataTable('#employeeEnrollmentTableId')) {
                    $('#employeeEnrollmentTableId').DataTable().destroy();
                }
                $('#employeeEnrollmentTableId').DataTable({
                    "data": result.Data,
                    "columns": [
                        {
                            render: function (data, type, row) {
                                if (row.OtherName == null) {
                                    return `${row.FirstName} ${row.LastName}`;
                                }
                                else {
                                    return `${row.FirstName} ${row.OtherName} ${row.LastName}`;
                                }
                            }
                        },
                        {
                            render: function (data, type, row) {
                                let emailParameter = JSON.stringify(row.Email)
                                return `<button class='item-button' id='detailBtn' onclick='GetRequestByEmployee(${row.AccountId},${emailParameter})'> See Requests</button>`;
                            }
                        }
                    ],
                });
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
            RemoveSpinner();
        },
        error: function () {
            RemoveSpinner();
            ShowNotification(false, "Error", "Communication has been interupted");
        },
    });
};
function GetRequestByEmployee(requestAccountId, requestAccountEmail) {
    let buttonObject = [];
    let actualId = 0;
    DisplaySpinner();
    $.ajax({
        type: "GET",
        url: "/ViewModel/GetTrainingEnrollmentViewModel",
        data: { accountId: requestAccountId },
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                if ($.fn.DataTable.isDataTable('#requestTableId')) {
                    $('#requestTableId').DataTable().destroy();
                }
                let approve = 3;
                let reject = 2;
                $('#requestTableId').DataTable({
                    "data": result.Data,
                    "columns": [
                        { "data": "Title" },
                        {
                            "data": "StartDate",
                            render: function (data) {
                                return (new Date(Number((data).match(/\d+/)[0]))).toString().split(' GMT')[0];;
                            },
                        },
                        { "data": "ShortDescription" },
                        {
                            render: function (data, type, row) {
                                if (row.PrerequisiteId > 0) {
                                    let button = `<button  class='item-button' id='viewDocumentBtn${row.TrainingId}' >Document(s)</button>`
                                    if (actualId != row.TrainingId) {
                                        buttonObject.push({ trainingId: row.TrainingId, accountId: row.AccountId, buttonId: `viewDocumentBtn${row.TrainingId}` })
                                        actualId = row.TrainingId
                                    }
                                    return button;
                                }
                                else {
                                    return "No document required";
                                }
                            }
                        },
                        {
                            render: function (data, type, row) {
                                let approvedEnrollmentParameter = {
                                    EnrollmentId: row.EnrollmentId
                                    , AccountId: row.AccountId
                                    , TrainingId: row.TrainingId
                                    , StateId: approve
                                    , SubmissionDate: row.SubmissionDate
                                };
                                approvedEnrollmentParameter = JSON.stringify(approvedEnrollmentParameter);
                                let rejectEnrollmentParameter = {
                                    EnrollmentId: row.EnrollmentId
                                    , AccountId: row.AccountId
                                    , TrainingId: row.TrainingId
                                    , StateId: reject
                                    , SubmissionDate: row.SubmissionDate
                                };
                                rejectEnrollmentParameter = JSON.stringify(rejectEnrollmentParameter);
                                let comment = "_"
                                let buttons =
                                    `<div class='split-Area'>
                                        <button class='item-button' id='detailBtn' onclick='UpdatRequestState( ${approvedEnrollmentParameter} ,${requestAccountId}, ${JSON.stringify(row.Title)}, ${JSON.stringify(requestAccountEmail)}, ${comment} )'>Approve</button> 
                                        <button class='item-button' id='detailBtn' onclick='RejectRequest( ${rejectEnrollmentParameter} , ${requestAccountId}, ${JSON.stringify(row.Title)} , ${JSON.stringify(requestAccountEmail)} )'> Reject</button>
                                    </div>`; 

                            return buttons
                            }
                        },
                    ],
                });
                buttonObject.forEach((button) => {
                    GetPrerequisiteByTraining(button.trainingId, button.accountId, button.buttonId);
                })
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
            let overlay = document.getElementById("screenOverlay");
            overlay.style.visibility = "visible";
            RemoveSpinner();
        },
        error: function () {
            RemoveSpinner();
            ShowNotification(false, "Error", "Communication has been interupted");
        },
    });
};
//#endregion
//#region  Overlay
function CloseTextArea() {
    let rejectionComment = document.getElementById("rejectionReasonid");
    let overlay = document.getElementById("commentContainerId");
    rejectionComment.value = "";
    overlay.style.visibility = "hidden";
}
function HideRequest() {
    let overlay = document.getElementById("screenOverlay");
    overlay.style.visibility = "hidden";
}
function GetDocument(prerequisiteIds, employeeId) {
    prerequisiteIds.forEach((prerequisiteId) => {
        $.ajax({
            type: "GET",
            url: "/RequiredFile/GetFile",
            data: { prerequisiteId: prerequisiteId, accountId: employeeId },
            xhrFields: {
                responseType: 'blob'
            },
            success: function (result) {
                let url = URL.createObjectURL(result);
                window.open(url, '_blank');
            },
            error: function () {
                ShowNotification(false, "Error", "File could not be load");
            }
        });
    }); 
}
//#endregion
//#region ManagerOption
function UpdatRequestState(enrollmentParameter, requestEmployeeId, requestTrainingTitle, requestAccountEmail) {
    let rejectionComment = document.getElementById("rejectionReasonid").value;
    DisplaySpinner()
    $.ajax({
        type: "POST",
        url: "/Enrollment/UpdateState",
        data: { enrollment: enrollmentParameter, trainingTitle: requestTrainingTitle, email: requestAccountEmail, comment: rejectionComment },
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                CreateNotification(requestEmployeeId, enrollmentParameter.StateId, requestTrainingTitle, rejectionComment, requestAccountEmail )
                GetRequestByEmployee(requestEmployeeId,requestAccountEmail)
                GetEnrollment()
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
            RemoveSpinner();
        },
        error: function () {
            RemoveSpinner();
            ShowNotification(false, "Error", "Communication has been interupted");
        },
    });
};
function RejectRequest(enrollmentParameter, requestEmployeeId, requestTrainingTitle, requestAccountEmail) {
    let overlay = document.getElementById("commentContainerId");
    enrollmentParameter = JSON.stringify(enrollmentParameter);
    overlay.style.visibility = "visible";
    document.getElementById("submitRejectionCommentBtn").setAttribute("onclick", `UpdatRequestState( ${enrollmentParameter} ,${requestEmployeeId}, ${JSON.stringify(requestTrainingTitle)} , ${JSON.stringify(requestAccountEmail)} );`);
};
//#endregion
function GetPrerequisiteByTraining(trainingId, accountId, buttonId) {
    let prerequisiteList = "[";
    let index = 0;
    $.ajax({
        type: "GET",
        url: "/Prerequisite/GetPrerequisiteByTraining",
        data: { trainingId: trainingId },
        success: function (result) {
            if (result.Success == true) {
                result.Data.forEach((prerequisite) => {
                    prerequisiteList += `${prerequisite.PrerequisiteId},`;
                    index += 1;
                })
                let prerequisites = prerequisiteList.substring(0, prerequisiteList.length - 1);
                prerequisites +="]"
                button = document.getElementById(buttonId);
                button.setAttribute("onclick", `GetDocument( ${prerequisites}, ${accountId} );`);
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
        },
        error: function () {
            ShowNotification(false, "Error", "File could not be load");
        }
    });
}

