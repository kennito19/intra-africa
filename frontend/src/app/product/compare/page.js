import { convertSpIdToQueryString, decryptId } from '@/lib/GetBaseUrl'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import apiPath from '@/api-urls'
import CompareProductPage from './CompareProductPage'

const page = async ({ searchParams }) => {
  let spIdLength = searchParams?.sp_id
    ?.split(',')
    ?.map((item) => decryptId(item))
  if (!spIdLength?.length > 0) {
    return {
      notFound: true
    }
  }
  let queryParams = convertSpIdToQueryString(searchParams, true)
  const getCompareProduct = await fetchServerSideApi({
    endpoint: apiPath.getComparisonProduct,
    queryParams
  })
    .then((response) => {
      if (typeof response === 'object' && Object.keys(response)?.length) {
        return response
      } else {
        return []
      }
    })
    .catch((error) => {
      return error
    })

  return <CompareProductPage product={getCompareProduct} />
}

export default page
