'use client'
import React, { useEffect, useState } from 'react'
import ProductList from './ProductList'
import BreadCrumb from './misc/BreadCrumb'
import MyaccountMenu from './MyaccountMenu'
import { useSelector } from 'react-redux'
import axiosProvider from '../lib/AxiosProvider'
import Loader from './Loader'
import Toaster from './base/Toaster'
import EmptyComponent from './EmptyComponent'
import { pageRangeDisplayed, showToast } from '../lib/GetBaseUrl'
import { _exception } from '../lib/exceptionMessage'
import ReactPaginate from 'react-paginate'
import { useImmer } from 'use-immer'
import Head from 'next/head'
import {
  _headDescription_,
  _headKeywords_,
  _headTitle_,
  _ogDescription_,
  _ogLogo_,
  _ogUrl_,
  _projectName_
} from '../lib/ConfigVariables'
import WishlistSkeleton from './skeleton/WishlistSkeleton'

const Wishlist = () => {
  const { user } = useSelector((state) => state?.user)
  const [loading, setLoading] = useState(true)
  const [data, setData] = useState()
  const [currentURL, setCurrentURL] = useState(false)
  const [filterDetails, setFilterDetails] = useImmer({
    pageSize: 10,
    pageIndex: 1,
    searchText: ''
  })
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })

  const fetchData = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'Wishlist/byUserId',
        queryString: `?userId=${user?.userId}&pageIndex=${filterDetails?.pageIndex}&pageSize=${filterDetails?.pageSize}`
      })
      setLoading(false)
      if (response?.status === 200) {
        setData(response)
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const handlePageClick = (event) => {
    setFilterDetails((draft) => {
      draft.pageIndex = event.selected + 1
    })
  }

  const handleRemove = async (userId, productId) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'DELETE',
        endpoint: 'Wishlist',
        queryString: `?userId=${userId}&productId=${productId}`
      })
      setLoading(false)

      if (response?.data?.code === 200) {
        fetchData()
        showToast(toast, setToast, response)
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  useEffect(() => {
    fetchData()
    setCurrentURL(window.location.href)
  }, [])

  useEffect(() => {
    if (!user?.userId) {
      router.push('/')
    }
  }, [user])

  // const handleWishlistClick = (product) => {
  //   if (product?.products?.guid) {
  //     handleRemove(user?.userId ?? '', product?.products?.guid ?? '')
  //   }
  // }

  return (
    <>
      <Head>
        <link rel='canonical' href={currentURL} />
        <title>My Wishlist</title>
        <meta name='description' content={_headDescription_} />
        <meta name='keywords' content={_headKeywords_} />
        <meta property='og:locale' content='en_US' />
        <meta
          property='og:title'
          content={`${_projectName_} - ${_headTitle_}`}
        />
        <meta property='og:description' content={_ogDescription_} />
        <meta property='og:url' content={_ogUrl_} />
        <meta property='og:image' content={_ogLogo_} />
      </Head>

      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}

      <div className='wish_main_flex'>
        <div className='wish_inner_20'>
          <MyaccountMenu activeTab='wish' />
        </div>

        {loading ? (
          <WishlistSkeleton />
        ) : (
          <div className='wish_inner_80'>
            {data && data?.data?.data?.length > 0 ? (
              <div className='order_grid_wishlist'>
                <div className='index-headingDiv'>
                  <span className='index-heading'>My Wishlist: </span>
                  <span className='index-count'>
                    {data && <>{data?.data?.data?.length} items</>}
                  </span>
                </div>
                <div className='p-prdlist-wishlist-wrapper'>
                  {data?.data?.data?.map((product) => (
                    <div
                      className='prdt-wishlist-relative'
                      key={Math.floor(Math.random() * 100000)}
                    >
                      <ProductList
                        product={product}
                        isView={true}
                        isWishlistProduct={true}
                        wishlistShow={false}
                      />
                      <div
                        className='whis_close'
                        onClick={() =>
                          handleRemove(product?.userId, product?.productId)
                        }
                      >
                        <i className='m-icon wishlist_close-icon'></i>
                      </div>
                    </div>
                  ))}
                </div>
                <ReactPaginate
                  className='list-inline m-cst--pagination flex justify-end gap-1'
                  breakLabel='...'
                  nextLabel=''
                  onPageChange={handlePageClick}
                  pageRangeDisplayed={pageRangeDisplayed}
                  pageCount={data?.data?.pagination?.pageCount ?? 0}
                  previousLabel=''
                  renderOnZeroPageCount={null}
                  forcePage={filterDetails?.pageIndex - 1}
                />
              </div>
            ) : (
              data && (
                <EmptyComponent
                  isButton
                  btnText={'Shop Now'}
                  redirectTo={'/'}
                  src={'/images/empty_wishlist.jpg'}
                  title={'Your wishlist is empty'}
                  description={
                    "seems like you don't have wishes here make a wish!"
                  }
                />
              )
            )}
          </div>
        )}
      </div>
    </>
  )
}

export default Wishlist
