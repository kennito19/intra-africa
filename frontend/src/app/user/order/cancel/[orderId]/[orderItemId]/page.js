'use client'
import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import Toaster from '../../../../../../components/base/Toaster'
import Loader from '../../../../../../components/Loader'
import OrderCancel from '../../../../../../components/misc/OrderCancel'
import { _exception } from '../../../../../../lib/exceptionMessage'
import axiosProvider from '../../../../../../lib/AxiosProvider'
import { useParams, useRouter, useSearchParams } from 'next/navigation'
import { decryptId, showToast } from '@/lib/GetBaseUrl'

const Page = () => {
  const router = useRouter()
  const params = useParams()
  const { user } = useSelector((state) => state?.user)
  let orderId = decryptId(decodeURIComponent(params.orderId))
  let orderItemId = decryptId(decodeURIComponent(params?.orderItemId))
  const [loading, setLoading] = useState(false)
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const [activeAccordion, setActiveAccordion] = useState(0)
  const initVal = {
    orderId: Number(orderId),
    OrderItemIds: orderItemId,
    newOrderNo: '',
    qty: 0,
    actionID: 0,
    userId: user?.userId,
    userName: '',
    userPhoneNo: '',
    userEmail: '',
    issue: '',
    issueId: '',
    reason: '',
    reasonId: '',
    comment: '',
    paymentMode: '',
    attachment: '',
    refundAmount: 0,
    refundType: '',
    bankName: '',
    bankBranch: '',
    bankIFSCCode: '',
    bankAccountNo: '',
    accountType: '',
    accountHolderName: ''
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

      if (response?.status === 200) {
        if (response?.data?.data?.userId === user?.userId) {
          setLoading(false)
          const orderData = response?.data?.data
          const orderItem = orderData?.orderItems?.find(
            (item) => item.id == orderItemId
          )
          if (orderItem) {
            setInitialValues({
              ...initialValues,
              orderId: Number(orderId),
              OrderItemIds: orderItemId,
              qty: orderItem?.qty,
              userId: orderData?.userId,
              userName: orderData?.userName,
              userPhoneNo: orderData?.userPhoneNo,
              userEmail: orderData?.userEmail,
              paymentMode: orderData?.paymentMode,
              refundAmount: orderItem?.sellingPrice,
              actionID: 1,
              returnAction: 'Cancel'
            })
            setOrderItemData(orderItem)
          }
        } else {
          setLoading(false)
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
            <OrderCancel
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
