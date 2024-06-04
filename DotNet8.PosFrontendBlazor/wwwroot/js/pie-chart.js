window.setPieChart = function (_labels, _series) {
    // ['Team A', 'Team B', 'Team C', 'Team D', 'Team E']
    //[44, 55, 13, 43, 22]
    var options = {
        series: _series,
        chart: {
            width: 380,
            type: 'pie',
        },
        labels: _labels,
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();
}     

window.setLineColumnChart = function (response) {
    console.log(response);
    const productName = response.productName;
    const quantity = response.quantity;

    // [440, 505, 414, 671, 227, 413, 201, 352, 752, 320, 257, 160]
    // [23, 42, 35, 27, 43, 22, 17, 31, 22, 22, 12, 16]
    //  ['01 Jan 2001', '02 Jan 2001', '03 Jan 2001', '04 Jan 2001', '05 Jan 2001', '06 Jan 2001', '07 Jan 2001', '08 Jan 2001', '09 Jan 2001', '10 Jan 2001', '11 Jan 2001', '12 Jan 2001']
    var options = {
        series: [{
            name: 'Quantity',
            type: 'column',
            data: quantity
        }],
        chart: {
            height: 350,
            type: 'line',
        },
        stroke: {
            width: [0, 4]
        },
        title: {
            text: 'Best Seller Product List'
        },
        dataLabels: {
            enabled: true,
            enabledOnSeries: [1]
        },
        labels: productName,
        xaxis: {
            type: 'string'
        },
        yaxis: [{
            title: {
                text: 'Quantity',
            },

        }]
    };

    var chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();
}

window.setLineChart = function () {
    Highcharts.chart('container', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Corn vs wheat estimated production for 2020',
            align: 'left'
        },
        subtitle: {
            text:
                'Source: <a target="_blank" ' +
                'href="https://www.indexmundi.com/agriculture/?commodity=corn">indexmundi</a>',
            align: 'left'
        },
        xAxis: {
            categories: ['USA', 'China', 'Brazil', 'EU', 'India', 'Russia'],
            crosshair: true,
            accessibility: {
                description: 'Countries'
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: '1000 metric tons (MT)'
            }
        },
        tooltip: {
            valueSuffix: ' (1000 MT)'
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [
            {
                name: 'Corn',
                data: [406292, 260000, 107000, 68300, 27500, 14500]
            },
            {
                name: 'Wheat',
                data: [51086, 136000, 5500, 141000, 107180, 77000]
            }
        ]
    });

}