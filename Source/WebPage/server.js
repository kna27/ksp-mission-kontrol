var express = require('express');
var app = express();
app.use(express.static('web'));
app.use(express.static('data'));
app.get('/', function(req, res){
   res.sendFile(__dirname + '/index.html');
});

app.listen(80);

