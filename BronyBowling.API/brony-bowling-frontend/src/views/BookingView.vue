<template>
  <div class="card">
    <h2>Бронирование</h2>

    <input type="date" v-model="date" />
    <input type="time" v-model="start" />
    <input type="time" v-model="end" />

    <button @click="search">Найти дорожки</button>

    <select v-if="lanes.length" v-model="laneId">
      <option v-for="l in lanes" :key="l.bowlingLaneId"
              :value="l.bowlingLaneId">
        Дорожка №{{ l.number }}
      </option>
    </select>

    <div v-if="!isAuth">
      <input v-model="guestFullName" placeholder="ФИО" />
      <input v-model="guestPhone" placeholder="Телефон" />
    </div>

    <button @click="book">Забронировать</button>
  </div>
</template>

<script setup>import { ref, computed } from 'vue'
import { getAvailableLanes, createBooking } from '../services/bookingService'
import { useRouter } from 'vue-router'

const router = useRouter()

const date = ref('')
const start = ref('')
const end = ref('')
const lanes = ref([])
const laneId = ref(null)

const guestFullName = ref('')
const guestPhone = ref('')

const isAuth = computed(() => !!localStorage.getItem('jwt'))

async function search() {
  const startTime = `${date.value}T${start.value}`
  const endTime = `${date.value}T${end.value}`

  lanes.value = await getAvailableLanes(startTime, endTime)
}

async function book() {
  const booking = await createBooking({
    bowlingLaneId: laneId.value,
    startTime: `${date.value}T${start.value}`,
    endTime: `${date.value}T${end.value}`,
    guestFullName: isAuth.value ? null : guestFullName.value,
    guestPhone: isAuth.value ? null : guestPhone.value
  })

  localStorage.setItem('booking', JSON.stringify(booking))
  router.push('/payment')
}</script>
