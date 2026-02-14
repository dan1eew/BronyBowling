<template>
  <header class="header">
    <div class="logo">Brony Bowling</div>
    <nav class="nav">
      <router-link to="/">Главная</router-link>
      <router-link to="/booking">Бронирование</router-link>
      <router-link v-if="!isAuth" to="/login">Вход</router-link>
      <router-link v-if="isAuth" to="/profile">Профиль</router-link>
      <button v-if="isAuth" @click="logout" class="logout">Выход</button>
    </nav>
  </header>
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
    height: 76px;
    background: #000000;
    border-bottom: 1px solid #1e2937;
    display: flex;
    align-items: center;
    padding: 0 60px;
    z-index: 100;
  }

  .logo {
    position: absolute;
    left: 50%;
    transform: translateX(-50%);
    font-size: 24px;
    font-weight: 800;
    color: #22c55e;
  }

  .nav {
    margin-left: auto;
    display: flex;
    gap: 40px;
    font-size: 15px;
  }

    .nav a {
      color: #cbd5e1;
      text-decoration: none;
    }

      .nav a:hover {
        color: #22c55e;
      }

  .logout {
    background: transparent;
    border: 1.5px solid #ef4444;
    color: #ef4444;
    padding: 6px 18px;
    border-radius: 40px;
    font-size: 14px;
  }
</style>
