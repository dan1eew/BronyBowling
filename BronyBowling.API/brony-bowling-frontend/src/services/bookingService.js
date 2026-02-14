import { bookingApi, authHeader } from './api'

export const getAvailableLanes = async (start, end) => {
  const r = await bookingApi.get('/lanes/available', { params: { start, end } })
  return r.data
}

export const createBooking = async (data) => {
  const r = await bookingApi.post('/bookings', data, {
    headers: authHeader()
  })
  return r.data
}
