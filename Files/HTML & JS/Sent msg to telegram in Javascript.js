// Goto bot father in telegram
// Create new bot and api token
// Goto your newly created bot and click start button
// View this site in browser for get client id
// https://api.telegram.org/bot${botToken}/getUpdates


function sendMessage() {
	// Replace 'YOUR_BOT_TOKEN' and 'YOUR_CHAT_ID' with your actual bot token and chat ID
  const botToken = 'YOUR_BOT_TOKEN';
  const chatId = 'YOUR_CHAT_ID';
  
  // Message to be sent
  const messageText = 'Button clicked!';
  
  // Send the message using the Telegram Bot API
  const apiUrl = `https://api.telegram.org/bot${botToken}/sendMessage`;
  const formData = new FormData();
  formData.append('chat_id', chatId);
  formData.append('text', messageText);

  fetch(apiUrl, {
    method: 'POST',
    body: formData,
  })
    .then(response => response.json())
    .then(data => {
      console.log('Message sent:', data);
    })
    .catch(error => {
      console.error('Error sending message:', error);
    });
}
	
	
	// Function to get updates using long polling
function getUpdates() {
  const apiUrl = `https://api.telegram.org/bot${botToken}/getUpdates`;

  fetch(apiUrl)
    .then(response => response.json())
    .then(data => {
      if (data.ok) {
        const updates = data.result;
        console.log('Received updates:', updates);
        // Process the updates as needed
      } else {
        console.error('Error getting updates:', data.description);
      }
    })
    .catch(error => {
      console.error('Error fetching updates:', error);
    });
}
