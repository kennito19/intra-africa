'use client'

import { destroyCookie, setCookie } from 'nookies'

const actionHandler = async (action, router) => {
  switch (action.code) {
    case 401: {
      if (action?.userId && action?.refreshToken) {
        // auth token
        setCookie(null, 'userToken', action?.userToken, {
          maxAge: 1 * 24 * 60 * 60,
          path: '/'
        })

        setCookie(null, 'deviceId', action?.deviceId, {
          maxAge: 1 * 24 * 60 * 60,
          path: '/'
        })

        setCookie(null, 'refreshToken', action?.refreshToken, {
          maxAge: 1 * 24 * 60 * 60,
          path: '/'
        })

        setCookie(null, 'userId', action?.userId, {
          maxAge: 1 * 24 * 60 * 60,
          path: '/'
        })

        setCookie(null, 'sessionId', action?.sessionId, {
          maxAge: 1 * 24 * 60 * 60,
          path: '/'
        })
      } else if (action?.userToken && action?.deviceId) {
        setCookie(null, 'userToken', action?.userToken, {
          maxAge: 1 * 24 * 60 * 60,
          path: '/'
        })

        setCookie(null, 'deviceId', action?.deviceId, {
          maxAge: 1 * 24 * 60 * 60,
          path: '/'
        })
        setCookie(null, 'sessionId', action?.sessionId, {
          maxAge: 1 * 24 * 60 * 60,
          path: '/'
        })
        const cookiesToDestroy = ['role', 'userId', 'refreshToken']
        cookiesToDestroy.forEach((cookieName) =>
          destroyCookie(null, cookieName)
        )

        if (action.forcefullyLoggedOut) {
          // logout the user and navigate to homepage
          // dispatch(logOut())
          router.push('/')
        }
      }

      break
    }
  }
}

export default actionHandler
