
var departmentsData = []
function LoadDepartment(element) {
    if (departmentsData.length == 0) {
        //debugger;
        $.ajax({
            type: "GET",
            url: "/User/LoadDepartment",
            success: function (data) {
                departmentsData = data.data;
                RenderDepartment(element);
            }
        })
    }
    else {
        RenderDepartment(element);
    }
}

function RenderDepartment(element) {
    //debugger;
    var $e = $(element);
    $e.empty();
    $e.append($('<option/>').val('0').text('Select Department').hide());
    $.each(departmentsData, function (i, val) {
        $e.append($('<option/>').val(val.id).text(val.name));
    })
}
LoadDepartment($('#CBDept'));

function Insert() {
    var Employee = new Object();
    Employee.email = $('#Email').val();
    Employee.password = $('#Password').val();
    Employee.firstName = $('#FirstName').val();
    Employee.lastName = $('#LastName').val();
    Employee.department_Id = $('#CBDept').val();
    Employee.birthDate = $('#BirthDate').val();
    Employee.phoneNumber = $('#PhoneNumber').val();
    Employee.address = $('#Address').val();
    $.ajax({
        type: 'POST',
        url: '/User/Insert/',
        data: Employee
    }).then((result) => {
        if (result.statusCode === 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Register Successfully',
                timer: 5000
            }).then(function () {
                window.location.href = '/User/Login';
            });
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
        }
    });
}

function Edit() {

    var Employee = new Object();
    Employee.firstName = $('#FirstName').val();
    Employee.lastName = $('#LastName').val();
    Employee.department_Id = $('#CBDept').val();
    Employee.email = $('#Email').val();
    Employee.birthDate = $('#BirthDate').val();
    Employee.phoneNumber = $('#PhoneNumber').val();
    Employee.address = $('#Address').val();
    $.ajax({
        type: 'POST',
        url: '/User/Edit/',
        data: Employee
    }).then((result) => {
        if (result.statusCode === 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee Updated Successfully',
                timer: 5000
            }).then(function () {
                
            });
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            
        }
    });
}
