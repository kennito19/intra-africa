import { addressData } from '@/redux/features/addressSlice'
import { cartData, clearCart, setCartCount } from '@/redux/features/cartSlice'
import { useRouter } from 'next/navigation'
import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { prepareOrderPlacingObject } from '../../../lib/AllGlobalFunction'
import axiosProvider from '../../../lib/AxiosProvider'
import { _exception } from '../../../lib/exceptionMessage'
import {
  convertToNumber,
  currencyIcon,
  maximumOrderValue,
  minimumOrderValue,
  showToast
} from '../../../lib/GetBaseUrl'
import { _toaster } from '../../../lib/tosterMessage'
import UserCheckout from './(component)/UserCheckout'

const CheckoutStepAccordions = ({
  data,
  setData,
  setLoading,
  cartCalculation,
  toast,
  setToast,
  cartId,
  values,
  setValues,
  activeAccordion,
  setActiveAccordion,
  handleAccordionChange,
  fetchPinCodeAndCheckCart,
  modalShow,
  setModalShow
}) => {
  const router = useRouter()
  const dispatch = useDispatch()
  const { user } = useSelector((state) => state?.user)
  const { address } = useSelector((state) => state?.address)
  const { cart } = useSelector((state) => state?.cart)

  const fetchAddress = async (id, accordionChange = false) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'Address/byUserId',
        queryString: `?userId=${user?.userId}`
      })
      setLoading(false)

      if (response?.status === 200) {
        const addressDataValue = response.data?.data || []
        const addressValData =
          id && addressDataValue.find((item) => item?.id === id)

        const addressVal = addressValData
          ? addressValData
          : addressDataValue?.length === 1
          ? addressDataValue[0]
          : {}

        setValues({
          ...values,
          addressData: addressDataValue,
          addressVal: addressVal
        })
        if (id && addressVal && !accordionChange) {
          fetchPinCodeAndCheckCart(addressVal)
        }

        dispatch(addressData(addressDataValue))
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const handleOrderPlacement = async (values, sellerProductIds) => {
    let data = await prepareOrderPlacingObject(values, cartId)
    setLoading(true)
    const resOrder = await axiosProvider({
      method: 'POST',
      endpoint: 'ManageOrder/SaveOrder',
      data
    })
    setLoading(false)

    if (resOrder?.data?.code === 200) {
      if (values?.paymentMode?.toLowerCase() === 'online') {
        router.push(resOrder?.data?.data?.redirect_url)
        return true
      }
      dispatch(cartData({ ...cart, coupon_code: null }))
      setLoading(true)
      const responseCart = await axiosProvider({
        method: 'DELETE',
        endpoint: 'Cart',
        queryString: `?sessionId=${
          values?.userId ? values?.userId : user?.userId
        }&userId=${
          values?.userId ? values?.userId : user?.userId
        }&sellerProductIds=${sellerProductIds}`
      })
      setLoading(false)

      if (responseCart?.data?.code === 200) {
        dispatch(clearCart())
        dispatch(setCartCount(0))
        router?.push(
          `/user/checkout/${
            values?.userId ? values?.userId : user?.userId
          }?refNo=${resOrder?.data?.data}`
        )
        return true
      }
    }
    return false
  }

  const onSubmit = async (values) => {
    const paidAmount = convertToNumber(values?.CartAmount?.paid_amount)

    if (paidAmount < minimumOrderValue) {
      showToast(toast, setToast, {
        data: {
          message: `The minimum purchase Value is ${currencyIcon}${minimumOrderValue}.`,
          code: 204
        }
      })
      return
    }
    try {
      const response = await cartCalculation(
        false,
        false,
        false,
        true,
        values?.addressVal,
        values?.paymentMode
      )
      let cartResponsePaidAmount = convertToNumber(
        response?.cartResponse?.data?.CartAmount?.paid_amount
      )
      if (cartResponsePaidAmount < minimumOrderValue) {
        showToast(toast, setToast, {
          data: {
            message: `The minimum purchase Value is ${currencyIcon}${minimumOrderValue}.`,
            code: 204
          }
        })
        return
      }

      if (response?.cartResponse?.status !== 200) {
        return
      }
      if (
        values?.paymentMode === 'cod' &&
        convertToNumber(
          response?.cartResponse?.data?.CartAmount?.paid_amount
        ) >= maximumOrderValue
      ) {
        paymentMod
        showToast(toast, setToast, {
          data: {
            message: `Your Cart Value is more than ${currencyIcon}${maximumOrderValue} so you can place COD order.`,
            code: 204
          }
        })
      } else {
        const cartObj = response?.cartResponse?.data.items?.filter(
          (item) => item?.status !== 'In stock'
        )

        if (cartObj?.length === 0) {
          const sellerProductIds = (
            response?.cartResponse?.items?.map(
              (product) => product?.taxinfo?.sellerProductID
            ) || []
          ).join(',')

          const orderPlaced = await handleOrderPlacement(
            response?.valuesResponse,
            sellerProductIds
          )
          if (!orderPlaced) {
            setLoading(false)
            showToast(toast, setToast, {
              data: {
                message: "Can't place order",
                code: 204
              }
            })
          }
        } else {
          setLoading(false)
          showToast(toast, setToast, {
            data: {
              message: "Can't place order",
              code: 204
            }
          })
        }
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: {
          message: _exception?.message,
          code: 204
        }
      })
    }
  }

  useEffect(() => {
    const handleKeyPress = (e) => {
      const searchElement = document.getElementById('product-searchbar')
      if (searchElement === document.activeElement) {
        return
      }
      const isEnterKey = e.key === 'Enter'

      if (isEnterKey) {
        switch (activeAccordion) {
          case 1:
            if (
              address &&
              address?.length > 1 &&
              Object.keys(values.addressVal).length === 0
            ) {
              showToast(toast, setToast, {
                data: { code: 204, message: _toaster?.addressError }
              })
            } else if (!modalShow?.show && values?.addressVal) {
              e.preventDefault()
              document.getElementById('deliverHereButton').click()
            }
            break
          case 3:
            if (modalShow?.show && modalShow?.type === 'cod') {
              e.preventDefault()
              document.getElementById('OrderPlace').click()
            } else {
              showToast(toast, setToast, {
                data: { code: 204, message: _toaster?.paymentError }
              })
            }
            break
          default:
            if (activeAccordion !== 3 && activeAccordion !== 1) {
              e.preventDefault()
              setActiveAccordion((prev) => (prev === 3 ? 0 : prev + 1))
            }
            break
        }
      }
    }

    document.addEventListener('keydown', handleKeyPress)

    return () => {
      document.removeEventListener('keydown', handleKeyPress)
    }
  }, [activeAccordion, setActiveAccordion, modalShow, values])

  return (
    <UserCheckout
      data={data}
      toast={toast}
      setToast={setToast}
      setData={setData}
      activeAccordion={activeAccordion}
      setActiveAccordion={setActiveAccordion}
      cartCalculation={cartCalculation}
      modalShow={modalShow}
      setModalShow={setModalShow}
      setLoading={setLoading}
      values={values}
      setValues={setValues}
      handleAccordionChange={handleAccordionChange}
      fetchPinCodeAndCheckCart={fetchPinCodeAndCheckCart}
      fetchAddress={fetchAddress}
      onSubmit={onSubmit}
    />
  )
}

export default CheckoutStepAccordions
