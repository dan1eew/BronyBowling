<template>
  <div class="auth">
    <div class="card">
      <h2>Вход</h2>
      <BaseInput v-model="phone" type="tel" placeholder="Номер телефона" />
      <BaseInput v-model="password" type="password" placeholder="Пароль" />
      <button @click="doLogin" class="btn">Войти</button>
      <p class="switch">Нет аккаунта? <router-link to="/register">Регистрация</router-link></p>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import { login } from '../services/authService'
  import { useRouter } from 'vue-router'
  import BaseInput from '../components/BaseInput.vue'

  const router = useRouter()
  const phone = ref('')
  const password = ref('')

  async function doLogin() {
    try {
      await login(phone.value, password.value)
      router.push('/booking')
    } catch {
      alert('Неверный номер или пароль')
    }
  }
</script>

<style scoped>
  .auth {
    min-height: 90vh;
    display: flex;
    align-items: center;
    justify-content: center;
    background: #000000;
  }

  .card {
    background: #111827;
    padding: 48px 40px;
    border-radius: 16px;
    width: 360px;
    border: 1px solid #1e2937;
  }

  h2 {
    text-align: center;
    margin-bottom: 36px;
    font-size: 26px;
  }

  .btn {
    width: 100%;
    padding: 15px;
    background: #22c55e;
    color: black;
    font-size: 16px;
    font-weight: 700;
    border-radius: 10px;
    margin-top: 20px;
  }

  .switch {
    text-align: center;
    margin-top: 28px;
    font-size: 14px;
  }
</style>
