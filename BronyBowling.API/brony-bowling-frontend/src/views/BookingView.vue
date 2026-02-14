<template>
  <div class="booking-page">
    <div class="container">
      <h1 class="title">Выберите время и дорожку</h1>

      <div class="search-panel glass-card">
        <div class="inputs">
          <BaseInput v-model="date" type="date" label="Дата" />
          <BaseInput v-model="start" type="time" label="Начало" />
          <BaseInput v-model="end" type="time" label="Конец" />
        </div>
        <button @click="search" class="search-btn">Найти свободные дорожки</button>
      </div>

      <BaseAlert v-if="error" type="error">{{ error }}</BaseAlert>
      <BaseAlert v-if="success" type="success">{{ success }}</BaseAlert>

      <div v-if="lanes.length" class="lanes-grid">
        <div v-for="lane in lanes" :key="lane.bowlingLaneId"
             class="lane-card"
             :class="{ selected: laneId === lane.bowlingLaneId }"
             @click="laneId = lane.bowlingLaneId">
          <div class="lane-number">№{{ lane.number }}</div>
          <div class="lane-status">Свободна</div>
        </div>
      </div>

      <div v-if="!isAuth" class="guest-form">
        <h3>Данные гостя</h3>
        <BaseInput v-model="guestFullName" placeholder="ФИО" label="ФИО" />
        <BaseInput v-model="guestPhone" placeholder="Телефон" type="tel" label="Телефон" />
      </div>

      <button v-if="laneId" @click="book" class="book-btn neon-btn green">
        Забронировать на {{ date }} {{ start }}–{{ end }}
      </button>
    </div>
  </div>
</template>

<script setup>
  import { ref, computed } from 'vue'
  import { getAvailableLanes, createBooking } from '../services/bookingService'
  import BaseInput from '../components/BaseInput.vue'
  import BaseAlert from '../components/BaseAlert.vue'

  const date = ref('')
  const start = ref('')
  const end = ref('')
  const lanes = ref([])
  const laneId = ref(null)
  const guestFullName = ref('')
  const guestPhone = ref('')
  const error = ref('')
  const success = ref('')

  const isAuth = computed(() => !!localStorage.getItem('jwt'))

  async function search() {
    error.value = ''
    if (!date.value || !start.value || !end.value) return error.value = 'Выберите дату и время'

    const startTime = `${date.value}T${start.value}:00`
    const endTime = `${date.value}T${end.value}:00`

    try {
      lanes.value = await getAvailableLanes(startTime, endTime)
    } catch {
      error.value = 'Не удалось загрузить дорожки'
    }
  }

  async function book() {
    if (!laneId.value) return error.value = 'Выберите дорожку'

    try {
      await createBooking({
        bowlingLaneId: laneId.value,
        startTime: `${date.value}T${start.value}:00`,
        endTime: `${date.value}T${end.value}:00`,
        guestName: isAuth.value ? null : guestFullName.value,
        guestPhone: isAuth.value ? null : guestPhone.value
      })
      success.value = 'Бронирование успешно создано!'
      setTimeout(() => { window.location.href = '/payment' }, 1500)
    } catch {
      error.value = 'Ошибка при бронировании'
    }
  }
</script>

<style scoped>
  .booking-page {
    padding: 40px 20px;
  }

  .container {
    max-width: 1100px;
    margin: 0 auto;
  }

  .search-panel {
    display: flex;
    gap: 20px;
    align-items: end;
    padding: 32px;
    margin-bottom: 40px;
  }

  .lanes-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
    gap: 20px;
    margin: 40px 0;
  }

  .lane-card {
    background: rgba(255,255,255,0.08);
    border: 2px solid transparent;
    border-radius: 20px;
    padding: 24px 16px;
    text-align: center;
    cursor: pointer;
    transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  }

    .lane-card:hover {
      transform: translateY(-8px);
      box-shadow: 0 20px 30px rgba(0,0,0,0.4);
    }

    .lane-card.selected {
      border-color: #22c55e;
      background: rgba(34, 197, 94, 0.15);
      box-shadow: 0 0 30px rgba(34, 197, 94, 0.5);
    }

  .book-btn {
    width: 100%;
    font-size: 1.3rem;
    padding: 22px;
    margin-top: 30px;
  }
</style>
