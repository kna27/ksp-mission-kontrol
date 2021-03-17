//Blue hex code: 4ab2f7
//Green hex code: 03fc03

  var backgroundColor = "#4ab2f7";
  var foregroundColor = "#ffffff";
  var timeData;
  var velData;
  var altData;
  var apData;
  var peData;
  var incData;
  var gData;
  var accData;

  var velChartCanvas = document.getElementById("velChart").getContext("2d")
  var altChartCanvas = document.getElementById("altChart").getContext("2d")
  
  var options = {
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
  velChartCanvas.width = screen.width * 0.42;
  velChartCanvas.width = screen.width * 0.42;

  function prependZero(number) { 
    if (number < 10) 
        return "0" + number; 
    else 
        return number; 
  } 
  
  function getData(data) {
    timeData = data.map(function (d) {
      return d.Time;  
    });
    velData = data.map(function (d) {
      return +d.SurfaceVelocity;
    });
    altData = data.map(function (d) {
      return +d.Altitude;
    });
    apData = data.map(function (d) {
      return d.Apoapsis;  
    });
    peData = data.map(function (d) {
      return +d.Periapsis;
    });
    incData = data.map(function (d) {
      return +d.Inclination;
    });
    gData = data.map(function (d) {
      return +d.GForce;
    });
    accData = data.map(function (d) {
      return +d.Acceleration;
    });
  }
  
  var velChartData = {
    labels: timeData,
    datasets: [
      {
        data: velData,
        borderColor: foregroundColor,
        fill: false
      }
    ]
  }
  
  var altChartData = {
    labels: timeData,
    datasets: [
      {
        data: altData,
        borderColor: foregroundColor,
        fill: false
      }
    ]
  }
  
  var velChartInfo = {
    type: 'line',
    data: velChartData,
    options: options
  };
  
  var altChartInfo = {
    type: 'line',
    data: altChartData,
    options: options
  };
  
  var velChart = new Chart(velChartCanvas, velChartInfo);
  var altChart = new Chart(altChartCanvas, altChartInfo);
  
  function reloadChartData(data) {
    getData(data);
    velChartData.datasets[0].data = velData;
    velChartData.labels = timeData;
    velChart.config.data = velChartData;
  
    altChartData.datasets[0].data = altData;
    altChartData.labels = timeData;
    altChart.config.data = altChartData;
    velChart.update();
    altChart.update();
  }
  
  function updateChart() {
    d3
      .csv("/data.csv")
      .then(reloadChartData);
  }
  
  window.onresize = function()
  {
    velChartCanvas.width = screen.width * 0.42;
    velChartCanvas.width = screen.width * 0.42;
  }
  
  setInterval(() => {
    updateChart();
  }, 500);
  