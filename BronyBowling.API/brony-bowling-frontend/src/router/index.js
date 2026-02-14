import { createRouter, createWebHistory } from 'vue-router'

import Home from '../views/HomeView.vue'
import Login from '../views/LoginView.vue'
import Register from '../views/RegisterView.vue'
import Booking from '../views/BookingView.vue'
import Profile from '../views/ProfileView.vue'
import Payment from '../views/PaymentView.vue'

const routes = [
  { path: '/', component: Home },
  { path: '/login', component: Login },
  { path: '/register', component: Register },
  { path: '/booking', component: Booking },
  { path: '/profile', component: Profile },
  { path: '/payment', component: Payment }
]

export const router = createRouter({
  history: createWebHistory(),
  routes
})
