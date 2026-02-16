import apiPath from '@/api-urls'
import { objectToQueryString, validateQuery } from '@/lib/GetBaseUrl'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import ProductListPage from '../products/[categoryName]/ProductListPage'

const page = async ({ searchParams }) => {
  let query = objectToQueryString(
    {
      ...searchParams,
      productCollectionId: searchParams?.productCollectionId
    },
    true
  )

  if (query) {
    const isValid = validateQuery(query)
    if (!isValid) return { data: null, code: 500 }
  }

  if (!searchParams?.productCollectionId) {
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
