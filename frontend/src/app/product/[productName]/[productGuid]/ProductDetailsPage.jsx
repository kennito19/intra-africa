'use client'
import ComparePopover from '@/components/ComparePopover/ComparePopover'
import DataNotFound from '@/components/DataNotFound'
import LoginSignup from '@/components/LoginSignup'
import OtherSellers from '@/components/OtherSellers'
import PincodeCheck from '@/components/PincodeCheck'
import Popover from '@/components/Popover'
import PrdtDetailContent from '@/components/PrdtDetailContent'
import ProductDetail from '@/components/ProductDetail'
import ProductList from '@/components/ProductList'
import Slider from '@/components/Slider'
import IpCheckBox from '@/components/base/IpCheckBox'
import Toaster from '@/components/base/Toaster'
import BreadCrumb from '@/components/misc/BreadCrumb'
import ProductDetailSkeleton from '@/components/skeleton/ProductDetailSkeleton'
import { handleWishlistClick } from '@/lib/AllGlobalFunction'
import axiosProvider from '@/lib/AxiosProvider'
import {
  currencyIcon,
  decryptId,
  encryptId,
  getCompareData,
  getUserId,
  isAllowComparison,
  maxAllowedProductInComparison,
  reactImageUrl,
  showToast,
  spaceToDash
} from '@/lib/GetBaseUrl'
import { _productImg_ } from '@/lib/ImagePath'
import { _exception } from '@/lib/exceptionMessage'
import { setCartCount } from '@/redux/features/cartSlice'
import actionHandler from '@/utils/actionHandler'
import { useParams, useRouter, useSearchParams } from 'next/navigation'
import nookies, { parseCookies } from 'nookies'
import PropTypes from 'prop-types'
import { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'

const { v4: uuidv4 } = require('uuid')

const ProductDetailsPage = ({ product }) => {
  const router = useRouter()
  const { productGuid } = useParams()
  const searchQuery = useSearchParams()
  const [loading, setLoading] = useState(true)
  const [offer, setOffer] = useState()
  const decodeProGuid = productGuid ? decryptId(productGuid) : null
  const sp_id = searchQuery?.get('sp_id')
  const decodeSp_id = sp_id ? decryptId(searchQuery?.get('sp_id')) : null
  const s_id = searchQuery.get('s_id')
  const decodeS_id = s_id ? decryptId(searchQuery?.get('s_id')) : null
  const dispatch = useDispatch()
  const [compareData, setCompareData] = useState()
  const { user } = useSelector((state) => state?.user)
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const [modalShow, setModalShow] = useState({
    show: false,
    data: null,
    module: 'buynow'
  })
  const [values, setValues] = useState()
  const [similarValue, setSimilarValue] = useState()
  const cookies = parseCookies()
  var mySessionId = cookies['sessionId'] ?? false
  const userIdCookie = getUserId()
  const [selectedMedia, setSelectedMedia] = useState(
    (values?.productImage && values?.productImage[0]) || null
  )

  const updateSizeInfoMap = (
    sizeInfoMap,
    price,
    product,
    isSelected,
    decodeS_id
  ) => {
    if (
      !sizeInfoMap[price.sizeName] ||
      price.sellingPrice < sizeInfoMap[price.sizeName].sellingPrice ||
      (price.sellingPrice === sizeInfoMap[price.sizeName].sellingPrice &&
        price.quantity > sizeInfoMap[price.sizeName].quantity)
    ) {
      sizeInfoMap[price.sizeName] = {
        sizeID: price.sizeID,
        sizeName: price.sizeName,
        mrp: price.mrp,
        sellingPrice: price.sellingPrice,
        discount: price.discount,
        sellerProductId: product.id,
        sellerName: product?.sellerName,
        sellerID: product?.sellerID,
        quantity: price.quantity,
        qty: 1,
        isSelected: isSelected || price.sizeID === Number(decodeS_id)
      }
    }
  }

  const filterAndSortProducts = (products, sellerId = null, sizeID = null) => {
    const sellerProducts = sellerId
      ? products.filter((product) => product.id === sellerId)
      : products
    const uniqueSizes = new Set()
    const sizeInfoMap = {}
    sellerProducts?.forEach((product) => {
      product?.productPrices?.forEach((price) => {
        uniqueSizes.add(price.sizeName)
        const isSelected =
          sellerId || sizeID
            ? product.id === sellerId && price.sizeID === sizeID
            : true

        if (sellerProducts?.length > 1) {
          if (price?.quantity > 0) {
            updateSizeInfoMap(
              sizeInfoMap,
              price,
              product,
              isSelected,
              decodeS_id
            )
          } else {
            updateSizeInfoMap(
              sizeInfoMap,
              price,
              product,
              isSelected,
              decodeS_id
            )
          }
        } else {
          updateSizeInfoMap(sizeInfoMap, price, product, isSelected, decodeS_id)
        }
      })
    })

    const result = [...uniqueSizes].map((sizeName) => {
      return sizeInfoMap[sizeName]
    })

    result.sort((a, b) => {
      if (a.sellingPrice !== b.sellingPrice) {
        return a.sellingPrice - b.sellingPrice
      } else {
        return b.quantity - a.quantity
      }
    })

    return result ?? []
  }

  const prepareProductDetailData = (data) => {
    let uniqueSizes, uniqueColors

    uniqueColors = data?.productColorMapping?.map((item, index) => ({
      ...item,
      isSelected: index === 0
    }))

    uniqueSizes = filterAndSortProducts(
      data?.sellerProducts,
      decodeSp_id ? Number(decodeSp_id) : null,
      decodeS_id ? Number(decodeS_id) : null
    )
    if (!decodeS_id) {
      uniqueSizes = uniqueSizes?.map((item, index) => ({
        ...item,
        isSelected: index === 0
      }))
    }

    setValues({
      ...data,
      allSizes: uniqueSizes ?? [],
      productColorMapping: uniqueColors,
      userId: '',
      sessionId: '',
      sellerProductMasterId: '',
      quantity: '',
      tempMRP: '',
      tempSellingPrice: '',
      tempDiscount: '',
      subTotal: '',
      pinCodeCheck: '',
      pinCodeValue: '',
      pinCodeCheckValue: '',
      bestSeller: false
    })
  }

  const fetchProduct = async (isWishlistClicked) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'user/Product/ById',
        queryString: `?ProductGUID=${decodeProGuid}&userId=${
          user?.userId ? user?.userId : ''
        }&sizeId=0`
      })
      setLoading(false)

      if (response?.status === 200 && response?.data?.code === 200) {
        prepareProductDetailData(response?.data?.data)
        if (isWishlistClicked) {
          setLoading(true)
          const response = await handleWishlistClick(
            isWishlistClicked,
            values,
            'specificProduct',
            toast,
            setToast
          )
          setLoading(false)
          if (response?.wishlistResponse?.data?.code === 200) {
            setValues(response)
          } else {
            setValues(values)
          }
          response?.wishlistResponse &&
            showToast(toast, setToast, response?.wishlistResponse)
        }
      } else {
        setValues({ data: null, code: 204 })
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  function generateSessionId() {
    const uuid = uuidv4()
    const uuidChars = uuid.replace(/-/g, '')
    const mixedCaseChars = []

    for (let i = 0; i < 12; i++) {
      const randomChar = uuidChars.charAt(i)
      const isUpperCase = Math.random() < 0.5
      mixedCaseChars.push(
        isUpperCase ? randomChar.toUpperCase() : randomChar.toLowerCase()
      )
    }

    return mixedCaseChars.join('')
  }

  const onClose = () => {
    setModalShow({ ...modalShow, show: false })

    if (user?.userId && userIdCookie) {
      setTimeout(() => {
        if (modalShow?.module === 'buynow') {
          onSubmit('buyNows')
        } else if (modalShow?.module === 'wishlist') {
          fetchProduct(modalShow?.data)
        } else if (modalShow?.module === 'wishlistProductList') {
          fetchSimilarProductList(modalShow?.data)
        }
      }, [500])
    }
  }

  const onSubmit = async (buttonType, item) => {
    let sessionId, redirectTo
    let getSelectedSize = values?.allSizes?.find((item) => item?.isSelected)

    if (mySessionId) {
      if (user?.userId) {
        if (mySessionId !== user?.userId) {
          const response = await axiosProvider({
            method: 'PUT',
            endpoint: 'Cart/UpdateSession',
            queryString: `?UserId=${user?.userId}&SessionId=${mySessionId}`
          })
          if (response?.data?.code === 200) {
            sessionId = user?.userId
            nookies.set(null, 'sessionId', user?.userId, { path: '/' })
          }
        }
      } else {
        sessionId = mySessionId
        nookies.set(null, 'sessionId', sessionId, { path: '/' })
      }
    } else {
      if (user?.userId) {
        sessionId = user?.userId
        nookies.set(null, 'sessionId', sessionId, { path: '/' })
      } else {
        sessionId = generateSessionId()
        nookies.set(null, 'sessionId', sessionId, { path: '/' })
      }
    }

    const addTocart = {
      userId: user?.userId ?? '',
      type: buttonType === 'buyNow' ? 'buynow' : '',
      sessionId: mySessionId ? mySessionId : sessionId,
      sellerProductMasterId: item
        ? item?.sellerProductId
        : getSelectedSize?.sellerProductId,
      sizeId: item ? item?.sizeID : getSelectedSize?.sizeID,
      quantity:
        buttonType === 'buyNow'
          ? 1
          : getSelectedSize?.qty
          ? getSelectedSize?.qty
          : 1,
      tempMRP: item ? item?.mrp : getSelectedSize?.mrp,
      tempSellingPrice: item
        ? item?.sellingPrice
        : getSelectedSize?.sellingPrice,
      tempDiscount: item ? item?.discount : getSelectedSize?.discount,
      warrantyId: 0
    }
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'POST',
        endpoint: 'Cart',
        data: addTocart
      })
      setLoading(false)
      if (response?.status === 200) {
        if (response?.data?.code === 200) {
          if (buttonType === 'buyNow') {
            dispatch(setCartCount(response?.data?.data?.cartCount))
            localStorage.setItem('cartId', response?.data?.data?.id)
            redirectTo = '/user/checkout'
          } else {
            dispatch(setCartCount(response?.data?.data?.cartCount))
          }
        }
        if (!redirectTo) {
          if (response?.data?.code === 200) {
            showToast(toast, setToast, {
              data: {
                message: `Item successfully added to cart: ${values?.productName}.`,
                code: 200
              }
            })
          } else {
            showToast(toast, setToast, response)
          }
        } else {
          router.push(redirectTo)
        }
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const isCheckboxDisabled = () => {
    const isCompareDataFull =
      compareData?.length >= maxAllowedProductInComparison
    const isSelected = compareData?.some((item) => item.id === values.productId)
    const hasSameCategory = compareData?.some(
      (item) => item?.categoryId === values?.categoryId
    )
    if (compareData.length === 0) {
      return false
    }
    return (
      (isCompareDataFull && !isSelected) ||
      (!isSelected && isCompareDataFull) ||
      !hasSameCategory
    )
  }

  const transformData = (pathIdsArray, pathNamesArray) => {
    pathIdsArray = pathIdsArray?.split('>')
    pathNamesArray = pathNamesArray?.split('>')
    const result = [{ text: 'Home', link: '/' }]

    for (
      let i = 0;
      i < Math.min(pathIdsArray?.length, pathNamesArray?.length);
      i++
    ) {
      result.push({
        text: pathNamesArray[i],
        link: `/products/${spaceToDash(
          pathNamesArray[i]
        )}?CategoryId=${encryptId(pathIdsArray[i])}`
      })
    }
    return result
  }

  const fetchCoupon = async (sellerId = null) => {
    const response = await axiosProvider({
      method: 'GET',
      endpoint: 'user/ManageOffers/search',
      queryString: `?userId=${
        user?.userId ?? ''
      }&showToCustomer=true&categoryId=${values?.categoryId}&productId=${
        values?.productId
      }&sellerId=${
        sellerId
          ? sellerId
          : values?.allSizes?.find((item) => item?.isSelected)?.sellerID
      }`
    })
    if (response?.status === 200) {
      setOffer(response)
    }
  }

  const fetchSimilarProductList = async (isWishlistClicked) => {
    try {
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'user/Product',
        queryString: `?CategoryId=${values?.categoryId}&pageIndex=1&pageSize=21`
      })
      if (response?.status === 200) {
        const updateProducts = response?.data?.data?.products?.filter(
          (item) =>
            item?.id !==
              values?.allSizes?.find((item) => item?.isSelected)?.productId &&
            item?.sellerProductId !==
              values?.allSizes?.find((item) => item?.isSelected)
                ?.sellerProductId
        )
        const updateResponse = {
          ...response?.data,
          data: {
            ...response?.data?.data,
            products: updateProducts
          }
        }
        setSimilarValue(updateResponse)
        if (isWishlistClicked) {
          setLoading(true)
          const wishListRes = await handleWishlistClick(
            isWishlistClicked,
            updateResponse,
            'productList',
            toast,
            setToast
          )
          setLoading(false)
          if (wishListRes?.wishlistResponse?.data?.code === 200) {
            setSimilarValue(wishListRes)
          } else {
            setSimilarValue(similarValue)
          }
          wishListRes?.wishlistResponse &&
            showToast(toast, setToast, wishListRes?.wishlistResponse)
        }
      }
    } catch (error) {
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  useEffect(() => {
    if (values?.categoryId) {
      fetchSimilarProductList()
    }
  }, [user?.userId, values?.categoryId])

  useEffect(() => {
    if (decodeProGuid) {
      fetchProduct()
      setCompareData(getCompareData() ?? [])
    }
  }, [user?.userId])

  useEffect(() => {
    if (product?.code === 200) {
      prepareProductDetailData(product?.data)
    } else if (product?.code === 204) {
      setValues({ data: null, code: 204 })
    }
  }, [product?.data])

  useEffect(() => {
    if (values?.categoryId && values?.productId) {
      !offer && fetchCoupon()
    }
  }, [values])

  const [isZoomed, setIsZoomed] = useState(false)
  const [mousePosition, setMousePosition] = useState({ x: 0, y: 0 })

  const handleMouseEnter = (e) => {
    setIsZoomed(true)
    handleMouseMove(e)
  }

  const handleMouseMove = (e) => {
    const { left, top, width, height } = e.target.getBoundingClientRect()
    const x = ((e.clientX - left) / width) * 100
    const y = ((e.clientY - top) / height) * 100
    setMousePosition({ x, y })
  }

  const handleMouseLeave = () => {
    setIsZoomed(false)
  }

  const ZoomedImageContainer = ({
    compressedImage,
    originalImage,
    mousePosition,
    isVisible
  }) => {
    const zoomedImageStyle = {
      backgroundImage: `url(${originalImage || compressedImage})`,
      backgroundPosition: `${mousePosition.x}% ${mousePosition.y}%`,
      display: isVisible ? 'block' : 'none'
    }

    return (
      <div
        id='zoomedImageContainer'
        className='zoomedImageContainer'
        style={zoomedImageStyle}
      />
    )
  }

  ZoomedImageContainer.propTypes = {
    compressedImage: PropTypes.string.isRequired,
    originalImage: PropTypes.string.isRequired,
    mousePosition: PropTypes.object.isRequired,
    isVisible: PropTypes.bool.isRequired
  }

  useEffect(() => {
    if (product?.action) {
      actionHandler(product?.action, router)
    }
  }, [])

  return (
    <>
      {/* <Head>
        <title>{values?.customeProductName ?? _projectName_}</title>
        <meta
          name='title'
          content={values?.customeProductName ?? _projectName_}
        />

        <meta
          name='description'
          content={
            truncateParagraph(values?.description?.replace(/<[^>]+>/g, '')) ??
            _projectName_
          }
        />
        <meta name='keywords' content={values?.keywords ?? _projectName_} />
        <link rel='canonical' href={currentURL} />
        <link rel='alternate' href={currentURL} hreflang='en-us' />
        <meta property='og:type' content='website' />
        <meta property='og:url' content={currentURL} />
        <meta
          property='og:title'
          content={values?.customeProductName ?? _projectName_}
        />
        <meta
          property='og:description'
          content={
            truncateParagraph(values?.description?.replace(/<[^>]+>/g, '')) ??
            _projectName_
          }
        />
        <meta
          property='og:image'
          content={`${reactImageUrl}/${_productImg_}${values?.productImage[0]?.image}`}
        />
      </Head> */}

      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
      {modalShow?.show && (
        <LoginSignup
          modal={modalShow}
          modalOpen={setModalShow}
          onClose={onClose}
          toast={toast}
          setToast={setToast}
        />
      )}

      {/* {loading && <Loader />} */}
      {loading && !values ? (
        <ProductDetailSkeleton />
      ) : values?.code !== 204 ? (
        <div className='site-container'>
          <BreadCrumb
            items={transformData(
              values?.categoryPathIds,
              values?.categoryPathName
            )}
            brandName={values?.brandName}
          />

          <div className='product_details_wrapper'>
            <div className='product_images_wrapper'>
              <ProductDetail
                values={values}
                setValues={setValues}
                setLoading={setLoading}
                toast={toast}
                setToast={setToast}
                setModalShow={setModalShow}
                modalShow={modalShow}
                fetchProduct={fetchProduct}
                onMouseMove={handleMouseMove}
                onMouseLeave={handleMouseLeave}
                onMouseEnter={handleMouseEnter}
                selectedMedia={selectedMedia}
                setSelectedMedia={setSelectedMedia}
              />
            </div>

            <div className='product_contents_details'>
              {/* <div id="zoomedImageContainer" className="zoomedImageContainer" style={zoomedImageStyle} /> */}
              {selectedMedia && (
                <ZoomedImageContainer
                  // compressedImage={encodeURI(
                  //   `${reactImageUrl}${_productImg_}${selectedMedia?.image}`
                  // )}
                  originalImage={encodeURI(
                    `${reactImageUrl}${_productImg_}${selectedMedia?.url}`
                  )}
                  mousePosition={mousePosition}
                  isVisible={isZoomed}
                />
              )}
              <div className='products_pricing_details'>
                {isAllowComparison && (
                  <div
                    className={`pv-comparecheckbox-main pv-comparecheckbox-main-${isCheckboxDisabled()}`}
                  >
                    <IpCheckBox
                      isDisabled={isCheckboxDisabled()}
                      id={`${values?.productName}${values?.productId}`}
                      label={'Add To Compare'}
                      checked={
                        compareData?.find(
                          (item) => item?.id === values?.productId
                        )
                          ? true
                          : false
                      }
                      onChange={(e) => {
                        const productDetailsData = {
                          ...values,
                          id: values?.productId,
                          image1: values?.productImage[0]?.image,
                          sellerProductId: values?.allSizes?.find(
                            (item) => item?.isSelected
                          )?.sellerProductId
                        }
                        const isChecked = e?.target?.checked
                        const productId = productDetailsData?.id

                        let productComparisonArr = isChecked
                          ? !compareData?.some((item) => item?.id === productId)
                            ? [...(compareData ?? []), productDetailsData]
                            : compareData
                          : compareData?.filter((item) => item.id !== productId)

                        setCompareData(productComparisonArr)

                        if (productComparisonArr.length) {
                          localStorage.setItem(
                            'hk-compare-data',
                            JSON.stringify(productComparisonArr)
                          )
                        } else {
                          localStorage.removeItem('hk-compare-data')
                        }
                      }}
                    />
                  </div>
                )}
                {values?.brandName && (
                  <div className='product_brands_wishlist_icon'>
                    {values?.brandName && (
                      <p className='prdct__brands_nm'>{values?.brandName}</p>
                    )}
                    <button
                      type='button'
                      className='m-btn btn-whishlist wishlist_icon'
                    >
                      <i
                        className={`m-icon m-wishlist-icon wishlist-checked`}
                      ></i>
                    </button>
                  </div>
                )}
                {values?.productName && (
                  <h1 className='product_name'>{values?.productName}</h1>
                )}
                {values?.allSizes?.find((item) => item?.isSelected) && (
                  <>
                    <div className='product_pricong_offer_deliverychrg'>
                      <span className='total_pricing_product'>
                        {currencyIcon}
                        {
                          values?.allSizes?.find((item) => item?.isSelected)
                            ?.sellingPrice
                        }
                      </span>
                      <span className='actual_pricing_product_mrp'>
                        {currencyIcon}
                        {
                          values?.allSizes?.find((item) => item?.isSelected)
                            ?.mrp
                        }
                      </span>
                      <span className='actual_pricing_product_dis'>
                        (
                        {
                          values?.allSizes?.find((item) => item?.isSelected)
                            ?.discount
                        }
                        % OFF)
                      </span>
                      <p className='prd_include_taxes'>
                        (Inclusive of all taxes)
                      </p>
                    </div>
                  </>
                )}
                <div className='prd_clr_varients_img'>
                  {values?.productColorMapping?.length > 0 && (
                    <div className='prd_clr_varients'>
                      <span className='rd_selectsize_label-seller'>Color:</span>
                      <div className='prd_varients_imgs'>
                        {values?.productColorMapping?.map((item) => (
                          <span
                            className={`prd_img_active_varient ${
                              item?.isSelected && 'active'
                            }`}
                            key={item?.valueId}
                            style={{
                              backgroundColor: `${item?.colorCode}`
                            }}
                          ></span>
                        ))}
                      </div>
                    </div>
                  )}
                </div>
                {values?.allSizes?.length > 0 && (
                  <>
                    <p className='prd_selectsize_label'>
                      <span className='rd_selectsize_label-seller'>
                        select size:
                      </span>
                    </p>

                    <div className='prds_input_labels_sizes_varients'>
                      {values?.allSizes?.map((size) => (
                        <div
                          key={size?.sizeID}
                          className='prds_sizes_labels_varients'
                        >
                          <input
                            type='radio'
                            id={size?.sizeID}
                            name='sizeId'
                            className='prdt_size_inpt_radio'
                            value={size?.sizeID}
                            checked={size?.isSelected}
                            onChange={() => {
                              let allSizes = values?.allSizes?.map((item) => {
                                if (size?.sizeID === item?.sizeID) {
                                  return {
                                    ...item,
                                    isSelected: true
                                  }
                                } else {
                                  return {
                                    ...item,
                                    isSelected: false
                                  }
                                }
                              })
                              setValues({ ...values, allSizes })
                            }}
                          />
                          <label
                            htmlFor={size?.sizeID}
                            className={
                              size?.quantity
                                ? 'prdt_size_label_varient'
                                : 'prdt_size_label_varient'
                            }
                          >
                            {size?.sizeName}
                          </label>
                        </div>
                      ))}
                    </div>
                  </>
                )}

                {values?.allSizes?.find((item) => item?.isSelected)
                  ?.quantity ? (
                  <div className='prdt_btns_atc_byn_wrapper'>
                    <button
                      className='m-btn btn-add-cart'
                      type='button'
                      onClick={() => {
                        onSubmit('addToCart')
                      }}
                    >
                      <i className='m-icon m-cart-icon'></i> Add to cart
                    </button>
                    <button
                      className='m-btn btn-buy-now'
                      type='button'
                      onClick={() => {
                        if (user?.userId) {
                          onSubmit('buyNow')
                          setModalShow({ ...modalShow, show: false })
                        } else {
                          if (userIdCookie) {
                            checkTokenAuthentication(toast, setToast)
                          } else {
                            setModalShow({ ...modalShow, show: true })
                          }
                        }
                      }}
                    >
                      <i className='m-icon m-buynow-icon'></i>Buy Now
                    </button>
                  </div>
                ) : (
                  <div className='prdt_sold'>
                    <h2 className='sold'>Sold out</h2>
                    <p>This item is currently out of stock</p>
                  </div>
                )}
              </div>

              <div className='products_pricing_details'>
                <PincodeCheck values={values} setValues={setValues} />
              </div>

              {offer?.data?.data?.length > 0 && (
                <div className='prdt_best_offers_wrapper'>
                  <p className='prdt_best_offer_lable'>best offers</p>
                  <div className='prdt_best_offers_wrapper-inner'>
                    {offer?.data?.data?.map((item) => (
                      <div
                        className='prdt_best_offers_multiple'
                        key={Math.floor(Math.random() * 100000)}
                      >
                        <p className='prdt_best_prgh_bestoffers-head'>
                          {item?.name}&nbsp;:
                        </p>
                        <p className='prdt_best_prgh_bestoffers'>
                          {item?.code}
                        </p>
                        <Popover
                          btntext='T&C'
                          content={item?.terms ? item?.terms : 'N/A'}
                        />
                      </div>
                    ))}
                  </div>
                </div>
              )}

              {/* Other Sellers */}
              {values?.sellerProducts?.filter(
                (data) =>
                  data?.sellerID !==
                  values?.allSizes?.find((item) => item?.isSelected)?.sellerID
              )?.length > 0 && (
                <OtherSellers
                  values={values}
                  onSubmit={(item) => {
                    onSubmit('otherSeller', item)
                  }}
                  toast={toast}
                  setToast={setToast}
                />
              )}

              {/* product details */}
              <PrdtDetailContent values={values} />
            </div>
          </div>
          {similarValue && similarValue?.data?.products?.length > 0 && (
            <>
              <div>
                <h2 className='h2_cmn'>similar products</h2>
              </div>
              <div className='site-container categories-section section_spacing_b'>
                <div className='categories-wrapper'>
                  {/* <div className='prdt-details-grid_wrapper'> */}
                  <Slider
                    spaceBetween={10}
                    slidesPerView={5}
                    loop={true}
                    autoplay={true}
                    navigation={true}
                    breakpoints={{
                      0: {
                        slidesPerView: 1
                      },

                      768: {
                        slidesPerView: 3
                      },
                      1024: {
                        slidesPerView: 4
                      },
                      1280: {
                        slidesPerView: 5
                      }
                    }}
                  >
                    {similarValue?.data?.products?.map((product) => (
                      <ProductList
                        key={product?.id}
                        product={product}
                        isView={true}
                        setModalShow={setModalShow}
                        modalShow={modalShow}
                        wishlistShow
                        setLoading={setLoading}
                        setProductData={setSimilarValue}
                        productData={similarValue}
                        toast={toast}
                        setToast={setToast}
                        fetchProductList={fetchSimilarProductList}
                      />
                    ))}
                  </Slider>
                </div>
              </div>
            </>
          )}
        </div>
      ) : (
        <DataNotFound
          image={'/images/data-not-found.png'}
          heading={'Product Not found!'}
        />
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

export default ProductDetailsPage
