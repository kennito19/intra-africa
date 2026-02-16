import { _projectName_ } from '@/lib/ConfigVariables'
import { addUser } from '@/redux/features/userSlice'
import axios from 'axios'
import { ErrorMessage, Form, Formik } from 'formik'
import Link from 'next/link'
import nookies from 'nookies'
import { useEffect, useState } from 'react'
import { useDispatch } from 'react-redux'
import * as Yup from 'yup'
import { focusInput } from '../../lib/AllGlobalFunction'
import axiosProvider from '../../lib/AxiosProvider'
import { _exception } from '../../lib/exceptionMessage'
import {
  getBaseUrl,
  getDeviceId,
  getRefreshToken,
  getSessionId,
  showToast
} from '../../lib/GetBaseUrl'
import {
  _alphabetRegex_,
  _emailRegex_,
  _passwordRegex_,
  _phoneNumberRegex_
} from '../../lib/Regex'
import InputComponent from '../base/InputComponent'
import IpRadio from '../base/IpRadio'
import PasswordStrengthCheck from '../base/PasswordStrengthCheck'
import TextError from '../base/TextError'
import Toaster from '../base/Toaster'
import Loader from '../Loader'

const SignUp = ({
  isLogin,
  isLoginOTP,
  toggleForm,
  onClose,
  toast,
  setToast,
  modal,
  modalOpen
}) => {
  const baseUrl = getBaseUrl()
  const dispatch = useDispatch()
  const [loading, setLoading] = useState(false)
  const accessToken = getRefreshToken()
  const mySessionId = getSessionId()
  const deviceId = getDeviceId()
  const initialValues = {
    firstName: '',
    lastName: '',
    MobileNo: '',
    emailID: '',
    password: '',
    gender: '',
    deviceId
  }

  const validationSchema = Yup.object().shape({
    firstName: Yup.string()
      .transform((value) => (value ? value.trim() : value))
      .matches(/^[A-Za-z]+$/, 'Only alphabetic characters are allowed')
      .min(3, 'First name must be at least 3 characters')
      .required('First name is required')
      .test(
        'no-special-characters',
        'Special characters are not allowed',
        (value) => {
          return !value || _alphabetRegex_.test(value)
        }
      ),
    lastName: Yup.string()
      .transform((value) => (value ? value.trim() : value))
      .matches(/^[A-Za-z]+$/, 'Only alphabetic characters are allowed')
      .min(3, 'Last name must be at least 3 characters')
      .required('Last name is required')
      .test(
        'no-special-characters',
        'Special characters are not allowed',
        (value) => {
          return !value || _alphabetRegex_.test(value)
        }
      ),
    MobileNo: Yup.string()
      .required('Mobile number is required')
      .matches(/^\d{10}$/, 'Mobile number must be a 10-digit number'),
    emailID: Yup.string()
      .matches(_emailRegex_, 'Please enter valid email')
      .required('Email Address is required'),
    password: Yup.string()
      .matches(_passwordRegex_, 'Please use strong password')
      .required('Password field is required')
      .test('Password is required', (value) => !value.includes(' ')),
    gender: Yup.string().required('Choose your gender')
  })

  const onSubmit = async (values, { resetForm }) => {
    const config = {
      headers: {
        'Content-Type': 'application/json',
        accept: '*/*'
      }
    }

    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'POST',
        endpoint: 'Account/Customer/signUp',
        data: values
      })

      if (response?.data?.code === 200) {
        const loginData = {
          userName: values?.emailID,
          password: values?.password,
          isRemember: true,
          deviceId
        }
        const logResponse = await axios.post(
          `${baseUrl}Account/Customer/Login`,
          loginData,
          config
        )

        setLoading(false)
        if (logResponse?.data?.code === 200) {
          let userId = logResponse?.data?.currentUser?.userId
          showToast(toast, setToast, logResponse)
          nookies.set(
            null,
            'userToken',
            logResponse?.data?.tokens?.accessToken,
            { path: '/' }
          )
          nookies.set(
            null,
            'refreshToken',
            logResponse?.data?.tokens?.refreshToken,
            { path: '/' }
          )
          nookies.set(null, 'userId', logResponse?.data?.currentUser?.userId, {
            path: '/'
          })
          nookies.set(
            null,
            'sessionId',
            logResponse?.data?.currentUser?.userId,
            { path: '/' }
          )
          localStorage.removeItem('hk-compare-data')
          if (mySessionId !== userId) {
            const responseSession = await axiosProvider({
              method: 'PUT',
              endpoint: 'Cart/UpdateSession',
              queryString: `?UserId=${userId}&SessionId=${mySessionId}`
            })
            if (responseSession?.data?.code === 200) {
              nookies.set(null, 'sessionId', userId, { path: '/' })
            }
          }
          dispatch(
            addUser({
              user: logResponse?.data?.currentUser,
              userToken: logResponse?.data?.tokens?.accessToken,
              refreshToken: logResponse?.data?.tokens?.refreshToken,
              deviceId: getDeviceId()
            })
          )
          resetForm({ values: '' })
          onClose()
          if (modal === 'offer') {
            modalOpen({ ...modal, show: true })
          }
        }
      } else {
        setLoading(false)
        showToast(toast, setToast, response)
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  useEffect(() => {
    if (accessToken !== null) {
      onClose()
    }
  }, [accessToken])

  return (
    <>
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

      {!isLoginOTP && (
        <Formik
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={onSubmit}
        >
          {({ values, setFieldValue, validateForm, errors }) => (
            <Form id='submit-form'>
              <div className='first-last-signup-main'>
                <div>
                  <InputComponent
                    labelText={'First Name'}
                    id={'firstName'}
                    type={'text'}
                    labelClass={'sign-com-label'}
                    onChange={(e) => {
                      setFieldValue('firstName', e?.target?.value)
                    }}
                    autoFocus
                    value={values.firstName}
                    name={'firstName'}
                    onBlur={(e) => {
                      let fieldName = e?.target?.name
                      setFieldValue(fieldName, values[fieldName]?.trim())
                    }}
                  />
                </div>
                <div>
                  <InputComponent
                    labelText={'Last Name'}
                    id={'lastName'}
                    type={'text'}
                    labelClass={'sign-com-label'}
                    onChange={(e) => {
                      setFieldValue('lastName', e?.target?.value)
                    }}
                    value={values.lastName}
                    name='lastName'
                    onBlur={(e) => {
                      let fieldName = e?.target?.name
                      setFieldValue(fieldName, values[fieldName]?.trim())
                    }}
                  />
                </div>
              </div>

              <InputComponent
                labelText={'Mobile Number'}
                id={'MobileNo'}
                type={'text'}
                name='MobileNo'
                labelClass={'sign-com-label'}
                onChange={(e) => {
                  const inputValue = e?.target?.value
                  const fieldName = e?.target?.name
                  const isValid = _phoneNumberRegex_.test(inputValue)
                  if (!inputValue || isValid) {
                    setFieldValue(fieldName, inputValue)
                  }
                }}
                onBlur={(e) => {
                  let fieldName = e?.target?.name
                  setFieldValue(fieldName, values[fieldName]?.trim())
                }}
                value={values.MobileNo}
              />

              <InputComponent
                labelText={'Email Address'}
                id={'emailID'}
                type={'emailID'}
                labelClass={'sign-com-label'}
                onChange={(e) => {
                  setFieldValue('emailID', e?.target?.value)
                }}
                value={values.emailID}
                name='emailID'
                onBlur={(e) => {
                  let fieldName = e?.target?.name
                  setFieldValue(fieldName, values[fieldName]?.trim())
                }}
              />

              <PasswordStrengthCheck
                onChange={(e) => {
                  setFieldValue('password', e?.target?.value)
                }}
                onBlur={(e) => {
                  let fieldName = e?.target?.name
                  setFieldValue(fieldName, values[fieldName]?.trim())
                }}
                value={values.password}
                name={'password'}
              />

              <div className='input_col'>
                <div className='radio_box'>
                  <IpRadio
                    MainHeadClass={'main_rd_gender'}
                    labelClass={'ico_fl_mn'}
                    rdclass={'gendar-man'}
                    labelText={
                      <span>
                        <i className='m-icon men-icon'></i>
                        Male
                      </span>
                    }
                    id={'men'}
                    name={'gender'}
                    onChange={(e) => {
                      setFieldValue('gender', 'Male')
                    }}
                  />

                  <IpRadio
                    MainHeadClass={'main_rd_gender'}
                    rdclass={'gendar-female'}
                    labelClass={'ico_fl_mn'}
                    labelText={
                      <span>
                        <i className='m-icon female-icon'></i>Female
                      </span>
                    }
                    id={'female'}
                    name={'gender'}
                    onChange={(e) => {
                      setFieldValue('gender', 'Female')
                    }}
                  />
                </div>
                <ErrorMessage name='gender' component={TextError} />
              </div>

              <button
                type='submit'
                onClick={() => {
                  validateForm()?.then((focusError) =>
                    focusInput(Object?.keys(focusError)?.[0])
                  )
                }}
                form='submit-form'
                className='sign-btn-create m-btn'
              >
                Create Account
              </button>

              <div className='new-account'>
                <div className='create-account-section'>
                  <span> Already have a {_projectName_} account?&nbsp;</span>
                  <Link
                    href='#.'
                    className='account-link-signup'
                    onClick={() => toggleForm('login')}
                  >
                    Login
                  </Link>
                </div>
              </div>
            </Form>
          )}
        </Formik>
      )}

      {loading && <Loader />}

      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
    </>
  )
}

export default SignUp
