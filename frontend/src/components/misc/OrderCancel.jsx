import { Form, Formik } from 'formik'
import React, { useEffect, useState } from 'react'
import AccordionCheckout from '../AccordionCheckout'
import ReasonForCancel from '../ManageCancel/ReasonForCancel'
import { useImmer } from 'use-immer'
import * as Yup from 'yup'
import axiosProvider from '../../lib/AxiosProvider'
import ProductList from '../ProductList'
import BankInfo from '../ManageOrderreturn/BankInfo'
import { _exception } from '../../lib/exceptionMessage'
import IpRadio from '../base/IpRadio'
import { _orderStatus_, showToast } from '../../lib/GetBaseUrl'
import { useRouter } from 'next/navigation'

const OrderCancel = ({
  setToast,
  setLoading,
  toast,
  orderItemData,
  setInitialValues,
  initialValues,
  activeAccordion,
  setActiveAccordion
}) => {
  const router = useRouter()
  const [allState, setAllState] = useImmer({
    issueTypes: []
  })
  const validationSchema = Yup.object().shape({
    actionID: Yup.string().required('Select Action')
  })

  const getReturnAction = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'ManageOrder/GetReturnActions?PageIndex=0&PageSize=0'
      })
      setLoading(false)
      if (response?.status === 200) {
        const findCancel = response?.data?.data?.find(
          (item) => item?.returnAction === _orderStatus_?.cancelled
        )
        setAllState((draft) => {
          draft.returnAction = [findCancel]
        })
        setInitialValues({
          ...initialValues,
          actionID: findCancel?.id
        })
        const IssueTypeRes = await axiosProvider({
          method: 'GET',
          endpoint: `IssueType/byActionId?actionId=${findCancel?.id}&pageIndex=0&pageSize=0`
        })
        if (IssueTypeRes?.data?.code === 200) {
          setAllState((draft) => {
            draft.issueTypes = IssueTypeRes?.data?.data
          })
        }
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

  useEffect(() => {
    getReturnAction()
  }, [])

  const onSubmit = async (values) => {
    const CancelData = { ...values, orderItem: {} }
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'POST',
        endpoint: 'ManageOrder/OrderCancel',
        data: CancelData
      })
      setLoading(false)
      if (response?.data?.code === 200) {
        showToast(toast, setToast, response)
        setTimeout(() => {
          router?.push('/user/orders')
        }, 1000)
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }
  return (
    <>
      <div className='check-orderlist pv-order-cancel-main'>
        <Formik
          enableReinitialize={true}
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={onSubmit}
        >
          {({ values, setFieldValue, errors, setFieldError }) => (
            <Form>
              <AccordionCheckout
                accordiontitle={`EASY CANCELLATION`}
                isActive={activeAccordion === 0}
                activeAccordion={activeAccordion}
                index={0}
                Name={
                  values?.reason
                    ? values?.reason
                    : values?.issue
                    ? values?.issue
                    : values?.returnAction
                }
                toggleAccordion={() => setActiveAccordion(0)}
                accordioncontent={
                  <ReasonForCancel
                    values={values}
                    setFieldValue={setFieldValue}
                    toast={toast}
                    setToast={setToast}
                    allState={allState}
                    setAllState={setAllState}
                    setActiveAccordion={setActiveAccordion}
                    errors={errors}
                    setFieldError={setFieldError}
                  />
                }
              />
              {/* {values?.paymentMode !== 'cod' && ( */}
              <AccordionCheckout
                accordiontitle={'REFUND MODES'}
                isActive={activeAccordion === 1}
                activeAccordion={activeAccordion}
                toggleAccordion={() => setActiveAccordion(2)}
                Name={'1 item'}
                index={2}
                accordioncontent={
                  <div>
                    <div className='order-replacement'>
                      What do you want to Refund ?
                    </div>
                    <div>
                      {values?.paymentMode !== 'cod' && (
                        <>
                          <IpRadio
                            labelText={'Payment Mode'}
                            id={'online'}
                            name={'payment'}
                            value='online'
                            checked={
                              values?.returnReplaceSec === 'online'
                                ? true
                                : false
                            }
                            onChange={() => {
                              setFieldValue('returnReplaceSec', 'online')
                            }}
                          />
                          {values?.paymentMode === 'online' && (
                            <BankInfo
                              values={values}
                              setFieldValue={setFieldValue}
                              errors={errors}
                              setFieldError={setFieldError}
                            />
                          )}
                        </>
                      )}
                      <IpRadio
                        labelText={
                          'Refund Not Applicable for COD Cancellations'
                        }
                        id={'cod'}
                        name={'payment'}
                        value='cod'
                        checked={
                          values?.returnReplaceSec === 'cod' ? true : false
                        }
                        onChange={() =>
                          setFieldValue('returnReplaceSec', 'cod')
                        }
                      />
                      {values?.returnReplaceSec === 'cod' && (
                        <>
                          <p>
                            As your order was paid using Cash on Delivery, there
                            is no refund associated.
                          </p>
                          <div className='transaction-section'>
                            <div>
                              <button
                                id='orderCancel'
                                className='m-btn checkout_btn'
                                type='submit'
                              >
                                Confirm Cancel
                              </button>
                            </div>
                          </div>
                        </>
                      )}
                    </div>
                  </div>
                }
              />
              {/* )} */}
            </Form>
          )}
        </Formik>
      </div>
      <div className='check-orderreturn'>
        <ProductList
          product={orderItemData}
          isView={true}
          wishlistShow={false}
        />
      </div>
    </>
  )
}

export default OrderCancel
