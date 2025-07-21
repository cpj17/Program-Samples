const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 8080 });

let rooms = {}; // Store rooms and game state

// Handle incoming WebSocket connections
wss.on('connection', (ws) => {
    let currentRoom = null;
    let currentPlayer = null;

    // Handle incoming messages from clients
    ws.on('message', (message) => {
        const data = JSON.parse(message);

        switch (data.type) {
            case 'createRoom':
                // Create a new room
                if (rooms[data.roomName]) {
                    ws.send(JSON.stringify({ message: 'Room already exists.' }));
                    return;
                }
                rooms[data.roomName] = {
                    players: [{ ws, role: 'X', score: 0 }, { ws: null, role: 'O', score: 0 }],
                    board: Array(9).fill(null),
                    currentPlayer: 'X', // Default starting player
                    isStarted: false,
                    session: 'X', // Track who started the last game in the session (X by default)
                };
                currentRoom = data.roomName;
                ws.send(JSON.stringify({ message: `Room ${data.roomName} created. Waiting for another player...` }));
                break;

            case 'joinRoom':
                // Join an existing room
                if (!rooms[data.roomName]) {
                    ws.send(JSON.stringify({ message: 'Room not found.' }));
                    return;
                }
                const room = rooms[data.roomName];
                if (room.players[1].ws) {
                    ws.send(JSON.stringify({ message: 'Room is full.' }));
                    return;
                }
                room.players[1].ws = ws;
                currentRoom = data.roomName;
                currentPlayer = 'O';
                ws.send(JSON.stringify({ message: `You have joined room ${data.roomName}.` }));

                // Notify both players that the second player has joined
                room.players.forEach(player => {
                    if (player.ws) {
                        player.ws.send(JSON.stringify({ message: `Player O has joined. The game will start now!` }));
                    }
                });

                // Start the game automatically once both players have joined
                room.isStarted = true;

                // Alternate starting player based on the session history
                room.currentPlayer = room.session === 'X' ? 'O' : 'X'; 

                room.players.forEach(player => {
                    if (player.ws) {
                        player.ws.send(JSON.stringify({
                            message: `The game has started! It's player ${room.currentPlayer}'s turn.`,
                            board: room.board,
                            currentPlayer: room.currentPlayer
                        }));
                    }
                });
                break;

            case 'move':
                // Handle player move
                handleMove(ws, data);
                break;

            case 'reset':  // Handle the reset
                // Reset the game board
                const resetRoom = rooms[data.roomName];
                if (resetRoom && resetRoom.players[1].ws) {
                    resetRoom.board = Array(9).fill(null); // Reset the board
                    // Alternate the starting player for the new round
                    resetRoom.currentPlayer = resetRoom.session === 'X' ? 'O' : 'X'; 
                    resetRoom.players.forEach(player => {
                        if (player.ws) {
                            player.ws.send(JSON.stringify({
                                message: 'The game has been reset. It\'s player ' + resetRoom.currentPlayer + '\'s turn.',
                                board: resetRoom.board,
                                currentPlayer: resetRoom.currentPlayer
                            }));
                        }
                    });
                }
                break;

            default:
                ws.send(JSON.stringify({ message: 'Invalid message type.' }));
        }
    });

    // Close connection
    ws.on('close', () => {
        // Remove player from the room on disconnect
        if (currentRoom && rooms[currentRoom]) {
            const room = rooms[currentRoom];
            room.players = room.players.filter(player => player.ws !== ws);
            if (room.players.length === 0) {
                delete rooms[currentRoom];
            } else {
                // Notify remaining player if their partner disconnected
                room.players.forEach(player => {
                    if (player.ws) {
                        player.ws.send(JSON.stringify({ message: 'Your partner disconnected.' }));
                    }
                });
            }
        }
    });
});

// Check for a winner
function checkWinner(board) {
    const winningCombinations = [
        [0, 1, 2], [3, 4, 5], [6, 7, 8], // rows
        [0, 3, 6], [1, 4, 7], [2, 5, 8], // columns
        [0, 4, 8], [2, 4, 6], // diagonals
    ];

    for (let combination of winningCombinations) {
        const [a, b, c] = combination;
        if (board[a] && board[a] === board[b] && board[a] === board[c]) {
            return board[a]; // Return the winner ('X' or 'O')
        }
    }

    return null;
}

// Handle a player move
function handleMove(ws, data) {
    const { roomName, index } = data;
    const room = rooms[roomName];
    if (!room) return;
    const currentPlayer = room.currentPlayer;
    const player = room.players.find(player => player.ws === ws);
    if (!player || (player.role !== currentPlayer)) {
        return; // It's not this player's turn
    }
    if (room.board[index] !== null) {
        return; // The cell is already taken
    }

    room.board[index] = currentPlayer;
    room.currentPlayer = currentPlayer === 'X' ? 'O' : 'X'; // Switch turn

    // Broadcast the updated board and current player to both players
    room.players.forEach(player => {
        if (player.ws) {
            player.ws.send(JSON.stringify({
                board: room.board,
                currentPlayer: room.currentPlayer,
                message: `It's player ${room.currentPlayer}'s turn.`
            }));
        }
    });

    // Check for a winner
    const winner = checkWinner(room.board);
    if (winner) {
        room.players.forEach(player => {
            if (player.ws) {
                player.ws.send(JSON.stringify({
                    message: `${winner} wins the game!`,
                    board: room.board
                }));
                if (player.role === winner) {
                    player.score++; // Update the winner's score
                }
            }
        });
        updateScores(room);
        resetRoomBoard(room); // Reset the board for the next round
    } else if (room.board.every(cell => cell !== null)) {
        // If it's a draw (all cells are filled and no winner)
        room.players.forEach(player => {
            if (player.ws) {
                player.ws.send(JSON.stringify({
                    message: "It's a draw!",
                    board: room.board
                }));
            }
        });
        resetRoomBoard(room); // Reset the board for the next round
    }
}

function updateScores(room) {
    room.players.forEach(player => {
        if (player.ws) {
            player.ws.send(JSON.stringify({
                message: `Scores: Player X - ${room.players[0].score}, Player O - ${room.players[1].score}`,
            }));
        }
    });
}

// Reset the room board
function resetRoomBoard(room) {
    room.board = Array(9).fill(null);
    room.session = room.currentPlayer; // Store who started the game
    room.currentPlayer = room.session === 'X' ? 'O' : 'X'; // Alternate the starting player
    room.players.forEach(player => {
        if (player.ws) {
            player.ws.send(JSON.stringify({
                message: 'The game has been reset. It\'s player ' + room.currentPlayer + '\'s turn.',
                board: room.board,
                currentPlayer: room.currentPlayer
            }));
        }
    });
}

console.log('WebSocket server is running on ws://localhost:8080');
