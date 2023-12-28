$(document).ready(
    GetEnrollment()
)
//#region DataTable
function GetEnrollment() {
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
                ShowNotification("Error", result.Message);
            };
        },
        error: function () {
            ShowNotification("Error", "Communication has been interupted");
        },
    });
};


function GetRequestByEmployee(requestAccountId,requestAccountEmail) {
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
                                return new Date(Number((data).match(/\d+/)[0]));
                            },
                        },
                        { "data": "ShortDescription" },
                        {
                            "data": "PrerequisiteId",
                            render: function (data) {
                                if (data != 0) {
                                    return "<button class='item-button' id='viewDocumentBtn' onclick='GetDocument(" + data + " , " + requestAccountId + ")'>Document(s)</button>";
                                }
                                else {
                                    return "No document required";
                                }
                            }
                        },
                        {
                            render: function (data, type, row) {
                                let enrollmentParameter = {
                                    EnrollmentId: row.EnrollmentId
                                    , AccountId: row.AccountId
                                    , TrainingId: row.TrainingId
                                    , StateId: approve
                                    , SubmissionDate: row.SubmissionDate
                                };
                                enrollmentParameter = JSON.stringify(enrollmentParameter);

                                return `<button class='item-button' id='detailBtn' onclick='UpdatRequestState( ${enrollmentParameter} , ${requestAccountId} )'>Approve</button>`;
                            }
                        },
                        {
                            render: function (data, type, row) {
                                let enrollmentParameter = {
                                    EnrollmentId: row.EnrollmentId
                                    , AccountId: row.AccountId
                                    , TrainingId: row.TrainingId
                                    , StateId: reject
                                    , SubmissionDate: row.SubmissionDate
                                };
                                enrollmentParameter = JSON.stringify(enrollmentParameter);
                                return `<button class='item-button' id='detailBtn' onclick='RejectRequest( ${enrollmentParameter} , ${requestAccountId} ,  ${JSON.stringify(requestAccountEmail)} )'>Reject</button>`;
                            }
                        }
                    ],
                });
            }
            else {
                ShowNotification("Error", result.Message);
            };
            let overlay = document.getElementById("screenOverlay");
            overlay.style.visibility = "visible";
        },
        error: function () {
            ShowNotification("Error", "Communication has been interupted");
        },
    });
};
//#endregion

//#region  Overlay
function CloseTextArea() {
    let overlay = document.getElementById("commentContainerId");
    overlay.style.visibility = "hidden";
}
function HideRequest() {
    let overlay = document.getElementById("screenOverlay");
    overlay.style.visibility = "hidden";
}
function GetDocument(prerequisiteId, employeeId) {
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
            ShowNotification("Error, File could not be load");
        }
    });
}
//#endregion

//#region ManagerOption
function UpdatRequestState(enrollmentParameter, requestEmployeeId ) {
    let data = { enrollment: enrollmentParameter }  ;
    $.ajax({
        type: "POST",
        url: "/Enrollment/UpdateState",
        data: data,
        dataType: 'json',
        success: function (result) {
            if (result.Success == true) {
                GetRequestByEmployee(requestEmployeeId)
                GetEnrollment()
            }
            else {
                ShowNotification("Error", result.Message);
            };
        },
        error: function () {
            ShowNotification("Error", "Communication has been interupted");
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
    $.ajax({
        type: "POST",
        url: "/Rejection/SetRejectionComment",
        data: { enrollmentId: requestEnrollmentId, email: requestAccountEmail,comment: rejectionComment },
        success: function (result) {
            if (result.Success == true) {
                UpdatRequestState(requestEnrollmentId, requestState, requestEmployeeId);
                CloseTextArea();
            }
            else {
                ShowNotification("Error", result.Message);
            };
        },
        error: function (result) {
            ShowNotification("Error", "Communication has been interupted");
        },
    });
}
//#endregion

