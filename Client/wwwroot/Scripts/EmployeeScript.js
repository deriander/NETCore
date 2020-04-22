$(document).ready(function () {
    $.fn.DataTable.ext.errMode = 'none';
    $('#empTable').DataTable({
        "ajax": {
            url: "/Employee/LoadEmployee",
            type: "GET",
            dataType: "json",
            //dataSrc: ""
        },
        "columnDefs": [
            { "orderable": false, "targets": 8 },
            { "searchable": false, "targets": 8 }
        ],
        "columns": [
            { "data": "fullName" },
            { "data": "departmentName" },
            { "data": "email" },
             {
                 "data": "birthDate", "render": function (data) {
                     return moment(data).format('DD/MM/YYYY');
                 }
             },
            { "data": "phoneNumber" },
            { "data": "address" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).format('MMMM Do YYYY, h:mm:ss a');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var notupdate = "Not update yet";
                    if (data === null) {
                        return notupdate;
                    } else {
                        return moment(data).format('MMMM Do YYYY, h:mm:ss a');
                    }
                }
            },
            {
                "data": null, "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="EditBtn" data-toggle="tooltip" data-placement="top" title="Edit" onclick="return GetById(\'' + row.email + '\')"><i class="mdi mdi-pencil"></i></button> <button type="button" class="btn btn-danger" id="DeleteBtn" data-toggle="tooltip" data-placement="top" title="Delete" onclick="return Delete(\'' + row.email + '\')"><i class="mdi mdi-delete"></i></button>';
                }
            }
        ]
    });
});

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

function ShowModal() {
    $('#myModal').modal('show');
    $('#Id').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#CBDept').val('0');
    $('#Email').val('');
    $('#BirthDate').val('');
    $('#PhoneNumber').val('');
    $('#Address').val('');
    $('#UpdateBtn').hide();
    $('#SaveBtn').show();
    $('#Email').show();
    $('#Password').show();
    $('#labelPassword').show();
    $('#labelEmail').show();
}


function MyTableReload() {
    var rtable = $('#empTable').DataTable({
        ajax: "data.json"
    });

    rtable.ajax.reload();
}

var departmentsData = []
function LoadDepartment(element) {
    if (departmentsData.length == 0) {
        //debugger;
        $.ajax({
            type: "GET",
            url: "/Department/LoadDepartment",
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

function Save() {
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
        url: '/Employee/Insert/',
        data: Employee
    }).then((result) => {
        if (result.statusCode === 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee Added Successfully',
                timer: 5000
            }).then(function () {
                MyTableReload();
            });
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            MyTableReload();
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
        url: '/Employee/Edit/',
        data: Employee
    }).then((result) => {

        if (result.statusCode === 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee Updated Successfully',
                timer: 5000
            }).then(function () {
                MyTableReload();
            });
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            MyTableReload();
        }
    });
}

function Delete(Email) {
    Swal.fire({
        title: "Are you sure?",
        showCanceButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: '/Employee/Delete/',
                data: { "Email": Email }
            }).then((result) => {
                if (result.statusCode === 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Employee Deleted Successfully'
                    }).then((result) => {
                        if (result.value) {
                            MyTableReload();
                        }
                    });
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ShowModal();
                }
            });
        }
    });
}

function GetById(Email) {
    $.ajax({
        url: "/Employee/GetById/" + Email,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: {"Email": Email},
        success: function (result) {
            var _result = JSON.parse(result);
            var obj = JSON.parse(_result);
            $('#FirstName').val(obj.firstName);
            $('#LastName').val(obj.lastName);
            $('#CBDept').val(obj.department_Id);
            $('#Email').val(obj.email);
            $('#BirthDate').val(moment(obj.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(obj.phoneNumber);
            $('#Address').val(obj.address);
            $('#myModal').modal('show');
            $('#UpdateBtn').show();
            $('#SaveBtn').hide();
            $('#Email').hide();
            $('#Password').hide();
            $('#labelPassword').hide();
            $('#labelEmail').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}