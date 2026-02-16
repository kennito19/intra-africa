import { decryptId } from '@/lib/GetBaseUrl'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import apiPath from '@/api-urls'
import ProductDetailsPage from './ProductDetailsPage'

const page = async ({ params }) => {
  const { productGuid } = params
  if (!productGuid) {
    return {
      notFound: true
    }
  }

  const queryParams = `?ProductGUID=${decryptId(productGuid)}`

  const getProducts = await fetchServerSideApi({
    endpoint: apiPath.getUserProductDetails,
    queryParams
  })
    .then((response) => {
      if (response?.code) {
        return response
      } else {
        return []
      }
    })
    .catch((error) => {
      return error
    })

  return <ProductDetailsPage product={getProducts} />
}

export default page
