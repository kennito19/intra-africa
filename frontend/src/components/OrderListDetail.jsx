'use client'
import moment from 'moment'
import Head from 'next/head'
import Image from 'next/image'
import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'
import 'react-loading-skeleton/dist/skeleton.css'
import ReactPaginate from 'react-paginate'
import { useSelector } from 'react-redux'
import Swal from 'sweetalert2'
import axiosProvider from '../lib/AxiosProvider'
import {
  _headDescription_,
  _headKeywords_,
  _headTitle_,
  _ogDescription_,
  _ogLogo_,
  _ogUrl_,
  _projectName_
} from '../lib/ConfigVariables'
import {
  _orderStatus_,
  encryptId,
  pageRangeDisplayed,
  reactImageUrl,
  showToast,
  spaceToDash
} from '../lib/GetBaseUrl'
import { _orderImg_ } from '../lib/ImagePath'
import { _SwalDelete, _exception } from '../lib/exceptionMessage'
import useDebounce from '../lib/useDebounce'
import EmptyComponent from './EmptyComponent'
import MyaccountMenu from './MyaccountMenu'
import ModalComponent from './base/ModalComponent'
import Toaster from './base/Toaster'
import OrderSkeleton from './skeleton/OrderSkeleton'

const OrderListDetail = ({
  toast,
  setToast,
  filterDetails,
  setFilterDetails
}) => {
  const router = useRouter()
  const [loading, setLoading] = useState(false)
  const [searchText, setSearchText] = useState()
  const apiEndpoint = 'User/Order/bysearchText'
  const { user } = useSelector((state) => state?.user)
  const apiQueryString = `?UserId=${user?.userId}&pageIndex=${
    filterDetails?.pageIndex
  }&pageSize=${filterDetails?.pageSize}${
    filterDetails?.searchText ? `&searchText=${filterDetails?.searchText}` : ''
  }`
  const debounceSearchText = useDebounce(searchText, 500)
  const validStatuses = ['Placed', 'Confirmed', 'Cancelled', 'Failed']
  const [orderCancel, setorderCancel] = useState(false)
  const [data, setData] = useState()
  const [isClient, setIsClient] = useState(false)
  const [currentURL, setCurrentURL] = useState(false)

  const fetchOrder = async (apiEndpointOrder, apiQueryStringOrder) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: apiEndpointOrder,
        queryString: apiQueryStringOrder
      })
      setLoading(false)
      if (response?.status === 200) {
        setData(response)
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  useEffect(() => {
    if (debounceSearchText) {
      setFilterDetails((draft) => {
        draft.searchText = debounceSearchText
        draft.pageIndex = 1
      })
    } else {
      setFilterDetails((draft) => {
        draft.searchText = ''
        draft.pageIndex = 1
      })
    }
  }, [debounceSearchText])

  useEffect(() => {
    if (apiEndpoint && apiQueryString) {
      fetchOrder(apiEndpoint, apiQueryString)
    }
  }, [filterDetails, apiEndpoint, apiQueryString])

  useEffect(() => {
    if (!user?.userId) {
      router.push('/')
    }
  }, [user])

  const handleSearch = (event) => {
    if (event.key === 'Enter' || event.type === 'click') {
      if (searchText?.trim().length > 2) {
        fetchOrder(apiEndpoint, apiQueryString)
      }
    }
  }

  const handlePageClick = (event) => {
    setFilterDetails((draft) => {
      draft.pageIndex = event.selected + 1
      router.push(`/user/orders?pi=${event.selected + 1}&ps=10`, undefined, {
        shallow: true
      })
    })
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  useEffect(() => {
    setIsClient(true)
    // setCurrentURL(window?.location?.href)
  }, [])

  return (
    <>
      <Head>
        <link
          rel='canonical'
          href={currentURL ?? process?.env?.REACT_APP_BASE_URL}
        />
        <title>My Order</title>
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
          <MyaccountMenu activeTab='order' />
        </div>
        <div className='wish_inner_80'>
          <div className='order-title-search-main'>
            <div className='titlebar-order-section'>
              <h1 className='order-menu-title'>
                <Link href={'/user/orders'}> Orders</Link>
              </h1>
            </div>
            {apiEndpoint === 'User/Order/bysearchText' && (
              <div className='search-filter-divmain'>
                <div className='search_bar_order'>
                  <input
                    className='search_in_order_product'
                    type='search'
                    id='searchbar'
                    value={searchText}
                    disabled={loading}
                    placeholder='Search in Orders'
                    onChange={(e) => {
                      setSearchText(e?.target?.value)
                    }}
                  />
                  <label htmlFor=''>
                    <i
                      className='m-icon order-m-search__icon'
                      onClick={handleSearch}
                    ></i>
                  </label>
                </div>
              </div>
            )}
          </div>
          {loading ? (
            <OrderSkeleton showMenu={false} />
          ) : isClient && data?.data?.data?.length > 0 ? (
            <>
              {data?.data?.data?.map((order) => (
                <div
                  className='orderconfirm-main'
                  key={Math.floor(Math.random() * 100000)}
                >
                  <div className='order-title-row'>
                    <div className='order-total-placed-main'>
                      <div className='order-place'>
                        <span>Order Placed</span>
                        <p>
                          {moment(order?.orderDate)?.format('DD MMMM YYYY')}
                        </p>
                      </div>
                      <div className='order-total'>
                        <span>Total</span>
                        <p>&#8377; {order?.paidAmount}</p>
                      </div>
                    </div>
                    <div className='ship-order-main'>
                      <div className='order-shipto'>
                        <span>Ship To</span>
                        <p>{order?.userName}</p>
                      </div>
                      <div className='order-number-invoice'>
                        <p>Order #{order?.orderNo}</p>
                        <ul>
                          <li>
                            <Link
                              href={`/user/orders/${encryptId(order?.orderId)}`}
                            >
                              View Order Details
                            </Link>
                          </li>
                          {order?.orderItems?.length > 1 &&
                            order?.orderItems?.filter((item) =>
                              [
                                _orderStatus_?.placed,
                                _orderStatus_?.confirmed
                              ]?.includes(item?.status)
                            )?.length === order?.orderItems?.length && (
                              <li>
                                {
                                  <button
                                    className='pv-order-cancelall'
                                    onClick={() =>
                                      Swal.fire({
                                        title: _SwalDelete?.title,
                                        text: _SwalDelete?.text,
                                        icon: _SwalDelete?.icon,
                                        showCancelButton:
                                          _SwalDelete?.showCancelButton,
                                        confirmButtonColor:
                                          _SwalDelete?.confirmButtonColor,
                                        cancelButtonColor:
                                          _SwalDelete?.cancelButtonColor,
                                        confirmButtonText:
                                          _SwalDelete?.confirmButtonText,
                                        cancelButtonText:
                                          _SwalDelete?.cancelButtonText,
                                        customClass: {
                                          title: 'sweet-alert-text'
                                        }
                                      }).then((result) => {
                                        if (result?.isConfirmed) {
                                          const orderItemIds = order?.orderItems
                                            ?.map((orderItem) => orderItem.id)
                                            .join(',')
                                          router?.push(
                                            `/user/order/cancel/${encryptId(
                                              order?.orderId
                                            )}/${encryptId(orderItemIds)}`
                                          )
                                        }
                                      })
                                    }
                                  >
                                    Cancel All
                                  </button>
                                }
                              </li>
                            )}
                        </ul>
                      </div>
                    </div>
                  </div>
                  {order && order?.orderItems.length > 0 && (
                    <div>
                      {order?.orderItems?.map((orderItem) => (
                        <div
                          className='order-product-image-info'
                          key={Math.floor(Math.random() * 100000)}
                        >
                          <div className='order-product-image'>
                            <Link
                              href={`/product/${spaceToDash(
                                orderItem?.productName
                              )}/${encryptId(
                                orderItem?.productGUID
                              )}?sp_id=${encryptId(
                                orderItem?.sellerProductID
                              )}&s_id=${encryptId(orderItem?.sizeID)}`}
                            >
                              <Image
                                className=''
                                src={
                                  orderItem &&
                                  encodeURI(
                                    `${reactImageUrl}${_orderImg_}${orderItem?.productImage}`
                                  )
                                }
                                alt={order?.productName}
                                width={71.25}
                                height={95}
                              />
                            </Link>
                          </div>
                          <div className='orderproduct-title'>
                            <Link
                              href={`/product/${spaceToDash(
                                orderItem?.productName
                              )}/${encryptId(
                                orderItem?.productGUID
                              )}?sp_id=${encryptId(
                                orderItem?.sellerProductID
                              )}&s_id=${encryptId(orderItem?.sizeID)}`}
                            >
                              <p>{orderItem?.productName}</p>
                            </Link>
                            {orderItem?.sizeValue && (
                              <span>
                                Size:<p>{orderItem?.sizeValue}</p>
                              </span>
                            )}
                            {orderItem?.color && (
                              <span>
                                Color:<p>{orderItem?.color}</p>
                              </span>
                            )}
                          </div>
                          {validStatuses.includes(orderItem?.status) && (
                            <div className='order-canl-retn-btn'>
                              {orderItem?.status === 'Cancelled' ||
                              orderItem?.status === 'Failed' ? (
                                <div>
                                  <span className='order-badge-danger'>
                                    {orderItem?.status}
                                  </span>
                                </div>
                              ) : (
                                <>
                                  {(_orderStatus_?.placed ===
                                    orderItem?.status ||
                                    _orderStatus_?.confirmed ===
                                      orderItem?.status ||
                                    _orderStatus_?.packed ===
                                      orderItem?.status) && (
                                    <button
                                      className='m-btn order-btn-cancel'
                                      onClick={() =>
                                        Swal.fire({
                                          title: `Are you sure you want to cancel the purchase of the item: ${orderItem?.productName}`,
                                          text: _SwalDelete.text,
                                          icon: _SwalDelete.icon,
                                          showCancelButton:
                                            _SwalDelete.showCancelButton,
                                          confirmButtonColor:
                                            _SwalDelete.confirmButtonColor,
                                          cancelButtonColor:
                                            _SwalDelete.cancelButtonColor,
                                          confirmButtonText:
                                            _SwalDelete.confirmButtonText,
                                          cancelButtonText:
                                            _SwalDelete.cancelButtonText
                                        }).then((result) => {
                                          if (result?.isConfirmed) {
                                            router?.push(
                                              `/user/order/cancel/${encryptId(
                                                orderItem?.orderID
                                              )}/${encryptId(orderItem?.id)}`
                                            )
                                          }
                                        })
                                      }
                                    >
                                      Cancel
                                    </button>
                                  )}
                                </>
                              )}
                            </div>
                          )}
                          {orderItem?.status === _orderStatus_?.delivered &&
                          moment(orderItem?.returnValidTillDate)?.diff(
                            moment(),
                            'days'
                          ) > 0 ? (
                            <button
                              className='m-btn order-btn-return'
                              onClick={() =>
                                Swal.fire({
                                  title: `Are you sure you want to return the purchase of the item: ${orderItem?.productName}`,
                                  text: _SwalDelete.text,
                                  icon: _SwalDelete.icon,
                                  showCancelButton:
                                    _SwalDelete.showCancelButton,
                                  confirmButtonColor:
                                    _SwalDelete.confirmButtonColor,
                                  cancelButtonColor:
                                    _SwalDelete.cancelButtonColor,
                                  confirmButtonText:
                                    _SwalDelete.confirmButtonText,
                                  cancelButtonText: _SwalDelete.cancelButtonText
                                }).then((result) => {
                                  if (result?.isConfirmed) {
                                    router?.push(
                                      `/user/order/return/${encryptId(
                                        order?.orderId
                                      )}/${encryptId(orderItem?.id)}`
                                    )
                                  }
                                })
                              }
                            >
                              Return
                            </button>
                          ) : orderItem?.status === _orderStatus_?.returned ? (
                            <>
                              {orderItem?.status ===
                                _orderStatus_?.returned && (
                                <div className='order-canceld'>
                                  <div class='order-canceld-green-mark'></div>
                                  <span class='order-canceld-text'>
                                    {orderItem?.status}
                                  </span>
                                </div>
                              )}
                            </>
                          ) : (
                            <>
                              {orderItem?.status ===
                                _orderStatus_?.replaceRequested && (
                                <div className='order-canceld'>
                                  <div class='order-canceld-green-mark'></div>
                                  <span class='order-canceld-text'>
                                    {orderItem?.status}
                                  </span>
                                </div>
                              )}
                            </>
                          )}
                          {orderItem?.status &&
                            (orderItem?.status === _orderStatus_?.packed ||
                              orderItem?.status === _orderStatus_?.ship) && (
                              <Link
                                href={`/user/orders/${encryptId(
                                  order?.orderId
                                )}`}
                              >
                                <button className='m-btn order-btn-return'>
                                  Track
                                </button>
                              </Link>
                            )}
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              ))}
              {data?.data?.pagination &&
                data?.data?.pagination?.pageCount > 1 && (
                  <ReactPaginate
                    className='pv-cst--pagination'
                    breakLabel='...'
                    onPageChange={handlePageClick}
                    pageRangeDisplayed={pageRangeDisplayed}
                    pageCount={
                      Math.ceil(data?.data?.pagination?.pageCount) ?? 0
                    }
                    previousLabel={<span className='test'>&lt;</span>}
                    nextLabel={<span> &gt;</span>}
                    forcePage={filterDetails?.pageIndex - 1}
                  />
                )}
            </>
          ) : data?.data?.code ? (
            <EmptyComponent
              title={
                filterDetails?.searchText?.length > 0
                  ? 'Sorry, no results found'
                  : 'No Order found in your account!'
              }
              description={
                filterDetails?.searchText?.length > 0 &&
                'Edit search or go back to My Orders Page'
              }
              alt={'empty_Add'}
              isButton
              onClick={() => {
                filterDetails?.searchText?.length > 0
                  ? setSearchText('')
                  : router?.push('/')
              }}
              btnText={
                filterDetails?.searchText?.length > 0
                  ? 'Go to my orders'
                  : 'Shop now'
              }
              src={'/images/empty_wishlist.jpg'}
            />
          ) : (
            <OrderSkeleton showMenu={false} />
          )}
        </div>
      </div>

      {orderCancel && (
        <ModalComponent
          isOpen={true}
          onClose={() => setorderCancel(false)}
          modalSize={'modal-lg'}
          header_main={'cancel-order-head'}
          headingText={'Are You Sure You Want to Cancel This Order?'}
          headClass={'order-cancel'}
          bodyClass={'modal-body'}
        >
          <>
            <div className='item-ordered'>
              <h6 className='item-order-title'>Items Ordered</h6>
              <p className='cancel-order-decrip'>
                1 of : Boult Audio Maverick Truly Wireless in Ear Earbuds With
                35H Playtime, Quad mic ENC, 45ms Xtreme low latency, Gaming
                LEDS, Made in India, 10mmBass Dri
              </p>
              <p className='cancel-order-seller'>
                Sold By : Appario Business (Seller Profile)
              </p>
            </div>
            <div className='cancellation-reson'>
              <label>Reason For Cancellation (optional):</label>
              <select className='cancel-order-reason'>
                <option>Select Cancellation Reason</option>
                <option value='US'>Order Created By mistake</option>
                <option value='CA'>Item (s) Would Not Arrive on time</option>
                <option value='FR'>Shipping Cost Too High</option>
                <option value='DE'>Item Price Too High</option>
                <option value='DE'>Found Cheaper Somewhere Else</option>
                <option value='DE'>Need to Change Shipping Address</option>
                <option value='DE'>Need to Change Shipping Speed</option>
                <option value='DE'>Need to Change Billing Address</option>
              </select>
            </div>
            <div className='btn-order-cancel'>
              <button className='cancel-selct-item-btn'>
                Cancel Selected items in this Order
              </button>
              <button className='return-summary-btn'>
                Return to order Summary
              </button>
            </div>
          </>
        </ModalComponent>
      )}
    </>
  )
}

export default OrderListDetail
