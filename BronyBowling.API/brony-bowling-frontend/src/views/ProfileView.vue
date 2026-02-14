<template>
  <div class="profile-page">
    <div class="container">
      <h1>Личный кабинет</h1>

      <div class="tabs">
        <button :class="{ active: tab === 'info' }" @click="tab = 'info'">Профиль</button>
        <button :class="{ active: tab === 'bookings' }" @click="tab = 'bookings'">Мои брони</button>
      </div>

      <div v-if="tab === 'info'" class="glass-card">
        <BaseInput v-model="form.firstName" label="Имя" />
        <BaseInput v-model="form.lastName" label="Фамилия" />
        <BaseInput v-model="form.phoneNumber" label="Телефон" type="tel" />
        <button @click="save" class="neon-btn">Сохранить изменения</button>
      </div>

      <div v-else class="glass-card">
        <p v-if="!bookings.length" class="empty">У вас пока нет броней</p>
        <!-- Здесь можно добавить список броней позже -->
      </div>
    </div>
  </div>
</template>

<script setup>import { ref } from 'vue'
import BaseInput from '../components/BaseInput.vue'

const tab = ref('info')
const form = ref({ firstName: '', lastName: '', phoneNumber: '' })
const bookings = ref([]) // можно подгрузить из API позже

function save() {
  alert('✅ Данные сохранены')
}</script>

<style scoped>
  .tabs {
    display: flex;
    gap: 8px;
    margin-bottom: 30px;
  }

    .tabs button {
      padding: 12px 28px;
      background: rgba(255,255,255,0.08);
      border: none;
      border-radius: 9999px;
      font-weight: 600;
    }

      .tabs button.active {
        background: #22c55e;
        color: black;
      }
</style>
