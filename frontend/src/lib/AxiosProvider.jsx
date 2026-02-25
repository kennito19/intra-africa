import axios from 'axios'
import {
  getBaseUrl,
  getDeviceId,
  getRefreshToken,
  getUserToken
} from './GetBaseUrl'
import { handleLogout } from './handleLogout'
import nookies from 'nookies'
import { logout } from '@/redux/features/userSlice'

const api = axios.create()

api.interceptors.request.use(
  (config) => {
    const token = getUserToken()
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

const removeUserOutOfSystem = async () => {
  import('../redux/store')
    .then((module) => {
      const { store } = module
      return store.dispatch(logout())
    })
    .catch((error) => {
      console.error('Failed to load store:', error)
    })
}

const getUserId = async () => {
  const { store } = await import('../redux/store')
  const userDetails = store.getState().user
  return userDetails?.user?.userId
}

let isRefreshing = false
let refreshPromise = null

api.interceptors.response.use(
  async (response) => {
    return response
  },
  async (error) => {
    const originalRequest = error.config
    let accessToken = getUserToken()
    let refreshToken = getRefreshToken()
    let deviceId = getDeviceId()
    const baseUrl = getBaseUrl()
    const userId = await getUserId()
    if (error?.response?.status === 401 && !originalRequest?._retry) {
      originalRequest._retry = true

      if (!isRefreshing) {
        isRefreshing = true

        try {
          if (userId) {
            const response = await axios.post(
              `${process?.env?.NEXT_PUBLIC_API_URL}Account/Token/GetNewTokens`,
              {
                accessToken,
                refreshToken,
                deviceId,
                userId
              }
            )

            if (response?.data?.code === 200) {
              let newAccessToken = response?.data?.accessToken
              let newRefreshToken = response?.data?.refreshToken
              refreshToken = newRefreshToken
              nookies.set(null, 'userToken', newAccessToken, { path: '/' })
              nookies.set(null, 'refreshToken', refreshToken, { path: '/' })
              originalRequest.headers.Authorization = `Bearer ${newAccessToken}`
              return api(originalRequest)
            } else if (response?.data?.code === 204) {
              removeUserOutOfSystem()
            } else {
              handleLogout()
            }
          } else {
            const response = await axios.get(
              `${baseUrl}Account/GenerateNoAuthToken?deviceId=${deviceId}`
            )

            if (response?.status === 200) {
              let newAccessToken = response?.data?.Token
              nookies.set(null, 'userToken', newAccessToken, { path: '/' })
              originalRequest.headers.Authorization = `Bearer ${
                accessToken ?? newAccessToken
              }`
              return api(originalRequest)
            }
          }
        } catch (error) {
          handleLogout()
        } finally {
          isRefreshing = false
        }
      } else {
        if (!refreshPromise) {
          refreshPromise = new Promise((resolve) => {
            setTimeout(async () => {
              await api(originalRequest)
              resolve()
              refreshPromise = null
            }, 10000)
          })
        }

        await refreshPromise
      }
    }

    return Promise.reject(error)
  }
)

async function axiosProvider(data) {
  let response = null
  let config = {
    ...data,
    headers: {
      ...data.headers,
      device_id: getDeviceId()
    }
  }
  let apiURL = `${getBaseUrl()}${config.endpoint}`

  if (config && config.queryString) {
    apiURL += config.queryString
  }

  try {
    switch (config.method) {
      case 'GET':
        response = await api.get(apiURL, {
          params: config.params || {},
          headers: config.headers || {}
        })
        break

      case 'POST':
        response = await api.post(apiURL, config.data, {
          params: config.params,
          headers: config.headers
        })
        break

      case 'PUT':
        response = await api.put(apiURL, config.data, {
          params: config.params,
          headers: config.headers
        })
        break

      case 'PATCH':
        response = await api.patch(apiURL, config.data, {
          params: config.params,
          headers: config.headers
        })
        break

      case 'DELETE':
        response = await api.delete(apiURL, {
          params: config.params,
          headers: config.headers
        })
        break

      default:
        break
    }
    return response ? response : []
  } catch (error) {
    return []
  }
}

export default axiosProvider
