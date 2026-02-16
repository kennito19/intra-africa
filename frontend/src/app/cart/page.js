'use client'
import CartSkeleton from '@/components/skeleton/CartSkeleton'
import { cartData, setCartCount } from '@/redux/features/cartSlice'
import { store } from '@/redux/store'
import { useRouter } from 'next/navigation'
import nookies from 'nookies'
import { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import EmptyComponent from '../../components/EmptyComponent'
import LoginSignup from '../../components/LoginSignup'
import PriceDetails from '../../components/PriceDetails'
import Toaster from '../../components/base/Toaster'
import AddToCartProduct from '../../components/misc/AddToCartProduct'
import ChargesLable from '../../components/misc/ChargesLable'
import OfferCoupan from '../../components/misc/OfferCoupan'
import axiosProvider from '../../lib/AxiosProvider'
import {
  convertToNumber,
  getSessionId,
  getUserId,
  showToast
} from '../../lib/GetBaseUrl'
import { _exception } from '../../lib/exceptionMessage'
const { v4: uuidv4 } = require('uuid')

const Page = () => {
  const router = useRouter()
  const dispatch = useDispatch()
  const { user } = useSelector((state) => state?.user)
  const { cart } = useSelector((state) => state?.cart)
  let mySessionId = getSessionId()
  const userIdCookie = getUserId()
  const [loading, setLoading] = useState(true)
  const [modalShow, setModalShow] = useState({ show: false, type: null })
  const [token, setToken] = useState(false)
  const [data, setData] = useState()
  const [offer, setOffer] = useState()
  const [modal, setModal] = useState(false)
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
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
    toastVariation = false,
    code = false,
    getReduxCouponCode = false
  ) => {
    if (!mySessionId?.length > 0) {
      if (user?.userId) {
        nookies.set(null, 'sessionId', user?.userId, { path: '/' })
      } else {
        mySessionId = generateSessionId()
        nookies.set(null, 'sessionId', mySessionId, { path: '/' })
      }
    }
    if (mySessionId) {
      const calculationData = {
        cartSessionId: mySessionId,
        userId: user?.userId,
        couponCode: getReduxCouponCode
          ? cart?.coupon_code ?? ''
          : code?.length > 0
          ? code
          : '',
        paymentMode: '',
        pincode: ''
      }
      try {
        setLoading(true)
        const response = await axiosProvider({
          method: 'POST',
          endpoint: 'Cart/CartCalculation',
          data: calculationData
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
          dispatch(
            cartData({
              ...cart,
              items: updatedResponse?.data?.items,
              CartAmount: updatedResponse?.data?.CartAmount
            })
          )
          const couponStatusFailed = updatedResponse?.data?.items?.filter(
            (item) => item?.coupon_status === 'failed'
          )
          const couponStatusSuccess = updatedResponse?.data?.items?.filter(
            (item) => item?.coupon_status === 'success'
          )

          if (updatedResponse?.data?.items?.length === 0) {
            dispatch(
              cartData({
                ...cart,
                coupon_code: null
              })
            )
          } else {
            if (
              code?.length > 0 ||
              toastVariation === 'remove' ||
              getReduxCouponCode
            ) {
              if (
                couponStatusSuccess?.length > 0 ||
                (toastVariation !== 'remove' && cart?.coupon_code)
              ) {
                dispatch(
                  cartData({
                    ...cart,
                    coupon_code: code ? code : cart?.coupon_code
                  })
                )
              } else {
                dispatch(
                  cartData({
                    ...cart,
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
          }
          if (toastVariation === 'remove') {
            showToast(toast, setToast, {
              data: {
                message: 'Coupon has been removed!',
                code: 200
              }
            })
            setModalShow({ show: false, type: 'coupon' })
          } else if (toastVariation) {
            couponStatusFailed?.length ===
              updatedResponse?.data?.items?.length &&
              showToast(toast, setToast, {
                data: {
                  message: updatedResponse?.data?.items[0]?.coupon_message,
                  code:
                    couponStatusFailed?.length ===
                    updatedResponse?.data?.items?.length
                      ? 400
                      : 200
                }
              })
          }
          if (modalShow?.show) {
            couponStatusFailed?.length !==
              updatedResponse?.data?.items?.length &&
              setModalShow({ show: false, type: 'coupon' })
          }
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
  }

  const fetchCoupon = async () => {
    setLoading(true)
    try {
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'user/ManageOffers/search',
        queryString: `?userId=${user?.userId ?? null}&showToCustomer=true`
      })
      setLoading(false)
      if (response?.status === 200) {
        setOffer(response)
      }
    } catch (error) {
      setLoading(false)
    }
  }

  const handleCheck = () => {
    if (!user?.userId) {
      showToast(toast, setToast, { data: _exception?.message, code: 204 })
    }
  }

  const handleLogin = () => {
    if (!user?.userId) {
      if (userIdCookie) {
        checkTokenAuthentication(toast, setToast)
      } else {
        setModal(true)
      }
    } else {
      setModal(false)
    }
  }

  const closeModal = () => {
    setModal(false)
    if (user?.userId) {
      router?.push('/user/checkout')
    }
  }

  useEffect(() => {
    cartCalculation(false, false, false, true)
  }, [user?.userId])

  useEffect(() => {
    if (modalShow?.show && modalShow?.type === 'coupon') {
      fetchCoupon()
    }
  }, [modalShow?.show])

  store?.subscribe(() => {
    setToken(nookies?.get()?.userToken)
  })

  return (
    <>
      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
      <div>
        {modal && (
          <LoginSignup onClose={closeModal} toast={toast} setToast={setToast} />
        )}
        {loading && !data ? (
          <CartSkeleton modalShow={modalShow} setModalShow={setModalShow} />
        ) : (
          <div className='small-container'>
            {data?.data?.items?.length > 0 ? (
              <div className='add-cart-compont'>
                <div className='cart-product-compont'>
                  {convertToNumber(data?.data?.CartAmount?.shipping_charges) ===
                    0 && <ChargesLable />}
                  <AddToCartProduct
                    data={data}
                    setData={setData}
                    mySessionId={mySessionId}
                    cartCalculation={cartCalculation}
                    setLoading={setLoading}
                    toast={toast}
                    setToast={setToast}
                    loading={loading}
                  />
                </div>
                <div className='cart-side-compont'>
                  {data?.data?.items?.some(
                    (item) => item?.status === 'In stock'
                  ) && (
                    <>
                      <OfferCoupan
                        modalShow={modalShow}
                        setModalShow={setModalShow}
                        data={offer}
                        cartCalculation={cartCalculation}
                        toast={toast}
                        setToast={setToast}
                      />
                      <PriceDetails
                        modalShow={modalShow}
                        setModalShow={setModalShow}
                        cart={data?.data}
                        handleCheck={handleCheck}
                        toast={toast}
                        setToast={setToast}
                        cartCalculation={cartCalculation}
                      />
                    </>
                  )}
                </div>
              </div>
            ) : (
              data && (
                <div
                  className='section_spacing_b'
                  style={{
                    height: '70vh',
                    textAlign: 'center',
                    display: 'grid',
                    placeContent: 'center',
                    fontSize: '30px'
                  }}
                >
                  <EmptyComponent
                    src={'/images/emty_cart.jpg'}
                    isCart
                    alt={'empty_cart'}
                    title={'Your cart is empty'}
                    description={
                      'Must add items to the cart before you proceed to checkout.'
                    }
                    onClick={handleLogin}
                  />
                </div>
              )
            )}
          </div>
        )}
      </div>
    </>
  )
}

export default Page
