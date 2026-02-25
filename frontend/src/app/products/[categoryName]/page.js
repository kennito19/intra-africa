import apiPath from '@/api-urls'
import { decryptId, objectToQueryString, validateQuery } from '@/lib/GetBaseUrl'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import ProductListPage from './ProductListPage'

const page = async ({ searchParams }) => {
  let query = objectToQueryString(searchParams, true)
  if (query) {
    const isValid = validateQuery(query)
    if (!isValid) return { data: null, code: 500 }
  }

  // CategoryId is optional - if missing, show all products (CategoryId=0)
  if (searchParams?.CategoryId) {
    try {
      const categoryId = decryptId(searchParams.CategoryId)
      if (!categoryId && categoryId !== '0') return { notFound: true }
    } catch {
      return { notFound: true }
    }
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

  return (
    <ProductListPage products={getProducts} module='categoryWiseProducts' />
  )
}

export default page
