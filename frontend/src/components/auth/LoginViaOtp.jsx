import InputComponent from '../base/InputComponent'
import { ErrorMessage, Form, Formik } from 'formik'
import { _phoneNumberRegex_, _integerRegex_ } from '../../lib/Regex'
import * as Yup from 'yup'
import TextError from '../base/TextError'
import nookies from 'nookies'
import axios from 'axios'
import {
  getBaseUrl,
  getDeviceId,
  getRefreshToken,
  getSessionId,
  showToast
} from '../../lib/GetBaseUrl'
import { useRef, useState } from 'react'
import { _positiveInteger_, _otpRegex_ } from '../../lib/Regex'
import Loader from '../Loader'
import { useImmer } from 'use-immer'
import { useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { _exception } from '../../lib/exceptionMessage'
import axiosProvider from '../../lib/AxiosProvider'
import { setCartCount } from '@/redux/features/cartSlice'
import { addressData } from '@/redux/features/addressSlice'
import { addUser } from '@/redux/features/userSlice'

const LoginViaOtp = ({ toast, setToast, toggleForm, onClose }) => {
  const baseUrl = getBaseUrl()
  const dispatch = useDispatch()
  const [loading, setLoading] = useState(false)
  const [mobile, setMobile] = useImmer(false)
  const mobileInputRef = useRef(null)
  const mySessionId = getSessionId()
  const initialValues = {
    mobileNo: '',
    otp: ''
  }
  const accessToken = getRefreshToken()
  const [cartId, setCartId] = useState()

  useEffect(() => {
    const handleFocus = () => {
      if (!mobile)
        if (mobileInputRef.current) {
          mobileInputRef.current.focus()
        }
    }

    window.addEventListener('load', handleFocus)

    return () => {
      window.removeEventListener('load', handleFocus)
    }
  }, [mobile])

  const validationSchema = Yup.object().shape({
    mobileNo: Yup.string()
      .matches(_phoneNumberRegex_, 'Please enter valid mobile number')
      .required('Mobile number is required'),
    otp: Yup.string().when('showOtpForm', {
      is: (value) => value,
      then: () => Yup.string().required('Otp is required'),
      otherwise: () => Yup.string().notRequired()
    })
  })

  const getCartCount = async (userId) => {
    try {
      if (userId?.length) {
        const res = await axiosProvider({
          method: 'GET',
          endpoint: `Cart/bysessionId?sessionId=${userId}`
        })
        dispatch(setCartCount(res?.data?.pagination?.recordCount))
      }
    } catch (error) {
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const onSubmit = async (
    values,
    setFieldValue,
    endpoint,
    method = 'put',
    resetForm
  ) => {
    const config = {
      headers: {
        'Content-Type': 'application/json'
      }
    }
    try {
      setLoading(true)
      const response = await axios[method](
        `${baseUrl}${endpoint}`,
        values,
        config
      )
      setLoading(false)

      if (response?.data?.code === 200) {
        if (endpoint?.includes('GenerateMobileOtp')) {
          setMobile(true)
          setFieldValue('showOtpForm', true)
        } else {
          showToast(toast, setToast, response)
          getCartCount(response?.data?.currentUser?.userId)
          localStorage.removeItem('hk-compare-data')
          let userId = response?.data?.currentUser?.userId
          nookies.set(null, 'userToken', response?.data?.tokens?.accessToken, {
            path: '/'
          })
          nookies.set(
            null,
            'refreshToken',
            response?.data?.tokens?.refreshToken,
            { path: '/' }
          )
          nookies.set(null, 'userId', response?.data?.currentUser?.userId, {
            path: '/'
          })
          nookies.set(null, 'sessionId', response?.data?.currentUser?.userId, {
            path: '/'
          })

          if (mySessionId) {
            if (mySessionId !== userId) {
              try {
                const responseSession = await axiosProvider({
                  method: 'PUT',
                  endpoint: 'Cart/UpdateSession',
                  queryString: `?UserId=${userId}&SessionId=${mySessionId}&CartId=${
                    cartId ? cartId : 0
                  }`
                })
                if (responseSession?.status === 200) {
                  nookies.set(null, 'sessionId', userId, { path: '/' })
                }
              } catch (error) {
                showToast(toast, setToast, {
                  data: { code: 204, message: _exception?.message }
                })
              }
            }
          }
          dispatch(
            addUser({
              user: response?.data?.currentUser,
              userToken: response?.data?.tokens?.accessToken,
              refreshToken: response?.data?.tokens?.refreshToken,
              deviceId: getDeviceId()
            })
          )
          resetForm({ values: '' })
          try {
            const responseAddress = await axiosProvider({
              method: 'GET',
              endpoint: 'Address/byUserId',
              queryString: `?userId=${userId}`
            })
            if (responseAddress?.data?.code === 200) {
              dispatch(addressData(responseAddress?.data?.data))
            }
          } catch (error) {
            showToast(toast, setToast, {
              data: { code: 204, message: _exception?.message }
            })
          }

          setFieldValue('setOtpForm', false)
          onClose()
        }
      }
      showToast(toast, setToast, response)
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  // const handleChangeButton = () => {
  //   document.getElementById('mobileNo').focus()
  //   setMobile(false)
  // }

  useEffect(() => {
    if (accessToken !== null) {
      onClose()
    }
  }, [accessToken])

  useEffect(() => {
    if (typeof window !== 'undefined' && window.localStorage) {
      let cartId = localStorage.getItem('cartId')
      setCartId(cartId)
    }
  }, [])

  return (
    <>
      {loading && <Loader />}
      <div className='login-otp-main'>
        <h1 className='forgot-title'>Login Via Otp</h1>
        <button className='forgot-back-btn' onClick={() => toggleForm('login')}>
          <i className='m-icon back-icon'></i>
          Go Back
        </button>
      </div>

      <Formik
        validateOnChange={false}
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={(values, { setFieldValue, resetForm }) => {
          let endpoint = values?.showOtpForm
            ? 'Account/Customer/LoginViaOtp'
            : 'Account/Customer/GenerateMobileOtp'
          let method = values?.showOtpForm ? 'post' : 'put'
          let data = values?.showOtpForm
            ? {
                ...values,
                deviceId: getDeviceId()
              }
            : values
          onSubmit(data, setFieldValue, endpoint, method, resetForm)
        }}
      >
        {({ values, setFieldValue, touched }) => (
          <Form>
            <div className='mp-mobile-otp'>
              {mobile !== false && (
                <button
                  onClick={() => {
                    document.getElementById('mobileNo').focus()
                    setMobile(false)
                    setFieldValue('showOtpForm', false)
                    setFieldValue('otp', '')
                  }}
                  id='changeButton'
                  type='button'
                >
                  Change
                </button>
              )}
              <p className='otp-mobile-num'>Mobile Number for OTP</p>
              <InputComponent
                disabled={mobile ? true : false}
                ref={mobileInputRef}
                labelClass={'sign-com-label'}
                MainHeadClass={'forgot-mobile'}
                type='number'
                name='mobileNo'
                id='mobileNo'
                onChange={(e) => {
                  const inputValue = e?.target?.value
                  const fieldName = e?.target?.name
                  const isValid = _phoneNumberRegex_.test(inputValue)
                  if (!inputValue || isValid) {
                    setFieldValue(fieldName, inputValue)
                  }
                }}
                value={values.mobileNo}
              />
              <ErrorMessage name='mobile' component={TextError} />
            </div>
            {mobile && (
              <div className='mp-mobile-otp'>
                <p className='otp-mobile-num'>Enter the Otp.</p>
                <InputComponent
                  labelClass={'sign-com-label'}
                  MainHeadClass={'forgot-mobile'}
                  type={'text'}
                  name={'otp'}
                  id={'otp'}
                  onChange={(e) => {
                    const inputValue = e?.target?.value
                    const fieldName = e?.target?.name
                    const isValid = _positiveInteger_.test(inputValue)
                    if (!inputValue || isValid) {
                      setFieldValue(fieldName, inputValue)
                    }
                  }}
                  value={values.otp}
                  maxlength={'6'}
                />
                {ErrorMessage.otp && touched.otp && (
                  <ErrorMessage name='otp' component={TextError} />
                )}
              </div>
            )}
            <div className='send-rest-forgot'>
              <button type='submit' className='get-otp-login'>
                {mobile ? 'Verify OTP' : 'Generate OTP'}
              </button>
            </div>
          </Form>
        )}
      </Formik>
    </>
  )
}

export default LoginViaOtp
