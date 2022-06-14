$(document).ready(function () {
    $('#tableGridWorkerList').DataTable({
        columns: [
            { data: '#' },
            { data: 'NIK' },
            { data: 'First Name' },
            { data: 'Last Name' },
            { data: 'Email' },
            { data: 'Phone' },
            { data: 'Birthdate' },
            { data: 'Salary' }
        ],
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

$.ajax(
    {
        type: "GET",
        url: "https://localhost:44309/API/Employees/GetEmployeeData"
    }).done((result) => {
        console.log(result);
        let text = "";

        $.each(result.data, function (key, value) {
            let workerFirstName = "";
            workerFirstName = toPascalCase(value.FirstName);
            let workerLastName = "";
            workerLastName = toPascalCase(Value.LastName);
            text += `<tr>
                        <td>${key + 1}</td>
                        <td>${value.NIK}</td>
                        <td>${workerFirstName}</td>
                        <td>${workerLastName}</td>
                        <td>${value.Email}</td>
                        <td>${value.Phone}</td>
                        <td>${value.BirthDate}</td>
                        <td>${value.Salary}</td>
                     </tr>`
        });

        $("#GridWorkerList").html(text);
    }).fail((error) => {
        console.log(error)
    });
