'use client'
import Link from 'next/link'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { handleLogout } from '../../lib/handleLogout'
import LoginSignup from '../LoginSignup'

import { useRouter } from 'next/navigation'
import { getUserId } from '../../lib/GetBaseUrl'
import { checkTokenAuthentication } from '../../lib/checkTokenAuthentication'

const MyAccount = ({ user, isloaded, toast, setToast }) => {
  const [modal, setModal] = useState(false)
  const [showMenubar, SetShowMenubar] = useState(false)
  const router = useRouter()
  const { cartCount } = useSelector((state) => state?.cart)
  const userIdCookie = getUserId()

  const setShowModal = () => {
    setModal(true)
    SetShowMenubar(false)
  }
  const closeModal = () => {
    setModal(false)
  }

  const menuOpen = () => {
    SetShowMenubar(true)
  }

  const closeMenu = () => {
    SetShowMenubar(false)
  }

  const checkModal = () => {
    SetShowMenubar(false)
    if (!user?.userId) {
      if (userIdCookie) {
        checkTokenAuthentication(toast, setToast)
      } else {
        setModal(true)
      }
    }
  }

  return (
    <>
      {modal && (
        <LoginSignup onClose={closeModal} toast={toast} setToast={setToast} />
      )}

      <div>
        <ul className='my-account-wrapper'>
          <li
            className={
              showMenubar ? 'my-account-item active' : 'my-account-item'
            }
            onMouseEnter={menuOpen}
            onMouseLeave={closeMenu}
          >
            <Link
              href={isloaded && user?.userId ? '/user/profile ' : '/'}
              className='my-account-link'
              onClick={checkModal}
            >
              <i className='m-icon profile-icon'></i>
              <p className='my-account-name'>Profile</p>
            </Link>
            <div className='my-account-profile-details-wrapper'>
              <ul>
                {isloaded && user?.userId ? (
                  <li className='my-account-profile-login-wrapper'>
                    <div className='my-account-profile-before-login'>
                      <p className='my-account-profile-titel'>Welcome</p>
                    </div>
                    <div className='my-account-profile-after-login'>
                      <p className='my-account-profile-titel'>
                        {user?.fullName}
                      </p>
                    </div>
                  </li>
                ) : (
                  <li className='my-account-profile-login-wrapper'>
                    <div className='my-account-profile-before-login'>
                      <button
                        className='my-account-profile-login-signup'
                        onClick={() => {
                          if (userIdCookie) {
                            checkTokenAuthentication(toast, setToast)
                          } else {
                            setShowModal()
                          }
                        }}
                      >
                        LOGIN / SIGNUP
                      </button>
                    </div>
                  </li>
                )}
              </ul>

              <ul className='my-account-profile-details-item'>
                <li className='my-account-profile-details-list'>
                  <Link
                    href={isloaded && user?.userId ? '/user/profile' : '#.'}
                    className='my-account-profile-link'
                    onClick={checkModal}
                  >
                    My Profile
                  </Link>
                </li>
                <li className='my-account-profile-details-list'>
                  <Link
                    href={isloaded && user?.userId ? '/user/orders' : '#.'}
                    className='my-account-profile-link'
                    onClick={checkModal}
                  >
                    Orders
                  </Link>
                </li>
                <li className='my-account-profile-details-list'>
                  <Link
                    href={isloaded && user?.userId ? '/user/wishlist' : '#.'}
                    className='my-account-profile-link'
                    onClick={checkModal}
                  >
                    Wishlist
                  </Link>
                </li>
                <li className='my-account-profile-details-list'>
                  <Link
                    href='/contact-us'
                    className='my-account-profile-link'
                    onClick={closeMenu}
                  >
                    Contact Us
                  </Link>
                </li>
              </ul>

              {isloaded && user?.userId && (
                <>
                  <ul className='my-account-profile-details-item'>
                    <li className='my-account-profile-details-list'>
                      <Link
                        href='#.'
                        className='my-account-profile-link'
                        onClick={closeMenu}
                      >
                        Coupons
                      </Link>
                    </li>
                    <li className='my-account-profile-details-list'>
                      <Link
                        href='/user/address'
                        className='my-account-profile-link'
                        onClick={closeMenu}
                      >
                        Saved Addresses
                      </Link>
                    </li>
                  </ul>
                  <div className='my-account-profile-login-wrapper'>
                    <div className='my-account-profile-before-login'>
                      <button
                        onClick={() => {
                          handleLogout(router, toast, setToast)
                          closeMenu()
                        }}
                        className='my-account-profile-login-signup'
                      >
                        Logout
                      </button>
                    </div>
                  </div>
                </>
              )}
            </div>
          </li>

          <li className='my-account-item'>
            <Link
              href={isloaded && user?.userId ? '/user/wishlist' : ''}
              className='my-account-link'
              onClick={checkModal}
            >
              <i className='m-icon wishlist-icon'></i>
              <p className='my-account-name'>Wishlist</p>
            </Link>
          </li>

          <li className='my-account-item'>
            <Link href='/cart' className='my-account-link' onClick={closeMenu}>
              <i className='m-icon bag-icon'></i>
              <p className='my-account-name'>Bag</p>
              <span className='my-account-bag-item'>
                {isloaded && cartCount ? cartCount : 0}
              </span>
            </Link>
          </li>
        </ul>
      </div>
    </>
  )
}

export default MyAccount
