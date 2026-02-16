import InputComponent from '../base/InputComponent'
import * as Yup from 'yup'
import { Form, Formik } from 'formik'
import {
  _phoneNumberRegex_,
  _emailRegex_,
  _positiveInteger_,
  _passwordRegex_
} from '../../lib/Regex'
import Loader from '../Loader'
import { useState } from 'react'
import axiosProvider from '../../lib/AxiosProvider'
import { showToast } from '../../lib/GetBaseUrl'
import { _exception } from '../../lib/exceptionMessage'
import { focusInput } from '@/lib/AllGlobalFunction'

const ForgotPassword = ({ toggleForm, toast, setToast, onClose }) => {
  const [loading, setLoading] = useState(false)
  const initialValues = {
    email: ''
  }

  const validationSchema = Yup.object().shape({
    email: Yup.string()
      .matches(_emailRegex_, 'Please enter valid email address')
      .required('Email address is required')
  })

  const onSubmit = async (values) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'POST',
        endpoint: 'Account/Customer/ForgotPassword',
        data: values
      })
      setLoading(false)
      if (response?.data?.code == 200) {
        setToast({
          show: true,
          text: response?.data?.message,
          variation: response?.data?.code !== 200 ? 'error' : 'success'
        })
        setTimeout(() => {
          setToast({ ...toast, show: false })
        }, 4000)
        onClose()
      } else {
        showToast(toast, setToast, response)
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
      {loading && <Loader />}

      <div className='forgot-main'>
        <h1 className='forgot-title'>Forgot password?</h1>
        <button className='forgot-back-btn' onClick={() => toggleForm('login')}>
          <i className='m-icon back-icon'></i>
          Go Back
        </button>
        {loading && <Loader />}
      </div>
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={onSubmit}
      >
        {({ values, setFieldValue, validateForm }) => (
          <Form>
            <div>
              <InputComponent
                labelText={'Email'}
                labelClass={'sign-com-label'}
                MainHeadClass={'forgot-mobile pv-forgot-input'}
                name={'email'}
                id={'email'}
                autoFocus
                onChange={(e) => {
                  setFieldValue('email', e?.target?.value)
                }}
                value={values.email}
                placeholder='Enter your email for a password reset OTP.'
              />
              <div className='send-rest-forgot'>
                <button
                  onClick={() => {
                    validateForm()?.then((focusError) =>
                      focusInput(Object?.keys(focusError)?.[0])
                    )
                  }}
                  type='submit'
                  className='reset-password-login'
                >
                  Send Reset
                </button>
              </div>
            </div>
          </Form>
        )}
      </Formik>
    </>
  )
}

export default ForgotPassword
