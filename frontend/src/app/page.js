import HomePage from './HomePage'
import { fetchServerSideApi } from '@/utils/serverSideUtils'
import apiPath from '@/api-urls'
export const dynamic = 'force-dynamic'

const Page = async () => {
  let getHomePage = null
  let fallbackBrands = []
  let fallbackProducts = []
  try {
    const [homeResponse, brandsResponse, productsResponse] = await Promise.all([
      fetchServerSideApi({
        endpoint: apiPath.getHomePage
      }),
      fetchServerSideApi({
        endpoint: apiPath.getHomepageBrands,
        queryParams: { pageIndex: 0, pageSize: 0, status: 'Active' }
      }),
      fetchServerSideApi({
        endpoint: apiPath.getHomepageFeaturedProducts,
        queryParams: { topProduct: 8 }
      })
    ])

    if (
      homeResponse &&
      typeof homeResponse === 'object' &&
      homeResponse.code !== undefined
    ) {
      getHomePage = JSON.parse(JSON.stringify(homeResponse))
    }

    if (
      brandsResponse &&
      typeof brandsResponse === 'object' &&
      brandsResponse.code === 200
    ) {
      fallbackBrands = JSON.parse(JSON.stringify(brandsResponse?.data ?? []))
    }

    if (
      productsResponse &&
      typeof productsResponse === 'object' &&
      productsResponse.code === 200
    ) {
      fallbackProducts = JSON.parse(
        JSON.stringify(productsResponse?.data ?? [])
      )
    }

    if (
      !getHomePage &&
      (fallbackBrands?.length > 0 || fallbackProducts?.length > 0)
    ) {
      getHomePage = { code: 200, data: null }
    }
  } catch (error) {
    getHomePage = null
  }

  if (!getHomePage) {
    getHomePage = { code: 200, data: null }
  }

  return (
    <HomePage
      homePageData={getHomePage}
      fallbackPayload={{
        brands: fallbackBrands,
        products: fallbackProducts
      }}
    />
  )
}

export default Page
