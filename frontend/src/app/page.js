import HomePage from './HomePage'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import apiPath from '@/api-urls'
import HomepageSkeleton from '@/components/skeleton/HomepageSkeleton'

const Page = async () => {
  const getHomePage = await fetchServerSideApi({
    endpoint: apiPath.getHomePage
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
