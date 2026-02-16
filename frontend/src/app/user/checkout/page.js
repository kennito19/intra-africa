'use client'
import CheckoutSkeleton from '@/components/skeleton/CheckoutSkeleton'
import { useRouter } from 'next/navigation'
import nookies from 'nookies'
import { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import EmptyComponent from '../../../components/EmptyComponent'
import PriceDetails from '../../../components/PriceDetails'
import Toaster from '../../../components/base/Toaster'
import OfferCoupan from '../../../components/misc/OfferCoupan'
import axiosProvider from '../../../lib/AxiosProvider'
import {
  convertToNumber,
  getSessionId,
  showToast
} from '../../../lib/GetBaseUrl'
import { _exception } from '../../../lib/exceptionMessage'
import { _toaster } from '../../../lib/tosterMessage'
import { cartData, setCartCount } from '../../../redux/features/cartSlice'
import CheckoutStepAccordions from './CheckoutStepAccordions'
import HeaderforCheckout from '@/components/layout/HeaderforCheckout'
const { v4: uuidv4 } = require('uuid')

const Page = () => {
  const router = useRouter()
  const [loading, setLoading] = useState(true)
  const { user } = useSelector((state) => state?.user)
  const { cart } = useSelector((state) => state?.cart)
  const { address } = useSelector((state) => state?.address)
  const dispatch = useDispatch()
  const [cartId, setCartId] = useState()
  let mySessionId = getSessionId()
  const [data, setData] = useState()
  const [offer, setOffer] = useState()
  const [modalShow, setModalShow] = useState({
    show: false,
    type: '',
    data: null
  })
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const [values, setValues] = useState({
    OrderPaymentSec: '',
    captchaValue: '',
    captchaInput: '',
    addressVal:
      address?.length === 1
        ? address[0]
        : address && Object.keys(address).length > 1
        ? address?.find((item) => item?.setDefault === true) ?? {}
        : {},
    userDetails: {},
    OrderNo: '',
    userId: user?.userId ?? '',
    userName: '',
    userPhoneNo: '',
    userEmail: user?.userName ?? '',
    userAddressLine1: '',
    userAddressLine2: '',
    userLendMark: '',
    userPincode: '',
    userCity: '',
    userState: '',
    userCountry: '',
    userGSTNo: '',
    paymentMode: '',
    totalShippingCharge:
      convertToNumber(cart?.CartAmount?.shipping_charges) ?? 0,
    totalExtraCharges:
      convertToNumber(cart?.CartAmount?.total_extra_charges) ?? 0,
    totalAmount: convertToNumber(cart?.CartAmount?.total_amount) ?? '',
    isCouponApplied: !cartId && cart?.coupon_code?.length > 0,
    coupon: cartId ? '' : cart?.coupon_code ?? '',
    coupontDiscount: cart?.CartAmount?.total_extradiscount ?? 0,
    coupontDetails: cartId ? null : cart?.coupon_code ?? null,
    codCharge: 0,
    codMessage: '',
    paidAmount: convertToNumber(cart?.CartAmount?.paid_amount) ?? '',
    isSale: false,
    saleType: '',
    orderDate: null,
    deliveryDate: null,
    deliverydays: 0,
    status: 'Placed',
    paymentInfo: '',
    orderBy: 'customer',
    isRetailer: false,
    isVertualRetailer: false,
    isReplace: false,
    sellerName: '',
    items: data?.data?.items,
    CartAmount: data?.data?.CartAmount,
    couponCode: cartId ? null : cart?.coupon_code ?? null
  })

  const [activeAccordion, setActiveAccordion] = useState(
    address?.length === 0 ? 1 : values?.addressVal ? 2 : 1
  )

  const generateSessionId = () => {
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

  const cartCalculation = async (
    val = false,
    toast = false,
    code = false,
    getReduxCouponCode = false,
    addressPin,
    paymentMode
  ) => {
    if (!mySessionId?.length > 0) {
      if (user?.userId) {
        nookies.set(null, 'sessionId', user?.userId, { path: '/' })
      } else {
        mySessionId = generateSessionId()
        nookies.set(null, 'sessionId', mySessionId, { path: '/' })
      }
    }
    let data = {
      cartId: cartId ? Number(cartId) : null,
      cartSessionId: mySessionId,
      userId: values?.userId ? values?.userId : user?.userId,
      couponCode: !cartId
        ? getReduxCouponCode
          ? cart?.coupon_code ?? ''
          : code?.length > 0
          ? code
          : ''
        : '',
      paymentMode: paymentMode ? paymentMode : '',
      pincode: addressPin ? addressPin?.pincode : ''
    }

    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'POST',
        endpoint: 'Cart/CartCalculation',
        data
      })
      setLoading(false)
      if (response?.status === 200) {
        const updatedResponse = {
          ...response,
          data: {
            ...response.data,
            items: Object.values(response?.data?.items || {}).flatMap(
              (sellerItems) => {
                return sellerItems.flatMap((item) => {
                  return item.Items.map((product) => {
                    return { ...product }
                  })
                })
              }
            )
          }
        }
        dispatch(setCartCount(updatedResponse?.data?.items?.length))
        setData(updatedResponse)
        const ValuesData = {
          ...values,
          items: updatedResponse?.data?.items,
          sellarViseItems: response?.data?.items,
          CartAmount: updatedResponse?.data?.CartAmount,
          addressVal: addressPin ? addressPin : {},
          userId: values?.userId ? values?.userId : user?.userId,
          userEmail: values?.userEmail ? values?.userEmail : user?.userName,
          totalShippingCharge:
            convertToNumber(
              updatedResponse?.data?.CartAmount?.shipping_charges
            ) ?? 0,
          totalExtraCharges:
            convertToNumber(
              updatedResponse?.data?.CartAmount?.total_extra_charges
            ) ?? 0,
          totalAmount:
            convertToNumber(updatedResponse?.data?.CartAmount?.total_amount) ??
            '',
          coupontDiscount:
            updatedResponse?.data?.CartAmount?.total_extradiscount ?? 0,
          codCharge: updatedResponse?.data?.CartAmount?.cod_charges ?? 0,
          codMessage: updatedResponse?.data?.CartAmount?.cod_message ?? '',
          paidAmount:
            convertToNumber(updatedResponse?.data?.CartAmount?.paid_amount) ??
            '',
          paymentMode: paymentMode ? paymentMode : '',
          deliverydays: addressPin?.deliverydays
            ? addressPin?.deliverydays
            : values?.deliverydays
        }
        setValues(ValuesData)
        dispatch(
          cartData({
            items: updatedResponse?.data?.items,
            CartAmount: updatedResponse?.data?.CartAmount
          })
        )
        if (updatedResponse?.data?.items?.length === 0) {
          localStorage.removeItem('cartId')
        } else {
          if (code || getReduxCouponCode || cart?.coupon_code) {
            const couponStatusFailed = updatedResponse?.data?.items?.filter(
              (item) => item?.coupon_status === 'failed'
            )
            const couponStatusSuccess = updatedResponse?.data?.items?.filter(
              (item) => item?.coupon_status === 'success'
            )
            const couponStatusNull = updatedResponse?.data?.items?.filter(
              (item) => !item?.coupon_status
            )
            if (code?.length > 0 || toast === 'remove' || getReduxCouponCode) {
              if (
                couponStatusSuccess?.length > 0 ||
                (toast !== 'remove' && cart?.coupon_code)
              ) {
                dispatch(
                  cartData({
                    items: updatedResponse?.data?.items,
                    CartAmount: updatedResponse?.data?.CartAmount,
                    coupon_code: code ? code : cart?.coupon_code
                  })
                )
              } else {
                dispatch(
                  cartData({
                    items: updatedResponse?.data?.items,
                    CartAmount: updatedResponse?.data?.CartAmount,
                    coupon_code: null
                  })
                )
              }
            } else {
              dispatch(
                cartData({
                  ...cart,
                  items: updatedResponse?.data?.items,
                  CartAmount: updatedResponse?.data?.CartAmount
                })
              )
            }
            const couponStatus =
              couponStatusFailed?.length ===
                updatedResponse?.data?.items?.length ||
              couponStatusNull?.length ===
                updatedResponse?.data?.items?.length ||
              (!(couponStatusSuccess?.length === 0) &&
                couponStatusSuccess?.length <=
                  updatedResponse?.data?.items?.length)
            if (toast === 'remove') {
              showToast(toast, setToast, {
                data: {
                  message: 'Coupon has been removed!',
                  code: 200
                }
              })
              setModalShow({ show: false, type: 'coupon' })
            } else if (toast) {
              couponStatus &&
                showToast(toast, setToast, {
                  data: {
                    message: couponStatusSuccess[0]?.coupon_message
                      ? couponStatusSuccess[0]?.coupon_message
                      : 'Coupon not applied',
                    code:
                      couponStatusSuccess?.length > 0 &&
                      couponStatusSuccess?.length <=
                        updatedResponse?.data?.items?.length
                        ? 200
                        : couponStatus
                        ? 400
                        : 200
                  }
                })
            }
            code &&
              couponStatusSuccess?.length > 0 &&
              couponStatusSuccess?.length <=
                updatedResponse?.data?.items?.length &&
              setModalShow({ show: false, type: 'coupon' })
          }
        }
        localStorage.removeItem('cartId')
        return { cartResponse: updatedResponse, valuesResponse: ValuesData }
      } else {
        setData({ code: 204, data: null })
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const fetchCoupon = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'user/ManageOffers/search',
        queryString: `?userId=${user?.userId ?? null}&showToCustomer=true`
      })
      setLoading(false)

      if (response?.status === 200) {
        setOffer(response)
      }
    } catch {
      setLoading(false)
    }
  }

  const handleAccordionChange = (activeAccordion) => {
    let getActiveElement
    switch (activeAccordion) {
      case 1:
        getActiveElement = document.getElementById('delivery-address')
        break

      case 2:
        getActiveElement = document.getElementById('order-summary')
        break

      case 3:
        getActiveElement = document.getElementById('payment-option')
        break

      default:
        getActiveElement = document.getElementById('order-summary')
        break
    }

    if (getActiveElement) {
      window.scrollTo({
        top: getActiveElement.offsetTop - 15,
        behavior: 'smooth'
      })
    }
  }

  const fetchPinCodeAndCheckCart = async (
    addressPin,
    toastFirst = true,
    paymentMode
  ) => {
    try {
      if (!addressPin) {
        setActiveAccordion(1)
        cartCalculation(false, false, false, true, false)
        return
      }

      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: `Delivery/byPincode?pincode=${Number(addressPin?.pincode)}`
      })
      setLoading(false)

      const responseData = response?.data?.data
      if (response?.status === 200) {
        if (
          responseData?.pincode === Number(addressPin?.pincode) &&
          responseData?.status === 'Active'
        ) {
          let lastDigit
          const deliveryDays = responseData?.deliveryDays

          if (deliveryDays?.includes('-')) {
            const parts = deliveryDays.split('-')
            lastDigit = Number(parts[1].trim())
          } else {
            lastDigit = Number(deliveryDays)
          }
          if (paymentMode === 'cod') {
            if (responseData?.isCODActive) {
              setActiveAccordion(3)
              setModalShow({ show: true, type: 'cod' })
              cartCalculation(
                false,
                false,
                false,
                true,
                { ...addressPin, deliverydays: lastDigit },
                paymentMode
              )
            } else {
              handleCodInactive()
            }
          } else {
            setActiveAccordion(2)
            cartCalculation(false, false, false, true, {
              ...addressPin,
              deliverydays: lastDigit
            })
          }
        } else {
          handlePinCodeError(toastFirst)
        }
      }
    } catch {
      showToast(toast, setToast, {
        data: {
          code: 204,
          message: _exception?.message
        }
      })
    }
  }

  const handlePinCodeError = (toastFirst) => {
    setActiveAccordion(1)
    toastFirst &&
      showToast(toast, setToast, {
        data: {
          code: 204,
          message: _toaster?.pinCodeError
        }
      })
    cartCalculation(false, false, false, true, false)
  }

  useEffect(() => {
    if (modalShow?.show && modalShow?.type === 'coupon') {
      fetchCoupon()
    }
  }, [modalShow])

  useEffect(() => {
    if (!user?.userId) {
      router.push('/')
    } else if (data?.items?.length === 0) {
      router?.push('/cart')
    }
  }, [user, data])

  useEffect(() => {
    if (typeof window !== 'undefined' && window.localStorage) {
      const cartId = localStorage.getItem('cartId')
      setCartId(cartId)
    }
  }, [])

  useEffect(() => {
    let addressValue = { ...values }
    addressValue.addressVal =
      address?.length === 1
        ? address[0]
        : (address &&
            Object.keys(address).length > 1 &&
            address?.find((item) => item?.setDefault === true)) ||
          null

    if (address?.length > 0 && addressValue?.addressVal) {
      fetchPinCodeAndCheckCart(addressValue?.addressVal, false)
    } else {
      cartCalculation(false, false, false, true, false)
      setActiveAccordion(1)
    }
  }, [user?.userId])

  return (
    <>
      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
      <HeaderforCheckout
        activeAccordion={activeAccordion}
        setActiveAccordion={setActiveAccordion}
        stateValues={values}
      />

      {loading && !data ? (
        <div className='site-container'>
          <CheckoutSkeleton cartId={cartId} />
        </div>
      ) : (
        <div className='site-container'>
          {data?.data?.items?.length > 0 ? (
            <div className='check-out-main'>
              <div className='check-orderlist'>
                <CheckoutStepAccordions
                  data={data}
                  setData={setData}
                  activeAccordion={activeAccordion}
                  setActiveAccordion={setActiveAccordion}
                  cartCalculation={cartCalculation}
                  modalShow={modalShow}
                  setModalShow={setModalShow}
                  setLoading={setLoading}
                  values={values}
                  setValues={setValues}
                  toast={toast}
                  setToast={setToast}
                  handleAccordionChange={handleAccordionChange}
                  fetchPinCodeAndCheckCart={fetchPinCodeAndCheckCart}
                />
              </div>
              <div className='offer-price-details'>
                {!cartId && (
                  <OfferCoupan
                    modalShow={modalShow}
                    setModalShow={setModalShow}
                    data={offer}
                    cartCalculation={cartCalculation}
                    toast={toast}
                    setToast={setToast}
                    values={values}
                  />
                )}
                <PriceDetails cart={data?.data} />
              </div>
            </div>
          ) : (
            data && (
              <EmptyComponent
                src={'/images/emty_cart.jpg'}
                isCart
                alt={'empty_cart'}
                title={'Your cart is empty'}
                description={
                  'Must add items to the cart before you proceed to checkout.'
                }
              />
            )
          )}
        </div>
      )}
    </>
  )
}

export default Page
