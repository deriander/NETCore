$(document).ready(function GetData() {
    $.ajax({
        url: "/User/GetById/",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var _result = JSON.parse(result);
            var obj = JSON.parse(_result);
            $('#Email').val(obj.email);
            $('#FirstName').val(obj.firstName);
            $('#LastName').val(obj.lastName);
            $('#CBDept').val(obj.department_Id);
            $('#BirthDate').val(moment(obj.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(obj.phoneNumber);
            $('#Address').val(obj.address);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
})