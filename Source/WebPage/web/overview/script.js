//Blue hex code: 4ab2f7
//Green hex code: 03fc03

function isMobile() {
  var check = false;
  (function(a){
    if(/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0,4))) 
      check = true;
  })(navigator.userAgent||navigator.vendor||window.opera);
  return check;
};
if(isMobile()){
  document.location = "/overview/overviewChartsMobile.html"
}
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

var years = 0;
var days = 0;
var hours = 0;
var mins = 0;
var secs = 0;

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

function updateDataFields(){
  document.getElementById('apField').innerHTML = apData[apData.length - 1] != undefined ? apData[apData.length - 1] : 0;
  document.getElementById('peField').innerHTML = peData[peData.length - 1] != undefined ? peData[peData.length - 1] : 0;
  document.getElementById('incField').innerHTML = incData[incData.length - 1] != undefined ? incData[incData.length - 1] : 0;
  document.getElementById('velField').innerHTML = velData[velData.length - 1] != undefined ? velData[velData.length - 1] : 0;
  document.getElementById('altField').innerHTML = altData[altData.length - 1] != undefined ? altData[altData.length - 1] : 0;
  document.getElementById('gField').innerHTML = gData[gData.length - 1] != undefined ? gData[gData.length - 1] : 0;
  document.getElementById('accField').innerHTML = accData[accData.length - 1] != undefined ? accData[accData.length - 1] : 0;

  var n = timeData[timeData.length - 1]
  years = ~~(n / ((6 * 3600 * 426) + (60 * 32)))
  n = n % (6 * 3600 * 426);
  days = ~~(n / (6 * 3600));
  n = n % (6 * 3600); 
  hours = ~~(n / 3600); 
  n %= 3600; 
  mins = ~~(n / 60); 
  n %= 60; 
  secs = ~~n; 
  document.getElementById('MET').innerHTML ="T+" + years + "Y " + days + "D, " + prependZero(hours) + ":" + prependZero(mins) + ":" + prependZero(secs);
}

function updateChart() {
  d3
    .csv("/data.csv")
    .then(reloadChartData);
}

window.onresize = function()
{
    velChartCanvas.width = window.innerWidth;
    velChartCanvas.style.width = window.innerWidth;
    velChartCanvas.height = window.innerHeight;
    velChartCanvas.style.height = window.innerHeight;
}

setInterval(() => {
  updateChart();
  updateDataFields();
}, 500);
