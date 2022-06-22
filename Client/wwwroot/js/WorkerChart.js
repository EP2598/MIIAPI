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

    //Test HttpClient Get
    //let nik = "062020220011";
    //let objReq = {
    //    NIK:nik};
    $.ajax({
        type: "get",
        url: "Employees/GetRegistered/"
    }).done((res) => {
        console.log(res);
    }).fail((err) => {
        console.log(err);
    });

    //Test HttpClient Post
    let nik = "061720220009";
    let objReq = {
        NIK: nik
    };
    $.ajax({
        type: "post",
        url: "Employees/GetDataByNIK/",
        data: objReq
    }).done((res) => {
        console.log(res);
    }).fail((err) => {
        console.log(err);
    });
})