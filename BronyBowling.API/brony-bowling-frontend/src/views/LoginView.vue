<template>
  <div class="card">
    <h2>Вход</h2>
    <input v-model="phone" placeholder="Телефон" />
    <input v-model="password" type="password" placeholder="Пароль" />
    <button @click="doLogin">Войти</button>
    <p class="error">{{ error }}</p>
  </div>
</template>

<script setup>import { ref } from 'vue'
import { login } from '../services/authService'
import { useRouter } from 'vue-router'

const router = useRouter()
const phone = ref('')
const password = ref('')
const error = ref('')

async function doLogin() {
  try {
    await login(phone.value, password.value)
    router.push('/')
  } catch {
    error.value = 'Ошибка входа'
  }
}</script>
