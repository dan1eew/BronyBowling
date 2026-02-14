<template>
  <div class="page">
    <h1>Бронирование дорожки</h1>

    <div class="search">
      <input type="date" v-model="date" />
      <input type="time" v-model="start" />
      <input type="time" v-model="end" />
      <button @click="search">Найти дорожки</button>
    </div>

    <div v-if="lanes.length" class="lanes">
      <h2>Доступные дорожки:</h2>

      <div v-for="lane in lanes"
           :key="lane.id"
           class="lane"
           :class="{ selected: laneId === lane.id }"
           @click="laneId = lane.id">
        Дорожка №{{ lane.number }}
      </div>
    </div>

    <div class="guest" v-if="!isAuth">
      <h2>Данные гостя</h2>
      <input placeholder="ФИО" v-model="guestFullName" />
      <input placeholder="Телефон" v-model="guestPhone" />
    </div>

    <button class="book-btn" @click="book">
      Забронировать
    </button>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed } from 'vue'
  import { getAvailableLanes, createBooking } from '../services/bookingService'
  import { useRouter } from 'vue-router'

  const router = useRouter()

  const date = ref('')
  const start = ref('')
  const end = ref('')
  const lanes = ref < any[] > ([])
  const laneId = ref < number | null > (null)

  const guestFullName = ref('')
  const guestPhone = ref('')

  const isAuth = computed(() => !!localStorage.getItem('jwt'))

  async function search() {
    if (!date.value || !start.value || !end.value) {
      alert('Выберите дату и время')
      return
    }

    const startTime = `${date.value}T${start.value}:00`
    const endTime = `${date.value}T${end.value}:00`

    lanes.value = await getAvailableLanes(startTime, endTime)
  }

  async function book() {
    if (!laneId.value) {
      alert('Выберите дорожку')
      return
    }

    const booking = await createBooking({
      bowlingLaneId: laneId.value,
      startTime: `${date.value}T${start.value}:00`,
      endTime: `${date.value}T${end.value}:00`,
      guestName: isAuth.value ? null : guestFullName.value,
      guestPhone: isAuth.value ? null : guestPhone.value
    })

    localStorage.setItem('booking', JSON.stringify(booking))
    router.push('/payment')
  }
</script>

<style scoped>
  .page {
    padding: 40px;
    color: white;
  }

  .search,
  .guest {
    margin: 20px 0;
    display: flex;
    gap: 10px;
  }

  .lanes {
    margin-top: 20px;
  }

  .lane {
    padding: 10px;
    border: 1px solid white;
    margin-bottom: 10px;
    cursor: pointer;
  }

    .lane.selected {
      background: #42b883;
    }

  .book-btn {
    margin-top: 20px;
    padding: 10px 20px;
  }
</style>
