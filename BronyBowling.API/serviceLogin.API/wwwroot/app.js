const AUTH_API = "http://localhost:5001";
const PROFILE_API = "http://localhost:5272";
const BOOKING_API = "http://localhost:5280";

function getToken() {
    return localStorage.getItem("jwt");
}

function authHeader() {
    const token = getToken();
    return token ? { "Authorization": "Bearer " + token } : {};
}

function showStatus(message, isError = false) {
    const el = document.getElementById("status");
    if (!el) return;

    el.className = "status " + (isError ? "error" : "success");
    el.innerText = message;
}

function logout() {
    localStorage.removeItem("jwt");
    location.href = "/";
}
