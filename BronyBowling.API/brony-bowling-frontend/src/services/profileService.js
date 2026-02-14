import { profileApi, authHeader } from './api'

export async function getProfile() {
  const r = await profileApi.get('/profile', {
    headers: authHeader()
  })
  return r.data
}

export async function updateProfile(data) {
  return await profileApi.put('/profile', data, {
    headers: authHeader()
  })
}

export async function deleteProfile() {
  return await profileApi.delete('/profile', {
    headers: authHeader()
  })
}
