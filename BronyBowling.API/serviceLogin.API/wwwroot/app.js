<script>
    const AUTH_API = "http://localhost:5001";
    const PROFILE_API = "http://localhost:5254";
    const BOOKING_API = "http://localhost:5280";

    function saveToken(token) {
        localStorage.setItem("jwt", token);
}

    function getToken() {
  return localStorage.getItem("jwt");
}

    function logout() {
        localStorage.removeItem("jwt");
    location.href = "/";
}

    function showStatus(text, ok = false) {
  const el = document.getElementById("status");
    if (!el) return;
    el.className = "status " + (ok ? "success" : "error");
    el.innerText = text;
}
</script>
