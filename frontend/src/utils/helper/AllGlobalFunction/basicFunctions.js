export const getBaseUrl = () => {
    // if (process.env.NODE_ENV !== 'production') {
    //   return process.env.NEXT_PUBLIC_API_URL
    // }
  
    return process.env.NEXT_PUBLIC_API_URL
  }

export const getUserToken = () => {
    const cookies = parseCookies()
    return !!cookies['userToken'] ? cookies['userToken'] : null
}

export const getRefreshToken = () => {
    const cookies = parseCookies()
    const refreshToken = cookies['refreshToken']
    return refreshToken ? refreshToken : null
}

export const getDeviceId = () => {
    const cookies = parseCookies()
    const deviceId = cookies['deviceId']
    return deviceId ? deviceId : null
}

export const getUserId = () => {
    const cookies = parseCookies()
    const userId = cookies['userId']
    return userId ? userId : null
}

export const getSessionId = () => {
    const cookies = parseCookies()
    const sessionId = cookies['sessionId']
    return sessionId ? sessionId : null
}

export const generateCaptcha = () => {
    let characters =
        'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'

    let result = ''
    const length = 6
    for (let i = 0; i < length; i++) {
        const randomIndex = Math.floor(Math.random() * characters.length)
        result += characters.charAt(randomIndex)
    }
    return result
}

export const focusInput = (elem) => {
    const inputField = document.getElementById(elem)
    if (inputField) {
        inputField.focus()
    }
}























