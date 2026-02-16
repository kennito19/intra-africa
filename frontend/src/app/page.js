import HomePage from './HomePage'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import apiPath from '@/api-urls'
import HomepageSkeleton from '@/components/skeleton/HomepageSkeleton'

export const dynamic = 'force-dynamic'

const Page = async () => {
  let getHomePage = null
  try {
    const response = await fetchServerSideApi({
      endpoint: apiPath.getHomePage
    })
    if (response && typeof response === 'object' && response.code !== undefined) {
      getHomePage = JSON.parse(JSON.stringify(response))
    }
  } catch (error) {
    getHomePage = null
  }
  return (
    <>
      {getHomePage ? (
        <HomePage homePageData={getHomePage} />
      ) : (
        <HomepageSkeleton />
      )}
    </>
  )
}

export default Page
