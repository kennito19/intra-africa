import { setCookie, parseCookies, destroyCookie } from 'nookies'
import axiosProvider from './AxiosProvider'
import axios from 'axios'
import { getBaseUrl } from './GetBaseUrl'
import { store } from '@/redux/store'
// import handleExceptions from './handleExceptions'

export const generateDeviceId = () => {
  const timestamp = Date.now()
  const randomString = Math.random().toString(36).substring(2, 12)
  const randomStringAdd = Math.random().toString(36).substring(2, 12)
  return `${timestamp}-${randomString + randomStringAdd}`
}

const generateToken = async () => {
  return new Promise(async (resolve, reject) => {
    const cookies = parseCookies()
    let deviceId = ''

    if (cookies.deviceId) {
      deviceId = cookies.deviceId
    } else {
      deviceId = generateDeviceId()
    }

    setCookie(null, 'deviceId', deviceId, {
      maxAge: 30 * 24 * 60 * 60,
      path: '/'
    })

    const response = await axios.get(
      `${getBaseUrl()}Account/GenerateNoAuthToken?deviceId=${deviceId}`
    )

    if (response?.status === 200) {
      setCookie(null, 'userToken', response?.data?.Token, {
        maxAge: 24 * 60 * 60 * 6,
        path: '/'
      })
      destroyCookie(null, 'refreshToken')
      destroyCookie(null, 'userId')
      destroyCookie(null, 'sessionId')
      return resolve()
    } else if (response?.status === 401) {
      generateToken()
    }

    return reject('Exception:- unable to generate new userToken')
  })
}

const updateToken = async (cookies) => {
  const { deviceId, userToken, refreshToken, userId } = cookies
  // let config = {
  //   headers: {
  //     'Content-Type': 'application/json',
  //     accept: '*/*'
  //   }
  // }
  // const data = {
  //   deviceId,
  //   accessToken: userToken,
  //   refreshToken,
  //   userId
  // }
  const response = await axiosProvider({
    method: 'POST',
    endpoint: 'gonari/token/update/v1',
    purplleUrl: true,
    headers: {
      token: userToken,
      deviceId,
      refreshToken
    },
    isToken: true
  })
  // const getNewToken = await axios.post(
  //   `${baseUrl}Account/Token/GetNewTokens`,
  //   data,
  //   config
  // )

  if (response && response?.status === 200) {
    // const newAccessToken = await getNewToken?.data?.accessToken
    // const newRefreshToken = await getNewToken?.data?.refreshToken
    // setCookie(null, 'userToken', newAccessToken, {
    //   maxAge: 30 * 24 * 60 * 60,
    //   path: '/'
    // })
    // setCookie(null, 'refreshToken', newRefreshToken, {
    //   path: '/'
    // })
  } else {
    localStorage.clear()
    const cookieVals = [
      'deviceId',
      'userToken',
      'refreshToken',
      'addressId',
      'pincode'
    ]

    for (let val of cookieVals) {
      destroyCookie(null, val)
    }

    nookieProvider('generate', parseCookies())
  }
}

export const nookieProvider = async (action, cookies) => {
  switch (action) {
    case 'update':
      await updateToken(cookies)
      break

    case 'generate':
      await generateToken()
      break

    default:
      const user =
        localStorage.getItem('user') === 'undefined'
          ? {}
          : JSON.parse(localStorage.getItem('user'))

      // if (user && user.id) {
      //     localStorage.clear()
      //     const cookieVals = ['addressId', 'pincode', 'userToken', 'refreshToken', 'deviceId']

      //     for (let val of cookieVals) {
      //         destroyCookie(null, val)
      //     }

      //     const { deviceId, userToken } = cookies

      //     const response = await axiosProvider({
      //         method: 'POST',
      //         endpoint: 'logout',
      //         headers: {
      //             token: userToken,
      //             deviceId
      //         },
      //         isUserUrl: true
      //     })

      //     if (response.status === 200) {
      //         nookieProvider('generate', parseCookies())
      //         window.location.pathname = '/login'
      //     } else {
      //         if (response?.status === 401) {
      //             await handleExceptions(response?.status, { action: response?.data?.action ? response?.data?.action : null })
      //         } else {
      //             handleExceptions(response?.status)
      //         }
      //     }
      // }
      break
  }
}
