import React from 'react'
import Image from 'next/image'
import Link from 'next/link'
import Slider from './Slider'
import { buildResourceImageUrl, imagePlaceholderUrl } from '../lib/GetBaseUrl'

function NewTopBrands({ images }) {
  return (
    <>
      <div className='m-brand-wrapper'>
        <Slider
          spaceBetween={50}
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
                className='brand-logo'
                key={Math.floor(Math.random() * 100000)}
              >
                <Image
                  src={buildResourceImageUrl(imageObj?.link)}
                  alt='image'
                  className='grid-img'
                  width={300}
                  height={300}
                  onError={(event) => {
                    event.currentTarget.src = imagePlaceholderUrl
                  }}
                />
              </Link>
            )
          })}
        </Slider>
      </div>

      <div className='grid_1-2by1__50 '>
        <div className='grid-item-image'>
          <Image
            src='/images/brand1new.png'
            alt='image'
            className='grid-img'
            width='0'
            height='0'
            sizes='100vw'
          />
        </div>

        <div className='grid-item-image'>
          <Image
            src='/images/brand2.png'
            alt='image'
            className='grid-img'
            width='0'
            height='0'
            sizes='100vw'
          />
        </div>
        <div className='grid-item-image'>
          <Image
            src='/images/brand3.png'
            alt='image'
            className='grid-img'
            width='0'
            height='0'
            sizes='100vw'
          />
        </div>
        <div className='grid-item-image'>
          <Image
            src='/images/brand4.png'
            alt='image'
            className='grid-img'
            width='0'
            height='0'
            sizes='100vw'
          />
        </div>
      </div>
    </>
  )
}

export default NewTopBrands
