'use client'
import React, { useEffect, useState } from 'react'
import OrderListDetail from '../../../components/OrderListDetail'
import { useSelector } from 'react-redux'
import Loader from '../../../components/Loader'
import { useImmer } from 'use-immer'
import { useRouter, useSearchParams } from 'next/navigation'
import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'
import Link from 'next/link'
import MyaccountMenu from '@/components/MyaccountMenu'
import OrderSkeleton from '@/components/skeleton/OrderSkeleton'

const Page = () => {
  const pi = useSearchParams()?.get('pi')
  const ps = useSearchParams()?.get('ps')
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const [filterDetails, setFilterDetails] = useImmer({
    pageSize: ps ? ps : 10,
    pageIndex: pi ? pi : 1,
    searchText: ''
  })

  useEffect(() => {
    setFilterDetails((draft) => {
      draft.pageIndex = pi ? parseInt(pi) : filterDetails?.pageIndex
      draft.pageSize = ps ? parseInt(ps) : filterDetails?.pageSize
    })
  }, [pi, ps])

  return (
    <div className='site-container'>
      <OrderListDetail
        toast={toast}
        setToast={setToast}
        setFilterDetails={setFilterDetails}
        filterDetails={filterDetails}
      />
    </div>
  )
}

export default Page
