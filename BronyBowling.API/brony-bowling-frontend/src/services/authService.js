import { authApi } from './api'

export async function login(phoneNumber, password) {
  const r = await authApi.post('/login', { phoneNumber, password })
  localStorage.setItem('jwt', r.data.token)
  return r.data
}

export async function register(data) {
  return await authApi.post('/register', data)
}

export function logout() {
  localStorage.removeItem('jwt')
}
