'use client'
import Image from 'next/image'
import Link from 'next/link'
import { useSelector } from 'react-redux'
import Swal from 'sweetalert2'
import axiosProvider from '../../lib/AxiosProvider'
import {
  convertToNumber,
  currencyIcon,
  encryptId,
  getSessionId,
  reactImageUrl,
  showToast,
  spaceToDash
} from '../../lib/GetBaseUrl'
import { _productImg_ } from '../../lib/ImagePath'
import { _SwalDelete, _exception } from '../../lib/exceptionMessage'
import MBtn from '../base/MBtn'

const AddToCartProduct = ({
  data,
  stateValues,
  setData,
  cartCalculation,
  setLoading,
  toast,
  setToast,
  loading
}) => {
  const { user } = useSelector((state) => state?.user)
  const sessionId = getSessionId()

  const handleChangeQty = async (value) => {
    const Values = {
      id: value?.cartId,
      userId: stateValues?.userId ? stateValues?.userId : user?.userId,
      sessionId: stateValues?.userId
        ? stateValues?.userId
        : user?.userId
        ? user?.userId
        : sessionId,
      sellerProductMasterId: value?.sellerProductID,
      quantity: value?.qty,
      createdBy: stateValues?.userId ? stateValues?.userId : user?.userId,
      sizeId: value?.sizeId ?? 0,
      tempMRP: Number(value?.itemPrice?.mrp) ?? 0,
      tempSellingPrice: Number(value?.itemPrice?.selling_price) ?? 0,
      tempDiscount: Number(value?.itemPrice?.discount) ?? 0,
      subTotal: convertToNumber(value?.ItemSubTotal) ?? 0,
      warrantyId: 0
    }
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'PUT',
        endpoint: 'Cart',
        data: Values
      })
      setLoading(false)
      if (response?.status === 200) {
        cartCalculation(false, false, false, true, stateValues?.addressVal)
        showToast(toast, setToast, response)
      } else {
        showToast(toast, setToast, response)
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: {
          message: _exception?.message,
          code: 204
        }
      })
    }
  }

  const handleDelete = async (cart) => {
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
        }&id=${cart?.cartId}`
      })

      if (response?.status === 200) {
        cartCalculation(false, false, false, true, stateValues?.addressVal)
        showToast(toast, setToast, {
          data: {
            message: `Successfully removed item from cart: ${cart?.productName}.`,
            code: 200
          }
        })
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

  const handleQty = (cartType, cartValue) => {
    const quantity =
      cartType === 'input'
        ? Number(cartValue?.qty) || 1
        : cartType === 'plus'
        ? Number(cartValue?.qty) + 1
        : Number(cartValue?.qty) - 1

    handleChangeQty({ ...cartValue, qty: quantity })
  }

  return (
    <>
      {data && data?.data?.items?.length > 0 ? (
        <>
          {data?.data?.items?.map((cart) => (
            <div
              className={`cart-product-main ${
                cart?.status !== 'In stock' ? 'cart-instock' : ''
              }`}
              key={Math.floor(Math.random() * 100000)}
            >
              {cart?.status !== 'In stock' && (
                <div className='pv-product-inner'></div>
              )}
              {cart && cart?.selling_price !== cart?.old_selling_price && (
                <div className='pv-product-oldselling'>
                  The selling prices have changed!
                </div>
              )}
              <div className='cart-product-image'>
                {cart?.Image ? (
                  <Link
                    href={`/product/${spaceToDash(
                      cart?.productName
                    )}/${encryptId(cart?.ProductGuid)}?sp_id=${encryptId(
                      cart?.sellerProductID
                    )}&s_id=${encryptId(cart?.sizeId)}`}
                  >
                    <Image
                      className='product-img-add-to-cart'
                      src={
                        cart?.Image &&
                        encodeURI(
                          `${reactImageUrl}${_productImg_}${cart?.Image}`
                        )
                      }
                      alt={cart?.productName}
                      width={180}
                      height={210}
                    />
                  </Link>
                ) : (
                  <Image
                    className='product-img-add-to-cart'
                    src='https://placehold.co/300x300.png'
                    alt={cart?.productName ?? 'image'}
                    width={180}
                    height={210}
                  />
                )}
              </div>
              <div className='cart-product-details'>
                <p className='cart-product-title'>
                  <Link
                    href={`/product/${spaceToDash(
                      cart?.productName
                    )}/${encryptId(cart?.ProductGuid)}?sp_id=${encryptId(
                      cart?.sellerProductID
                    )}&s_id=${encryptId(cart?.sizeId)}`}
                  >
                    {cart?.productName}
                  </Link>
                </p>
                {cart?.coupon_status === 'success' && (
                  <span className='pv-coupon-applytext'>Coupon Applied %</span>
                )}
                {(cart?.size || cart?.color) && (
                  <div className='cart-size'>
                    {cart?.size && (
                      <span>
                        Size:<b>{cart?.size}</b>
                      </span>
                    )}
                    {cart?.color && (
                      <span>
                        Color:<b>{cart?.color}</b>
                      </span>
                    )}
                  </div>
                )}
                {cart?.sellerName && (
                  <span className='cart-seller-name'>
                    Seller:
                    <p className='seller-name-title'>{cart?.sellerName}</p>
                  </span>
                )}
                {cart?.status === 'In stock' && (
                  <div className='product_pricong_offer_deliverychrg'>
                    <span className='total_pricing_product'>
                      {currencyIcon}
                      {cart?.itemPrice?.selling_price}
                    </span>
                    <span className='actual_pricing_product_mrp'>
                      {currencyIcon}
                      {cart?.itemPrice?.mrp}
                    </span>
                    <span className='actual_pricing_product_dis'>
                      {cart?.itemPrice?.discount}% OFF
                    </span>
                  </div>
                )}

                <div className='cart-count-btn'>
                  {cart?.status === 'In stock' ? (
                    <div className='counter-btn-cart'>
                      <button
                        disabled={cart?.qty === 1}
                        onClick={() => {
                          handleQty('minus', cart)
                        }}
                        type='button'
                      >
                        -
                      </button>
                      <input
                        type='text'
                        className='pv-counter-inp'
                        name=''
                        id=''
                        disabled={loading}
                        value={cart?.qty}
                        onChange={(e) => {
                          const inputValue = e?.target?.value
                          const isValidInput = /^[1-9]\d*$/.test(inputValue)
                          if (isValidInput || !inputValue) {
                            handleQty('input', {
                              ...cart,
                              qty: e?.target?.value
                            })
                          }
                        }}
                      />
                      <button
                        onClick={() => {
                          handleQty('plus', cart)
                        }}
                        type='button'
                      >
                        +
                      </button>
                    </div>
                  ) : (
                    <div className='badge-danger'>Out of stock</div>
                  )}
                  <div
                    className='delete-btn'
                    onClick={() => {
                      Swal.fire({
                        title: 'Delete Item from Cart',
                        text: `Are you sure you want to remove ${cart?.productName} from your cart?`,
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: _SwalDelete?.confirmButtonColor,
                        cancelButtonColor: _SwalDelete?.cancelButtonColor,
                        confirmButtonText: 'Yes, Remove It',
                        cancelButtonText: 'Cancel'
                      }).then((result) => {
                        if (result.isConfirmed) {
                          handleDelete(cart)
                        }
                      })
                    }}
                  >
                    <span className='remove_items_cart_sn'>REMOVE</span>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </>
      ) : (
        ''
      )}
    </>
  )
}

export default AddToCartProduct
