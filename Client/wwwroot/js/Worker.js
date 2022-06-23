$(document).ready(function () {
    var table = $('#tableGridWorkerList').DataTable({
        ajax: {
            "url": "https://localhost:44309/API/Employees/GetEmployeeData",
            "dataType": "json",
            "dataSrc": "data"
        },
        dom: 'lBfrtip',
        buttons: {
            buttons: [
                {
                    extend: 'csv',
                    className: 'ml-1 mr-1',
                    exportOptions: {
                        columns: [0, 1, 3, 4, 5]
                    }
                },
                {
                    extend: 'excel',
                    className: 'ml-1 mr-1',
                    exportOptions: {
                        columns: [0, 1, 3, 4, 5]
                    }
                },
                {
                    extend: 'pdf',
                    className: 'ml-1 mr-1',
                    exportOptions: {
                        columns: [0, 1, 3, 4, 5]
                    }
                }
            ]
        },
        columns: [
            {
                data: 'nik',
                render: function (data) {
                    return `<a class="text-info" href="#" onclick="getDetails('${data}')" data-toggle="modal" data-target="#modalEmployee">${data}</a>`;
                }
            },
            {
                data: 'firstName'
            },
            {
                data: 'lastName'
            },
            {
                data: 'email'
            },
            {
                data: 'phone'
            },
            {
                data: 'birthDate',
                render: function (data) {
                    var date = moment(data);

                    return '<span>' + date.format('DD/MM/YYYY') + '</span>';
                }
            },
            //{
            //    data: 'salary',
            //    render: function (data, type) {
            //        var number = $.fn.dataTable.render
            //            .number(',', '.', 2, 'Rp. ')
            //            .display(data);

            //        return '<span>' + number + '</span>';
            //    }
            //}
        ],
        columnDefs: [
            {
                targets: [1],
                render: function (data, type, row) {
                    return toPascalCase(data) + ' ' + toPascalCase(row.lastName) + '';
                },
                width: "20%"
            },
            {
                visible: false, targets: [2]
            }
        ],
    });
    $('#formEditEmployee').validate({
        rules: {
            formEmail: {
                required: true,
                email: true
            }
        },
        message: {
            formEmail: {
                email: "Format email yang diterima adalah abc@domain.com"
            }
        }
    });
});

function toPascalCase(string) {
    return `${string}`
        .toLowerCase()
        .replace(new RegExp(/[-_]+/, 'g'), ' ')
        .replace(new RegExp(/[^\w\s]/, 'g'), '')
        .replace(
            new RegExp(/\s+(.)(\w*)/, 'g'),
            ($1, $2, $3) => `${$2.toUpperCase() + $3}`
        )
        .replace(new RegExp(/\w/), s => s.toUpperCase());
}

function getDetails(nik) {
    let objReq = { NIK: nik };
    $.ajax({
        type: "post",
        url: "https://localhost:44309/API/Employees/GetEmployeeDetail",
        data: JSON.stringify(objReq),
        contentType: "application/json; charset=utf-8"
    }).done((res) => {
        console.log(res);

        //Edit Employee Data
        document.getElementById("formFirstName").value = toPascalCase(res.data.firstName);
        document.getElementById("formLastName").value = toPascalCase(res.data.lastName);
        document.getElementById("formEmail").value = res.data.email;
        document.getElementById("formPhone").value = res.data.phone;
        document.getElementById("formDate").value = moment(res.data.birthDate).format('yyyy-MM-DD');
        document.getElementById("formNIK").value = res.data.nik;
        document.getElementById("formSalary").value = res.data.salary;
        document.getElementById("formGender").value = res.data.gender;

        //Edit Education Data
        let uniId = 1;
        if (res.data.universityName[0] === "Universitas Pokemon") {
            uniId = 1;
        }
        else if (res.data.universityName[0] === "GachArena") {
            uniId = 2;
        }
        else {
            uniId = 3;
        }
        document.getElementById("formUniversity").value = uniId;

        let degreeId = 1;
        if (res.data.educationDegree[0] === "D3") { degreeId = 0; }
        else if (res.data.educationDegree[0] === "D4") { degreeId = 1; }
        else if (res.data.educationDegree[0] === "S1") { degreeId = 2;}
        else if (res.data.educationDegree[0] === "S2") { degreeId = 3;}
        else { degreeId = 4}
        document.getElementById("formDegree").value = degreeId;
        document.getElementById("formGPA").value = res.data.educationGPA[0];

        //Preview Roles Data
        let roleList = res.data.roleName;     
        let roleDetail = "";

        for (var i = 0; i < roleList.length; i++) {
            roleDetail += `         <span class="badge badge-success">${roleList[i]}</span>`;
        }

        let empName = toPascalCase(res.data.firstName) + " " + toPascalCase(res.data.lastName);
        let title = `<h5 class="modal-title" id="modalEmployeeTitle">${res.data.nik} - ${empName}</h5>`;

        $("#modalEmployeeTitle").html(title);
        $("#innerDetailRolesPlaceholder").html(roleDetail);
        //console.log(obj);
    });
}

