'use client'
import Image from 'next/image'
import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useSelector } from 'react-redux'
import { handleWishlistClick } from '../lib/AllGlobalFunction'
import {
  currencyIcon,
  encryptId,
  getUserId,
  isAllowComparison,
  maxAllowedProductInComparison,
  reactImageUrl,
  showToast,
  spaceToDash
} from '../lib/GetBaseUrl'
import { _productImg_ } from '../lib/ImagePath'
import { checkTokenAuthentication } from '../lib/checkTokenAuthentication'
import { _toaster } from '../lib/tosterMessage'
import IpCheckBox from './base/IpCheckBox'

const ProductList = ({
  product,
  isView,
  productData,
  withoutPrice,
  isWishlistProduct,
  wishlistShow,
  setModalShow,
  compareData,
  setCompareData,
  showComparison,
  toast,
  setToast,
  setProductData,
  setLoading,
  fetchProductList
}) => {
  const router = useRouter()
  const { user } = useSelector((state) => state?.user)

  const userIdCookie = getUserId()
  const isCheckboxDisabled = () => {
    const isCompareDataFull =
      compareData?.length >= maxAllowedProductInComparison
    const isSelected = compareData?.some((item) => item.id === product.id)
    const hasSameCategory = compareData?.some(
      (item) => item?.categoryId === product?.categoryId
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

  return (
    <div
      className={
        isView
          ? isView
            ? 'pd-list__card'
            : 'pd-list__card-gridview'
          : 'pd-list__card'
      }
    >
      <Link
        href={`/product/${spaceToDash(
          product?.productName
            ? product?.productName?.replace('/', '-')
            : product?.products?.productName
            ? product?.products?.productName?.replace('/', '-')
            : ''
        )}/${encryptId(
          product?.guid
            ? product?.guid
            : product?.products?.guid
            ? product?.products?.guid
            : product?.productGUID
        )}`}
        // target='_blank'
      >
        <div className='pd-list__img'>
          <Image
            src={encodeURI(
              `${reactImageUrl}${_productImg_}${
                product?.image1 ??
                product?.products?.image1 ??
                product?.productImage
              }`
            )}
            alt={product?.productName}
            width={300}
            height={300}
            className='prd-list-image'
          />
        </div>
      </Link>
      {wishlistShow && (
        <button
          type='button'
          className='m-btn btn-whishlist wishlist_icon'
          onClick={async () => {
            try {
              if (user?.userId) {
                setLoading(true)
                const response = await handleWishlistClick(
                  product,
                  productData,
                  'productList',
                  toast,
                  setToast
                )
                setLoading(false)
                if (response?.wishlistResponse?.data?.code === 200) {
                  setProductData(response)
                } else if (response?.code === 500) {
                  router?.push('/')
                } else {
                  setProductData(productData)
                }
                response?.wishlistResponse &&
                  showToast(toast, setToast, response?.wishlistResponse)
              } else {
                if (userIdCookie) {
                  const authenticatedUser = await checkTokenAuthentication(
                    toast,
                    setToast
                  )
                  if (authenticatedUser === userIdCookie) {
                    if (fetchProductList) {
                      await fetchProductList(product)
                    }
                  }
                } else {
                  setLoading(false)
                  setModalShow({
                    show: true,
                    data: product,
                    module: 'wishlistProductList'
                  })
                }
              }
            } catch (error) {
              setLoading(false)
              showToast(toast, setToast, {
                data: { code: 204, message: _toaster?.wishlistAuth }
              })
            }
          }}
        >
          <i
            className={`m-icon m-wishlist-icon ${
              product?.isWishlistProduct || product?.isWishlist
                ? 'wishlist-checked'
                : isWishlistProduct
                ? 'wishlist-checked'
                : ''
            }`}
          ></i>
        </button>
      )}
      <div
        className={
          isView
            ? isView
              ? 'main_prd_fl'
              : 'main_prd_fl active'
            : 'main_prd_fl'
        }
      >
        <Link
          href={`/product/${spaceToDash(
            product?.productName?.replace('/', '-')
          )}/${encryptId(
            product?.guid
              ? product?.guid
              : product?.products?.guid
              ? product?.products?.guid
              : product?.productGUID
          )}`}
        >
          <div
            className={
              isView
                ? isView
                  ? 'prd-list__details'
                  : 'prd-list__details-gridview'
                : 'prd-list__details'
            }
          >
            <h2 className='prd-list-title'>
              {product?.brandName ?? product?.products?.brandName}
            </h2>
            <p className='prd-list-contains'>{product?.productName}</p>
            {!withoutPrice && (
              <div className='prd-list-price__wrapper'>
                <h2 className='prd-total-price'>
                  {currencyIcon}
                  {product?.sellingPrice?.toFixed(2) ??
                    product?.products?.sellingPrice?.toFixed(2)}
                </h2>
                <p className='prd-check-price'>
                  {currencyIcon}
                  {product?.mrp?.toFixed(2) ??
                    product?.products?.mrp.toFixed(2)}
                </p>
                <span className='prd-list-offer'>
                  ({product?.discount ?? product?.products?.discount}% OFF)
                </span>
              </div>
            )}
            {!isView && (
              <div
                className='jp_prdlist_content'
                dangerouslySetInnerHTML={{
                  __html: product?.highlights ?? product?.products?.highlights
                }}
              ></div>
            )}
          </div>
        </Link>
        {/* {!isWishlistProduct && isAllowComparison && showComparison && (
          <div
            className={`pv-comparecheckbox-main pv-comparecheckbox-main-${isCheckboxDisabled()}`}
          >
            <IpCheckBox
              isDisabled={isCheckboxDisabled()}
              id={`${product?.name}${product?.id}`}
              label={'Add To Compare'}
              checked={
                compareData?.find((item) => item?.id === product?.id)
                  ? true
                  : false
              }
              onChange={(e) => {
                const isChecked = e?.target?.checked
                const productId = product?.id

                let productComparisonArr = isChecked
                  ? !compareData?.some((item) => item?.id === productId)
                    ? [...(compareData ?? []), product]
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
        )} */}
      </div>
    </div>
  )
}

export default ProductList
