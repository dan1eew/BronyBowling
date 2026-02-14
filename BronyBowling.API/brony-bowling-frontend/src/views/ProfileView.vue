<template>
  <div class="profile">
    <div class="container">
      <div class="left">
        <h2>Профиль</h2>
        <BaseInput v-model="form.firstName" placeholder="Имя" />
        <BaseInput v-model="form.lastName" placeholder="Фамилия" />
        <BaseInput v-model="form.phoneNumber" type="tel" placeholder="Телефон" />
        <div class="actions">
          <button @click="save" class="save">Сохранить</button>
          <button @click="deleteProfile" class="delete">Удалить профиль</button>
        </div>
      </div>

      <div class="right">
        <h2>Мои бронирования</h2>
        <p v-if="!bookings.length" class="empty">Пока нет броней</p>
        <!-- сюда потом добавишь таблицу -->
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import BaseInput from '../components/BaseInput.vue'

  const form = ref({ firstName: '', lastName: '', phoneNumber: '' })
  const bookings = ref([])
  const save = () => alert('Сохранено')
  const deleteProfile = () => confirm('Удалить?') && alert('Удалено')
</script>

<style scoped>
  .profile {
    padding: 60px 40px;
  }

  .container {
    max-width: 1100px;
    margin: 0 auto;
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 60px;
  }

  .left, .right {
    background: #111827;
    padding: 40px;
    border-radius: 16px;
    border: 1px solid #1e2937;
  }

  h2 {
    margin-bottom: 32px;
    font-size: 24px;
  }

  .actions {
    display: flex;
    gap: 12px;
    margin-top: 32px;
  }

  .save, .delete {
    flex: 1;
    padding: 14px;
    border-radius: 10px;
    font-weight: 600;
  }

  .save {
    background: #22c55e;
    color: black;
  }

  .delete {
    background: #ef4444;
    color: white;
  }

  .empty {
    opacity: 0.6;
  }
</style>
