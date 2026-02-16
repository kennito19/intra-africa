import React from 'react'
import Image from 'next/image'
import Link from 'next/link'
import Slider from './Slider'

function DealOfTheDay({ images }) {
  return (
    <>
      <Slider
        spaceBetween={0}
        loop={true}
        withArrows={false}
        autoplay={true}
        navigation={false}
        pagination={false}
        breakpoints={{
          0: {
            slidesPerView: 2
          },
          479: {
            slidesPerView: 3
          },
          768: {
            slidesPerView: 4
          },
          1024: {
            slidesPerView: 5
          },
          1280: {
            slidesPerView: 7
          }
        }}
      >
        {images?.map((imageObj) => {
          return (
            <Link
              href='/'
              className='grid-item-image'
              key={Math.floor(Math.random() * 100000)}
            >
              <Image
                src={imageObj.link}
                alt='image'
                className='grid-img'
                width={300}
                height={300}
              />
            </Link>
          )
        })}
      </Slider>
    </>
  )
}

export default DealOfTheDay
