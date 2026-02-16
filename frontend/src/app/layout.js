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

export const dynamic = 'force-dynamic'

export default async function RootLayout({ children }) {
  const headersList = headers()
  const pathname = headersList.get('next-url')

  let getCategoryMenu = null
  try {
    const menuResponse = await fetchServerSideApi({
      endpoint: apiPath?.getMenu
    })
    if (menuResponse && menuResponse.code !== undefined) {
      getCategoryMenu = JSON.parse(JSON.stringify(menuResponse))
    }
  } catch (error) {
    getCategoryMenu = null
  }

  let getStaticData = null
  try {
    const staticResponse = await fetchServerSideApi({
      endpoint: apiPath?.getStaticPages,
      userToken: getCategoryMenu?.action?.userToken || null,
      deviceId: getCategoryMenu?.action?.deviceId || null
    })
    if (staticResponse && staticResponse.code !== undefined) {
      getStaticData = JSON.parse(JSON.stringify(staticResponse))
    }
  } catch (error) {
    getStaticData = null
  }

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
