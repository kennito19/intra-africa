import React, { useEffect, useState } from 'react'
import AccordionCheckout from '../AccordionCheckout'
import { Form, FormikProvider, useFormik } from 'formik'
import {
  _alphabetRegex_,
  _integerRegex_,
  _phoneNumberRegex_
} from '../../lib/Regex'
import axiosProvider from '../../lib/AxiosProvider'
import AddressModal from '../AddressModal'
import { _exception } from '../../lib/exceptionMessage'
import { _orderStatus_, showToast } from '../../lib/GetBaseUrl'
import IpRadio from '../base/IpRadio'
import ReasonForReturn from '../ManageOrderreturn/ReasonForReturn'
import AddressSection from '../AddressSection'
import BankInfo from '../ManageOrderreturn/BankInfo'
import * as Yup from 'yup'
import { useDispatch, useSelector } from 'react-redux'
import { useImmer } from 'use-immer'
import ProductList from '../ProductList'
import { _toaster } from '../../lib/tosterMessage'
import { addressData } from '@/redux/features/addressSlice'
import { useParams, useRouter } from 'next/navigation'

const OrderActionDetails = ({
  setToast,
  setLoading,
  toast,
  setInitialValues,
  initialValues,
  orderItemData,
  activeAccordion,
  setActiveAccordion
}) => {
  const dispatch = useDispatch()
  const router = useRouter()
  const params = useParams()
  const [modalShow, setModalShow] = useState({ show: false, data: null })

  const [allState, setAllState] = useImmer({
    returnAction: [],
    issueTypes: []
  })

  const { user } = useSelector((state) => state?.user)
  const validationSchema = Yup.object().shape({
    actionID: Yup.string().required('Select Action'),
    bankIFSCCode: Yup.string()
      .matches(/^([A-Z]{4}\d{7})$/, 'Invalid IFSC Code')
      .required('Please Enter IFSC Code'),
    bankName: Yup.string()
      .matches(
        _alphabetRegex_,
        'Numeric value and special charecters are not allowed'
      )
      .required('Please Enter Bank name'),
    bankAccountNo: Yup.string()
      .matches(_integerRegex_, 'Invalid bank account number')
      .min(6, 'Bank Account number must be at least 6 characters long')
      .max(18, 'Bank Account number must not exceed 18 characters')
      .required('Bank Account number is required'),
    ConfirmbankAccountNo: Yup.string()
      .oneOf(
        [Yup.ref('bankAccountNo'), null],
        'Confirm Account number must match'
      )
      .required('Confirm Account number is required'),
    accountHolderName: Yup.string()
      .matches(
        _alphabetRegex_,
        'Numeric value and special charecters are not allowed'
      )
      .required("Please Enter Account Holder's name"),
    phoneNumber: Yup.string()
      .matches(_phoneNumberRegex_, 'Please enter valid mobile number')
      .typeError('Please Enter valid mobile number')
      .required('Mobile number is required'),
    bankBranch: Yup.string()
      .matches(
        /^[A-Za-z\s-]+$/,
        'Only alphabetic characters, spaces, and hyphens are allowed'
      )
      .required('Please Enter Branch name'),
    accountType: Yup.string()
      .notOneOf([''], 'Select Account type')
      .required('Account type is required'),
    comment: Yup.string()
      .trim()
      .required('This field is required.')
      .test(
        'is-not-empty',
        'This field is required.',
        (value) => value.trim() !== ''
      )
  })

  const fetchAddress = async (id, valueData) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'Address/byUserId',
        queryString: `?userId=${user?.userId}`
      })
      setLoading(false)
      if (response?.status === 200) {
        dispatch(addressData(response?.data?.data))
        const addressValData =
          id && response?.data?.data?.find((item) => item?.id === id)
        setInitialValues({
          ...valueData,
          userName: addressValData?.fullName,
          userPhoneNo: addressValData?.mobileNo,
          userGSTNo: addressValData?.gstNo ? addressValData?.gstNo : '',
          addressLine1: addressValData?.addressLine1,
          addressLine2: addressValData?.addressLine2,
          landmark: addressValData?.landmark,
          pincode: addressValData?.pincode,
          city: addressValData?.cityId,
          state: addressValData?.stateId,
          country: addressValData?.countryId
        })
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const getReturnAction = async () => {
    try {
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'ManageOrder/GetReturnActions?PageIndex=0&PageSize=0'
      })
      if (response?.status === 200) {
        const cancelFilter = response?.data?.data?.filter(
          (item) => item?.returnAction !== _orderStatus_?.cancelled
        )

        setAllState((draft) => {
          draft.returnAction = cancelFilter
        })

        const IssueTypeRes = await axiosProvider({
          method: 'GET',
          endpoint: `IssueType/byActionId?actionId=${response?.data?.data?.[0]?.id}&pageIndex=0&pageSize=0`
        })
        if (IssueTypeRes?.data?.code === 200) {
          setAllState((draft) => {
            draft.issueTypes = IssueTypeRes?.data?.data
          })
        }
      }
    } catch (error) {
      showToast(toast, setToast, {
        data: {
          message: _exception?.message,
          code: 204
        }
      })
    }
  }

  const fetchPinCodeAndCheck = async (addressData) => {
    if (addressData) {
      try {
        setLoading(true)
        const response = await axiosProvider({
          method: 'GET',
          endpoint: `Delivery/byPincode?pincode=${addressData?.pincode}`
        })
        setLoading(false)
        if (response?.status === 200) {
          if (response?.data?.data?.pincode === Number(addressData?.pincode)) {
            formik?.setFieldValue('addressVal', addressData)
            setActiveAccordion(2)
          } else {
            showToast(toast, setToast, {
              data: {
                code: 204,
                message: _toaster?.pinCodeError
              }
            })
          }
        }
      } catch {
        showToast(toast, setToast, {
          data: {
            code: 204,
            message: _exception?.message
          }
        })
      }
    } else {
      setActiveAccordion(1)
    }
  }

  const onSubmit = async (values) => {
    let returnValue = {
      ...values,
      addressLine1: values?.addressVal?.addressLine1,
      addressLine2: values?.addressVal?.addressLine2,
      landmark: values?.addressVal?.landmark,
      pincode: values?.addressVal?.pincode,
      city: values?.addressVal?.cityName,
      state: values?.addressVal?.stateName,
      country: values?.addressVal?.countryName,
      refundType: values?.paymentMode === 'cod' ? 'new bank' : 'existing bank'
    }

    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'POST',
        endpoint: 'ManageOrder/OrderReturn',
        data: returnValue
      })
      setLoading(false)
      if (response?.data?.code === 200) {
        showToast(toast, setToast, response)
        setTimeout(() => {
          router?.push(`/user/orders/${params?.orderId}`)
        }, 1000)
      }
    } catch (error) {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const formik = useFormik({
    initialValues: initialValues,
    enableReinitialize: true,
    validationSchema,
    onSubmit: onSubmit
  })

  useEffect(() => {
    getReturnAction()
  }, [])

  useEffect(() => {
    const handleKeyPress = (e) => {
      if (e.key === 'Enter' && activeAccordion !== 2 && activeAccordion !== 1) {
        e.preventDefault()
        document.getElementById('reasonReturn')?.click()
      } else if (e.key === 'Enter' && activeAccordion === 1) {
        if (
          !modalShow?.show &&
          typeof formik.values?.addressVal === 'object' &&
          Object.keys(formik?.values?.addressVal)?.length > 0
        ) {
          e.preventDefault()
          document.getElementById('deliverHereButton').click()
        } else {
          if (modalShow?.show) {
            document.getElementById('onSubmitAddress')
          } else {
            showToast(toast, setToast, {
              data: { code: 204, message: _toaster?.addressError }
            })
          }
        }
      } else if (e.key === 'Enter' && activeAccordion === 2) {
        e.preventDefault()

        document.getElementById('orderReturn').click()
      }
    }

    document.addEventListener('keydown', handleKeyPress)
    return () => {
      document.removeEventListener('keydown', handleKeyPress)
    }
  }, [activeAccordion, setActiveAccordion, modalShow])

  return (
    <>
      <div className='check-orderlist pv-order-cancel-main'>
        <FormikProvider value={formik}>
          {/* <form onSubmit={formik.handleSubmit} id='weddingStoryForm'></form> */}
          <Form>
            <AccordionCheckout
              accordiontitle={`REASON FOR RETURN`}
              isActive={activeAccordion === 0}
              activeAccordion={activeAccordion}
              index={0}
              Name={
                formik.values?.reason
                  ? formik.values?.reason
                  : formik.values?.issue
                  ? formik.values?.issue
                  : formik.values?.returnAction
              }
              toggleAccordion={() => setActiveAccordion(0)}
              accordioncontent={
                <ReasonForReturn
                  values={formik.values}
                  setFieldValue={formik.setFieldValue}
                  toast={toast}
                  setToast={setToast}
                  setModalShow={setModalShow}
                  modalShow={modalShow}
                  allState={allState}
                  setAllState={setAllState}
                  setActiveAccordion={setActiveAccordion}
                />
              }
            />
            <AccordionCheckout
              accordiontitle={'PICKUP ADDRESS'}
              isActive={activeAccordion === 1}
              activeAccordion={activeAccordion}
              toggleAccordion={() => setActiveAccordion(1)}
              Name={formik.values?.addressVal?.fullName ?? ''}
              index={1}
              Content={
                formik.values?.addressVal &&
                `${formik.values?.addressVal?.addressLine1}, ${formik.values?.addressVal?.addressLine2}, ${formik.values?.addressVal?.cityName}, ${formik.values?.addressVal?.stateName} - ${formik.values?.addressVal?.pincode}`
              }
              accordioncontent={
                <AddressSection
                  values={formik.values}
                  setFieldValue={formik.setFieldValue}
                  setModalShow={setModalShow}
                  modalShow={modalShow}
                  buttonText={
                    formik.values?.actionID === 1 ? 'Select' : 'Order Replace'
                  }
                  setActiveAccordion={setActiveAccordion}
                  toast={toast}
                  setToast={setToast}
                  setLoading={setLoading}
                />
              }
            />
            {formik.values?.actionID === 1 && (
              <AccordionCheckout
                accordiontitle={'RETURN ACTION'}
                isActive={activeAccordion === 2}
                activeAccordion={activeAccordion}
                toggleAccordion={() => setActiveAccordion(2)}
                Name={'1 item'}
                index={2}
                accordioncontent={
                  <div>
                    <div className='order-replacement'>
                      What do you want to return ?
                    </div>
                    <div>
                      <IpRadio
                        onChange={(e) =>
                          formik.setFieldValue(
                            'returnReplaceSec',
                            e?.target?.value
                          )
                        }
                        labelText={'Replacement'}
                        id={'Replacement'}
                        value='Replacement'
                        checked={
                          formik.values?.returnReplaceSec === 'Replacement'
                            ? true
                            : false
                        }
                      />
                      {formik.values?.returnReplaceSec === 'Replacement' && (
                        <BankInfo
                          values={formik.values}
                          setFieldValue={formik.setFieldValue}
                          errors={formik.errors}
                        />
                      )}
                    </div>
                  </div>
                }
              />
            )}
          </Form>
        </FormikProvider>

        {modalShow?.show && (
          <AddressModal
            stateValues={formik.values}
            setFieldValue={formik.setFieldValue}
            modalShow={modalShow}
            setModalShow={setModalShow}
            setLoading={setLoading}
            fetchAllAddress={fetchAddress}
            fetchPinCodeAndCheck={fetchPinCodeAndCheck}
          />
        )}
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

export default OrderActionDetails
