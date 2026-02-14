<template>
  <nav class="header">
    <div class="logo">🎳 Brony Bowling</div>

    <div class="links">
      <router-link to="/">Главная</router-link>
      <router-link to="/booking">Бронирование</router-link>

      <router-link v-if="!isAuth" to="/login">Вход</router-link>
      <router-link v-if="!isAuth" to="/register">Регистрация</router-link>

      <router-link v-if="isAuth" to="/profile">Профиль</router-link>
      <button v-if="isAuth" @click="logout">Выход</button>
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
  display: flex;
  justify-content: space-between;
  padding: 16px 32px;
  background: rgba(0,0,0,0.3);
  backdrop-filter: blur(10px);
}

.links {
  display: flex;
  gap: 16px;
  align-items: center;
}

a {
  color: white;
  text-decoration: none;
}

button {
  background: transparent;
  border: 1px solid white;
  color: white;
  padding: 6px 12px;
  border-radius: 6px;
  cursor: pointer;
}
</style>
