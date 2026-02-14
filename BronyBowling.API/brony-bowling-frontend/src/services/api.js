import axios from 'axios'
baseURL: import.meta.env.VITE_AUTH_API

export const authApi = axios.create({
  baseURL: import.meta.env.VITE_AUTH_API
})

export const profileApi = axios.create({
  baseURL: import.meta.env.VITE_PROFILE_API
})

export const bookingApi = axios.create({
  baseURL: import.meta.env.VITE_BOOKING_API
})

export function authHeader() {
  const token = localStorage.getItem('jwt')
  return token ? { Authorization: `Bearer ${token}` } : {}
}
