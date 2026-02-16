'use client'
import React, { useEffect, useState } from 'react'
import { useParams, useRouter } from 'next/navigation'
import OrderActionDetails from '../../../../../../components/misc/OrderActionDetails'
import { useSelector } from 'react-redux'
import Toaster from '../../../../../../components/base/Toaster'
import Loader from '../../../../../../components/Loader'
import {
  _orderStatus_,
  decryptId,
  showToast
} from '../../../../../../lib/GetBaseUrl'
import { _exception } from '../../../../../../lib/exceptionMessage'
import axiosProvider from '../../../../../../lib/AxiosProvider'

const Page = () => {
  const router = useRouter()
  const { user } = useSelector((state) => state?.user)
  const params = useParams()
  let orderId = decryptId(decodeURIComponent(params?.orderId))
  let orderItemId = decryptId(decodeURIComponent(params?.orderItemId))
  const [loading, setLoading] = useState(false)
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const [activeAccordion, setActiveAccordion] = useState(0)
  const initVal = {
    returnReplaceSec: 'Replacement',
    id: 0,
    orderID: orderId,
    orderItemID: orderItemId,
    newOrderNo: '',
    qty: null,
    actionID: 1,
    userId: '',
    userName: '',
    userPhoneNo: '',
    userEmail: '',
    userGSTNo: '',
    addressLine1: '',
    addressLine2: '',
    landmark: '',
    pincode: '',
    city: '',
    state: '',
    country: '',
    issue: '',
    reason: '',
    returnAction: 'Return',
    comment: '',
    paymentMode: '',
    attachment: '',
    refundAmount: '',
    refundType: '',
    bankName: '',
    bankBranch: '',
    bankIFSCCode: '',
    bankAccountNo: '',
    accountType: '',
    accountHolderName: '',
    ConfirmbankAccountNo: '',
    orderItem: null,
    phoneNumber: ''
  }
  const [initialValues, setInitialValues] = useState(initVal)
  const [orderItemData, setOrderItemData] = useState()

  const fetchOrderDetails = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'User/Order/byId',
        queryString: `?UserId=${user?.userId}&id=${orderId}&Isdeleted=false&PageIndex=1&PageSize=10`
      })
      setLoading(false)

      if (response?.status === 200) {
        if (response?.data?.data?.userId === user?.userId) {
          let responseStatus = response?.data?.data?.status
          if (
            responseStatus === _orderStatus_?.returnRequested ||
            responseStatus === _orderStatus_?.returnRejected ||
            responseStatus === _orderStatus_?.returned ||
            responseStatus === _orderStatus_?.replaceRequested ||
            responseStatus === _orderStatus_?.replaced
          ) {
            router?.push(`/user/orders/${params?.orderId}`)
          }
          const orderData = response?.data?.data
          const orderItem = orderData?.orderItems?.find(
            (item) => item.id == orderItemId
          )
          if (orderItem) {
            setInitialValues({
              ...initialValues,
              orderID: Number(orderId),
              orderItemID: Number(orderItemId),
              qty: orderItem?.qty,
              userId: orderData?.userId,
              userName: orderData?.userName,
              userPhoneNo: orderData?.userPhoneNo,
              userEmail: orderData?.userEmail,
              paymentMode: orderData?.paymentMode,
              refundAmount: orderItem?.sellingPrice,
              actionID: 1,
              returnAction: 'Return'
            })
            setOrderItemData(orderItem)
          }
        } else {
          router?.push('/')
        }
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  useEffect(() => {
    if (orderId) {
      fetchOrderDetails()
    }
  }, [orderId])

  useEffect(() => {
    if (!user?.userId) {
      router.push('/')
    }
  }, [user])

  return (
    <>
      {loading && <Loader />}
      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
      {user?.userId && orderItemData && (
        <div className='site-container'>
          <div className='check-order-return'>
            <OrderActionDetails
              setToast={setToast}
              toast={toast}
              setLoading={setLoading}
              setInitialValues={setInitialValues}
              initialValues={initialValues}
              orderItemData={orderItemData}
              activeAccordion={activeAccordion}
              setActiveAccordion={setActiveAccordion}
            />
          </div>
        </div>
      )}
    </>
  )
}

export default Page
