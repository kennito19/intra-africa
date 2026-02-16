import React from 'react'
import {
  encryptId,
  getSessionId,
  reactImageUrl,
  showToast,
  spaceToDash
} from '../../lib/GetBaseUrl'
import Image from 'next/image'
import Link from 'next/link'
import { _productImg_ } from '../../lib/ImagePath'
import MBtn from '../base/MBtn'
import { useSelector } from 'react-redux'
import axiosProvider from '../../lib/AxiosProvider'
import { _exception } from '../../lib/exceptionMessage'
import { useRouter } from 'next/router'

const OutofStock = ({
  stockItems,
  stateValues,
  toast,
  setToast,
  cartCalculation,
  modalShow,
  setModalShow
}) => {
  const { user } = useSelector((state) => state?.user)
  const sessionId = getSessionId()
  const router = useRouter()

  const handleDelete = async () => {
    const cartIds = (stockItems?.map((item) => item?.cartId) || []).join(',')
    try {
      const response = await axiosProvider({
        method: 'DELETE',
        endpoint: 'Cart',
        queryString: `?sessionId=${
          stateValues?.userId
            ? stateValues?.userId
            : user?.userId
            ? user?.userId
            : sessionId
        }&id=${cartIds}`
      })

      if (response?.data?.code === 200) {
        cartCalculation(false, false, false, true, stateValues?.addressVal)
        router?.push('/user/checkout')
        // showToast(toast, setToast, {
        //   data: {
        //     message: `Successfully removed item from cart: ${cart?.productName}.`,
        //     code: 200
        //   }
        // })
      } else {
        showToast(toast, setToast, response)
      }
    } catch {
      showToast(toast, setToast, {
        data: {
          message: _exception?.message,
          code: 204
        }
      })
    }
  }

  return (
    <>
      {stockItems &&
        stockItems?.map((cart) => (
          <div
            className='mp-outofstock-item'
            key={cart.id}
          >
            <Link
              href={`/product/${spaceToDash(cart?.productName)}/${encryptId(
                cart?.ProductGuid
              )}?sp_id=${encryptId(cart?.sellerProductID)}&s_id=${encryptId(
                cart?.sizeId
              )}`}
            >
              <Image
                className='product-img-add-to-cart'
                src={
                  cart?.Image &&
                  encodeURI(`${reactImageUrl}${_productImg_}${cart?.Image}`)
                }
                alt={cart?.productName ?? "image"}
                width={180}
                height={210}
              />
            </Link>
            <p className='cart-product-title'>
              <Link
                href={`/product/${spaceToDash(cart?.productName)}/${encryptId(
                  cart?.ProductGuid
                )}?sp_id=${encryptId(cart?.sellerProductID)}&s_id=${encryptId(
                  cart?.sizeId
                )}`}
              >
                {cart?.productName}
              </Link>
            </p>
          </div>
        ))}

      <div className='mp-outofstock-btn-main'>
        <div className='mp-outofstock-badge'>Out of stock</div>
        <div className='mp-outofstock-btn'>
          <MBtn
            btnText='Cancel'
            buttonClass='mp-outofstock-cancel'
            onClick={() => {
              setModalShow({
                show: !modalShow?.show,
                type: 'outOfStockProduct'
              })
            }}
          />
          <MBtn
            btnText='Continue'
            buttonClass='mp-outofstock-continue'
            onClick={() => {
              handleDelete()
            }}
          />
        </div>
      </div>
    </>
  )
}

export default OutofStock
