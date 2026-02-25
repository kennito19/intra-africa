import { getBaseUrl } from './utils/helper/AllGlobalFunction/basicFunctions'

const baseUrl = getBaseUrl()

const apiPath = {
  getHomePage: baseUrl + `ManageHomePageSection/GetHomePageSection`,
  getHomepageBrands: baseUrl + 'ManageHomePageSection/GetBrands',
  getHomepageFeaturedProducts:
    baseUrl + 'ManageHomePageSection/GetFeaturedProducts',
  getStaticPages: baseUrl + 'ManageStaticPages',
  getStaticPagesDetails: baseUrl + 'ManageStaticPages/byId',
  getMenu: baseUrl + 'ManageHomePageSection/GetMenu',
  getOrders: baseUrl + 'User/Order/bysearchText',
  getOrder: baseUrl + 'User/Order/byId',
  getUserProduct: baseUrl + 'user/Product',
  getUserProductDetails: baseUrl + 'user/Product/ById',
  getComparisonProduct: baseUrl + 'user/Product/productComparison'
}

export default apiPath
