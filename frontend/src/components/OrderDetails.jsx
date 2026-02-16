'use client'
import FileSaver from 'file-saver'
import moment from 'moment'
import Image from 'next/image'
import Link from 'next/link'
import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'
import Swal from 'sweetalert2'
import axiosProvider from '../lib/AxiosProvider'
import {
  _orderStatus_,
  arrangeItemsByStatusAndPackage,
  arrangeOrderItems,
  currencyIcon,
  decryptId,
  encryptId,
  formatNumberWithCommas,
  getBaseUrl,
  getDeviceId,
  getUserToken,
  reactImageUrl,
  showToast,
  spaceToDash
} from '../lib/GetBaseUrl'
import { _orderImg_ } from '../lib/ImagePath'
import { _SwalDelete, _exception } from '../lib/exceptionMessage'
import ShoppingCheckOut from './OrderTrackingSystem'
import ModalComponent from './base/ModalComponent'
import BreadCrumb from './misc/BreadCrumb'
import OrderDetailSkeleton from './skeleton/OrderDetailSkeleton'

const OrderDetails = ({ toast, setToast, loading, setLoading }) => {
  const router = useRouter()
  const params = useParams()
  const decodeId = decryptId(decodeURIComponent(params?.id))
  const [data, setData] = useState()
  const [track, setTrack] = useState()
  const [shoppingCheckOut, setShoppingCheckOut] = useState(false)
  const validStatuses = ['Placed', 'Confirmed', 'Cancelled']
  const breadCrumbData = [
    { link: '/', text: 'Home' },
    { link: '/user/orders', text: 'My Orders' },
    { link: false, text: data?.orderNo }
  ]

  const fetchOrderDetails = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'User/Order/byId',
        queryString: `?id=${decodeId}&Isdeleted=false&PageIndex=1&PageSize=10`
      })
      setLoading(false)
      if (response?.data?.code === 200) {
        setData(response?.data?.data)
      } else {
        throw Error
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const fetchOrdertrack = async (orderId, orderItemID) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'ManageOrder/OrderTrack',
        queryString: `?OrderID=${orderId}&OrderItemID=${orderItemID}&PageIndex=0&PageSize=0`
      })
      setLoading(false)
      if (response?.status === 200) {
        setShoppingCheckOut(true)
        setTrack(response?.data?.data)
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  // const handleCancel = async (order) => {
  //   const orderCancelData = {
  //     orderId: order?.orderID,
  //     orderItemIds: `${order?.id}`
  //   }
  //   try {
  //     setLoading(true)
  //     const response = await axiosProvider({
  //       method: 'POST',
  //       endpoint: 'ManageOrder/OrderCancel',
  //       data: orderCancelData
  //     })
  //     setLoading(false)
  //     if (response?.status === 200) {
  //       fetchOrderDetails()
  //       showToast(toast, setToast, response)
  //     }
  //   } catch {
  //     setLoading(false)
  //     showToast(toast, setToast, {
  //       data: { code: 204, message: _exception?.message }
  //     })
  //   }
  // }

  useEffect(() => {
    if (decodeId) {
      fetchOrderDetails()
    }
  }, [decodeId])
  return loading ? (
    <OrderDetailSkeleton />
  ) : (
    <div>
      <div className='order-details-main'>
        <div className='order-detail-title-main'>
          <BreadCrumb items={breadCrumbData} />
          <div className='order-detail-title'>
            <h1 className='od-title'>Order Details</h1>
            <div className='order-date'>
              <span>
                Ordered {moment(data?.orderDate)?.format('DD MMMM YYYY')}
              </span>
              <p>Order #{data?.orderNo}</p>
            </div>
          </div>
        </div>
        <div className='pv-order-listmain'>
          {data?.orderItems &&
            Array.from(
              new Set(data?.orderItems?.map((item) => item?.sellerID))
            )?.map((sellerID) => (
              <>
                {Object.values(
                  arrangeItemsByStatusAndPackage(
                    arrangeOrderItems(data?.orderItems)?.filter(
                      (item) => item?.sellerID === sellerID
                    )
                  )
                )?.map((orderItem) => (
                  <div className='order-seller-Items' key={orderItem?.id}>
                    {(orderItem[0]?.status === _orderStatus_?.ship ||
                      orderItem[0]?.status === _orderStatus_?.delivered) && (
                      <div className='od-invoice'>
                        <button
                          // onClick={async () => {
                          //   try {
                          //     setLoading(true)
                          //     const response = await axiosProvider({
                          //       method: 'GET',
                          //       endpoint: 'Seller/Order/Invoice',
                          //       queryString: `?packageId=${orderItem[0]?.packageId}`
                          //     })
                          //     setLoading(false)
                          //     if (response?.data?.code === 200) {
                          //       setModalShow({
                          //         type: 'invoice',
                          //         show: !modalShow?.show,
                          //         data: response?.data?.data
                          //       })
                          //     }
                          //     showToast(toast, setToast, response)
                          //   } catch {
                          //     setLoading(false)
                          //     showToast(toast, setToast, {
                          //       data: {
                          //         message: _exception?.message,
                          //         code: 204
                          //       }
                          //     })
                          //   }
                          // }}
                          onClick={async () => {
                            if (orderItem[0]?.packageId) {
                              let downloadUrl = `${getBaseUrl()}GenerateInvoice/GenerateInvoice?Packageid=${
                                orderItem[0]?.packageId
                              }`
                              let headers = new Headers()
                              headers.append(
                                'Authorization',
                                `Bearer ${getUserToken()}`
                              )
                              headers.append('device_id', `${getDeviceId()}`)
                              setLoading(true)
                              fetch(downloadUrl, {
                                method: 'POST',
                                headers: headers
                              })
                                .then((response) => {
                                  setLoading(false)
                                  const blob = response.blob()
                                  return blob
                                })
                                .then((blob) => {
                                  const customFileName = `${data?.orderNo}.pdf`
                                  FileSaver.saveAs(blob, customFileName)
                                })
                            } else {
                              showToast(toast, setToast, {
                                data: {
                                  message: _exception?.message,
                                  code: 204
                                }
                              })
                            }
                          }}
                          className='order-bill'
                        >
                          Invoice
                        </button>
                      </div>
                    )}

                    {orderItem?.map((order) => (
                      <div className='order-product-detail' key={order?.id}>
                        <div className='btn-product-review'>
                          {validStatuses.includes(order?.status) && (
                            <div className='order-canl-retn-btn'>
                              {(_orderStatus_?.placed === order?.status ||
                                _orderStatus_?.confirmed === order?.status ||
                                _orderStatus_?.packed === order?.status) && (
                                <button
                                  className='product-support'
                                  onClick={() =>
                                    Swal.fire({
                                      title: `Are you sure you want to cancel the purchase of the item: ${order?.productName}`,
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
                                            order?.orderID
                                          )}/${encryptId(order?.id)}`
                                        )
                                      }
                                    })
                                  }
                                >
                                  Cancel
                                </button>
                              )}
                            </div>
                          )}
                          {shoppingCheckOut && (
                            <ModalComponent
                              isOpen={true}
                              onClose={() => setShoppingCheckOut(false)}
                              modalSize={'modal-sm'}
                              header_main={'cancel-order-head'}
                              headingText={'Track Order'}
                              headClass={'HeaderText'}
                              bodyClass={'modal-body'}
                            >
                              <ShoppingCheckOut
                                order={order}
                                PlaceDate={data?.orderDate}
                                track={track}
                              />
                            </ModalComponent>
                          )}
                          {order?.status === 'Delivered' && (
                            <Link
                              href={`/user/order/return/${encryptId(
                                order?.orderID
                              )}/${encryptId(order?.id)}`}
                            >
                              <button className='orderdetail-btn-return'>
                                Return
                              </button>
                            </Link>
                          )}
                          <button
                            onClick={() => {
                              fetchOrdertrack(order?.orderID, order?.id)
                            }}
                            className='pv-order-track'
                          >
                            Track Order
                          </button>
                        </div>
                        <div className='order-product-detail-image'>
                          <Link
                            href={`/product/${spaceToDash(
                              order?.productName
                            )}/${encryptId(
                              order?.productGUID
                            )}?sp_id=${encryptId(
                              order?.sellerProductID
                            )}&s_id=${encryptId(order?.sizeID)}`}
                          >
                            {order?.productImage && (
                              <Image
                                className='order-detail-image-info'
                                src={
                                  order?.productImage &&
                                  encodeURI(
                                    `${reactImageUrl}${_orderImg_}${order?.productImage}`
                                  )
                                }
                                alt={order?.productName}
                                width={122}
                                height={163}
                              />
                            )}
                          </Link>
                        </div>
                        <div className='order-productdetail-info'>
                          <Link
                            href={`/product/${spaceToDash(
                              order?.productName
                            )}/${encryptId(
                              order?.productGUID
                            )}?sp_id=${encryptId(
                              order?.sellerProductID
                            )}&s_id=${encryptId(order?.sizeID)}`}
                          >
                            <p>{order?.productName}</p>
                          </Link>
                          {order?.sizeValue && (
                            <span className='product-size-color'>
                              Size : <b>{order?.sizeValue}</b>
                            </span>
                          )}
                          {order?.color && (
                            <span className=''>
                              Color : <b>{order?.color}</b>
                            </span>
                          )}
                          {order?.qty && (
                            <span className=''>
                              Qty : <b>{order?.qty}</b>
                            </span>
                          )}
                          <div className='pv-order-mrp'>
                            <span>MRP : </span>{' '}
                            <span>
                              <div className='prd-list-price__wrapper'>
                                <h2 className='prd-total-price'>
                                  {order?.sellingPrice}
                                </h2>
                                <p className='prd-check-price'>{order?.mrp}</p>
                                <span className='prd-list-offer'>
                                  ({order?.discount}% OFF)
                                </span>
                              </div>
                            </span>
                          </div>

                          <dl>
                            {moment(data?.orderDate)?.format('DD MMMM YYYY')}
                          </dl>
                          {(order?.status === _orderStatus_?.cancelled ||
                            order?.status === _orderStatus_?.failed) && (
                            <div>
                              <span className='order-badge-danger'>
                                {order?.status}
                              </span>
                            </div>
                          )}
                        </div>
                      </div>
                    ))}
                  </div>
                ))}
              </>
            ))}
        </div>

        <div className='shipping-main'>
          {data?.userName && (
            <>
              <div className='payment'>
                <h2 className='payment-title'>Payment Methods</h2>
                <p>{data?.paymentMode}</p>
              </div>
              <div className='order-summary'>
                <h2 className='order-title'>Order Summary</h2>
                <table>
                  <tbody>
                    <tr className='ord-summry-price'>
                      <td className='subtotal-label'>Item(S) Subtotal:</td>
                      <td className='order-price'>
                        {currencyIcon}
                        {formatNumberWithCommas(data?.totalAmount)}
                      </td>
                    </tr>
                    <tr className='ord-summry-price'>
                      <td className='subtotal-label'>Shipping:</td>
                      <td className='order-price'>
                        {currencyIcon}
                        {formatNumberWithCommas(data?.totalShippingCharge)}
                      </td>
                    </tr>
                    <tr className='ord-summry-price'>
                      <td className='subtotal-label'>Extra Charges:</td>
                      <td className='order-price'>
                        {currencyIcon}
                        {formatNumberWithCommas(data?.totalExtraCharges)}
                      </td>
                    </tr>
                    {Boolean(data?.codCharge) && (
                      <tr className='ord-summry-price'>
                        <td className='subtotal-label'>COD Charges:</td>
                        <td className='order-price'>
                          {currencyIcon}
                          {formatNumberWithCommas(data?.codCharge)}
                        </td>
                      </tr>
                    )}
                    {Boolean(data?.coupontDiscount) && (
                      <tr className='ord-summry-price'>
                        <td className='subtotal-label'>Extra discount:</td>
                        <td className='order-price'>
                          - {currencyIcon}
                          {formatNumberWithCommas(data?.coupontDiscount)}
                        </td>
                      </tr>
                    )}
                    <tr className='ord-summry-price'>
                      <td className='grandtotal-label'>Grand Total:</td>
                      <td className='grand-price'>
                        {currencyIcon}{' '}
                        {formatNumberWithCommas(data?.paidAmount)}
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
              <div className='ship-address'>
                <h2 className='ship-title'>Shipping Address</h2>
                <span>{data?.userName}</span>
                <p>
                  {data?.userAddressLine1}, {data?.userAddressLine2},
                  {data?.userCity} - {data?.userState},{data?.userCountry},
                  {data?.userPincode}
                </p>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  )
}

export default OrderDetails
