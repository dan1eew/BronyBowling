<template>
  <div class="auth">
    <div class="card">
      <h2>Регистрация</h2>
      <BaseInput v-model="phoneNumber" type="tel" placeholder="Номер телефона" />
      <BaseInput v-model="password" type="password" placeholder="Пароль" />
      <BaseInput v-model="lastName" placeholder="Фамилия" />
      <BaseInput v-model="firstName" placeholder="Имя" />
      <BaseInput v-model="middleName" placeholder="Отчество (необязательно)" />
      <button @click="doRegister" class="btn">Создать аккаунт</button>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import { register } from '../services/authService'
  import BaseInput from '../components/BaseInput.vue'

  const phoneNumber = ref('')
  const password = ref('')
  const firstName = ref('')
  const lastName = ref('')
  const middleName = ref('')

  async function doRegister() {
    // твой старый код регистрации
  }
</script>

<style scoped>
  /* тот же стиль, что и в LoginView */
</style>
