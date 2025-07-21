const WebSocket = require('ws');
const fs = require('fs');
const wss = new WebSocket.Server({ port: 8080 });


// Handle new WebSocket connections
wss.on('connection', (ws) => {

  // Handle messages from the clients
  ws.on('message', (message) => {
    // The JSON data you provided
    const jsonData = [
      { "value": "0", "ismatched": "NO", "visible": "N", "toshowother": "N", "class": "class", "player": "" },
      { "value": "1", "ismatched": "NO", "visible": "N", "toshowother": "N", "class": "class", "player": "" },
      { "value": "1", "ismatched": "NO", "visible": "N", "toshowother": "N", "class": "class", "player": "" },
      { "value": "A", "ismatched": "NO", "visible": "N", "toshowother": "N", "class": "class", "player": "" },
      { "value": "A", "ismatched": "NO", "visible": "N", "toshowother": "N", "class": "class", "player": "" },
      { "value": "B", "ismatched": "NO", "visible": "N", "toshowother": "N", "class": "class", "player": "" },
      { "value": "B", "ismatched": "NO", "visible": "N", "toshowother": "N", "class": "class", "player": "" },
      { "value": "0", "ismatched": "NO", "visible": "N", "toshowother": "N", "class": "class", "player": "" }
    ];

    // Write JSON data to a file
    fs.writeFileSync('data.json', JSON.stringify(jsonData, null, 2), 'utf8');
    console.log("JSON data written to 'data.json'");

    // Read the JSON data from the file
    const rawData = fs.readFileSync('data.json', 'utf8');
    const readData = JSON.parse(rawData);
    console.log("JSON data read from file:");
    console.log(readData);
  });

  // When a player disconnects
  ws.on('close', () => {
    console.log('Disconnected');
  });
});