//function addEducationSection() {
//    var eduDiv = document.getElementById("divEducationSection");

//    var lastDiv = $('.form-education').length;
    
//    lastDiv++;
//    console.log(lastDiv);


//    let eduSection = `<div id="divInnerEducation${lastDiv}" class="form-education form-row mt-2">
//                            <div class="col-md-4 mb-3">
//                                <label for="inputUniversity${lastDiv}">University</label>
//                                <select id="inputUniversity${lastDiv}" class="form-control" required>
//                                    <option value="1" selected>Universitas Pokemon</option>
//                                    <option value="2">GachArena</option>
//                                    <option value="3">Poke Institute</option>
//                                </select>
//                                <div class="invalid-feedback">
//                                    Please choose a University.
//                                </div>
//                                <div class="valid-feedback">
//                                    Looks good!
//                                </div>
//                            </div>
//                            <div class="col-md-2 mb-3">
//                                <label for="inputDegree${lastDiv}">Degree</label>
//                                <select id="inputDegree${lastDiv}" class="form-control" required>
//                                    <option value="1" selected>D3</option>
//                                    <option value="2">D4</option>
//                                    <option value="3">S1</option>
//                                    <option value="4">S2</option>
//                                    <option value="5">S3</option>
//                                </select>
//                                <div class="invalid-feedback">
//                                    Please choose a Degree.
//                                </div>
//                                <div class="valid-feedback">
//                                    Looks good!
//                                </div>
//                            </div>
//                            <div class="col-md-2 mb-3">
//                                <label for="validationCustom05${lastDiv}">GPA</label>
//                                <input type="text" class="form-control" id="validationCustom05${lastDiv}" placeholder="GPA" value="3.50" required>
//                                <div class="valid-feedback">
//                                    Looks good!
//                                </div>
//                            </div>
//                        </div>`;

//    eduDiv.innerHTML += eduSection;

//    document.getElementById("btnDelEdu").disabled = false;
//}

//function delEducationSection() {
//    var eduDiv = document.getElementById("divEducationSection");

//    var lastDiv = $('.form-education').length;
//    console.log(lastDiv);

//    let eduSectionName = "divInnerEducation" + lastDiv;
//    console.log(eduSectionName);

//    let lastEduSection = document.getElementById(eduSectionName);
//    console.log(lastEduSection);

//    lastEduSection.parentElement.removeChild(lastEduSection);

//    if (lastDiv - 1 === 0) {
//        document.getElementById("btnDelEdu").disabled = true;
//    }
//    else {
//        document.getElementById("btnDelEdu").disabled = false;
//    }
    
//}

