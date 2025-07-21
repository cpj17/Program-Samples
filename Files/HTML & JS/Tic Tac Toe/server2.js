const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 8080 });

let players = []; // Store the two players

// Handle new WebSocket connections
wss.on('connection', (ws) => {
  // Add player to the game (we support 2 players max)
  if (players.length < 2) {
    players.push(ws);
    console.log(`Player ${players.length} connected`);

    if (players.length === 2) {
      startGame(); // Start the game when two players are connected
    }
  } else {
    ws.send(JSON.stringify({ message: 'Game is full. Please try again later.' }));
    ws.close();
  }

  // Handle messages from the clients
  ws.on('message', (message) => {
    const data = JSON.parse(message);
    handlePlayerMove(data, ws);
  });

  // When a player disconnects
  ws.on('close', () => {
    players = players.filter(player => player !== ws);
    console.log('A player disconnected');
    if (players.length < 2) {
      resetGame(); // Reset the game if less than 2 players are connected
    }
  });
});

let gameState = {
  board: Array(9).fill(null), // 3x3 Tic-Tac-Toe board
  currentPlayer: 'X', // 'X' always starts
};

function startGame() {
  // Send a message to both players that the game has started
  players.forEach((player, index) => {
    player.send(JSON.stringify({
      message: `Game Started. You are Player ${index + 1}. Your symbol is ${index === 0 ? 'X' : 'O'}.`,
      board: gameState.board,
      currentPlayer: gameState.currentPlayer,
    }));
  });
}

function handlePlayerMove(data, ws) {
  const { index } = data;
  const currentPlayer = gameState.currentPlayer;
  if (gameState.board[index] === null && ws === players[(currentPlayer === 'X' ? 0 : 1)]) {
    gameState.board[index] = currentPlayer;
    gameState.currentPlayer = currentPlayer === 'X' ? 'O' : 'X'; // Switch turn

    // Check if the current move has won the game
    const winner = checkWinner();
    if (winner) {
      players.forEach(player => {
        player.send(JSON.stringify({
          message: `${winner} wins!`,
          board: gameState.board,
        }));
      });
      resetGame();
    } else if (gameState.board.every(cell => cell !== null)) {
      // It's a draw
      players.forEach(player => {
        player.send(JSON.stringify({
          message: "It's a draw!",
          board: gameState.board,
        }));
      });
      resetGame();
    } else {
      // Send updated board to both players
      players.forEach(player => {
        player.send(JSON.stringify({
          message: `It's Player ${gameState.currentPlayer}'s turn.`,
          board: gameState.board,
          currentPlayer: gameState.currentPlayer,
        }));
      });
    }
  } else {
    // If it's not the player's turn or invalid move
    ws.send(JSON.stringify({ message: "Invalid move. Try again." }));
  }
}

// Check for a winner
function checkWinner() {
  const winningCombinations = [
    [0, 1, 2], [3, 4, 5], [6, 7, 8], // rows
    [0, 3, 6], [1, 4, 7], [2, 5, 8], // columns
    [0, 4, 8], [2, 4, 6], // diagonals
  ];

  for (let combination of winningCombinations) {
    const [a, b, c] = combination;
    if (gameState.board[a] && gameState.board[a] === gameState.board[b] && gameState.board[a] === gameState.board[c]) {
      return gameState.board[a]; // Return the winner ('X' or 'O')
    }
  }

  return null;
}

function resetGame() {
  gameState.board = Array(9).fill(null);
  gameState.currentPlayer = 'X';
  players.forEach(player => {
    player.send(JSON.stringify({
      message: 'The game has been reset. Waiting for players to join.',
      board: gameState.board,
      currentPlayer: gameState.currentPlayer,
    }));
  });
}
