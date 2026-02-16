import { getBaseUrl } from '@/lib/GetBaseUrl'
import axios from 'axios'
import { cookies } from 'next/headers'

const generateDeviceId = () => {
  const timestamp = Date.now()
  const randomString = Math.random().toString(36).substring(2, 8)
  return `${timestamp}-${randomString}`
}

const genreateNoAuthToken = async () => {
  const deviceId = generateDeviceId()
  try {
    const response = await axios.get(
      `${getBaseUrl()}Account/GenerateNoAuthToken?deviceId=${deviceId}`
    )
    if (response?.status === 200) {
      return { userToken: response?.data?.Token, deviceId }
    }
  } catch (error) {
    return null
  }
}

const genreateAuthToken = async () => {
  const nextCookies = cookies()
  const userId = nextCookies.get('userId')?.value
  const deviceId = nextCookies.get('deviceId')?.value
  const userToken = nextCookies.get('userToken')?.value
  const refreshToken = nextCookies.get('refreshToken')?.value
  const data = {
    deviceId,
    accessToken: userToken,
    refreshToken,
    userId
  }

  try {
    const response = await axios.post(
      `${process?.env?.NEXT_PUBLIC_API_URL}Account/Token/GetNewTokens`,
      data
    )
    if (response?.data?.code === 200) {
      return {
        userToken: response?.data?.accessToken,
        deviceId,
        refreshToken,
        userId
      }
    } else {
      const getNoAuthToken = await genreateNoAuthToken()
      return {
        userToken: getNoAuthToken?.userToken,
        deviceId: getNoAuthToken?.deviceId
      }
    }
  } catch (error) {
    return null
  }
}

//@implement error expectional handler
const handleException = async (status_code, endpoint) => {
  const nextCookies = cookies()
  const userId = nextCookies.get('userId')?.value
  const refreshToken = nextCookies.get('refreshToken')?.value

  switch (status_code) {
    case 401: {
      if (userId && refreshToken) {
        const getAuthToken = await genreateAuthToken()

        if (
          getAuthToken &&
          getAuthToken?.userToken &&
          getAuthToken?.deviceId &&
          getAuthToken?.refreshToken &&
          getAuthToken?.userId
        ) {
          const headers = {
            Authorization: `Bearer ${getAuthToken.userToken}`,
            device_id: getAuthToken?.deviceId
          }
          try {
            const response = await axios.get(endpoint, { headers })
            return (
              {
                ...response?.data,
                action: {
                  code: 401,
                  userToken: getAuthToken?.userToken,
                  deviceId: getAuthToken?.deviceId,
                  refreshToken: getAuthToken.refreshToken,
                  userId: getAuthToken.userId
                }
              } ?? []
            )
          } catch (error) {
            return []
          }
        } else {
          if (
            getAuthToken &&
            getAuthToken?.userToken &&
            getAuthToken?.deviceId
          ) {
            const headers = {
              Authorization: `Bearer ${getAuthToken.userToken}`,
              device_id: getAuthToken?.deviceId
            }
            try {
              const response = await axios.get(endpoint, { headers })
              return (
                {
                  ...response?.data,
                  action: {
                    code: 401,
                    userToken: getAuthToken?.userToken,
                    deviceId: getAuthToken?.deviceId,
                    forcefullyLoggedOut: true
                  }
                } ?? []
              )
            } catch (error) {
              return []
            }
          }
        }
      } else {
        const getNoAuthToken = await genreateNoAuthToken()
        if (
          getNoAuthToken &&
          getNoAuthToken?.userToken &&
          getNoAuthToken?.deviceId
        ) {
          const headers = {
            Authorization: `Bearer ${getNoAuthToken.userToken}`,
            device_id: getNoAuthToken?.deviceId
          }
          try {
            const response = await axios.get(endpoint, { headers })
            return (
              {
                ...response?.data,
                action: {
                  code: 401,
                  userToken: getNoAuthToken?.userToken,
                  deviceId: getNoAuthToken?.deviceId
                }
              } ?? []
            )
          } catch (error) {
            return []
          }
        }
      }
    }
    default:
      break
  }
}

//@ Implementation a genric funcion for calling server side apis
export const fetchServerSideApi = async ({
  endpoint,
  queryParams = {},
  userToken = null,
  deviceId = null
}) => {
  const nextCookies = cookies()
  let queryString

  switch (typeof queryParams) {
    case 'object':
      queryString =
        '?' +
        Object.keys(queryParams)
          .map((key) => `${key}=${queryParams[key]}`)
          .join('&')

      break
    case 'string':
      queryString = queryParams
      break
    default: {
      throw error
    }
  }
  const headers = {
    Authorization: `Bearer ${
      userToken ? userToken : nextCookies.get('userToken')?.value
    }`,
    device_id: deviceId ? deviceId : nextCookies.get('deviceId')?.value
  }
  try {
    const response = await axios.get(`${endpoint}${queryString}`, { headers })
    if (response?.status == 200 && response?.data) {
      return response?.data
    } else {
      return []
    }
  } catch (error) {
    const getDataFromException = await handleException(
      error?.response?.status,
      `${endpoint}${queryString}`
    )

    return getDataFromException
  }
}
