'use client'

import { _projectName_ } from '@/lib/ConfigVariables'
import moment from 'moment'
import Image from 'next/image'
import Link from 'next/link'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import axiosProvider from '../../lib/AxiosProvider'
import { getUserToken } from '../../lib/GetBaseUrl'

const Footer = () => {
  const year = moment().format('YYYY')
  const [staticData, setStaticData] = useState()
  const token = getUserToken()
  const { userToken } = useSelector((state) => state?.user)

  const fetchStaticData = async () => {
    const response = await axiosProvider({
      method: 'GET',
      endpoint: 'ManageStaticPages'
    })

    if (response?.status === 200) {
      setStaticData(response?.data?.data)
    }
  }

  useEffect(() => {
    if (userToken || token) {
      !staticData && fetchStaticData()
    }
  }, [userToken, token])

  return (
    <footer className='footer'>
      {/* <div className='footer_contact_details'>
        <div className='footer_contact_wrapper site-container'>
          <div className='footer_contact_col'>
            <div className='footer_contact_special_dec_wrapper'>
              <i className='m-icon footer_mail-icon'></i>
              <p className='footer_contact_dec'>
                Get special discount on your inbox
              </p>
            </div>
            <div className='footer_mail_ip_wrapper'>
              <label className='footer_mail_label'>
                <i className='m-icon footer_mail_ip_icon'></i>
              </label>
              <input
                type='email'
                name=''
                placeholder='Enter your email'
                className='footer_mail_ip'
              />
            </div>
          </div>
          <div className='footer_contact_col'>
            <div className='footer_contact_mobile_wrapper'>
              <i className='m-icon footer_mobile-icon'></i>
              <p className='footer_contact_dec'>
                EXPERIENCE THE HASHKART MOBILE APP
              </p>
            </div>
            <div className='footer_mobile_app_wrapper'>
              <Link href='#.' className='footer_mobile_app_link'>
                <Image
                  src='/images/play-store.png'
                  width='130'
                  height='130'
                  sizes='100vw'
                  className='footer_mobile_img'
                  alt='image'
                />
              </Link>
              <Link href='#.' className='footer_mobile_app_link'>
                <Image
                  src='/images/app-store.png'
                  width='130'
                  height='130'
                  sizes='100vw'
                  className='footer_mobile_img'
                  alt='image'
                />
              </Link>
            </div>
          </div>
          <div className='footer_contact_col'>
            <div className='footer_contact_help-wrapper'>
              <i className='m-icon footer_help-icon'></i>
              <p className='footer_contact_dec'>
                FOR ANY HELP, YOU MAY CALL US AT 1600-207-3333
              </p>
            </div>
            <p className='footer_contact_dec'>
              (Monday to Saturday, 8AM to 10PM and Sunday, 10AM to 7PM)
            </p>
          </div>
        </div>
      </div> */}
      <div className='footer_middle_details'>
        <div className='site-container'>
          <div className='footer_menu_wrapper'>
            <ul className='footer_menu_item'>
              <li className='footer_menu_list'>
                <img
                  src='/images/logo.png'
                  alt='Intra Africa logo'
                  className='footer_site_logo'
                />{' '}
              </li>
              <li className='footer_menu_list'>
                <p>
                  Lorem ipsum dolor sit, amet consectetur adipisicing elit.
                  Labore esse illum excepturi beatae voluptatibus accusantium
                  tempore incidunt!
                </p>
              </li>
            </ul>
            <ul className='footer_menu_item'>
              <li className='footer_menu_list'>
                <h3 className='footer_menu_heading'>Company</h3>
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  About
                </Link>{' '}
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  Products
                </Link>{' '}
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  Contact
                </Link>{' '}
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  Blog
                </Link>{' '}
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  Careers
                </Link>
              </li>
            </ul>
            <ul className='footer_menu_item'>
              <li className='footer_menu_list'>
                <h3 className='footer_menu_heading'>Information</h3>
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  Help Center
                </Link>{' '}
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  Payment Methods
                </Link>{' '}
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  Return & Refund
                </Link>{' '}
              </li>
              <li className='footer_menu_list'>
                <Link href='#' className='footer_menu_link'>
                  Privacy Policy
                </Link>{' '}
              </li>
            </ul>
            <ul className='footer_menu_item pv-footer-socia-col'>
              <li className='footer_menu_list'>
                <h3 className='footer_menu_heading'>Quick Links</h3>
              </li>
              <li className='footer_menu_list'>
                <div className='pv-footer-social-main'>
                  <Link href='#.' className='footer_social_link'>
                    <Image
                      src='/icon/instagram.svg'
                      width='0'
                      height='0'
                      sizes='100vw'
                      className='footer_social_icon'
                    />
                  </Link>
                  <Link href='#.' className='footer_social_link'>
                    <Image
                      src='/icon/facebook.svg'
                      width='0'
                      height='0'
                      sizes='100vw'
                      className='footer_social_icon'
                    />
                  </Link>
                  <Link href='#.' className='footer_social_link'>
                    <Image
                      src='/icon/Twitter.svg'
                      width='0'
                      height='0'
                      sizes='100vw'
                      className='footer_social_icon'
                    />
                  </Link>
                  <Link href='#.' className='footer_social_link'>
                    <Image
                      src='/icon/Linkedin.svg'
                      width='0'
                      height='0'
                      sizes='100vw'
                      className='footer_social_icon'
                    />
                  </Link>
                </div>
                <div className='pv-footer-contact'>
                  <ul className='pv-topbar-call-main'>
                    <li>
                      <a href='tel:+12 345 67890'>+12 345 67890</a>{' '}
                    </li>
                  </ul>
                  <ul className='pv-topbar-mail-main'>
                    <li>
                      <a href='mailto:support@intraafrica.com'>
                        support@intraafrica.com
                      </a>
                    </li>
                  </ul>
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
      {/* <div className="footer_shipping_details">
        <div className="site-container">
          <div className="footer_shipping_wrapper">
            <div className="footer_shipping_col">
              <div className="footer_shipping_details_wrapper">
                <span className="footer_shipping_span">
                  <i className="m-icon free_shipping-icon"></i>
                </span>
                <div className="footer_shipping_item">
                  <p className="footer_shipping_name">FREE SHIPPING</p>
                  <p className="footer_shipping_order-details">
                    On Orders Above ₹299
                  </p>
                </div>
              </div>
            </div>
            <div className="footer_shipping_col">
              <div className="footer_shipping_details_wrapper">
                <span className="footer_shipping_span">
                  <i className="m-icon return-icon"></i>
                </span>
                <div className="footer_shipping_item">
                  <p className="footer_shipping_name">EASY RETURNS</p>
                  <p className="footer_shipping_order-details">
                    15-Day Return Policy
                  </p>
                </div>
              </div>
            </div>
            <div className="footer_shipping_col">
              <div className="footer_shipping_details_wrapper">
                <span className="footer_shipping_span">
                  <i className="m-icon authentic-icon"></i>
                </span>
                <div className="footer_shipping_item">
                  <p className="footer_shipping_name">100% AUTHENTIC</p>
                  <p className="footer_shipping_order-details">
                    Products Sourced Directly
                  </p>
                </div>
              </div>
            </div>
            <div className="footer_shipping_col">
              <div className="footer_shipping_details_wrapper">
                <span className="footer_shipping_span">
                  <i className="m-icon brand-icon"></i>
                </span>
                <div className="footer_shipping_item">
                  <p className="footer_shipping_name">1900+ BRANDS</p>
                  <p className="footer_shipping_order-details">
                    1.2 Lakh+ Products
                  </p>
                </div>
              </div>
            </div>
            <div className='footer_shipping_col'>
              <div className='footer_shipping_details_wrapper'>
                <div className='footer_shipping_social'>
                  <p className='footer_shipping_order-details'>
                    Show us some love ❤ on social media
                  </p>
                  <Link href="#." className="footer_social_link">
                    <Image
                      src="/images/instagram.png"
                      width="0"
                      height="0"
                      sizes="100vw"
                      className="footer_social_icon"
                    />
                  </Link>
                  <Link href="#." className="footer_social_link">
                    <Image
                      src="/images/Vector.png"
                      width="0"
                      height="0"
                      sizes="100vw"
                      className="footer_social_icon"
                    />
                  </Link>
                  <Link href="#." className="footer_social_link">
                    <Image
                      src="/images/facebook.png"
                      width="0"
                      height="0"
                      sizes="100vw"
                      className="footer_social_icon"
                    />
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div> */}
      <hr />
      <div className='footer_policy_details'>
        <div className='site-container'>
          {/* <div className="footer_policy_wrapper">
            <Link href="#." className="footer_policy_link">
              Terms & Conditions
            </Link>
            <Link href="#." className="footer_policy_link">
              Shipping
            </Link>
            <Link href="#." className="footer_policy_link">
              Policy Cancellation
            </Link>
            <Link href="#." className="footer_policy_link">
              Privacy Policy
            </Link>
          </div> */}
          <div className='footer_copy-right'>
            <p>
              {' '}
              &#169; {year} {_projectName_} Limited. All Right Reseved
            </p>
            <div>
              <Link
                href={'https://www.hashtechy.com/'}
                className='footer_hash_logo'
                target={'_blank'}
              >
                Site Designed By
                <img
                  src='/images/hashtechy.png'
                  alt=''
                  className='footer_powered_logo'
                />
              </Link>
            </div>
          </div>
        </div>
      </div>
    </footer>
  )
}

export default Footer
