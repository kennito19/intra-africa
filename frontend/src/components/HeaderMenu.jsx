'use client'

import Image from 'next/image'
import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'
import { useDetectClickOutside } from 'react-detect-click-outside'
import Masonry from 'react-masonry-css'
import {
  encryptId,
  getUserId,
  getUserToken,
  reactImageUrl,
  spaceToDash
} from '../lib/GetBaseUrl'
import { _subMenuImg_ } from '../lib/ImagePath'
import { checkTokenAuthentication } from '../lib/checkTokenAuthentication'
import { handleLogout } from '../lib/handleLogout'
import Loader from './Loader'
import LoginSignup from './LoginSignup'
import MBtn from './base/MBtn'
import Toaster from './base/Toaster'

const HeaderMenu = ({ categoryMenu, user, isloaded }) => {
  const userIdCookie = getUserId()
  const router = useRouter()
  const [loading, setLoading] = useState(false)
  const [isOpen, setIsOpen] = useState(false)
  const [activeSubmenus, setActiveSubmenus] = useState([])
  const [thirdSubmenus, setActivethirdSubmenus] = useState([])
  const [activeItems, setActiveItems] = useState([])
  const [modal, setModal] = useState(false)
  let nav_linkimg = false
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })

  const breakpointColumnsObj = {
    default: 4,
    1024: 1
  }

  const setShowModal = () => {
    setIsOpen(false)
    if (!user?.userId) {
      if (userIdCookie) {
        checkTokenAuthentication(toast, setToast)
      } else {
        setModal(true)
      }
    }
  }

  const closeModal = () => {
    setModal(false)
  }

  const handleMouseEnter = (index) => {
    setActiveItems((prevActiveItems) => {
      const updatedActiveItems = [...prevActiveItems]
      updatedActiveItems[index] = true
      return updatedActiveItems
    })
  }

  const handleMouseLeave = (index) => {
    setActiveItems((prevActiveItems) => {
      const updatedActiveItems = [...prevActiveItems]
      updatedActiveItems[index] = false
      return updatedActiveItems
    })
  }

  const handleClick = (index) => {
    setActiveItems((prevActiveItems) => {
      const updatedActiveItems = [...prevActiveItems]
      updatedActiveItems[index] = false
      return updatedActiveItems
    })
  }

  const closeMenu = (e) => {
    const wrapper = document.getElementById('nav_wrapper')

    if (
      wrapper?.classList.contains('active') &&
      (e.target.classList.contains('navmenu_wrapper') ||
        e.target.classList.contains('m-icon'))
    ) {
      setIsOpen(false)
      setActiveSubmenus(' ')
      setActivethirdSubmenus(' ')
      document.body.style.overflow = ''
    }
  }

  const toggleSubmenu = (index) => {
    if (activeSubmenus.includes(index)) {
      setActiveSubmenus(activeSubmenus.filter((i) => i !== index))
    } else {
      setActiveSubmenus([...activeSubmenus, index])
    }
  }

  const toggleThirdSubmenu = (index) => {
    if (thirdSubmenus.includes(index)) {
      setActivethirdSubmenus(thirdSubmenus.filter((i) => i !== index))
    } else {
      setActivethirdSubmenus([...thirdSubmenus, index])
    }
  }
  const specificPartRef = useDetectClickOutside({ onTriggered: closeMenu })

  const checkMenuCase = (card) => {
    let url = ''
    switch (card?.redirectTo) {
      case 'Product List':
        url = ''
        if (card?.categoryId) {
          url += `/products/${spaceToDash(card?.name)}?CategoryId=${encryptId(
            card?.categoryId
          )}`
        }
        if (card?.brands) {
          url += `&BrandIds=${card?.brands
            ?.split(',')
            ?.map((item) => encryptId(item))}`
        }
        if (card?.sizes) {
          url += `&SizeIds=${card?.sizes
            ?.split(',')
            ?.map((item) => encryptId(item))}`
        }
        if (card?.colors) {
          url += `&ColorIds=${card?.colors
            ?.split(',')
            ?.map((item) => encryptId(item))}`
        }
        break
      case 'Collection Page':
        url = `/collection?productCollectionId=${encryptId(card?.collectionId)}`
        break
      case 'Static Page':
        url = `/static?id=${encryptId(card?.staticPageId)}`
        break
      case 'Other Links':
        if (card?.customLink && card.customLink.startsWith('http')) {
          url = card.customLink
        } else {
          url = card?.customLink ? card.customLink : '#.'
        }
        break
      default:
        url = '#.'
        break
    }
    return url
  }

  useEffect(() => {
    const interval = setInterval(() => {
      const token = getUserToken()
      if (token) {
        clearInterval(interval)
      }
    }, 1)
  }, [])

  return (
    <>
      {loading && <Loader />}
      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
      {modal && (
        <LoginSignup
          modal={modal}
          toast={toast}
          setToast={setToast}
          onClose={closeModal}
        />
      )}

      <div>
        <button
          className='m_toggle'
          onClick={() => {
            setIsOpen(() => true)
            document.body.style.overflow = 'hidden'
          }}
        >
          <span></span>
          <span></span>
          <span></span>
        </button>
        <div
          className={`navmenu_wrapper ${isOpen ? 'active' : ''}`}
          id='nav_wrapper'
        >
          {isOpen && (
            <div
              className='hidden_desk_overlay'
              onClick={() => {
                setIsOpen(() => false)
                document.body.style.overflow = ''
              }}
            ></div>
          )}
          <ul ref={specificPartRef} className='navbar__items'>
            {isloaded && !user?.userId && (
              <>
                <Link href='#.' className='res_hidd' onClick={setShowModal}>
                  <Image
                    src={
                      'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT40bKdGB_AF5LvkTHyvRaW3GqCZY7G2vLN30rm8Y5ubA&usqp=CAU&ec=48600112'
                    }
                    className='login_image'
                    width={100}
                    height={100}
                    alt='image'
                  />
                </Link>
                <MBtn
                  buttonClass={'pv-toggle-close resimg_welclosebtn res_hidd'}
                  withIcon
                  iconClass={'closetoggle-icon'}
                  onClick={() => {
                    setIsOpen(() => false)
                    document.body.style.overflow = ''
                  }}
                />
              </>
            )}
            {isloaded && user?.userId && (
              <div>
                <ul className='res_hidd after_login_div'>
                  <li className='navbar__list li-f_fir'>
                    <Link
                      className='navbar__link dropdown__icon'
                      href='/user/profile'
                    >
                      <i className='m-icon m-after-login_icon'></i>
                    </Link>
                  </li>
                  <li className='navbar__list li-s_sec'>
                    <Link
                      className='navbar__link dropdown__icon'
                      href='/user/profile'
                    >
                      {user?.fullName}
                      <i className='m-icon menu-dropdown-icon'></i>
                    </Link>
                  </li>
                  <MBtn
                    buttonClass={'pv-toggle-close'}
                    withIcon
                    iconClass={'closetoggle-icon'}
                    onClick={() => setIsOpen(false)}
                  />
                </ul>
              </div>
            )}

            {user?.userId && (
              <div>
                <ul className='res_hidd after_login_div'>
                  <li className='navbar__list li-f_fir'>
                    <Link
                      className='navbar__link dropdown__icon'
                      href='/user/profile'
                    >
                      <i className='m-icon m-after-login_icon'></i>
                    </Link>
                  </li>
                  <li className='navbar__list li-s_sec'>
                    <Link
                      className='navbar__link dropdown__icon'
                      href='/user/profile'
                    >
                      {user?.fullName}
                      <i className='m-icon menu-dropdown-icon'></i>
                    </Link>
                  </li>

                  <MBtn
                    buttonClass={'pv-toggle-close'}
                    withIcon
                    iconClass={'closetoggle-icon'}
                    onClick={() => setIsOpen(false)}
                  />
                </ul>
              </div>
            )}

            {categoryMenu?.data?.length > 0 &&
              categoryMenu?.data?.map((main, index) => (
                <li
                  className={
                    activeItems[index] && main?.subMenu?.length > 0
                      ? 'navbar__list pv-nav-itemmain active'
                      : 'navbar__list pv-nav-itemmain'
                  }
                  key={main?.id}
                  onMouseEnter={() => handleMouseEnter(index)}
                  onMouseLeave={() => handleMouseLeave(index)}
                  onClick={() => handleClick(index)}
                >
                  <span className='navbar__link dropdown__icon'>
                    <Link
                      href={checkMenuCase(main)}
                      target={
                        main?.redirectTo === 'Other Links' ? '_blank' : '_self'
                      }
                      onClick={() => {
                        setIsOpen(false)
                        document.body.style.overflow = ''
                        setActiveSubmenus([])
                        setActivethirdSubmenus([])
                      }}
                    >
                      {main?.name}
                    </Link>
                    {main?.subMenu?.length > 0 && (
                      <i
                        className='m-icon menu-dropdown-icon'
                        onClick={() => toggleSubmenu(index)}
                      ></i>
                    )}
                  </span>

                  <Masonry
                    breakpointCols={breakpointColumnsObj}
                    id={index}
                    columnClassName={nav_linkimg ? `pv-imglinkset-col` : ' '}
                    className={`navbar__submenu_div ${
                      activeSubmenus?.includes(index) ? 'active' : ''
                    }`}
                  >
                    {main?.subMenu?.map((item, index) => (
                      <ul className='navbar__subitems' key={index}>
                        <li className='navbar__sublist'>
                          <span className='navbar__sublink'>
                            <Link
                              className='pv-navbarsub-link'
                              href={checkMenuCase(item)}
                              target={
                                item?.redirectTo === 'Other Links'
                                  ? '_blank'
                                  : '_self'
                              }
                              onClick={() => {
                                setIsOpen(false)
                                document.body.style.overflow = ''
                                setActiveSubmenus([])
                                setActivethirdSubmenus([])
                              }}
                            >
                              {nav_linkimg && (
                                <Image
                                  className='pv-navbar__sublinkimg'
                                  src={encodeURI(
                                    `${reactImageUrl}${_subMenuImg_}${item?.image}`
                                  )}
                                  alt={item?.imageAlt ?? 'image'}
                                  width={100}
                                  height={100}
                                />
                              )}
                              {item?.name}
                            </Link>
                            {item?.childMenu?.length > 0 && (
                              <i
                                className='m-icon menu-dropdown-icon'
                                onClick={() => toggleThirdSubmenu(index)}
                              ></i>
                            )}
                          </span>
                          <div
                            className={`navbar__submenu_sub_wrapper ${
                              thirdSubmenus?.includes(index) ? 'active' : ''
                            }`}
                          >
                            <ul className='navbar__subitems_sub'>
                              {item?.childMenu.map((item, subIndex) => (
                                <li
                                  className='navbar__sublist_sub'
                                  key={subIndex}
                                >
                                  <Link
                                    href={checkMenuCase(item)}
                                    className='navbar__sublink_sub'
                                    onClick={() => {
                                      setIsOpen(false)
                                      document.body.style.overflow = ''
                                      setActiveSubmenus([])
                                      setActivethirdSubmenus([])
                                    }}
                                    target={
                                      item?.redirectTo === 'Other Links'
                                        ? '_blank'
                                        : '_self'
                                    }
                                  >
                                    {item?.name}
                                  </Link>
                                </li>
                              ))}
                            </ul>
                          </div>
                        </li>
                      </ul>
                    ))}
                  </Masonry>
                </li>
              ))}

            <div className='res_hidd important_links-link'>
              {/* Show Login/Signup button prominently when not logged in */}
              {!(isloaded && user?.userId) && (
                <li className='navbar__list'>
                  <button
                    className='mobile-menu-login-btn'
                    onClick={() => setShowModal()}
                  >
                    LOGIN / SIGN UP
                  </button>
                </li>
              )}
              {isloaded && user?.userId && (
              <li className='navbar__list'>
                <Link
                  className='navbar__link dropdown__icon'
                  href={
                    isloaded && user?.userId ? '/user/orders?pi=1&ps=10' : '#.'
                  }
                  onClick={() => {
                    setShowModal()
                  }}
                >
                  My Order
                </Link>
              </li>
              )}
              {isloaded && user?.userId && (
                <li className='navbar__list'>
                  <Link
                    className='navbar__link dropdown__icon'
                    href={user?.userId ? '/user/profile' : '#.'}
                    onClick={() => {
                      setShowModal()
                    }}
                  >
                    My Profile
                  </Link>
                </li>
              )}
              <li className='navbar__list'>
                <Link
                  className='navbar__link dropdown__icon'
                  href={isloaded && user?.userId ? '/user/wishlist' : '#.'}
                  onClick={() => {
                    setShowModal()
                  }}
                >
                  Wishlist
                </Link>
              </li>
              <li className='navbar__list'>
                <Link
                  className='navbar__link dropdown__icon'
                  href={isloaded && user?.userId ? '/user/address' : '#.'}
                  onClick={() => {
                    setShowModal()
                  }}
                >
                  My Addresses
                </Link>
              </li>
              {isloaded && user?.userId && (
                <li className='navbar__list'>
                  <button
                    className='navbar__link dropdown__icon'
                    onClick={() => {
                      handleLogout(router, toast, setToast)
                      setIsOpen(false)
                    }}
                  >
                    Logout
                  </button>
                </li>
              )}
            </div>
          </ul>
        </div>
      </div>
    </>
  )
}

export default HeaderMenu
