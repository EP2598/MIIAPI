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
        document.getElementById("formFirstName").value = toPascalCase(res.firstName);
        document.getElementById("formLastName").value = toPascalCase(res.lastName);
        document.getElementById("formEmail").value = res.email;
        document.getElementById("formPhone").value = res.phone;
        document.getElementById("formDate").value = moment(res.birthDate).format('yyyy-MM-DD');
        document.getElementById("formNIK").value = res.nik;
        document.getElementById("formSalary").value = res.salary;
        document.getElementById("formGender").value = res.gender;

        //Edit Education Data
        let uniId = 1;
        if (res.universityName[0] === "Universitas Pokemon") {
            uniId = 1;
        }
        else if (res.universityName[0] === "GachArena") {
            uniId = 2;
        }
        else {
            uniId = 3;
        }
        document.getElementById("formUniversity").value = uniId;

        let degreeId = 1;
        if (res.educationDegree[0] === "D3") { degreeId = 1; }
        else if (res.educationDegree[0] === "D4") { degreeId = 2; }
        else if (res.educationDegree[0] === "S1") { degreeId = 3;}
        else if (res.educationDegree[0] === "S2") { degreeId = 4;}
        else { degreeId = 5}
        document.getElementById("formDegree").value = degreeId;
        document.getElementById("formGPA").value = res.educationGPA[0];

        //Preview Roles Data
        let roleList = res.roleName;     
        let roleDetail = "";

        for (var i = 0; i < roleList.length; i++) {
            roleDetail += `         <span class="badge badge-success">${roleList[i]}</span>`;
        }

        let empName = toPascalCase(res.firstName) + " " + toPascalCase(res.lastName);
        let title = `<h5 class="modal-title" id="modalEmployeeTitle">${res.nik} - ${empName}</h5>`;

        $("#modalEmployeeTitle").html(title);
        $("#innerDetailRolesPlaceholder").html(roleDetail);
        //console.log(obj);
    });
}

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

        if (res.statusCode === 200) {
            alert('Update succes!')
        }
        else {
            alert('Update gagal!')
        }

        return true;
    });
}

$(document).ready(function () {
    $('#tableGridWorkerList').DataTable({
        ajax: {
            "url": "https://localhost:44309/API/Employees/GetEmployeeData",
            "dataType": "json",
            "dataSrc": "data"
        },
        dom: 'lBfrtip',
        buttons: {
            buttons: [
                { extend: 'csv', className: 'ml-1 mr-1' },
                { extend: 'excel', className: 'ml-1 mr-1' },
                { extend: 'pdf', className: 'ml-1 mr-1' }
            ]
        },
        columns: [
            {
                data: 'nik',
                render: function (data)
                {
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
                render: function (data)
                {
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
});

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
        //Bisa post
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

        let table = document.getElementById("tableGridWorkerList");
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

        let table = document.getElementById("tableGridWorkerList");
        table.ajax.reload();

        let toastText = `Employee Data successfully updated!`;

        // TODO Tampilin Toast
        $('#divToastBody').innerHTML = toastText;

        return true;
    }
    return false;
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