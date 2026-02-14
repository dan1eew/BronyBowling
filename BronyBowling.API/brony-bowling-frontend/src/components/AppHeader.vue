<template>
  <nav class="header">
    <div class="logo">
      <span class="icon">🎳</span>
      <span class="text">Brony</span>
    </div>

    <div class="nav-links">
      <router-link to="/">Главная</router-link>
      <router-link to="/booking">Бронирование</router-link>

      <div v-if="!isAuth" class="auth-buttons">
        <router-link to="/login" class="login-btn">Войти</router-link>
        <router-link to="/register" class="register-btn">Регистрация</router-link>
      </div>

      <div v-else class="profile-section">
        <router-link to="/profile" class="profile-btn">👤 Кабинет</router-link>
        <button @click="logout" class="logout-btn">Выход</button>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const isAuth = computed(() => !!localStorage.getItem('jwt'))

function logout() {
  localStorage.removeItem('jwt')
  router.push('/')
}
</script>

<style scoped>
.header {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  z-index: 100;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 18px 40px;
  background: rgba(15, 23, 42, 0.85);
  backdrop-filter: blur(20px);
  border-bottom: 1px solid rgba(255,255,255,0.08);
}

.logo {
  display: flex;
  align-items: center;
  font-size: 28px;
  font-weight: 800;
  letter-spacing: -1px;
}
.icon { margin-right: 8px; filter: drop-shadow(0 0 12px #22c55e); }

.nav-links {
  display: flex;
  align-items: center;
  gap: 32px;
  font-size: 16px;
}

a, button {
  color: white;
  text-decoration: none;
  font-weight: 500;
  transition: all 0.3s;
}

a:hover { color: #22c55e; }

.login-btn, .register-btn {
  padding: 10px 24px;
  border-radius: 9999px;
  font-weight: 600;
}
.register-btn {
  background: linear-gradient(90deg, #22c55e, #a855f7);
  color: white;
}

.logout-btn {
  background: transparent;
  border: 2px solid #ef4444;
  padding: 8px 20px;
  border-radius: 9999px;
  color: #ef4444;
}
</style>
