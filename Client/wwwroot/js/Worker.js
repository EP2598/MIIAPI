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
    $.ajax({
        type: "POST",
        url: "https://localhost:44309/API/Employees/GetEmployeeDetail",
        data: JSON.stringify({"NIK" : nik}),
        success: function (res) {
            console.log(res);
        },
        error: function (res) {
            console.log(res);
        },
        dataType: "json"
    });
}

$(document).ready(function () {
    $('#tableGridWorkerList').DataTable({
        ajax: {
            "url": "https://localhost:44309/API/Employees/GetEmployeeData",
            "dataType": "json",
            "dataSrc": "data"
        },
        columns: [
            {
                data: 'nik',
                render: function (data)
                {
                    return `<a class="text-info" href="#" onclick="getDetails(${data})" data-toggle="modal" data-target="#modalEmployee">${data}</a>`;
                }
            },
            {
                data: 'firstName',
                render: function (data)
                {
                    return '<span>' + toPascalCase(data) + '</span>';
                }
            },
            {
                data: 'lastName',
                render: function (data) {
                    return '<span>' + toPascalCase(data) + '</span>';
                }
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
            {
                data: 'salary',
                render: function (data, type) {
                    var number = $.fn.dataTable.render
                        .number(',', '.', 2, 'Rp. ')
                        .display(data);

                    return '<span>' + number + '</span>';
                }
            }
        ],
    });
});

