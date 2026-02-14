import { bookingApi, authHeader } from './api'

export async function getAvailableLanes(start, end) {
  const r = await bookingApi.get('/lanes/available', {
    params: { start, end }
  })
  return r.data
}

export async function createBooking(data) {
  const r = await bookingApi.post('/bookings', data, {
    headers: authHeader()
  })
  return r.data
}
