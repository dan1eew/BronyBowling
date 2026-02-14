import { createRouter, createWebHistory } from 'vue-router'

import Login from '../views/LoginView.vue'
import Register from '../views/RegisterView.vue'
import Booking from '../views/BookingView.vue'
import Payment from '../views/PaymentView.vue'

const routes = [
  { path: '/', component: Booking },
  { path: '/login', component: Login },
  { path: '/register', component: Register },
  { path: '/payment', component: Payment }
]

export const router = createRouter({
  history: createWebHistory(),
  routes
})
