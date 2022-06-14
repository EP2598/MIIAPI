console.log("Test latihan")

function Camelize(str)
{
    return str.replace(/(?:^\w|[A-Z]|\b\w|\s+)/g, function (match, index) {
        if (+match === 0) return ""; // or if (/\s+/.test(match)) for white spaces
        return index === 0 ? match.toLowerCase() : match.toUpperCase();
    });
}

function getAbilityDetail(url) {
    $.ajax(
        {
            url: url
        }).done((result) => {
            console.log(result);

            let text = "";

            var stories = result.effect_entries;

            for (var i = 0; i < stories.length; i++) {
                if (stories[i].language.name === "en") {
                    text += stories[i].short_effect;
                    break;
                }
            }

            console.log(text);

            return text;

        }).fail((error) => {
            console.log(error)
        });
}

//const animals = [
//    { name: "Garfield", species: "cat", class: { name: "mamalia" } },
//    { name: "Nemo", species: "fish", class: { name: "invertebrata" } },
//    { name: "Tom", species: "cat", class: { name: "mamalia" } },
//    { name: "Bruno", species: "fish", class: { name: "invertebrata" } },
//    { name: "Carlo", species: "cat", class: { name: "mamalia" } }
//]

//const OnlyCat = [];

//for (var i = 0; i < animals.length; i++)
//{
//    if (animals[i].species == "fish")
//    {
//        animals[i].class.name = "Non-Mamalia";
//    }
//    else if (animals[i].species == "cat")
//    {
//        OnlyCat.push(animals[i]);
//    }
//}

//console.log("Animals");
//console.log(animals);
//console.log("Cats");
//console.log(OnlyCat);

$.ajax(
    {
        url: "https://pokeapi.co/api/v2/pokemon/"
    }).done((result) => {
        console.log(result);

        let text = "";

        $.each(result.results, function (key, value) {
            text += `<tr>
                        <td>${key+1}</td>
                        <td>${value.name}</td>
                        <td>
                            <button type="button" onclick="detailFunction('${value.url}')" class="btn btn-primary" data-toggle="modal" data-target="#modalDetail">Detail</button>
                        </td>
                     </tr>`
        });

        $("#tablePoke").html(text);
    }).fail((error) => {
        console.log(error)
    });

function detailFunction(url)
{
    $("#divPokeStats").empty();
    $("#tableAbilities").empty();

    $.ajax(
        {
            url: url
        }).done((result) => {
            console.log(result);

            //Image
            let img = `
                            <img class="border border-dark rounded-circle" src="${result.sprites.other["official-artwork"].front_default}" style="width:256px; height:256px;" />
                        `;

            //Name
            let name = `
                            <h1>${result.name}</h1>
                        `;

            //Pokemon Types
            let types = "";
            $.each(result.types, function (key, value) {
                if (value.type.name === "grass") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-success">${value.type.name}</span>`;
                }
                else if (value.type.name === "fire") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-warning">${value.type.name}</span>`;
                }
                else if (value.type.name === "water") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-primary">${value.type.name}</span>`
                }
                else if (value.type.name === "bug") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#6b8e23">${value.type.name}</span>`
                }
                else if (value.type.name === "normal") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#eee8aa">${value.type.name}</span>`
                }
                else if (value.type.name === "electric") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#ffd700">${value.type.name}</span>`
                }
                else if (value.type.name === "ice") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-info">${value.type.name}</span>`
                }
                else if (value.type.name === "fighting") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-danger">${value.type.name}</span>`
                }
                else if (value.type.name === "poison") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#9370db">${value.type.name}</span>`
                }
                else if (value.type.name === "ground") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#cd853f">${value.type.name}</span>`
                }
                else if (value.type.name === "flying") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#d8bfd8">${value.type.name}</span>`
                }
                else if (value.type.name === "psychic") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#db7093">${value.type.name}</span>`
                }
                else if (value.type.name === "rock") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#f4a460">${value.type.name}</span>`
                }
                else if (value.type.name === "ghost") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#7b68ee">${value.type.name}</span>`
                }
                else if (value.type.name === "dark") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#708090">${value.type.name}</span>`
                }
                else if (value.type.name === "dragon") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#6a5acd">${value.type.name}</span>`
                }
                else if (value.type.name === "steel") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#b0e0e6">${value.type.name}</span>`
                }
                else if (value.type.name === "fairy") {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light" style="background-color:#ffc0cb">${value.type.name}</span>`
                }
                else
                {
                    types += `<span class="ml-1 mr-1 badge badge-pill badge-light">${value.type.name}</span>`
                }
            });

            //Pokemon Story
            let storyText = getPokemonStory(result.species.url);
            console.log(storyText);

            //Pokemon Abilities
            let abilityList = result.abilities;

            for (var i = 0; i < abilityList.length; i++)
            {
                getPokemonAbility(abilityList[i].ability.url);
            }

            //Pokemon Status
            let statList = result.stats;
            let statNames = [];
            let statNumber = [];
            let stats = "";
            for (var i = 0; i < statList.length; i++)
            {
                statNames.push(statList[i].stat.name);
                statNumber.push(statList[i].base_stat);

                stats += `
                            <div class="progress">
                              <div class="progress-bar" role="progressbar" style="width: ${statList[i].base_stat}%;" aria-valuenow="${statList[i].base_stat}" aria-valuemin="0" aria-valuemax="100">${statList[i].stat.name}</div>
                            </div>
                        `;
            }
            getPokemonStats(statNames, statNumber, result.name);


            name = Camelize(name);

            $("#modalImg").html(img);
            $("#modalTitle").html(name);
            $("#modalType").html(types);

            
            $("#divPokeStats").append(stats);
            
        }).fail((error) => {
            console.log(error)
        });
}

function getPokemonStory(url)
{
    $.ajax(
        {
            url: url
        }).done((result) => {

            let text = "";

            var stories = result.flavor_text_entries;

            for (var i = 0; i < stories.length; i++)
            {
                if (stories[i].language.name === "en")
                {
                    text += stories[i].flavor_text;
                    break;
                }
            }

            console.log(text);

            $("#pills-story").html(text);
        }).fail((error) => {
            console.log(error)
        });
}

function getPokemonAbility(url)
{
    $.ajax(
        {
            url: url
        }).done((result) => {

            let text = "";

            var stories = result.effect_entries;

            for (var i = 0; i < stories.length; i++) {
                if (stories[i].language.name === "en") {
                    var abilityName = Camelize(result.name);
                    text += 
                    `
                        <tr>
                            <td>${abilityName}</td>
                            <td>${stories[i].short_effect}</td>
                        </tr>
                     `
                    break;
                }
            }

            
            $("#tableAbilities").append(text);
        }).fail((error) => {
            console.log(error)
        });
}

function getPokemonStats(statName, statNumber, pokeName)
{
    var checkElement = document.getElementById("radarChart");
    checkElement.remove();

    var newCanvas = document.createElement("canvas");
    newCanvas.id = "radarChart";

    var addElement = document.getElementById("divPokeRadar");
    addElement.appendChild(newCanvas);

    var ctxR = document.getElementById("radarChart").getContext('2d');
    var myRadarChart = new Chart(ctxR, {
        type: 'radar',
        data: {
            labels: statName,
            datasets: [{
                label: Camelize(pokeName),
                data: statNumber,
                backgroundColor: [
                    'rgba(105, 0, 132, .2)',
                ],
                borderColor: [
                    'rgba(200, 99, 132, .7)',
                ],
                borderWidth: 2
            }
            ]
        },
        options: {
            responsive: true
        }
    });
}