'use client'
import { customStyles } from '@/components/CustomStyles'
import Loader from '@/components/Loader'
import LoginSignup from '@/components/LoginSignup'
import MBtn from '@/components/base/MBtn'
import CompareSkeleton from '@/components/skeleton/CompareSkeleton'
import axiosProvider from '@/lib/AxiosProvider'
import {
  convertSpIdToQueryString,
  currencyIcon,
  decryptId,
  encryptId,
  getCompareData,
  isAllowAddToCartInComparison,
  isAllowBuyNowInComparison,
  maxAllowedProductInComparison,
  reactImageUrl,
  spaceToDash
} from '@/lib/GetBaseUrl'
import { _productImg_ } from '@/lib/ImagePath'
import Image from 'next/image'
import Link from 'next/link'
import { useRouter, useSearchParams } from 'next/navigation'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import Select from 'react-select'
import { useImmer } from 'use-immer'

const CompareProductPage = ({ product }) => {
  const router = useRouter()
  const spId = useSearchParams()?.get('sp_id')
  const [sellerProductIds, setSellerProductIds] = useState()
  const [loading, setLoading] = useState(true)
  const [data, setData] = useState()
  const [compareData, setCompareData] = useState()
  const [allState, setAllState] = useImmer({
    brand: [],
    product: [],
    brandId: null,
    brandName: '',
    sellerProductId: null,
    productName: '',
    activeComparisonData: null
  })
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const { user } = useSelector((state) => state?.user)
  const [modalShow, setModalShow] = useState(false)

  const fetchExtraData = async (cat_id, brand_id) => {
    let endpoint = brand_id
      ? `user/Product/productComparisonBrandProduct?CategoryId=${cat_id}&BrandId=${brand_id}`
      : `user/Product/productComparisonBrand?CategoryId=${cat_id}`

    if (endpoint) {
    }
    const response = await axiosProvider({
      method: 'GET',
      endpoint
    })

    if (response?.data?.code === 200) {
      if (brand_id) {
        setAllState((draft) => {
          draft.product = response?.data.data
        })
      } else {
        setAllState((draft) => {
          draft.brand = response?.data?.data
        })
      }
    }
  }

  const prepareCompareData = (response) => {
    setData(response)
    setAllState((draft) => {
      draft.brandId = null
      draft.brandName = ''
      draft.sellerProductId = null
      draft.productName = ''
      draft.product = []
      draft.activeComparisonData = null
    })
    const sellerProId = spId?.split(',')?.map((item) => decryptId(item))
    setSellerProductIds(sellerProId)

    let hkCompareData = response?.productSummary?.image?.value?.map(
      (product, index) => {
        let sp_id = Object.keys(product)
        let productName = response?.productSummary?.productName?.value[index]
        let productId = response?.productSummary?.productId?.value[index]
        return {
          id: Number(productId[sp_id]?.value),
          image1: product[sp_id]?.value,
          sellerProductId: Number(sp_id[0]),
          productName: productName[sp_id]?.value,
          categoryId: Number(response?.productCategory?.categoryId)
        }
      }
    )

    localStorage.setItem('hk-compare-data', JSON.stringify(hkCompareData))

    if (response?.productCategory?.categoryId) {
      fetchExtraData(response?.productCategory?.categoryId)
    }

    const responseIds = []
    response?.productSummary?.color?.value?.forEach((responseId) => {
      const productId = Object.keys(responseId)[0]
      responseIds.push(productId)
    })
    if (responseIds?.join(',') !== sellerProId?.join(',')) {
      changeUrl({ sp_id: responseIds?.join(',') })
    }
  }

  const changeUrl = (sp_idObj) => {
    if (sp_idObj?.sp_id) {
      let query = convertSpIdToQueryString(sp_idObj)
      if (query) {
        router.push(`compare?${query}`)
      } else {
        router?.push('/')
      }
    } else {
      router?.push('/')
    }
  }

  const closeModal = () => {
    setModalShow(false)
  }

  useEffect(() => {
    if (typeof product === 'object' && Object.keys(product)?.length) {
      prepareCompareData(product)
    }
  }, [product])

  useEffect(() => {
    setCompareData(getCompareData() ?? [])
    setLoading(false)
  }, [])

  return (
    <>
      {modalShow && (
        <LoginSignup toast={toast} setToast={setToast} onClose={closeModal} />
      )}
      {!loading && data && sellerProductIds ? (
        <div className='pv-compare-main'>
          <table border className='pv-compare-table'>
            <thead>
              <tr>
                <th className='pv-comlabel-blank'>
                  <p className='kl-compare-product-name'>
                    Compare{' '}
                    {data?.productSummary?.productName?.value?.length > 0 &&
                      Object.values(
                        data?.productSummary?.productName?.value[0]
                      )[0]?.value}{' '}
                    vs others
                  </p>
                  <p className='kl-total-item-count'>
                    {data?.productSummary?.productName?.value?.length} items
                  </p>
                </th>
                {data?.productSummary?.image?.value?.map((product, index) => {
                  let sp_id = Object.keys(product)
                  let productName =
                    data?.productSummary?.productName?.value[index]
                  let mrp = data?.productSummary?.mrp?.value[index]
                  let sellingPrice =
                    data?.productSummary?.sellingPrice?.value[index]
                  let discount = data?.productSummary?.discount?.value[index]
                  let size = data?.productSummary?.size?.value[index]
                  return (
                    <th key={Math.floor(Math.random() * 100000)}>
                      {sellerProductIds?.length > 1 && (
                        <MBtn
                          buttonClass={'pv-compare-itembtn'}
                          withIcon
                          iconClass={'closetoggle-icon'}
                          onClick={() => {
                            let filteredCompareData = compareData?.filter(
                              (data) =>
                                data?.sellerProductId !== Number(sp_id[0])
                            )
                            localStorage.setItem(
                              'hk-compare-data',
                              JSON.stringify(filteredCompareData)
                            )
                            let sp_idObj = {
                              sp_id: sellerProductIds
                                ?.filter((item) => item !== sp_id[0])
                                .join(',')
                            }

                            changeUrl(sp_idObj)
                          }}
                        />
                      )}
                      <Link
                        href={`/product/${spaceToDash(
                          product[sp_id]?.value
                        )}/${encryptId(product[sp_id]?.ProductGuid)}`}
                        className='pv-compare-card'
                      >
                        <Image
                          src={
                            product[sp_id]?.value &&
                            encodeURI(
                              `${reactImageUrl}${_productImg_}${product[sp_id]?.value}`
                            )
                          }
                          alt={`${product[sp_id]?.value}`}
                          width={50}
                          height={50}
                          className='pv-compare-img'
                        />
                        <p className='pv-compr-disc'>
                          {productName[sp_id]?.value}
                        </p>
                      </Link>

                      {(mrp || sellingPrice || discount) && (
                        <>
                          <div className='pv-compr-price'>
                            <span>
                              {currencyIcon}
                              {sellingPrice[sp_id]?.value}
                            </span>
                            <span className='prd-check-price'>
                              {currencyIcon}
                              {mrp[sp_id]?.value}
                            </span>
                            <span>{discount[sp_id]?.value}% off</span>
                          </div>
                          {isAllowAddToCartInComparison && (
                            <button
                              className='m-btn btn-add-cart'
                              type='button'
                              onClick={() => {
                                let item = {
                                  sellerProductId: sp_id,
                                  mrp: mrp[sp_id]?.value,
                                  sellingPrice: sellingPrice[sp_id]?.value,
                                  discount: discount[sp_id]?.value,
                                  sizeId: size[sp_id]?.id
                                }
                                onSubmit('addToCart', item)
                              }}
                            >
                              <i className='m-icon m-cart-icon'></i> Add to cart
                            </button>
                          )}

                          {isAllowBuyNowInComparison && (
                            <button
                              className='m-btn btn-buy-now'
                              type='button'
                              onClick={() => {
                                if (user?.userId) {
                                  onSubmit('buyNow')
                                  setModalShow(false)
                                } else {
                                  setModalShow(true)
                                }
                              }}
                            >
                              <i className='m-icon m-buynow-icon'></i>Buy Now
                            </button>
                          )}
                        </>
                      )}
                    </th>
                  )
                })}

                {data?.productSummary?.productName?.value?.length <
                  maxAllowedProductInComparison &&
                  Array.from(
                    {
                      length:
                        maxAllowedProductInComparison -
                        data?.productSummary?.productName?.value?.length
                    },
                    (_, index) => (
                      <th key={index}>
                        <div className='pv-compare-card-main'>
                          <div className='pv-blackandwhite-img'></div>
                          <div className='pv-selectbrand-main'>
                            <Select
                              menuPortalTarget={document.body}
                              styles={customStyles}
                              value={
                                allState?.brandId &&
                                allState?.activeComparisonData === index && {
                                  label: allState?.brandName,
                                  value: allState?.brandId
                                }
                              }
                              options={
                                allState?.brand?.length > 0 &&
                                allState?.brand?.map(
                                  ({ brandName, brandId }) => ({
                                    label: brandName,
                                    value: brandId
                                  })
                                )
                              }
                              onChange={(e) => {
                                setAllState((draft) => {
                                  draft.brandId = e?.value
                                  draft.brandName = e?.label
                                  draft.activeComparisonData = index
                                })
                                fetchExtraData(
                                  data?.productCategory?.categoryId,
                                  e?.value
                                )
                              }}
                              placeholder={'Select Brand'}
                            />
                          </div>

                          <div className='pv-selectbrand-main'>
                            <Select
                              isDisabled={
                                allState?.activeComparisonData === index &&
                                allState?.product?.length > 0
                                  ? false
                                  : true
                              }
                              menuPortalTarget={document.body}
                              styles={customStyles}
                              value={
                                allState?.sellerProductId &&
                                allState?.activeComparisonData === index && {
                                  label: allState?.productName,
                                  value: allState?.sellerProductId
                                }
                              }
                              options={allState?.product
                                ?.filter(
                                  (item) =>
                                    !sellerProductIds.includes(
                                      item?.sellerProductId
                                    )
                                )
                                ?.map(({ sellerProductId, productName }) => ({
                                  label: productName,
                                  value: sellerProductId
                                }))}
                              onChange={(e) => {
                                let sp_idObj = {
                                  sp_id: [...sellerProductIds, e?.value]?.join(
                                    ','
                                  )
                                }
                                changeUrl(sp_idObj)
                                setAllState((draft) => {
                                  draft.sellerProductId = e?.value
                                  draft.productName = e?.label
                                  draft.activeComparisonData = index
                                })
                              }}
                              placeholder={'Select product'}
                            />
                          </div>
                        </div>
                      </th>
                    )
                  )}
              </tr>
            </thead>
            <tbody>
              {/* for static productSummary */}
              <tr>
                <td className='pv-comlabel'>
                  <span> Highlights</span>
                </td>
                {data?.productSummary?.description?.value?.map((product) => {
                  let sp_id = Object.keys(product)
                  return (
                    <td key={Math.floor(Math.random() * 100000)}>
                      <div
                        dangerouslySetInnerHTML={{
                          __html: product[sp_id]?.value
                        }}
                      ></div>
                    </td>
                  )
                })}

                {data?.productSummary?.description?.value?.length <
                  maxAllowedProductInComparison &&
                  Array.from(
                    {
                      length:
                        maxAllowedProductInComparison -
                        data?.productSummary?.description?.value?.length
                    },
                    (_) => <td key={Math.floor(Math.random() * 100000)}></td>
                  )}
              </tr>

              {/* Highlight */}
              <tr>
                <td className='pv-comlabel'>
                  <span> Highlights</span>
                </td>
                {data?.productSummary?.highlights?.value?.map((product) => {
                  let sp_id = Object.keys(product)
                  return (
                    <td key={Math.floor(Math.random() * 100000)}>
                      <div
                        dangerouslySetInnerHTML={{
                          __html: product[sp_id]?.value
                        }}
                      ></div>
                    </td>
                  )
                })}

                {data?.productSummary?.highlights?.value?.length <
                  maxAllowedProductInComparison &&
                  Array.from(
                    {
                      length:
                        maxAllowedProductInComparison -
                        data?.productSummary?.highlights?.value?.length
                    },
                    (_) => <td key={Math.floor(Math.random() * 100000)}></td>
                  )}
              </tr>

              {/* Size */}
              <tr>
                <td className='pv-comlabel'>
                  <span>Size</span>
                </td>
                {data?.productSummary?.size?.value?.map((product) => {
                  let sp_id = Object.keys(product)
                  return (
                    <td key={Math.floor(Math.random() * 100000)}>
                      <div
                        dangerouslySetInnerHTML={{
                          __html: product[sp_id]?.value
                        }}
                      ></div>
                    </td>
                  )
                })}

                {data?.productSummary?.size?.value?.length <
                  maxAllowedProductInComparison &&
                  Array.from(
                    {
                      length:
                        maxAllowedProductInComparison -
                        data?.productSummary?.size?.value?.length
                    },
                    (_) => <td key={Math.floor(Math.random() * 100000)}></td>
                  )}
              </tr>

              {/* Color */}
              <tr>
                <td className='pv-comlabel'>
                  <span>Color</span>
                </td>
                {data?.productSummary?.color?.value?.map((product) => {
                  let sp_id = Object.keys(product)
                  return (
                    <td key={Math.floor(Math.random() * 100000)}>
                      <div
                        dangerouslySetInnerHTML={{
                          __html: product[sp_id]?.value
                        }}
                      ></div>
                    </td>
                  )
                })}

                {data?.productSummary?.color?.value?.length <
                  maxAllowedProductInComparison &&
                  Array.from(
                    {
                      length:
                        maxAllowedProductInComparison -
                        data?.productSummary?.color?.value?.length
                    },
                    (_) => <td key={Math.floor(Math.random() * 100000)}></td>
                  )}
              </tr>

              {data?.productSpecification?.length > 0 &&
                data?.productSpecification?.map((item) => (
                  <>
                    <tr key={Math.floor(Math.random() * 100000)}>
                      <td
                        className='pv-compareheading'
                        colspan={maxAllowedProductInComparison + 1}
                      >
                        <span>{item?.featureName}</span>
                      </td>
                    </tr>
                    {item?.values?.map((subSpec) => (
                      <tr key={Math.floor(Math.random() * 100000)}>
                        <td className='pv-comlabel'>
                          {subSpec?.specificationName}
                        </td>
                        {subSpec?.value?.map((childSpec) => {
                          let guid = Object.keys(childSpec)
                          return (
                            <td key={Math.floor(Math.random() * 100000)}>
                              {childSpec[guid]?.value}
                            </td>
                          )
                        })}
                        {Array.from(
                          {
                            length:
                              maxAllowedProductInComparison -
                              subSpec?.value?.length
                          },
                          (_) => (
                            <td key={Math.floor(Math.random() * 100000)}>-</td>
                          )
                        )}
                      </tr>
                    ))}
                  </>
                ))}
            </tbody>
          </table>
        </div>
      ) : (
        <CompareSkeleton />
      )}
    </>
  )
}

export default CompareProductPage
