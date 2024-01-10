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
                                    return row.FirstName + ' ' + row.LastName;
                                }
                                else {
                                    return row.FirstName + ' ' + row.OtherName + ' ' + row.LastName;
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
            RemoveSpinnner();
        },
        error: function () {
            RemoveSpinnner();
            ShowNotification(false, "Error", "Communication has been interupted");
        },
    });
};
function GetRequestByEmployee(requestAccountId, requestAccountEmail) {
    let buttonIndex = 0;
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
                                    GetPrerequisiteByTraining(row.TrainingId, row.AccountId, `viewDocumentBtn${buttonIndex}`);
                                    let button = `<button class='item-button' id='viewDocumentBtn${buttonIndex}' >Document(s)</button>`
                                    buttonIndex += 1;
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
                                let buttons =
                                    `<div class='split-Area'>
                                        <button class='item-button' id='detailBtn' onclick='UpdatRequestState( ${approvedEnrollmentParameter} , ${requestAccountId} ,  ${JSON.stringify(requestAccountEmail)})'>Approve</button> 
                                        <button class='item-button' id ='detailBtn' onclick='RejectRequest( ${rejectEnrollmentParameter} , ${requestAccountId} , ${JSON.stringify(requestAccountEmail)} )'> Reject</button>
                                    </div>`; 

                            return buttons
                                
                            }
                        },
                    ],
                });
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
            let overlay = document.getElementById("screenOverlay");
            overlay.style.visibility = "visible";
            RemoveSpinnner();
        },
        error: function () {
            RemoveSpinnner();
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
function UpdatRequestState(enrollmentParameter, requestEmployeeId, requestAccountEmail) {
    let data = { enrollment: enrollmentParameter };
    DisplaySpinner()
    $.ajax({
        type: "POST",
        url: "/Enrollment/UpdateState",
        data: data,
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                GetRequestByEmployee(requestEmployeeId,requestAccountEmail)
                GetEnrollment()
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
            RemoveSpinnner();
        },
        error: function () {
            RemoveSpinnner();
            ShowNotification(false, "Error", "Communication has been interupted");
        },
    });
};
function RejectRequest(enrollmentParameter, requestEmployeeId, requestAccountEmail) {
    let overlay = document.getElementById("commentContainerId");
    enrollmentParameter = JSON.stringify(enrollmentParameter);
    overlay.style.visibility = "visible";
    document.getElementById("submitRejectionCommentBtn").setAttribute("onclick", `SubmitRejectionReason( ${enrollmentParameter} , ${requestEmployeeId} , ${JSON.stringify(requestAccountEmail)} );`);
};
function SubmitRejectionReason(enrollmentParameter, requestEmployeeId, requestAccountEmail) {
    let rejectionComment = document.getElementById("rejectionReasonid").value;
    let requestEnrollmentId = enrollmentParameter.EnrollmentId;
    DisplaySpinner();
    $.ajax({
        type: "POST",
        url: "/Rejection/SetRejectionComment",
        data: { enrollmentId: requestEnrollmentId, email: requestAccountEmail,comment: rejectionComment },
        success: function (result) {
            if (result.Success == true) {
                UpdatRequestState(enrollmentParameter, requestEmployeeId, requestAccountEmail);
                rejectionComment.value = "";
                CloseTextArea();
            }
            else {
                ShowNotification(false, "Error", result.Message);
            };
            RemoveSpinnner();
        },
        error: function (result) {
            RemoveSpinnner();
            ShowNotification(false, "Error", "Communication has been interupted");
        },
    });
}
//#endregion
function GetPrerequisiteByTraining(trainingId, accountId, buttonId) {
    let prerequisiteList = "[";
    let index = 0;
    DisplaySpinnner();
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
            RemoveSpinnner();
        },
        error: function () {
            RemoveSpinnner();
            ShowNotification(false, "Error", "File could not be load");
        }
    });
}

