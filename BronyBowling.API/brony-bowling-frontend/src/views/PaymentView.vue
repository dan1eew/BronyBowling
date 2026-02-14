<template>
  <div class="payment-page">
    <div class="glass-card payment-card">
      <h1>Оплата бронирования</h1>

      <div class="order-summary">
        <div class="row">
          <span>Дорожка</span>
          <span class="value">#{{ booking?.bowlingLaneId }}</span>
        </div>
        <div class="row">
          <span>Время</span>
          <span class="value">{{ formatTime(booking?.startTime) }} — {{ formatTime(booking?.endTime) }}</span>
        </div>
        <div class="row total">
          <span>Итого</span>
          <span class="price">{{ price }} ₽</span>
        </div>
      </div>

      <button @click="pay" class="pay-btn">Оплатить картой •••••• 4242</button>
      <p class="secure">🔒 Защищённое соединение</p>
    </div>
  </div>
</template>

<script setup>
  import { computed } from 'vue'

  const booking = JSON.parse(localStorage.getItem('booking') || '{}')

  const price = computed(() => {
    if (!booking.startTime || !booking.endTime) return 0
    const diff = (new Date(booking.endTime) - new Date(booking.startTime)) / 3600000
    return Math.round(diff * 600)
  })

  function formatTime(time) {
    return new Date(time).toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' })
  }

  function pay() {
    alert('✅ Оплата прошла успешно! Приятной игры!')
    localStorage.removeItem('booking')
    setTimeout(() => window.location.href = '/', 800)
  }
</script>

<style scoped>
  .payment-page {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 90vh;
  }

  .payment-card {
    max-width: 460px;
    padding: 50px 40px;
  }

  .order-summary {
    background: rgba(255,255,255,0.05);
    border-radius: 16px;
    padding: 24px;
    margin: 30px 0;
  }

  .row {
    display: flex;
    justify-content: space-between;
    padding: 12px 0;
    border-bottom: 1px solid rgba(255,255,255,0.08);
  }

  .total {
    font-size: 1.4rem;
    font-weight: 700;
    border: none;
  }

  .price {
    color: #22c55e;
    font-size: 2rem;
  }

  .pay-btn {
    width: 100%;
    padding: 22px;
    background: linear-gradient(90deg, #22c55e, #eab308);
    font-size: 1.25rem;
    font-weight: 700;
    border: none;
    border-radius: 16px;
  }
</style>
