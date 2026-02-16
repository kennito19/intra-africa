'use client'
import React, { useEffect, useState } from 'react'
import ModalComponent from '../base/ModalComponent'
import InputComponent from '../base/InputComponent'
import { useSelector } from 'react-redux'
import { Form, Formik } from 'formik'
import { currencyIcon, getUserId } from '../../lib/GetBaseUrl'
import * as Yup from 'yup'
import LoginSignup from '../LoginSignup'
import { checkTokenAuthentication } from '../../lib/checkTokenAuthentication'

const OfferCoupan = ({
  data,
  modalShow,
  setModalShow,
  cartCalculation,
  toast,
  setToast,
  values
}) => {
  const { user } = useSelector((state) => state?.user)
  const { cart } = useSelector((state) => state?.cart)
  const userIdCookie = getUserId()

  const handleClick = () => {
    if (!user?.userId) {
      if (userIdCookie) {
        checkTokenAuthentication(toast, setToast)
      } else {
        setModalShow({ show: true, type: 'login' })
      }
    } else {
      setModalShow({ show: true, type: 'coupon' })
    }
  }

  const handleClose = () => {
    setModalShow({ show: false, type: 'login' })
    if (user?.userId) {
      cartCalculation(false, false, false, true)
      setModalShow({ ...modalShow, type: 'coupon' })
    }
  }

  const handleDelete = () => {
    cartCalculation(false, 'remove', false, false, values?.addressVal)
  }

  const offerApply = (code) => {
    cartCalculation(false, true, code, false, values?.addressVal)
  }

  const onSubmit = (values) => {
    cartCalculation(false, true, values?.couponCode, false, values?.addressVal)
  }

  return (
    <>
      {modalShow?.show && modalShow?.type === 'login' && (
        <LoginSignup onClose={handleClose} toast={toast} setToast={setToast} />
      )}
      {cart?.coupon_code && cart?.coupon_code !== null ? (
        <div className='coupan-main'>
          <h1 className='offer-title'>Offers</h1>
          <div className='offer'>
            <div className='apply-code'>
              <p>{cart?.coupon_code}</p>
            </div>
            <div className='offer-btn'>
              <button className='apply-btn' onClick={handleDelete}>
                X
              </button>
            </div>
          </div>
        </div>
      ) : (
        <div className='coupan-main'>
          <h1 className='offer-title'>Offers</h1>
          <div className='offer'>
            <div className='apply-code'>
              <i className='offer-icon'></i>
              <p>Apply Coupons</p>
            </div>
            <div className='offer-btn'>
              <button className='apply-btn' onClick={handleClick}>
                Apply
              </button>
            </div>
          </div>
        </div>
      )}
      {modalShow?.type === 'coupon' && modalShow?.show && (
        <ModalComponent
          isOpen={true}
          onClose={() => setModalShow({ show: false, type: 'coupon' })}
          modalSize={'modal-sm'}
          headingText={'available offers'}
          headClass={'HeaderText'}
          bodyClass={'modal-body'}
        >
          <>
            <Formik
              initialValues={{
                couponCode: ''
              }}
              validationSchema={Yup.object().shape({
                couponCode: Yup.string().required('Please enter Coupon code')
              })}
              onSubmit={onSubmit}
            >
              {({ values, setFieldValue, errors, touched }) => (
                <Form>
                  <div className='pos_rel'>
                    <InputComponent
                      id='couponCode'
                      name='couponCode'
                      labelClass={'Dnone'}
                      inputClass={'custom_padding'}
                      placeholder={'Enter Coupon Code'}
                      value={values?.couponCode}
                      onChange={(e) => {
                        setFieldValue('couponCode', e.target.value)
                      }}
                    />
                    <div className='btn_apply_offer'>
                      <button className='m-btn' type='submit'>
                        Apply
                      </button>
                    </div>
                  </div>
                </Form>
              )}
            </Formik>

            <div>
              {data && data?.data?.data?.length > 0 ? (
                <>
                  <p className='best_offer_text'>More Coupons</p>
                  {data?.data?.data?.map((item) => (
                    <div
                      className='parent_offer disable_offer'
                      key={item?.id ?? Math.floor(Math.random() * 100000)}
                    >
                      <div
                        className={`offer_apply ${
                          cart?.coupon_code === item?.code ? 'active' : ''
                        }`}
                      >
                        <div>
                          <p className='offer_name'>{item?.name}</p>
                          <p className='offer_detail'>
                            Save upto
                            {currencyIcon}
                            {item?.maximumDiscountAmount} with this code
                          </p>
                        </div>
                        <button
                          className={`offer_code ${
                            cart?.coupon_code === item?.code ? 'active' : ''
                          }`}
                          onClick={() => offerApply(item?.code)}
                        >
                          {item?.code}
                        </button>
                      </div>
                    </div>
                  ))}
                </>
              ) : (
                <p className='best_offer_text'>No Offers</p>
              )}
            </div>
          </>
        </ModalComponent>
      )}
    </>
  )
}

export default OfferCoupan
