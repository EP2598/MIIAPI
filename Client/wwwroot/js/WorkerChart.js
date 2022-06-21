$(document).ready(function () {
    //Gender Chart
    $.ajax({
        type: "GET",
        url: "https://localhost:44309/API/Employees/GetGenderCount"
    }).done((res) => {
        var options = {
            dataLabels: {
                enabled: true,
                formatter: function (val) {
                    return val + "%"
                }
            },
            chart: {
                type: 'donut'
            },
            series: res.data.genderCount,
            labels: res.data.gender
        }

        var chart = new ApexCharts(document.querySelector("#innerDetailEmployee"), options);

        chart.render();


    });

    //Alumni Chart
    $.ajax({
        type: "GET",
        url: "https://localhost:44309/API/Universities/GetAlumniCount"
    }).done((res) => {
        console.log(res);
        
        var options = {
            chart: {
                type: 'bar'
            },
            plotOptions: {
                bar: {
                    borderRadius: 4,
                    horizontal: false
                }
            },
            series: [{
                data: res.data.universityCount
            }],
            xaxis: {
                categories: res.data.universityName
            }
        }

        var chart = new ApexCharts(document.querySelector("#innerDetailEducation"), options);

        chart.render();


    });
})