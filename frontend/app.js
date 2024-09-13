
document.getElementById('main-page').classList.remove('main-page');

// Handle login and register actions
document.getElementById('login-form').addEventListener('submit', function(event) {
    event.preventDefault();
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const hashedPassword = btoa(password); // Simple base64 encoding, replace with real hashing

    // Simulate a login process
    console.log('Login data:', { username, hashedPassword });
    document.getElementById('feedback').innerText = "Login erfolgreich!";

    // Hide the login page and show the main page
    document.getElementById('login-page').classList.add('hidden');
    document.getElementById('login-page').classList.remove('login-page');
    document.getElementById('main-page').classList.remove('hidden');
    document.getElementById('main-page').classList.add('main-page');
});

document.getElementById('register-btn').addEventListener('click', function() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const hashedPassword = btoa(password); // Simple base64 encoding, replace with real hashing

    // Simulate a registration process
    console.log('Register data:', { username, hashedPassword });
    document.getElementById('feedback').innerText = "Registrierung erfolgreich!";

    // Optionally switch to the main page after registration
    document.getElementById('login-page').classList.add('hidden');
    document.getElementById('main-page').classList.remove('hidden');
});

// Toggle dark mode / light mode
document.getElementById('toggle-theme').addEventListener('click', function() {
    document.body.classList.toggle('light-mode');
});