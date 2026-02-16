import React from 'react'
import Link from 'next/link'
import Image from 'next/image'

const HeaderforCheckout = ({
  activeAccordion,
  setActiveAccordion,
  stateValues
}) => {
  return (
    <header>
      <div className='site-container'>
        <nav className='nav_checkoutwrapper'>
          <div className='m_logo'>
            <Link href='/' className='navbar__logo'>
              <Image
                className='m_nav_logo desktop_logo'
                src='/images/logo.png'
                alt='company logo'
                width={100}
                height={100}
              />
              <Image
                className='m_nav_logo responsive_logo'
                src='/images/mobile-logo.png'
                alt='company logo'
                width={100}
                height={100}
              />
            </Link>
          </div>
          <div className='mp-checkout-action'>
            <div>
              <button
                className={`mp-header-menu ${
                  Object?.keys(stateValues?.addressVal)?.length > 0 && 'true'
                } ${activeAccordion === 0 && 'active'}`}
                onClick={() => {
                  setActiveAccordion(1)
                }}
              >
                {'User Delivery'}
              </button>
            </div>
            <span>-----</span>
            <div>
              <button
                className={`mp-header-menu ${
                  activeAccordion === 2 && 'active'
                }  ${activeAccordion === 3 && 'true'}`}
                onClick={() => {
                  if (Object.keys(stateValues?.addressVal)?.length > 0) {
                    setActiveAccordion(2)
                  }
                }}
              >
                Cart
              </button>
            </div>
            <span>-----</span>
            <div>
              <button
                className={`mp-header-menu  ${
                  activeAccordion === 3 && 'active'
                }`}
                onClick={() => {
                  if (Object.keys(stateValues?.addressVal)?.length > 0) {
                    setActiveAccordion(3)
                  }
                }}
              >
                Payment
              </button>
            </div>
          </div>
          <div className='mp-security-icons'>
            <div>
              <i className='m-icon mp-lock'></i>
              <span>Safe & Secure</span>
            </div>
            <div>
              <i className='m-icon mp-secure'></i>
              <span>Easy Returns</span>
            </div>
            <div>
              <i className='m-icon mp-protection'></i>
              <span>100% Protection</span>
            </div>
          </div>
        </nav>
      </div>
    </header>
  )
}

export default HeaderforCheckout
