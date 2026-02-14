<template>
  <div class="card">
    <h2>Регистрация</h2>
    <input v-model="phoneNumber" placeholder="Телефон" />
    <input v-model="password" type="password" placeholder="Пароль" />
    <input v-model="lastName" placeholder="Фамилия" />
    <input v-model="firstName" placeholder="Имя" />
    <input v-model="middleName" placeholder="Отчество" />
    <button @click="doRegister">Создать аккаунт</button>
  </div>
</template>

<script setup>import { ref } from 'vue'
import { register } from '../services/authService'

const phoneNumber = ref('')
const password = ref('')
const firstName = ref('')
const lastName = ref('')
const middleName = ref('')

async function doRegister() {
  await register({
    phoneNumber: phoneNumber.value,
    password: password.value,
    firstName: firstName.value,
    lastName: lastName.value,
    middleName: middleName.value || null
  })

  alert('Успешно!')
}</script>
