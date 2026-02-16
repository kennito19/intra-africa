import React from 'react'
import Image from 'next/image'
import Slider from './Slider'
import Link from 'next/link'

const SummerCollection = ({ images }) => {
  return (
    <Slider
      spaceBetween={20}
      slidesPerView={3}
      loop={false}
      autoplay={3000}
      navigation={true}
      pagination={false}
      breakpoints={{
        0: {
          slidesPerView: 1
        },
        489: {
          slidesPerView: 2
        },
        768: {
          slidesPerView: 3
        }
      }}
    >
      {images?.map((imageObj) => {
        return (
          <Link
            href='/'
            className='summber_collection_link'
            key={Math.floor(Math.random() * 100000)}
          >
            <Image
              src={imageObj.link}
              alt='image'
              className='summber_collection_img'
              width='0'
              height='0'
              sizes='100vw'
            />
          </Link>
        )
      })}
    </Slider>
  )
}

export default SummerCollection
