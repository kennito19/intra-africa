import Image from 'next/image'
import Link from 'next/link'
import React from 'react'

const NotFound = ({ title, subTitle }) => {
  const srcNotFound = '/images/nfimage.png'
  return (
    <>
      <div className='site-container'>
        <div className='not_found_main-site'>
          <div className='not_found_7'>
            <h1 className='nf_ff_h1'>404</h1>
            <h5 className='h5_fzf_title'>
              {title ? title : 'This page has been probably moved somewhere...'}
            </h5>
            <p className='cotnt_fzf_ret_mn'>
              {subTitle
                ? subTitle
                : 'Please back to homepage or check our offer'}
            </p>
            <Link href={'/'} className='m-btn btn-buy-now'>
              Back To Home
            </Link>
          </div>
          <div className='not_found_5'>
            <Image
              src={srcNotFound}
              width={200}
              height={200}
              alt='not_found_Image'
            />
          </div>
        </div>
      </div>
    </>
  )
}

export default NotFound
