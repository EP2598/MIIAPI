$(document).ready(function () {
    $('#tableGridPokelist').DataTable({
        columns: [
            { data: '#' },
            { data: 'Name' },
            { data: 'Details' }
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
        url: "https://pokeapi.co/api/v2/pokemon/"
    }).done((result) => {
        let text = "";

        $.each(result.results, function (key, value) {
            let pokeName = "";
            pokeName = toPascalCase(value.name);
            text += `<tr>
                        <td>${key + 1}</td>
                        <td>${pokeName}</td>
                        <td>
                            <button type="button" onclick="detailFunction('${value.url}')" class="btn btn-primary" data-toggle="modal" data-target="#modalDetail">Detail</button>
                        </td>
                     </tr>`
        });

        $("#GridPokelist").html(text);
    }).fail((error) => {
        console.log(error)
    });