function registerData(firstName, lastName, email, phone, gender, birthDate, degree, gpa, university)
{
    console.log("Masuk proses post");
    let objReq = {
        FirstName: firstName,
        LastName: lastName,
        Email: email,
        Phone: phone,
        Gender: gender,
        BirthDate: birthDate,
        Salary: parseFloat(0),
        Password: "defaultPassword",
        Degree: parseInt(degree),
        GPA: gpa,
        UniversityId: parseInt(university)
    }

    console.log(objReq);

    $.ajax({
        type: "POST",
        url: "https://localhost:44309/api/Employees/RegisterEmp",
        data: JSON.stringify(objReq),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        headers: {
            'Access-Control-Allow-Origin': '*',
        }
    }).done((res) => {
        console.log(res);
        if (res.statusCode === 200) {
            Swal.fire(
                res.message,
                res.data,
                'success'
            )

        }
        else {
            Swal.fire({
                icon: 'error',
                title: res.message,
                text: res.data
            })
        }
        
        return res;
    });
}

function editEmployeeData(nik, firstName, lastName, email, phone, gender, birthDate, salary) {
    let objReq = {
        NIK: nik,
        FirstName: firstName,
        LastName: lastName,
        Email: email,
        Phone: phone,
        Gender: gender,
        BirthDate: birthDate,
        Salary: parseFloat(salary)
    }

    console.log(objReq);

    $.ajax({
        type: "POST",
        url: "https://localhost:44309/api/Employees/UpdateEmp",
        data: JSON.stringify(objReq),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        headers: {
            'Access-Control-Allow-Origin': '*',
        }
    }).done((res) => {
        console.log(res);

        let toastText = "";

        if (res.statusCode === 200) {
            toastText = `<span>Employee Data successfully updated!</span>`;
        }
        else {
            toastText = `<span>Employee Data failed to update!</span>`;
        }

        showToast(toastText);

        return true;
    });
}

function editEducationData(nik, universityId, degree, gpa) {
    let objReq = {
        NIK: nik,
        UniversityId: universityId,
        Degree: degree,
        GPA: gpa
    }

    console.log(objReq);

    $.ajax({
        type: "POST",
        url: "https://localhost:44309/api/Employees/UpdateEducation",
        data: JSON.stringify(objReq),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        headers: {
            'Access-Control-Allow-Origin': '*',
        }
    }).done((res) => {
        console.log(res);

        let toastText = "";

        if (res.statusCode === 200) {
            toastText = `<span>Education Data successfully updated!</span>`;
        }
        else {
            toastText = `<span>Education Data failed to update!</span>`;
        }

        showToast(toastText);

        return true;
    });
}

function deleteEmployee() {
    let nik = document.getElementById("formNIK").value;

    let objReq = { NIK: nik }

    $.ajax({
        type: "POST",
        url: "https://localhost:44309/api/Employees/DeleteEmployee",
        data: JSON.stringify(objReq),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        headers: {
            'Access-Control-Allow-Origin': '*',
        }
    }).done((res) => {
        console.log(res);

        let toastText = "";

        if (res.statusCode === 200) {
            Swal.fire(
                res.message,
                res.data,
                'success'
            )
        }
        else {
            Swal.fire({
                icon: 'error',
                title: res.message,
                text: res.data
            })
        }

        showToast(toastText);

        return true;
    });

    $('#tableGridWorkerList').DataTable.ajax.reload();

    $('#modalEmployee').modal('toggle');
}

function confirmDelete() {
    Swal.fire({
        title: 'Delete this employee?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Delete'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteEmployee();
            
        }
    })
}

function updateEmployee() {
    const FIELD_REQUIRED = "This field is required.";
    const EMAIL_INVALID = "Please enter a valid email address format.";

    let emailValid = validateEmail(document.getElementById("formEmail").value, FIELD_REQUIRED, EMAIL_INVALID);
    let emailDuplicate = validateDuplicateData(document.getElementById("formEmail").value, "email");
    let phoneDuplicate = validateDuplicateData(document.getElementById("validationCustom03").value, "phone");
    if (emailValid && !emailDuplicate && !phoneDuplicate) {
        //Bisa post
        let nik = document.getElementById("formNIK").value;
        let firstName = document.getElementById("formFirstName").value;
        let lastName = document.getElementById("formLastName").value;
        let email = document.getElementById("formEmail").value;
        let phone = document.getElementById("formPhone").value;
        let gender = document.getElementById("formGender").value;
        let birthDate = document.getElementById("formDate").value;
        let salary = document.getElementById("formSalary").value;

        let res = editEmployeeData(nik, firstName, lastName, email, phone, gender, birthDate, salary);

        table.ajax.reload();

        $('#modalEmployee').modal('toggle');

    }
}

