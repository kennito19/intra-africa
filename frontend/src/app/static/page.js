import { decryptId } from '@/lib/GetBaseUrl'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import apiPath from '@/api-urls'
import StaticPage from './StaticPage'

const page = async ({ searchParams }) => {
  const { id } = searchParams
  if (!id) {
    return {
      notFound: true
    }
  }

  const queryParams = `?id=${decryptId(decodeURIComponent(id))}`

  const getStaticData = await fetchServerSideApi({
    endpoint: apiPath.getStaticPagesDetails,
    queryParams
  })
    .then((response) => {
      if (response && response?.code == 200) {
        return response
      } else {
        return []
      }
    })
    .catch((error) => {
      return error
    })

  return <StaticPage staticData={getStaticData} />
}

export default page
