To create a multiplayer Tic-Tac-Toe game using Node.js and the ws package (WebSockets), you'll need two main parts:

A server that handles WebSocket connections.
A client that communicates with the server and allows players to interact with the game.
We'll set up the server and client as follows:

1. Server (Node.js with ws)
This will manage the WebSocket connections, the game state, and the rules of the game. Each game session will be between two players.

Paste server.js and index.html code

Explanation
Server (server.js):

We use WebSockets (ws package) to allow two players to connect to the server.
The game state is stored and updated as players take turns.
The server checks for a winner after every move and notifies the players if there's a winner, a draw, or an invalid move.
If a player disconnects, the game resets.
Client (index.html):

The client connects to the WebSocket server and updates the game board based on messages received from the server.
Each player can click on the cells of the Tic-Tac-Toe grid to make a move.
The game messages, such as who’s turn it is and whether there’s a winner, are displayed to the players.
Running the Game
Install the necessary dependencies in your project:

npm init -y
npm install ws

node server.js

Open index.html in two different browser windows (or tabs) to play as two players on the same network.

This implementation creates a multiplayer Tic-Tac-Toe game where players can join through WebSockets and play against each other over a local network.