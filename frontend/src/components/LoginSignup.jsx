import { useState, useEffect } from 'react'
import Login from './auth/Login'
import LoginViaOtp from './auth/LoginViaOtp'
import SignUp from './auth/SignUp'
import ForgotPassword from './auth/ForgotPassword'

const LoginSignup = ({ onClose, modal, modalOpen, toast, setToast }) => {
  const [isLogin, setIsLogin] = useState(true)
  const [isforgot, setIsForgot] = useState(false)
  const [isLoginOTP, setLoginOTP] = useState(false)

  const toggleForm = (val) => {
    if (val === 'login') {
      setIsLogin(true)
      setLoginOTP(false)
      setIsForgot(false)
    } else if (val === 'signup') {
      setIsLogin(false)
      setLoginOTP(false)
      setIsForgot(false)
    }
  }

  const LoginOTPForm = (val) => {
    if (val === 'login') {
      setIsLogin(true)
      setLoginOTP(false)
    } else if (val === 'loginotp') {
      setIsLogin(false)
      setLoginOTP(() => true)
    }
  }

  const handleForgot = () => {
    setIsForgot(true)
  }

  useEffect(() => {
    document.body.style.overflowY =
      isLogin || !isLogin || isforgot || isLoginOTP ? 'hidden' : 'auto'
    return () => {
      document.body.style.overflowY = 'auto'
    }
  }, [isLogin, isforgot, isLoginOTP])

  return (
    <>
      <div>
        <div className='auth-main'>
          <div className='auth-login-main'>
            <button onClick={onClose} className='close-btn-login'>
              <svg
                className='btn-close-login'
                fill='none'
                viewBox='0 0 24 24'
                stroke='currentColor'
              >
                <path
                  strokeLinecap='round'
                  strokeLinejoin='round'
                  strokeWidth={2}
                  d='M6 18L18 6M6 6l12 12'
                />
              </svg>
            </button>

            {isLogin && !isforgot && !isLoginOTP && (
              <Login
                isLogin={isLogin}
                setLoginOTP={setLoginOTP}
                LoginOTPForm={LoginOTPForm}
                handleForgot={handleForgot}
                toggleForm={toggleForm}
                onClose={onClose}
                modal={modal}
                toast={toast}
                setToast={setToast}
              />
            )}

            {isLoginOTP && (
              <LoginViaOtp
                toggleForm={toggleForm}
                onClose={onClose}
                toast={toast}
                setToast={setToast}
              />
            )}

            {!isLogin && !isforgot && !isLoginOTP && (
              <SignUp
                isLogin={isLogin}
                isLoginOTP={isLoginOTP}
                toggleForm={toggleForm}
                onClose={onClose}
                modal={modal}
                modalOpen={modalOpen}
                toast={toast}
                setToast={setToast}
              />
            )}
            {isforgot && (
              <ForgotPassword
                toast={toast}
                setToast={setToast}
                toggleForm={toggleForm}
                onClose={onClose}
              />
            )}
          </div>
        </div>
      </div>
    </>
  )
}

export default LoginSignup
