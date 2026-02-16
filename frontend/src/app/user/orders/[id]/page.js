'use client'
import React, { useEffect, useState } from 'react'
import OrderDetails from '../../../../components/OrderDetails'
import { useSelector } from 'react-redux'
import { decryptId, showToast } from '../../../../lib/GetBaseUrl'
import OrderListDetail from '../../../../components/OrderListDetail'
import { _exception } from '../../../../lib/exceptionMessage'
import { useImmer } from 'use-immer'
import Loader from '../../../../components/Loader'
import { useParams, useRouter, useSearchParams } from 'next/navigation'

const Page = () => {
  const router = useRouter()
  const { user } = useSelector((state) => state?.user)
  const params = useParams()
  const [loading, setLoading] = useState(false)
  const decodeId = decryptId(decodeURIComponent(params?.id))
  const [apiData, setApiData] = useState({
    apiEndpoint: null,
    apiQueryString: null
  })
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const pi = useSearchParams()?.get('pi')
  const ps = useSearchParams()?.get('ps')
  const [filterDetails, setFilterDetails] = useImmer({
    pageSize: ps ? ps : 10,
    pageIndex: pi ? pi : 1,
    searchText: ''
  })

  const getApiData = (Id) => {
    let endPointAndQuery
    if (Id) {
      try {
        if (
          /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(
            Id
          )
        ) {
          endPointAndQuery = {
            apiEndpoint: 'User/Order/byOrderRefNo',
            apiQueryString: `?orderRefNo=${Id}`
          }
        } else if (/^\d+$/.test(Id)) {
          endPointAndQuery = {
            apiEndpoint: 'User/Order/byId',
            apiQueryString: `?id=${Id}&Isdeleted=false&PageIndex=1&PageSize=10`
          }
        } else {
          throw new Error('Invalid ID format')
        }
        setApiData(endPointAndQuery)
      } catch (error) {
        setLoading(false)
        showToast(toast, setToast, {
          data: { code: 204, message: _exception?.message }
        })
      }
    }
  }

  useEffect(() => {
    if (decodeId) {
      getApiData(decodeId)
    }
  }, [decodeId])

  useEffect(() => {
    if (!user?.userId) {
      router.push('/')
    }
  }, [user])
  return (
    apiData?.apiEndpoint &&
    apiData?.apiQueryString && (
      <>
        {user?.userId && apiData?.apiEndpoint === 'User/Order/byId' ? (
          <div className='site-container section_spacing_b'>
            <OrderDetails
              toast={toast}
              setToast={setToast}
              setLoading={setLoading}
              loading={loading}
            />
          </div>
        ) : (
          <div className='site-container'>
            <OrderListDetail
              setLoading={setLoading}
              loading={loading}
              toast={toast}
              setToast={setToast}
              apiEndpoint={apiData?.apiEndpoint}
              apiQueryString={apiData?.apiQueryString}
              setFilterDetails={setFilterDetails}
              filterDetails={filterDetails}
            />
          </div>
        )}
      </>
    )
  )
}
export default Page
