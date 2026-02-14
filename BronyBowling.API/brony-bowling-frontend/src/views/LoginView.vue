<template>
  <div class="auth-page">
    <div class="glass-card">
      <h1>Вход в аккаунт</h1>

      <BaseAlert v-if="error" type="error">{{ error }}</BaseAlert>

      <BaseInput v-model="phone" placeholder="Телефон" type="tel" label="Номер телефона" />
      <BaseInput v-model="password" placeholder="Пароль" type="password" label="Пароль" />

      <button @click="doLogin" class="neon-btn green">Войти</button>

      <p class="switch">Нет аккаунта? <router-link to="/register">Зарегистрироваться</router-link></p>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import { login } from '../services/authService'
  import { useRouter } from 'vue-router'
  import BaseAlert from '../components/BaseAlert.vue'
  import BaseInput from '../components/BaseInput.vue'

  const router = useRouter()
  const phone = ref('')
  const password = ref('')
  const error = ref('')

  async function doLogin() {
    error.value = ''
    if (!phone.value || !password.value) return error.value = 'Заполните все поля'

    try {
      await login(phone.value, password.value)
      router.push('/booking')
    } catch {
      error.value = 'Неверный номер или пароль'
    }
  }
</script>

<style scoped>
  .auth-page {
    display: flex;
    align-items: center;
    justify-content: center;
    min-height: 100vh;
    padding: 40px;
  }

  .glass-card {
    background: rgba(255,255,255,0.07);
    backdrop-filter: blur(20px);
    border: 1px solid rgba(255,255,255,0.1);
    border-radius: 24px;
    padding: 48px 40px;
    width: 100%;
    max-width: 420px;
    box-shadow: 0 25px 50px rgba(0,0,0,0.5);
  }

  .neon-btn {
    width: 100%;
    padding: 18px;
    font-size: 18px;
    font-weight: 700;
    border: none;
    border-radius: 16px;
    cursor: pointer;
    margin-top: 12px;
    transition: all 0.3s;
  }

  .green {
    background: linear-gradient(90deg, #22c55e, #4ade80);
  }
</style>
