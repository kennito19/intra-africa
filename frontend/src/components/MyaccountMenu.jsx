import Link from 'next/link'
import React, { useState } from 'react'

const MyaccountMenu = ({ activeTab }) => {
  const [activeToggle, setActiveToggle] = useState(
    activeTab ? activeTab : 'order'
  )

  return (
    <>
      <div className='order-sidebar-main'>
        <h1 className='order-menu-title'>Account</h1>
        <div className='order-menu'>
          <ul className='order-menu-list'>
            <li>
              <Link
                href='/user/orders?pi=1&ps=10'
                data-toggle='tab'
                className={`nav-link fw-semibold ${
                  activeToggle === 'order' ? 'active show' : ''
                }`}
                onClick={() => setActiveToggle('order')}
              >
                My Order
              </Link>
            </li>
            <li>
              <Link
                href='/user/profile'
                data-toggle='tab'
                className={`nav-link fw-semibold ${
                  activeToggle === 'profile' ? 'active show' : ''
                }`}
                onClick={() => {
                  setActiveToggle('profile')
                }}
              >
                My Profile
              </Link>
            </li>

            <li>
              <Link
                href='/user/wishlist'
                data-toggle='tab'
                className={`nav-link fw-semibold ${
                  activeToggle === 'wish' ? 'active show' : ''
                }`}
                onClick={() => setActiveToggle('wish')}
              >
                My Wishlist
              </Link>
            </li>

            <li>
              <Link
                href='/user/address'
                data-toggle='tab'
                className={`nav-link fw-semibold ${
                  activeToggle === 'address' ? 'active show' : ''
                }`}
                onClick={() => setActiveToggle('address')}
              >
                My Address
              </Link>
            </li>
          </ul>
        </div>
      </div>
    </>
  )
}

export default MyaccountMenu
