import apiPath from '@/api-urls'
import { encryptId, objectToQueryString, validateQuery } from '@/lib/GetBaseUrl'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import ProductListPage from '../../[categoryName]/ProductListPage'

const page = async ({ searchParams, params }) => {
  let query = objectToQueryString(
    {
      ...searchParams,
      searchTexts: encryptId(params?.searchText?.replace(/-/g, ' '))
    },
    true
  )
  if (query) {
    const isValid = validateQuery(query)
    if (!isValid) return { data: null, code: 500 }
  }

  if (!params?.searchText) {
    return { notFound: true }
  }

  const queryParams = `?${query}&pageIndex=1&pageSize=10`
  const getProducts = await fetchServerSideApi({
    endpoint: apiPath.getUserProduct,
    queryParams
  })
    .then((response) => {
      if (response) {
        return response
      }
    })
    .catch((error) => {
      return error
    })
  return <ProductListPage products={getProducts} module='SearchWiseProduct' />
}

export default page
