'use client'
import React, { useEffect, useState } from 'react'
import OrderConfirmed from '../../../../components/OrderConfirmed'
import OrderDelivering from '../../../../components/OrderDelivering'
import { useSelector } from 'react-redux'
import { _orderStatus_, getSessionId } from '../../../../lib/GetBaseUrl'
import axiosProvider from '../../../../lib/AxiosProvider'
import Loader from '../../../../components/Loader'
import { useParams, useRouter, useSearchParams } from 'next/navigation'

const Page = () => {
  const router = useRouter()
  const { sessionId } = useParams()
  const refNo = useSearchParams()?.get('refNo')
  const [loading, setLoading] = useState(false)
  const [data, setData] = useState()
  const session_Id = getSessionId()
  const { user } = useSelector((state) => state.user)
  const successStatuses = [
    'Placed',
    'Confirmed',
    'Packed',
    'Shipped',
    'Delivered',
    'Returned',
    'Replaced'
  ]
  const fetchData = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'User/Order/byOrderRefNo',
        queryString: `?orderRefNo=${refNo}`
      })
      setLoading(false)
      if (response?.data?.code === 200) {
        if (response?.data?.data[0]?.userId === session_Id) {
          setData(response)
        } else {
          router?.push('/')
        }
      }
    } catch {
      setLoading(false)
    }
  }

  const verifyOrder = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'PUT',
        endpoint: 'ManageOrder/VerifyOrder',
        queryString: `?orderRefNo=${refNo}`
      })

      if (response?.data?.code === 200) {
        fetchData()
      } else {
        router?.push('/')
        setLoading(false)
      }
    } catch {
      setLoading(false)
    }
  }

  useEffect(() => {
    if (sessionId === user?.userId) {
      if (refNo) {
        verifyOrder()
      }
    } else {
      router?.push('/')
    }
  }, [refNo])

  useEffect(() => {
    if (user?.userId && data?.data) {
      if (data?.data?.data[0]?.userId !== user?.userId) {
        router?.push('/')
      }
    }
  }, [user, data])

  return (
    <div className='site-container section_spacing_b'>
      {loading && <Loader />}
      {user?.userId && data && (
        <div className='order-confirm-place'>
          <OrderConfirmed
            status={
              successStatuses.includes(data?.data?.data[0]?.status)
                ? 'success'
                : 'failed'
            }
            data={data?.data?.data[0]}
          />

          <OrderDelivering data={data} orderId={refNo} />
        </div>
      )}
    </div>
  )
}

export default Page
