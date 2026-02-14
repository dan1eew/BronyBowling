<template>
  <div class="auth-page">
    <div class="glass-card">
      <h1>Создать аккаунт</h1>

      <BaseAlert v-if="error" type="error">{{ error }}</BaseAlert>
      <BaseAlert v-if="success" type="success">{{ success }}</BaseAlert>

      <BaseInput v-model="phoneNumber" placeholder="Телефон" type="tel" label="Номер телефона" />
      <BaseInput v-model="password" placeholder="Пароль" type="password" label="Пароль" />
      <BaseInput v-model="lastName" placeholder="Фамилия" label="Фамилия" />
      <BaseInput v-model="firstName" placeholder="Имя" label="Имя" />
      <BaseInput v-model="middleName" placeholder="Отчество (необязательно)" label="Отчество" />

      <button @click="doRegister" class="neon-btn purple">Создать аккаунт</button>

      <p class="switch">Уже есть аккаунт? <router-link to="/login">Войти</router-link></p>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import { register } from '../services/authService'
  import BaseAlert from '../components/BaseAlert.vue'
  import BaseInput from '../components/BaseInput.vue'

  const phoneNumber = ref('')
  const password = ref('')
  const firstName = ref('')
  const lastName = ref('')
  const middleName = ref('')
  const error = ref('')
  const success = ref('')

  async function doRegister() {
    error.value = ''
    success.value = ''

    if (!phoneNumber.value || !password.value || !lastName.value || !firstName.value) {
      return error.value = 'Заполните обязательные поля'
    }

    try {
      await register({
        phoneNumber: phoneNumber.value,
        password: password.value,
        firstName: firstName.value,
        lastName: lastName.value,
        middleName: middleName.value || null
      })
      success.value = 'Аккаунт создан! Теперь можете войти.'
    } catch (e) {
      error.value = e.response?.status === 409 ? 'Пользователь уже существует' : 'Ошибка регистрации'
    }
  }
</script>

<style scoped>
  /* те же стили что и в LoginView, только .purple { background: linear-gradient(90deg, #7c3aed, #a855f7); } */
  .purple {
    background: linear-gradient(90deg, #7c3aed, #c026d3);
  }
</style>