function updateEducation() {
    let nik = document.getElementById("formNIK").value;
    let universityId = document.getElementById("formUniversity").value;
    let degree = document.getElementById("formDegree").value;
    let gpa = document.getElementById("formGPA").value;

    let res = editEducationData(nik, universityId, degree, gpa);

    table.ajax.reload();

    $('#modalEmployee').modal('toggle');
}

function registerEmployee() {
    const FIELD_REQUIRED = "This field is required.";
    const EMAIL_INVALID = "Please enter a valid email address format.";

    let emailValid = validateEmail(document.getElementById("validationCustomEmail").value, FIELD_REQUIRED, EMAIL_INVALID);
    let emailDuplicate = validateDuplicateData(document.getElementById("validationCustomEmail").value, "email");
    let phoneDuplicate = validateDuplicateData(document.getElementById("validationCustom03").value, "phone");
    if (emailValid && !emailDuplicate && !phoneDuplicate) {
        //Set Employee Data
        let firstName = document.getElementById("validationCustom01").value;
        let lastName = document.getElementById("validationCustom02").value;
        let email = document.getElementById("validationCustomEmail").value;
        let phone = document.getElementById("validationCustom03").value;
        let gender = document.getElementById("inputGender").value;
        let birthDate = document.getElementById("validationCustom04").value;
        let degree = document.getElementById("inputDegree").value;
        let gpa = document.getElementById("validationCustom05").value;
        let university = document.getElementById("inputUniversity").value;

        let res = registerData(firstName, lastName, email, phone, gender, birthDate, degree, gpa, university);

        table.ajax.reload();

        $('#modalForm').modal('toggle');
    }
}

function showMessage(input, msg, type) {
    const message = input.pa.querySelector("small");
    message.innerText = msg;
    input.className = type ? "success" : "error";

    return type;
}

function showError(input, msg) {
    return showMessage(input, msg, false);
}
function showSuccess(input) {
    return showMessage(input, "", true);
}

function hasValue(input, msg) {
    if (input.trim() === "") {
        return false;
    }
    return true;
}

function validateEmail(input, requiredMsg, invalidMsg) {
    if (!hasValue(input, requiredMsg)) {
        return false;
    }

    const regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    const email = input.trim();
    if (!regex.test(email)) {
        return showError(input, invalidMsg);
    }
    return true;
}

function validateDuplicateData(input, type) {
    let table = document.getElementById("tableGridWorkerList");
    let itemList = [];
    let index = 0;

    if (type === "email") {
        index = 3;
    }
    else if (type === "phone") {
        index = 4;
    }

    for (let i = 1; i < table.rows.length; i++) {
        const objCells = table.rows.item(i).cells;

        for (let j = index; j <= index; j++) {
            itemList.push(objCells.item(j).innerHTML);
        }
    }

    for (let k = 0; k < itemList.length; k++) {
        if (itemList[k] === input) {
            return true;
        }
    }

    return false;

}

function showToast(msg) {
    $('#divToastBody').html(msg);
    $('#modalForm').modal('hide');
    $('#modalEmployee').modal('hide');
    $('.toast').toast('show');
}



