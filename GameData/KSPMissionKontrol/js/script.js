
//Blue hex code: 4ab2f7
//Green hex code: 03fc03
var backgroundColor = "#4ab2f7";
var foregroundColor = "#ffffff";

d3
 .csv("/data.csv")
 .then(makeChart);

function makeChart(data) {

    var options =  {  
      scales: {
        xAxes: [{
          display: false
        }],
        yAxes: [{
          ticks: {
            fontColor: backgroundColor
          },
          gridLines: {
            color: backgroundColor
          }
        }]
    },
    responsive: false,
    animation: {
      duration: 0
  },
  elements: {
    line: {
      tension: 0
    },
    point: {
      radius: 0
    }
  }, legend: {
    display: false
},
layout: {
  padding: {
    bottom: 20,
    top: 20
  }
},
  events: ['click']

}


  var timeData = data.map(function(d) {
    return d.Time;
  });
  var velData = data.map(function(d) {
    return +d.SurfaceVelocity;
  });
  var altData = data.map(function(d) {
    return +d.Altitude;
  });

  var velChartCanvas = document.getElementById("velChart").getContext("2d")

  var velChart = new Chart(velChartCanvas, {
    type: 'line',
    data: {
      labels: timeData,
      datasets: [
        {
          data: velData,
          borderColor: foregroundColor,
          fill: false
        }
      ]
    },
    options: options
});


var altChartCanvas = document.getElementById("altChart").getContext("2d")

var altChart = new Chart(altChartCanvas, {
  type: 'line',
  data: {
    labels: timeData,
    datasets: [
      {
        data: altData,
        borderColor: foregroundColor,
        fill: false
      }
    ]
  },
  options: options
});
}

var updateChart = function(){
  d3
  .csv("/data.csv")
  .then(makeChart);
}


updateChart();
setInterval(() => {
  updateChart();
  }, 1000);
