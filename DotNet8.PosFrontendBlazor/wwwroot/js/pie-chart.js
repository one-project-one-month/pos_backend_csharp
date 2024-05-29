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
    const xData = response.xData;
    const websiteBlogData = response.websiteBlogData;
    const socialMediaData = response.socialMediaData;

    // [440, 505, 414, 671, 227, 413, 201, 352, 752, 320, 257, 160]
    // [23, 42, 35, 27, 43, 22, 17, 31, 22, 22, 12, 16]
    //  ['01 Jan 2001', '02 Jan 2001', '03 Jan 2001', '04 Jan 2001', '05 Jan 2001', '06 Jan 2001', '07 Jan 2001', '08 Jan 2001', '09 Jan 2001', '10 Jan 2001', '11 Jan 2001', '12 Jan 2001']
    var options = {
        series: [{
            name: 'Website Blog',
            type: 'column',
            data: websiteBlogData
        }, {
            name: 'Social Media',
            type: 'line',
            data: socialMediaData
        }],
        chart: {
            height: 350,
            type: 'line',
        },
        stroke: {
            width: [0, 4]
        },
        title: {
            text: 'Traffic Sources'
        },
        dataLabels: {
            enabled: true,
            enabledOnSeries: [1]
        },
        labels: xData,
        xaxis: {
            type: 'datetime'
        },
        yaxis: [{
            title: {
                text: 'Website Blog',
            },

        }, {
            opposite: true,
            title: {
                text: 'Social Media'
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();
}