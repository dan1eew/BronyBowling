<template>
  <div class="card">
    <h2>Оплата</h2>

    <div v-if="booking">
      <p>Дорожка: {{ booking.bowlingLaneId }}</p>
      <p>Начало: {{ booking.startTime }}</p>
      <p>Конец: {{ booking.endTime }}</p>
      <p>Сумма: {{ price }} ₽</p>

      <button @click="pay">Оплатить</button>
    </div>
  </div>
</template>

<script setup>import { computed } from 'vue'

const booking = JSON.parse(localStorage.getItem('booking'))

const price = computed(() => {
  if (!booking) return 0
  const start = new Date(booking.startTime)
  const end = new Date(booking.endTime)
  return ((end - start) / 1000 / 60 / 60) * 600
})

function pay() {
  alert('Тестовая оплата успешна ✅')
}</script>
