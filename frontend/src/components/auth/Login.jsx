import Link from 'next/link'
import InputComponent from '../base/InputComponent'
import PasswordStrengthCheck from '../base/PasswordStrengthCheck'
import { Form, Formik } from 'formik'
import {
  _passwordRegex_,
  _emailRegex_,
  _phoneNumberRegex_
} from '../../lib/Regex'
import * as Yup from 'yup'
import {
  getDeviceId,
  getRefreshToken,
  getSessionId,
  showToast
} from '../../lib/GetBaseUrl'
import { useDispatch } from 'react-redux'
import { addUser } from '@/redux/features/userSlice'
import nookies from 'nookies'
import { useEffect, useState } from 'react'
import axiosProvider from '../../lib/AxiosProvider'
import Loader from '../Loader'
import { addData } from '@/redux/features/categoryMenuSlice'
import { setCartCount } from '@/redux/features/cartSlice'
import { _exception } from '../../lib/exceptionMessage'
import { focusInput } from '../../lib/AllGlobalFunction'
import { addressData } from '@/redux/features/addressSlice'
import { _projectName_ } from '@/lib/ConfigVariables'

const Login = ({
  LoginOTPForm,
  isLogin,
  handleForgot,
  toggleForm,
  onClose,
  toast,
  setToast,
  modal
}) => {
  const [loading, setLoading] = useState(false)
  const dispatch = useDispatch()
  const deviceId = getDeviceId()
  const initialValues = {
    userName: '',
    password: '',
    deviceId: deviceId
  }
  const accessToken = getRefreshToken()
  const mySessionId = getSessionId()
  const [cartId, setCartId] = useState()

  const isPhoneNumber = (value) => /^\d+$/.test(value)

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

  const validationSchema = Yup.object().shape({
    userName: Yup.string()
      .test(
        'valid-login',
        'Please enter a valid email address or phone number',
        (value) => {
          return _emailRegex_.test(value) || _phoneNumberRegex_.test(value)
        }
      )
      .required('Email or phone number is required'),
    password: Yup.string()
      .required('Password field is required')
      .test('Password is required', (value) => !value.includes(' '))
  })

  const onSubmit = async (values, { resetForm }) => {
    if (accessToken === null) {
      try {
        setLoading(true)
        const response = await axiosProvider({
          method: 'POST',
          endpoint: 'Account/Customer/Login',
          data: values,
          headers: {
            'Content-Type': 'application/json'
          }
        })
        setLoading(false)
        if (response?.status === 200) {
          if (response?.data?.code === 200) {
            showToast(toast, setToast, response)
            getCartCount(response?.data?.currentUser?.userId)
            localStorage.removeItem('hk-compare-data')
            let userId = response?.data?.currentUser?.userId

            nookies.set(
              null,
              'userToken',
              response?.data?.tokens?.accessToken,
              {
                path: '/'
              }
            )
            nookies.set(
              null,
              'refreshToken',
              response?.data?.tokens?.refreshToken,
              { path: '/' }
            )
            nookies.set(null, 'userId', response?.data?.currentUser?.userId, {
              path: '/'
            })
            nookies.set(
              null,
              'sessionId',
              response?.data?.currentUser?.userId,
              {
                path: '/'
              }
            )
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

            onClose()
          }
          showToast(toast, setToast, response)
        }
      } catch (error) {
        setLoading(false)
        showToast(toast, setToast, {
          data: { code: 204, message: _exception?.message }
        })
      }
    }
  }

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
    <div>
      {loading && <Loader />}
      <Formik
        enableReinitialize={true}
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={onSubmit}
      >
        {({ values, setFieldValue, validateForm }) => (
          <Form>
            <h2 className='login-title'>
              <button
                className={`${isLogin && 'activeblue'} loginaccount`}
                onClick={() => toggleForm('login')}
              >
                Login
              </button>
              /
              <button
                className={`${!isLogin && 'activeblue'} signupaccount`}
                onClick={() => toggleForm('signup')}
              >
                Sign Up
              </button>
            </h2>

            <InputComponent
              labelText={'Mobile Number or Email Address'}
              id={'userName'}
              type={'text'}
              labelClass={'sign-com-label'}
              maxLength={isPhoneNumber(values?.userName) ? 10 : 255}
              onChange={(e) => {
                setFieldValue('userName', e?.target?.value)
              }}
              autoFocus
              value={values?.userName}
              name='userName'
              onBlur={(e) => {
                let fieldName = e?.target?.name
                setFieldValue(fieldName, values[fieldName]?.trim())
              }}
            />
            <div className='eye-main-pasw'>
              <PasswordStrengthCheck
                isLogin={isLogin}
                onChange={(e) => {
                  setFieldValue('password', e?.target?.value)
                }}
                id='password'
                value={values?.password}
                name='password'
                onBlur={(e) => {
                  let fieldName = e?.target?.name
                  setFieldValue(fieldName, values[fieldName]?.trim())
                }}
              />
            </div>

            <div className='forget-login'>
              <Link href='#.' onClick={handleForgot}>
                Forgot Password?
              </Link>
            </div>

            <div className='btn-submit-login'>
              <button
                onClick={() => {
                  validateForm()?.then((focusError) =>
                    focusInput(Object?.keys(focusError)?.[0])
                  )
                }}
                type='submit'
                className='m-btn btn-login'
              >
                Login
              </button>

              <button
                type='button'
                className='m-btn login-otp-btn'
                onClick={() => LoginOTPForm('loginotp')}
              >
                Login Via OTP
              </button>
            </div>
            <div className='new-account'>
              <div className='create-account-section'>
                <span> New to {_projectName_}?&nbsp;</span>
                <Link
                  href='#.'
                  className='account-link-signup'
                  onClick={() => toggleForm('signup')}
                >
                  Create Account
                </Link>
              </div>
            </div>
          </Form>
        )}
      </Formik>
    </div>
  )
}

export default Login
