$(document).ready(
    GetEnrollment()
)
//#region DataTable
function GetEnrollment() {
    $.ajax({
        type: "GET",
        url: "/Account/GetEmployeeEnrolled",
        success: function (result) {
            if (result.message == "success") {
                if ($.fn.DataTable.isDataTable('#employeeEnrollmentTableId')) {
                    $('#employeeEnrollmentTableId').DataTable().destroy();
                }
                $('#employeeEnrollmentTableId').DataTable({
                    "data": result.data,
                    "columns": [
                        {
                            render: function (data, type, row) {
                                return row.FirstName + ' ' + row.OtherName + ' ' + row.LastName;
                            }
                        },
                        {
                            "data": "AccountId",
                            render: function (data) {
                                return "<button class='item-button' id='detailBtn' onclick='GetRequestByEmployee(" + data + ")'> See Requests</button>";
                            }
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


function GetRequestByEmployee(requestAccountId) {
    $.ajax({
        type: "GET",
        url: "/ViewModel/GetTrainingEnrollmentViewModel",
        data: { accountId: requestAccountId },
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                if ($.fn.DataTable.isDataTable('#requestTableId')) {
                    $('#requestTableId').DataTable().destroy();
                }
                let approve = 3;
                let reject = 2;
                $('#requestTableId').DataTable({
                    "data": result.data,
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
                            "data": "EnrollmentId",
                            render: function (data) {
                                return "<button class='item-button' id='detailBtn' onclick='UpdatRequestState(" + data + ", " + approve + " , " + requestAccountId + ")'>Approve</button>";
                            }
                        },
                        {
                            "data": "EnrollmentId",
                            render: function (data) {
                                return "<button class='item-button' id='detailBtn' onclick='RejectRequest(" + data + ", " + reject + " , " + requestAccountId + ")'>Reject</button>";
                            }
                        }
                    ],
                });
            }
            else {
                ShowNotification("Error", result.data);
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
function UpdatRequestState(requestEnrollmentId, requestState, requestEmployeeId) {
    let data = { enrollmentId: requestEnrollmentId, state: requestState }
    $.ajax({
        type: "POST",
        url: "/Enrollment/UpdateState",
        data: data,
        dataType: 'json',
        success: function (result) {
            if (result.message == "success") {
                GetRequestByEmployee(requestEmployeeId)
                GetEnrollment()
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
function RejectRequest(requestEnrollmentId, requestState, requestEmployeeId) {
    let overlay = document.getElementById("commentContainerId");
    overlay.style.visibility = "visible";
    document.getElementById("submitRejectionCommentBtn").setAttribute("onclick", "SubmitRejectionReason(" + requestEnrollmentId + " , " + requestState + " , "  + requestEmployeeId + ");");
};
function SubmitRejectionReason(requestEnrollmentId, requestState, requestEmployeeId) {
    let rejectionComment = document.getElementById("rejectionReasonid").value;
    $.ajax({
        type: "POST",
        url: "/Rejection/SetRejectionComment",
        data: { enrollmentId: requestEnrollmentId, comment: rejectionComment },
        success: function (result) {
            if (result.message == "success") {
                UpdatRequestState(requestEnrollmentId, requestState, requestEmployeeId);
                CloseTextArea();
            }
            else {
                ShowNotification("Error", result.data);
            };
        },
        error: function (result) {
            ShowNotification("Error", "Communication has been interupted");
        },
    });
}
//#endregion

