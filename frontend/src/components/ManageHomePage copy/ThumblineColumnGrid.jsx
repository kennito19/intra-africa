import Image from 'next/image'
import Link from 'next/link'
import React from 'react'

const ThumblineColumnGrid = ({ data }) => {
  return (
    <div className={`thumblinecolumngrid_wrapper rowgrid_${data[0].colcount}`}>
      {data[0].images.map((item) => (
        <Link
          href={item?.hrefLink}
          className='thumblineLink_grid'
          key={Math.floor(Math.random() * 100000)}
        >
          <Image
            src={item?.thumbimg}
            alt={item?.alt}
            className='thumblinegrid_img'
            width={0}
            height={0}
            quality={100}
            sizes='100vw'
          />
        </Link>
      ))}
    </div>
  )
}

export default ThumblineColumnGrid
