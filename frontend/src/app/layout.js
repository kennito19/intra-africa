import apiPath from '@/api-urls'
import Footer from '@/components/layout/Footer'
import Header from '@/components/layout/Header'
import { Providers } from '@/redux/provider'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import { headers } from 'next/headers'
import ClientProvider from './ClientProvider'
import './globals.css'
import './main.css'

export const metadata = {
  title: `Intra Africa - Your Destination for Quality Products`,
  description: ''
}

export default async function RootLayout({ children }) {
  const headersList = headers()
  const pathname = headersList.get('next-url')
  const getCategoryMenu = await fetchServerSideApi({
    endpoint: apiPath?.getMenu
  })
    .then((response) => {
      if (response) {
        return response
      }
    })
    .catch((error) => {
      return error
    })

  const getStaticData = await fetchServerSideApi({
    endpoint: apiPath?.getStaticPages,
    userToken: getCategoryMenu?.action?.userToken
      ? getCategoryMenu?.action?.userToken
      : null,
    deviceId: getCategoryMenu?.action?.deviceId
      ? getCategoryMenu?.action?.deviceId
      : null
  })
    .then((response) => {
      if (response) {
        return response
      }
    })
    .catch((error) => {
      return error
    })

  return (
    <html lang='en'>
      <body>
        <Providers>
          <ClientProvider>
            {pathname !== '/user/checkout' && (
              <Header categoryMenu={getCategoryMenu} />
            )}
            {children}
            <Footer staticData={getStaticData} />
          </ClientProvider>
        </Providers>
      </body>
    </html>
  )
}
