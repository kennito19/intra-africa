'use client'

import actionHandler from '@/utils/actionHandler'
import Link from 'next/link'
import { usePathname, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import HeaderMenu from '../HeaderMenu'
import ProductSearchBar from '../base/ProductSearchBar'
import HeaderTopbar from '../misc/HeaderTopbar'
import MyAccount from '../misc/MyAccounts'

const Header = ({ categoryMenu }) => {
  const [isloaded, setIsloaded] = useState(false)
  const { user } = useSelector((state) => state?.user)
  const [isScrolled, setIsScrolled] = useState(false)
  const router = useRouter()
  const pathName = usePathname()
  const [path, setPath] = useState(pathName)
  const [searchTexts, setSearchTexts] = useState(router?.query?.searchTexts)
  const [scrollDirection, setScrollDirection] = useState('up')
  const [isDesktop, setIsDesktop] = useState(false)
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })

  useEffect(() => {
    function handleScroll() {
      const scrollTop = window.pageYOffset

      if (scrollTop > 100 && !isScrolled) {
        setIsScrolled(true)
      } else if (scrollTop === 0 && isScrolled) {
        setIsScrolled(false)
      }
    }

    window.addEventListener('scroll', handleScroll)
    return () => {
      window.removeEventListener('scroll', handleScroll)
    }
  }, [isScrolled])

  useEffect(() => {
    if (!router?.query?.searchTexts) {
      setSearchTexts('')
    } else {
      setSearchTexts(router?.query?.searchTexts)
    }
  }, [router?.query])

  useEffect(() => {
    let prevScrollPosition = window.pageYOffset

    const handleScroll = () => {
      const currentScrollPosition = window.pageYOffset

      if (currentScrollPosition > prevScrollPosition) {
        setScrollDirection('down')
      } else {
        setScrollDirection('up')
      }

      prevScrollPosition = currentScrollPosition
    }

    window.addEventListener('scroll', handleScroll)

    const handleResize = () => {
      const isDesktop = window.innerWidth >= 1024
      setIsDesktop(isDesktop)
    }
    handleResize()
    window.addEventListener('resize', handleResize)

    return () => {
      window.removeEventListener('scroll', handleScroll)
      window.removeEventListener('resize', handleResize)
    }
  })

  useEffect(() => {
    if (user === null || user) {
      setIsloaded(true)
    }
  }, [user])

  useEffect(() => {
    if (categoryMenu?.action) {
      actionHandler(categoryMenu?.action, router)
    }
  }, [])

  useEffect(() => {
    setPath(pathName)
  }, [pathName])

  return (
    <header>
      <HeaderTopbar />
      <div className='header_navmenu'>
        <div className='site-container-max'>
          <nav className='nav_wrapper'>
            <div className='m_logo'>
              <Link href='/' className='navbar__logo'>
                <img
                  src='/images/logo.png'
                  alt='Intra-Africa-logo'
                  className='m_nav_logo desktop_logo'
                />
                <img
                  src='/images/mobile-logo.png'
                  alt='Intra-Africa-logo'
                  className='m_nav_logo responsive_logo'
                />
              </Link>
            </div>
            <HeaderMenu
              categoryMenu={categoryMenu}
              user={user}
              isloaded={isloaded}
            />
            <div className='pv-nav-item-main'></div>
            <div className='search_myaccount_wrapper'>
              <div className='search-wrappper'>
                <ProductSearchBar
                  searchBtnVisible
                  placeholder={'Search for product,brand and more'}
                />
              </div>
            </div>
            <MyAccount toast={toast} setToast={setToast} />
          </nav>
        </div>
      </div>
      <div className='pv-secondary-nav'>
        <ul className='pv-secondary-nav-inner'>
          <li>
            <a href='#'>Super Deals</a>
          </li>
          <li>
            <a href='#'>Featured Brands</a>
          </li>
          <li>
            <a href='#'>Trending Styles</a>
          </li>
          <li>
            <a href='#'>Gift Cards</a>
          </li>
        </ul>
      </div>
    </header>
  )
}

export default Header
