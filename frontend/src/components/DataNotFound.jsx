import React from 'react'
import Link from 'next/link'
import Image from 'next/image'

const DataNotFound = ({ image, heading, description }) => {
  return (
    <div className='data_not_found_wrapper'>
      <Image
        className='data_not_found_img'
        src={image}
        width={100}
        height={100}
        alt='data_not_found_img'
      />
      <h1 className='data_not_found_heading'>{heading}</h1>
      <p className='data_not_found_dec'>{description}</p>
      <div className='empty_fl'>
        <Link href={'/'} className='m-btn btn_shop_now_login'>
          Shop More
        </Link>
      </div>
    </div>
  )
}

export default DataNotFound
