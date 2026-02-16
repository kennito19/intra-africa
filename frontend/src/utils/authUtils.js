import { getBaseUrl } from '@/lib/GetBaseUrl'
import { generateDeviceId } from '@/lib/nookieProvider'
import axios from 'axios'

export const fetchNoAuthToken = async () => {
  const deviceId = generateDeviceId()
  try {
    const response = await axios.get(
      `${getBaseUrl()}Account/GenerateNoAuthToken?deviceId=${deviceId}`
    )

    if (response?.status === 200) {
      return { userToken: response?.data?.Token, deviceId }
    }
  } catch (error) {
    console.error('An error occurred:', error)
    return 'Something Went Wrong'
  }
}
