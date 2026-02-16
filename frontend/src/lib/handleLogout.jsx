import {
  getDeviceId,
  getRefreshToken,
  getSessionId,
  showToast
} from './GetBaseUrl'
import axiosProvider from './AxiosProvider'
import { nookieProvider } from './nookieProvider'
import { parseCookies } from 'nookies'
import { _exception } from './exceptionMessage';
import { logout } from '@/redux/features/userSlice'
import { clearCart } from '@/redux/features/cartSlice'
import { clearAddress } from '@/redux/features/addressSlice'



export const handleLogout = async (router, toast, setToast) => {
  const { store } = await import('../redux/store')
  const userState = store?.getState()?.user?.user
  const userId = userState?.userId || null
  const refreshToken = getRefreshToken()
  const sessionId = getSessionId()
  try {
    if (refreshToken) {
      const response = await axiosProvider({
        method: 'POST',
        endpoint: 'Account/Customer/logout',
        data: {
          userId: userId ? userId : sessionId,
          deviceid: getDeviceId(),
          refreshToken: getRefreshToken()
        }
      })
      if (response?.status === 200) {
        store.dispatch(logout())
        store.dispatch(clearCart())
        store.dispatch(clearAddress())
        showToast(toast, setToast, response)
        const cookies = parseCookies()
        nookieProvider('generate', cookies)
        router && router?.push('/')
      }
    } else {
      store.dispatch(clearCart())
      store.dispatch(clearAddress())
      store.dispatch(logout())
      const cookies = parseCookies()
      nookieProvider('generate', cookies)
    }
  } catch (error) {
    showToast(toast, setToast, {
      data: { code: 204, message: _exception?.message }
    })
  }
}
