'use client'
import { usePathname, useRouter } from 'next/navigation'
import React, { useState } from 'react'
import { useSelector } from 'react-redux'
import LoginSignup from './LoginSignup'
import {
  convertToNumber,
  currencyIcon,
  formatNumberWithCommas,
  getUserId,
  showToast
} from '../lib/GetBaseUrl'
import { checkTokenAuthentication } from '../lib/checkTokenAuthentication'
import { _toaster } from '../lib/tosterMessage'
import ModalComponent from './base/ModalComponent'
import OutofStock from './outOfStockModal/OutofStock'

const PriceDetails = ({
  cart,
  toast,
  setToast,
  cartCalculation,
  setModalShow,
  modalShow
}) => {
  const router = useRouter()
  const path = usePathname()
  const { user } = useSelector((state) => state?.user)
  const outOfCart = cart?.items?.filter((item) => item?.status !== 'In stock')
  const userIdCookie = getUserId()

  const handleCheckLogin = () => {
    if (!user?.userId) {
      if (userIdCookie) {
        checkTokenAuthentication(toast, setToast)
      } else {
        setModalShow({ show: true, type: 'login' })
      }
    } else {
      if (outOfCart?.length === 0) {
        router?.push('/user/checkout')
      } else {
        // showToast(toast, setToast, {
        //   data: {
        //     code: 204,
        //     message: _toaster?.OutOfstockProduct
        //   }
        // })
        setModalShow({ show: true, type: 'outOfStockProduct' })
      }
    }
  }

  const onClose = () => {
    // setModal(false)
    setModalShow({ show: false, type: 'login' })
  }

  return (
    <>
      <div>
        {modalShow?.show && modalShow?.type === 'login' && (
          <LoginSignup
            toast={toast}
            setToast={setToast}
            onClose={onClose}
            modal={true}
          />
        )}
        <div className='price_details'>
          <div className='price_details_card_wrapper'>
            <h2 className='price_details_heading'>PRICE DETAILS</h2>
            <div className='price_details_card'>
              {Boolean(cart?.CartAmount?.total_mrp) && (
                <div className='price_details_wrapper'>
                  <p className='price_details_name'>
                    MRP ({cart?.items?.length} items)
                  </p>
                  <p className='price_details_price'>
                    {currencyIcon}
                    {formatNumberWithCommas(cart?.CartAmount?.total_mrp)}
                  </p>
                </div>
              )}
              {Boolean(cart?.CartAmount?.total_discount) && (
                <div className='price_details_wrapper'>
                  <p className='price_details_name '>Discount</p>
                  <p className='price_details_price pv-pricing-discount'>
                    - {currencyIcon} {cart?.CartAmount?.total_discount}
                  </p>
                </div>
              )}
              {Boolean(
                convertToNumber(cart?.CartAmount?.total_selling_price)
              ) && (
                <div className='price_details_wrapper'>
                  <p className='price_details_name'>Selling Price</p>
                  <p className='price_details_price'>
                    {currencyIcon}
                    {formatNumberWithCommas(
                      cart?.CartAmount?.total_selling_price
                    )}
                  </p>
                </div>
              )}
              {Boolean(
                convertToNumber(cart?.CartAmount?.total_extradiscount)
              ) && (
                <div className='price_details_wrapper'>
                  <p className='price_details_name '>Extra Discount</p>
                  <p className='price_details_price pv-pricing-discount'>
                    - {currencyIcon}
                    {formatNumberWithCommas(
                      Number(cart?.CartAmount?.total_extradiscount)
                    )}
                  </p>
                </div>
              )}
              {Boolean(
                convertToNumber(cart?.CartAmount?.total_extra_charges)
              ) && (
                <div className='price_details_wrapper'>
                  <p className='price_details_name'>Extra Charges</p>
                  <p className='price_details_price red'>
                    {currencyIcon} {cart?.CartAmount?.total_extra_charges}
                  </p>
                </div>
              )}
              <div className='price_details_wrapper'>
                <p className='price_details_name'>Delivery Charges</p>
                <p className='price_details_price'>
                  {convertToNumber(cart?.CartAmount?.shipping_charges) ===
                  cart?.CartAmount?.actual_shipping_charges ? (
                    <div
                      className={`price_delivery_charges ${
                        cart?.CartAmount?.shipping_charges === '0' &&
                        'price_free'
                      }`}
                    >
                      {cart?.CartAmount?.shipping_charges === '0' ? (
                        <>Free </>
                      ) : (
                        <>
                          {currencyIcon}
                          {cart?.CartAmount?.actual_shipping_charges}
                        </>
                      )}
                    </div>
                  ) : (
                    <span className='price_free'>
                      {cart?.CartAmount?.shipping_charges === '0' ? (
                        <>Free </>
                      ) : (
                        <>{cart?.CartAmount?.shipping_charges} </>
                      )}
                      <span
                        className={`price_delivery_charges ${
                          cart?.CartAmount?.shipping_charges !==
                            cart?.CartAmount?.actual_shipping_charges &&
                          'active'
                        }`}
                      >
                        {currencyIcon}{' '}
                        {cart?.CartAmount?.actual_shipping_charges}
                      </span>
                    </span>
                  )}
                </p>
              </div>
  
              {Boolean(cart?.CartAmount?.cod_charges) && (
                <div className='price_details_wrapper'>
                  <p className='price_details_name'>COD Charges</p>
                  <p className='price_details_price red'>
                    {currencyIcon} {cart?.CartAmount?.cod_charges}
                  </p>
                </div>
              )}
            </div>

            <div className='price_details_card_total'>
              {Boolean(cart?.CartAmount?.total_inclusivegst) && (
                <div className='price_details_wrapper'>
                  <p className='price_details_GST'>Inclusive GST</p>
                  <p className='price_details_price'>
                    {currencyIcon}
                    {formatNumberWithCommas(
                      convertToNumber(cart?.CartAmount?.total_inclusivegst)
                    )}
                  </p>
                </div>
              )}
              <div className='price_details_wrapper'>
                <p className='price_details_name'>Total Amount</p>
                <p className='price_details_price'>
                  {currencyIcon}
                  {formatNumberWithCommas(
                    convertToNumber(cart?.CartAmount?.paid_amount)
                  )}
                </p>
              </div>
            </div>
          </div>
          <p className='price_save_note'>
            You will save {currencyIcon} {cart?.CartAmount?.total_SaveAmt} on
            this order
          </p>

          {path === '/cart' && (
            <div>
              <button
                className='price_place-order'
                onClick={() => handleCheckLogin()}
              >
                CHECKOUT
              </button>
            </div>
          )}
        </div>
      </div>
      {modalShow?.show && modalShow?.type === 'outOfStockProduct' && (
        <ModalComponent
          isOpen={true}
          onClose={() => {
            setModalShow({ show: !modalShow?.show, type: 'outOfStockProduct' })
          }}
          modalSize={'modal-md'}
          headingText={'Few items are unavailable for checkout'}
          headClass={'HeaderText'}
          bodyClass={'modal-body'}
        >
          <OutofStock
            stockItems={outOfCart}
            toast={toast}
            setToast={setToast}
            cartCalculation={cartCalculation}
            modalShow={modalShow}
            setModalShow={setModalShow}
          />
        </ModalComponent>
      )}
    </>
  )
}
export default PriceDetails
