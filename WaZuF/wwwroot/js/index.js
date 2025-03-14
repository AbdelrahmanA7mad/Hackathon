function navigateTo(page) {
    window.location.href = page; // ينتقل إلى الصفحة المطلوبة
}

function navigateTo(page) {
    const fadeOut = document.createElement('div');
    fadeOut.style.position = 'fixed';
    fadeOut.style.top = '0';
    fadeOut.style.left = '0';
    fadeOut.style.width = '100%';
    fadeOut.style.height = '100%';
    fadeOut.style.backgroundColor = '#000';
    fadeOut.style.opacity = '0';
    fadeOut.style.transition = 'opacity 0.3s ease';
    fadeOut.style.zIndex = '1000';

    document.body.appendChild(fadeOut);

    setTimeout(() => {
        fadeOut.style.opacity = '1';
    }, 50);

    setTimeout(() => {
        window.location.href = page;
    }, 300);
}

//////////////////////////////////////////////////////////////////////////////////////////

// Simple in-memory storage for demo purposes
const users = JSON.parse(localStorage.getItem('users')) || [];
const currentUser = JSON.parse(localStorage.getItem('currentUser')) || null;

// Check if user is logged in
function checkAuth() {
    if (!currentUser) {
        window.location.href = 'login.html';
    }
}

// Handle login form submission
function handleLogin(event) {
    event.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const user = users.find(u => u.email === email && u.password === password);

    if (user) {
        localStorage.setItem('currentUser', JSON.stringify(user));
        window.location.href = 'dashboard.html';
    } else {
        alert('Invalid email or password');
    }
}

// Handle signup form submission
function handleSignup(event) {
    event.preventDefault();

    const companyName = document.getElementById('companyName').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (password !== confirmPassword) {
        alert('Passwords do not match');
        return;
    }

    if (users.some(u => u.email === email)) {
        alert('Email already exists');
        return;
    }

    const newUser = {
        id: Date.now().toString(),
        companyName,
        email,
        password,
        jobPosts: []
    };

    users.push(newUser);
    localStorage.setItem('users', JSON.stringify(users));
    localStorage.setItem('currentUser', JSON.stringify(newUser));

    window.location.href = 'dashboard.html';
}

// Handle logout
function logout() {
    localStorage.removeItem('currentUser');
    window.location.href = 'login.html';
}


////////////////////////////////////////////////////////////////////////////////////////

function navigateTo(page) {
    const fadeOut = document.createElement('div');
    fadeOut.style.position = 'fixed';
    fadeOut.style.top = '0';
    fadeOut.style.left = '0';
    fadeOut.style.width = '100%';
    fadeOut.style.height = '100%';
    fadeOut.style.backgroundColor = '#000';
    fadeOut.style.opacity = '0';
    fadeOut.style.transition = 'opacity 0.3s ease';
    fadeOut.style.zIndex = '1000';

    document.body.appendChild(fadeOut);

    setTimeout(() => {
        fadeOut.style.opacity = '1';
    }, 50);

    setTimeout(() => {
        window.location.href = page;
    }, 300);
}
////////////////////////////////////////////////////////////////////////////////

// Load dashboard data
function loadDashboard() {
    const user = JSON.parse(localStorage.getItem('currentUser'));
    if (!user) return;

    document.getElementById('companyName').textContent = user.companyName;
    document.getElementById('companyEmail').textContent = user.email;

    const jobPostsContainer = document.getElementById('jobPosts');
    jobPostsContainer.innerHTML = '';

    user.jobPosts.forEach(post => {
        const jobCard = createJobCard(post);
        jobPostsContainer.appendChild(jobCard);
    });
}