$("#formRegisterEmployee").submit(function (e) {
    console.log("Masuk submit");
    e.preventDefault();
        
    const form = $(this);

    const FIELD_REQUIRED = "This field is required.";
    const EMAIL_INVALID = "Please enter a valid email address format.";

    console.log(form);
    let emailValid = validateEmail(document.getElementById("validationCustomEmail").value, FIELD_REQUIRED, EMAIL_INVALID);
    let emailDuplicate = validateDuplicateData(document.getElementById("validationCustomEmail").value, "email");
    let phoneDuplicate = validateDuplicateData(document.getElementById("validationCustom03").value, "phone");
    if (emailValid && !emailDuplicate && !phoneDuplicate) {
        //Set Employee Data
        let firstName = document.getElementById("validationCustom01").value;
        let lastName = document.getElementById("validationCustom02").value;
        let email = document.getElementById("validationCustomEmail").value;
        let phone = document.getElementById("validationCustom03").value;
        let gender = document.getElementById("inputGender").value;
        let birthDate = document.getElementById("validationCustom04").value;
        let degree = document.getElementById("inputDegree").value;
        let gpa = document.getElementById("validationCustom05").value;
        let university = document.getElementById("inputUniversity").value;

        let res = registerData(firstName, lastName, email, phone, gender, birthDate, degree, gpa, university);

        console.log(res);

        table.ajax.reload();

        $('#modalForm').modal('toggle');

        return true;
    }
    return false;
})
$("#formEditEmployee").submit(function (e) {
    console.log("Masuk submit");
    e.preventDefault();

    const form = $(this);

    const FIELD_REQUIRED = "This field is required.";
    const EMAIL_INVALID = "Please enter a valid email address format.";

    console.log(form);
    let emailValid = validateEmail(document.getElementById("formEmail").value, FIELD_REQUIRED, EMAIL_INVALID);
    let emailDuplicate = validateDuplicateData(document.getElementById("formEmail").value, "email");
    let phoneDuplicate = validateDuplicateData(document.getElementById("validationCustom03").value, "phone");
    if (emailValid && !emailDuplicate && !phoneDuplicate) {
        //Bisa post
        let nik = document.getElementById("formNIK").value;
        let firstName = document.getElementById("formFirstName").value;
        let lastName = document.getElementById("formLastName").value;
        let email = document.getElementById("formEmail").value;
        let phone = document.getElementById("formPhone").value;
        let gender = document.getElementById("formGender").value;
        let birthDate = document.getElementById("formDate").value;
        let salary = document.getElementById("formSalary").value;

        let res = editEmployeeData(nik, firstName, lastName, email, phone, gender, birthDate, salary);

        table.ajax.reload();

        $('#modalEmployee').modal('toggle');

        return true;
    }

    return false;
})
$("#formEditEducation").submit(function (e) {
    console.log("Masuk submit");
    e.preventDefault();

    const form = $(this);

    console.log(form);
    //Bisa post
    let nik = document.getElementById("formNIK").value;
    let universityId = document.getElementById("formUniversity").value;
    let degree = document.getElementById("formDegree").value;
    let gpa = document.getElementById("formGPA").value;

    let res = editEducationData(nik, universityId, degree, gpa);

    table.ajax.reload();

    $('#modalEmployee').modal('toggle');

    return true;
})

$("#divEmployeeSection").on('hide.bs.collapse', function () {
    $("#headerEmployeeSection").html(`<h3 id="headerEmployeeSection" data-toggle="collapse" data-target="#divEmployeeSection" aria-expanded="true"> [+] Employee Data</h3>`)
})
$("#divEmployeeSection").on('show.bs.collapse', function () {
    $("#headerEmployeeSection").html(`<h3 id="headerEmployeeSection" data-toggle="collapse" data-target="#divEmployeeSection" aria-expanded="true"> [-] Employee Data</h3>`)
})
$("#divEducationSection").on('hide.bs.collapse', function () {
    $("#headerEducationSection").html(`<h3 id="headerEducationSection" data-toggle="collapse" data-target="#divEducationSection" aria-expanded="false"> [+] Education Data</h3>`)
})
$("#divEducationSection").on('show.bs.collapse', function () {
    $("#headerEducationSection").html(`<h3 id="headerEducationSection" data-toggle="collapse" data-target="#divEducationSection" aria-expanded="false"> [-] Education Data</h3>`)
})