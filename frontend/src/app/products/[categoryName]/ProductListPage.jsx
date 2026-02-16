'use client'
import ComparePopover from '@/components/ComparePopover/ComparePopover'
import DataNotFound from '@/components/DataNotFound'
import LoginSignup from '@/components/LoginSignup'
import OffCanvasBottom from '@/components/OffCanvasBottom'
import ProductList from '@/components/ProductList'
import ProductlistSidebar from '@/components/Productlist-Sidebar'
import SortAndFilter from '@/components/SortAndFilter'
import Toaster from '@/components/base/Toaster'
import ProductCardSkeleton from '@/components/skeleton/ProductCardSkeleton'
import ProductListSkeleton from '@/components/skeleton/ProductListSkeleton'
import ProductViewSkeleton from '@/components/skeleton/ProductViewSkeleton'
import { handleWishlistClick } from '@/lib/AllGlobalFunction'
import axiosProvider from '@/lib/AxiosProvider'
import { _headTitle_, _projectName_ } from '@/lib/ConfigVariables'
import {
  decryptId,
  encryptId,
  filterSpecification,
  getCompareData,
  isAllowComparison,
  objectToQueryString,
  reactImageUrl,
  showToast,
  spaceToDash,
  stringToIntegerOrArray,
  validateQuery
} from '@/lib/GetBaseUrl'
import { _categoryImg_ } from '@/lib/ImagePath'
import { _exception } from '@/lib/exceptionMessage'
import actionHandler from '@/utils/actionHandler'
import Head from 'next/head'
import { useParams, useRouter, useSearchParams } from 'next/navigation'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { Waypoint } from 'react-waypoint'
import { useImmer } from 'use-immer'

