<template>
  <div class="page">
    <h1>Бронирование</h1>

    <div class="panel">
      <input type="date" v-model="date" />
      <input type="time" v-model="start" />
      <input type="time" v-model="end" />
      <button @click="search">Найти</button>
    </div>

    <div v-if="error" class="error">{{ error }}</div>
    <div v-if="success" class="success">{{ success }}</div>

    <div class="lanes" v-if="lanes.length">
      <div v-for="lane in lanes"
           :key="lane.bowlingLaneId"
           class="lane"
           :class="{ selected: laneId === lane.bowlingLaneId }"
           @click="laneId = lane.bowlingLaneId">
        Дорожка №{{ lane.number }}
      </div>
    </div>

    <div v-if="!isAuth" class="guest">
      <input placeholder="ФИО" v-model="guestFullName" />
      <input placeholder="Телефон" v-model="guestPhone" />
    </div>

    <button class="book-btn" @click="book">Забронировать</button>
  </div>
</template>

<script setup>
  import { ref, computed } from 'vue'
  import { getAvailableLanes, createBooking } from '../services/bookingService'

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

  function validate() {
    if (!date.value || !start.value || !end.value)
      return 'Выберите дату и время'

    if (!isAuth.value) {
      if (!guestFullName.value) return 'Введите ФИО'
      if (!guestPhone.value) return 'Введите телефон'
    }

    return null
  }

  async function search() {
    error.value = ''
    const startTime = `${date.value}T${start.value}:00`
    const endTime = `${date.value}T${end.value}:00`

    try {
      lanes.value = await getAvailableLanes(startTime, endTime)
      if (!lanes.value.length) error.value = 'Нет свободных дорожек'
    } catch {
      error.value = 'Ошибка загрузки дорожек'
    }
  }

  async function book() {
    error.value = ''
    success.value = ''

    const validationError = validate()
    if (validationError) {
      error.value = validationError
      return
    }

    if (!laneId.value) {
      error.value = 'Выберите дорожку'
      return
    }

    try {
      await createBooking({
        bowlingLaneId: laneId.value,
        startTime: `${date.value}T${start.value}:00`,
        endTime: `${date.value}T${end.value}:00`,
        guestName: isAuth.value ? null : guestFullName.value,
        guestPhone: isAuth.value ? null : guestPhone.value
      })

      success.value = 'Бронирование успешно создано 🎉'
    } catch {
      error.value = 'Ошибка бронирования'
    }
  }
</script>

<style scoped>
  .page {
    padding: 40px;
    color: white;
    max-width: 900px;
    margin: auto;
  }

  .panel {
    display: flex;
    gap: 10px;
    margin-bottom: 20px;
  }

  .lanes {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
    gap: 10px;
    margin: 20px 0;
  }

  .lane {
    padding: 12px;
    border-radius: 8px;
    background: rgba(255,255,255,0.1);
    cursor: pointer;
    text-align: center;
  }

    .lane.selected {
      background: #42b883;
    }

  .error {
    background: #ef4444;
    padding: 10px;
    border-radius: 6px;
    margin-bottom: 10px;
  }

  .success {
    background: #22c55e;
    padding: 10px;
    border-radius: 6px;
    margin-bottom: 10px;
  }

  .book-btn {
    margin-top: 20px;
    padding: 12px;
    width: 100%;
    background: #42b883;
    border: none;
    border-radius: 8px;
  }
</style>
