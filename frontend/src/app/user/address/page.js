'use client'
import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { useRouter } from 'next/navigation'
import MyAddress from '../../../components/MyAddress'
import Loader from '../../../components/Loader'

const Page = () => {
  const [loading, setLoading] = useState(false)
  const [isClient, setIsClient] = useState(false)
  const router = useRouter()
  const { user } = useSelector((state) => state?.user)

  useEffect(() => {
    if (!user?.userId) {
      router.push('/')
    }
  }, [user])

  useEffect(() => {
    setIsClient(true)
  }, [])

  return (
    <>
      {loading && <Loader />}
      <div className='site-container'>
        {isClient && user?.userId && <MyAddress setLoading={setLoading} />}
      </div>
    </>
  )
}

export default Page