const ProductListPage = ({ products, module }) => {
  const router = useRouter()
  const searchQuery = useSearchParams()
  const routerQuery = Object.fromEntries(searchQuery)
  const params = useParams()
  const [loading, setLoading] = useState(false)
  const [productData, setProductData] = useState()
  const { user } = useSelector((state) => state?.user)
  const [isActiveDrawer, setIsActiveDrawer] = useState({
    sortDrawer: false,
    filterDrawer: false
  })
  const [modalShow, setModalShow] = useState({
    show: false,
    data: null,
    module: null
  })
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const [filterDetails, setFilterDetails] = useImmer({
    pageSize: 10,
    pageIndex: 1,
    searchText: ''
  })
  const [hasNextPage, setHasNextPage] = useState(true)
  const [compareData, setCompareData] = useState()
  const [isGridViewVisible, setIsGridViewVisible] = useState(true)
  const [searchText, setSearchText] = useState()

  const currentURL = typeof window !== 'undefined' ? window.location.href : ''
  const [filtersObj, setFiltersObj] = useImmer({
    CategoryId:
      stringToIntegerOrArray(
        decryptId(searchQuery.get('CategoryId')),
        'category'
      ) ?? null,
    productCollectionId:
      stringToIntegerOrArray(params?.collectionId, 'productCollectionId') ??
      null,
    BrandIds: searchQuery.get('BrandIds')
      ? (searchQuery.get('BrandIds') || '')
          .split(',')
          .map((item) => Number(decryptId(item)))
      : [],
    SizeIds: searchQuery.get('SizeIds')
      ? (searchQuery.get('SizeIds') || '')
          .split(',')
          .map((item) => Number(decryptId(item)))
      : [],
    ColorIds: searchQuery.get('ColorIds')
      ? (searchQuery.get('ColorIds') || '')
          .split(',')
          .map((item) => Number(decryptId(item)))
      : [],
    SpecTypeValueIds: searchQuery.get('SpecTypeValueIds')
      ? (searchQuery.get('SpecTypeValueIds') || '')
          .split('|')
          .map((set) =>
            set
              .split(',')
              .map((item) => decryptId(item))
              .join(',')
          )
          .join('|')
      : '',
    specifications: [],
    fby: stringToIntegerOrArray(decryptId(searchQuery.get('fby'))) ?? '',
    MinDiscount:
      stringToIntegerOrArray(decryptId(searchQuery.get('MinDiscount'))) ?? null,
    MinPrice:
      stringToIntegerOrArray(decryptId(searchQuery.get('MinPrice'))) ?? '',
    MaxPrice:
      stringToIntegerOrArray(decryptId(searchQuery.get('MaxPrice'))) ?? '',
    PriceSort:
      stringToIntegerOrArray(decryptId(searchQuery.get('PriceSort'))) ?? 0
  })

  const setInitialStateFromQueryString = (productData) => {
    const query = filtersObj
    let initialFilters = {}
    const filterTypeToDataTypeMap = {
      BrandIds: 'array',
      ColorIds: 'array',
      SizeIds: 'array',
      fby: 'string',
      PriceSort: 'number',
      SpecTypeValueIds: 'array',
      CategoryId: 'number',
      searchTexts: 'string',
      MinDiscount: 'number'
    }
    Object.entries(query).forEach(([filterType, value]) => {
      const dataType = filterTypeToDataTypeMap[filterType]
      switch (dataType) {
        case 'array':
          if (filterType === 'SpecTypeValueIds') {
            if (value?.length) {
              const specValueIdArray =
                value && !Array.isArray(value)
                  ? value.includes(',') || value.includes('|')
                    ? value.split(/[|,]/).map(Number)
                    : [Number(value)]
                  : value
                  ? value
                  : []
              const specifications = []

              productData?.data?.filterList?.filter_types?.forEach(
                (filterType) => {
                  const matchingValues = filterType?.filterValues?.filter(
                    (filterValue) => {
                      return specValueIdArray?.includes(
                        filterValue?.filterValueId
                      )
                    }
                  )

                  if (matchingValues?.length > 0) {
                    const specId = filterType?.filterTypeId
                    matchingValues.forEach((filterValue) => {
                      const value = filterValue?.filterValueId
                      const valueName = filterValue?.filterValueName
                      specifications.push({
                        specId,
                        value,
                        valueName
                      })
                    })
                  }
                }
              )

              initialFilters['SpecTypeValueIds'] = specValueIdArray
              initialFilters['specifications'] = specifications
            }
          } else {
            initialFilters[filterType] =
              value?.length > 0 ? value?.map((item) => parseInt(item, 10)) : []
          }

          break
        case 'string':
          initialFilters[filterType] = value.toString()
          break
        case 'number':
          initialFilters[filterType] = value
          break
        // default:
        //   initialFilters[filterType] = value
        //   break
      }
    })
    setFiltersObj(initialFilters)
  }

  const handleViewClick = (view) => {
    if (view === 'Grid') {
      setIsGridViewVisible(true)
    } else {
      setIsGridViewVisible(false)
    }
  }

  const changeUrl = (filtersObj, categoryName) => {
    const allValuesAreNull = Object.values(filtersObj).every(
      (value) => value === null
    )
    if (!allValuesAreNull) {
      let endpoint
      switch (module) {
        case 'categoryWiseProducts':
          endpoint = `/products/${spaceToDash(
            categoryName ? categoryName : params?.categoryName
          )}?`
          break
        case 'SearchWiseProduct':
          endpoint = `/products/search/${spaceToDash(params?.searchText)}?`
          break
        case '':
          endpoint = `/collection`
          break
        default:
          console.log('error')
      }
      endpoint = endpoint + objectToQueryString(filtersObj)
      router.push(endpoint)
    }
  }

  const handleOptionChange = (event) => {
    let filterObj = filtersObj
    filterObj = {
      ...filterObj,
      PriceSort: event?.target?.value
        ? Number(event?.target?.value)
        : Number(event)
    }
    setFiltersObj(filterObj)
    changeUrl(filterObj, params?.categoryName)
    if (isActiveDrawer?.sortDrawer) {
      setIsActiveDrawer({ ...isActiveDrawer, sortDrawer: false })
    }
  }

  const collectionByCountDown = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'ManageCollection/byId',
        queryString: `?id=${params?.collectionId}`
      })
      setLoading(false)
      if (response?.status === 200) {
        setDate({
          startDate: response?.data?.data?.startDate,
          endDate: new Date(response?.data?.data?.endDate)
        })
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const fetchProductList = async (isWishlistClicked) => {
    let query = objectToQueryString(
      {
        ...routerQuery,
        categoryName: params?.categoryName,
        searchTexts: params?.searchText
          ? encryptId(params?.searchText?.replace(/-/g, ' '))
          : ''
      },
      true
    )
    if (query) {
      const isValid = validateQuery(query)
      if (isValid) {
        try {
          let data
          setLoading(true)
          const response = await axiosProvider({
            method: 'GET',
            endpoint: 'user/Product',
            queryString: query
              ? `?${query}&pageIndex=${filterDetails?.pageIndex}&pageSize=${filterDetails?.pageSize}`
              : `?pageIndex=${filterDetails?.pageIndex}&pageSize=${filterDetails?.pageSize}`
          })
          setLoading(false)
          if (response?.status === 200) {
            if (
              response?.data?.data?.products?.length ===
              response?.data?.pagination?.recordCount
            ) {
              setHasNextPage(false)
            } else {
              setFilterDetails((draft) => {
                draft.pageIndex = filterDetails?.pageIndex + 1
              })
            }
            setInitialStateFromQueryString(response?.data)

            if (
              productData?.pagination?.recordCount >
              productData?.data?.products?.length
            ) {
              data = {
                ...response?.data,
                data: {
                  ...response?.data?.data,
                  products: [
                    ...productData?.data?.products,
                    ...response?.data?.data?.products
                  ],
                  filterList: {
                    ...response?.data?.data?.filterList,
                    filteredBrand:
                      response?.data?.data?.filterList?.brand_filter,
                    filteredColor:
                      response?.data?.data?.filterList?.color_filter,
                    filteredSize: response?.data?.data?.filterList?.size_filter
                  }
                }
              }
            } else {
              data = {
                ...response?.data,
                data: {
                  ...response?.data?.data,
                  filterList: {
                    ...response?.data?.data?.filterList,
                    filteredBrand:
                      response?.data?.data?.filterList?.brand_filter,
                    filteredColor:
                      response?.data?.data?.filterList?.color_filter,
                    filteredSize: response?.data?.data?.filterList?.size_filter
                  }
                }
              }
            }
            setProductData(data)
            if (isWishlistClicked) {
              setProductData({ ...productData, code: null })
              setLoading(true)
              const response = await handleWishlistClick(
                isWishlistClicked,
                data,
                'productList',
                toast,
                setToast
              )
              setLoading(false)
              if (response?.wishlistResponse?.data?.code === 200) {
                setProductData(response)
              } else {
                setProductData(data)
              }
              response?.wishlistResponse &&
                showToast(toast, setToast, response?.wishlistResponse)
            }
          } else {
            setProductData(response?.data)
          }
        } catch (error) {
          setLoading(false)
          showToast(toast, setToast, {
            data: { code: 204, message: _exception?.message }
          })
        }
      } else {
        router?.push('/')
      }
    }
  }

  const handleSearch = async (
    searchText,
    propertyName,
    originalFieldName,
    fieldName
  ) => {
    const filteredResults = productData?.data?.filterList[
      originalFieldName
    ]?.filter((item) => {
      return item[propertyName]
        ?.toLowerCase()
        .includes(searchText?.trim()?.toLowerCase())
    })
    setProductData({
      ...productData,
      data: {
        ...productData?.data,
        filterList: {
          ...productData?.data?.filterList,
          [fieldName]: filteredResults
        }
      }
    })
  }

  const getMoreData = () => {
    if (filterDetails?.pageIndex > 1) {
      fetchProductList()
    }
  }

  const onClose = () => {
    setModalShow({ ...modalShow, show: false })
    if (user?.userId) {
      setTimeout(() => {
        fetchProductList(modalShow?.data)
      }, [500])
    }
  }

  // const closeMenu = (e) => {
  //   const wrapper = document.getElementById('p-prdlist__sidebar')

  //   if (
  //     wrapper?.classList.contains('active') &&
  //     e.target.classList.contains('p-prdlist__sidebar')
  //   ) {
  //     setIsActiveDrawer({ ...isActiveDrawer, filterDrawer: false })
  //   }
  // }

  // const specificPartRef = useDetectClickOutside({ onTriggered: closeMenu })

  useEffect(() => {
    if (
      searchQuery.get('categoryName') &&
      products?.data?.products?.length > 0
    ) {
      if (
        spaceToDash(searchQuery.get('categoryName'), true) !==
        products?.data?.products[0]?.categoryName
      ) {
        let endpoint = `/products/${spaceToDash(
          products?.data?.products[0]?.categoryName
            ? products?.data?.products[0]?.categoryName
            : searchQuery.get('categoryName')
        )}?`
        endpoint = endpoint + objectToQueryString(filtersObj)
        router.push(endpoint)
      }
    }
  }, [])

  useEffect(() => {
    if (params?.collectionId) {
      collectionByCountDown()
    }

    if (products?.action) {
      actionHandler(products?.action, router)
    }
  }, [])

  useEffect(() => {
    if (products && products?.code === 200) {
      setProductData({
        ...products,
        data: {
          ...products?.data,
          filterList: {
            ...products?.data?.filterList,
            filteredBrand: products?.data?.filterList?.brand_filter,
            filteredColor: products?.data?.filterList?.color_filter,
            filteredSize: products?.data?.filterList?.size_filter
          }
        }
      })
      if (products?.filterList?.filter_types?.length > 0) {
        setFiltersObj((draft) => {
          draft.specifications = filterSpecification(
            products?.data?.filterList?.filter_types
          )
        })
      }
      if (
        products?.data?.products?.length === products?.pagination?.recordCount
      ) {
        setHasNextPage(false)
      } else {
        setFilterDetails((draft) => {
          draft.pageIndex = filterDetails?.pageIndex + 1
        })
      }
    } else if (products?.code === 204) {
      setProductData({ data: null, code: 204 })
    } else if (products?.code === 500) {
      router?.push('/')
    }

    if (products) {
      setInitialStateFromQueryString(products)
    }
    setCompareData(getCompareData() ?? [])
  }, [products])

  return (
    <>
      <Head>
        <title>
          {params.categoryName ?? _projectName_} - {_headTitle_}
        </title>
        <meta
          name='title'
          content={`${params?.categoryName ?? _projectName_} - ${_headTitle_}`}
        />

        <meta
          name='description'
          content={`${params?.categoryName ?? _projectName_} - ${_headTitle_}`}
        />
        <meta
          name='keywords'
          content={`${params?.categoryName ?? _projectName_} - ${_headTitle_}`}
        />
        <link rel='canonical' href={currentURL} />
        <link rel='alternate' href={currentURL} hreflang='en-us' />
        <meta property='og:type' content='website' />
        <meta property='og:url' content={currentURL} />
        <meta
          property='og:title'
          content={`${params?.categoryName ?? _projectName_} - ${_headTitle_}`}
        />
        <meta
          property='og:description'
          content={`${params?.categoryName ?? _projectName_} - ${_headTitle_}`}
        />
        <meta
          property='og:image'
          content={`${reactImageUrl}/${_categoryImg_}logo.png`}
        />
      </Head>

      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}

      {modalShow?.show && (
        <LoginSignup
          onClose={onClose}
          modal={modalShow}
          toast={toast}
          setToast={setToast}
        />
      )}
      {loading && !productData?.code ? (
        <ProductListSkeleton
          isView={isGridViewVisible}
          isActiveDrawer={isActiveDrawer}
          productItem={1}
        />
      ) : productData && productData?.code === 200 ? (
        // checkProductDataAvailable(productData?.data?.filterList) &&
        // (containsKey(filtersObj, productData?.data?.filterList)?.initStatus ||
        // containsKey(filtersObj, productData?.data?.filterList)?.status
        //   ? productData?.data?.filterList?.category_filter?.length === 1
        //     ? productData?.data?.filterList?.category_filter[0]?.categoryId ===
        //       filtersObj?.CategoryId
        //     : true
        //   : true)
        <>
          <div className='site-container'>
            <div className='p-prdlist__wrapper'>
              <div
                className={
                  isActiveDrawer.filterDrawer
                    ? 'p-prdlist__sidebar active'
                    : 'p-prdlist__sidebar'
                }
                id='p-prdlist__sidebar'
              >
                <ProductlistSidebar
                  productData={{
                    ...productData,
                    data: {
                      ...productData?.data
                    }
                  }}
                  loading={loading}
                  setProductData={setProductData}
                  filtersObj={filtersObj}
                  setFiltersObj={setFiltersObj}
                  changeUrl={changeUrl}
                  searchText={searchText}
                  setSearchText={setSearchText}
                  handleSearch={handleSearch}
                  setIsActiveDrawer={setIsActiveDrawer}
                  isActiveDrawer={isActiveDrawer}
                  redirectTo={'products'}
                  // specificPartRef={specificPartRef}
                />
              </div>
              <div className='p-prdlist__products'>
                {loading && !productData ? (
                  <ProductViewSkeleton />
                ) : (
                  productData &&
                  productData?.data?.products?.length > 0 && (
                    <>
                      <div className='p-prdlist-right-header__wrapper'>
                        <div className='p-prdlist-title-wrapper'>
                          <span className='p-prd-total'>
                            {productData?.pagination?.recordCount} Products
                          </span>
                          <div className='p-prdlist-sortby'>
                            <select
                              className='cancel-order-reason'
                              value={filtersObj?.PriceSort}
                              onChange={handleOptionChange}
                            >
                              <option value={0}>Latest</option>
                              <option value={1}>
                                Price - Lowest to Highest
                              </option>
                              <option value={2}>
                                Price - Highest to Lowest
                              </option>
                              <option value={3}>Discount</option>
                            </select>
                          </div>
                        </div>
                        <div className='p-prdview__wrapper'>
                          <i
                            className='m-icon p-prdview-icon'
                            onClick={() => handleViewClick('Grid')}
                          ></i>
                          <i
                            className='m-icon p-prdlist-icon'
                            onClick={() => handleViewClick('List')}
                          ></i>
                        </div>
                      </div>
                    </>
                  )
                )}
                {loading && !productData ? (
                  <div
                    className={
                      isGridViewVisible
                        ? 'p-prdlist-grid__wrapper'
                        : 'p-prdgrid-view__wrapper'
                    }
                  >
                    <ProductCardSkeleton
                      isView={isGridViewVisible}
                      productItem={16}
                    />
                  </div>
                ) : productData?.data?.products?.length === 0 ? (
                  productData && (
                    <DataNotFound
                      image={'/images/data-not-found.png'}
                      heading={'Products Not found!'}
                      description={'Under this category product is not found'}
                    />
                  )
                ) : (
                  <>
                    <div
                      className={
                        isGridViewVisible
                          ? 'p-prdlist-grid__wrapper'
                          : 'p-prdgrid-view__wrapper'
                      }
                    >
                      {productData &&
                        productData?.data?.products?.length > 0 &&
                        productData?.data?.products?.map((product) => (
                          <ProductList
                            loading={loading}
                            key={product?.id}
                            product={product}
                            isView={isGridViewVisible}
                            wishlistShow
                            compareData={compareData}
                            setCompareData={setCompareData}
                            showComparison={true}
                            modalShow={modalShow}
                            setModalShow={setModalShow}
                            setLoading={setLoading}
                            toast={toast}
                            setToast={setToast}
                            productData={productData}
                            setProductData={setProductData}
                            fetchProductList={fetchProductList}
                          />
                        ))}
                      {productData && hasNextPage && (
                        <>
                          <Waypoint onEnter={getMoreData}>
                            <div>
                              <ProductCardSkeleton
                                isView={isGridViewVisible}
                                productItem={1}
                              />
                            </div>
                          </Waypoint>
                        </>
                      )}
                    </div>
                  </>
                )}
              </div>
            </div>
          </div>

          <div className='sort_and_filter'>
            <div className='site-container'>
              <SortAndFilter
                buttonText1={'Sort'}
                buttonText2={'Filter'}
                isActiveDrawer={isActiveDrawer}
                setIsActiveDrawer={setIsActiveDrawer}
              />
            </div>
          </div>
          <OffCanvasBottom
            headingText={'SORT BY'}
            headClass={'sortby'}
            isActiveDrawer={isActiveDrawer}
            setIsActiveDrawer={setIsActiveDrawer}
            // children={
            //   <>
            //     <ul className='sort_productlist'>
            //       <span>
            //         <li>
            //           <div className='ripple-container'>
            //             <button
            //               className='default'
            //               onClick={() => handleOptionChange(0)}
            //             >
            //               <i className='m-icon m-latest'></i>
            //               <span className='sortByValues'> Latest</span>
            //             </button>
            //             <div className='ripple'></div>
            //           </div>
            //         </li>
            //         <li>
            //           <div className='ripple-container'>
            //             <button
            //               className='default'
            //               onClick={() => handleOptionChange(3)}
            //             >
            //               <i className='m-icon m-discount'></i>
            //               <span className='sortByValues'> Discount</span>
            //             </button>
            //             <div className='ripple'></div>
            //           </div>
            //         </li>
            //         <li>
            //           <div className='ripple-container'>
            //             <button
            //               className='default'
            //               onClick={() => handleOptionChange(2)}
            //             >
            //               <i className='m-icon m-hightolow'></i>
            //               <span className='sortByValues'>
            //                 Price: High to Low
            //               </span>
            //             </button>
            //             <div className='ripple'></div>
            //           </div>
            //         </li>
            //         <li>
            //           <div className='ripple-container'>
            //             <button
            //               className='default'
            //               onClick={() => handleOptionChange(1)}
            //             >
            //               <i className='m-icon m-lowtohigh'></i>
            //               <span className='sortByValues'>
            //                 Price: Low to High
            //               </span>
            //             </button>
            //             <div className='ripple'></div>
            //           </div>
            //         </li>
            //       </span>
            //     </ul>
            //   </>
            // }
          />
        </>
      ) : (
        productData && (
          <DataNotFound
            image={'/images/data-not-found.png'}
            heading={'Products Not found!'}
            description={
              'Please check the spelling or try searching for something else'
            }
          />
        )
      )}
      {isAllowComparison && compareData?.length > 0 && (
        <ComparePopover
          compareData={compareData}
          setCompareData={setCompareData}
        />
      )}
    </>
  )
}

export default ProductListPage
