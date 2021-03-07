

d3
 .csv("/data.csv")
 .then(makeChart);

function makeChart(data) {

    var options =  {  
      scales: {
        xAxes: [{
          display: false
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
          data: velData
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
        data: altData
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
