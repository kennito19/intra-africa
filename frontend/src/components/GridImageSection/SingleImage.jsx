import Image from 'next/image'
import Link from 'next/link'
import React from 'react'
import Slider from '../Slider'
import { SwiperSlide } from 'swiper/react'
import { _homePageImg_ } from '../../lib/ImagePath'
import { checkCase, reactImageUrl } from '../../lib/GetBaseUrl'

const SingleImage = ({ data }) => {
  const renderComponent = (card) => {
    if (card)
      return (
        <Link
          href={checkCase(card)}
          target={card?.redirect_to === 'Custom link' ? '_blank' : '_self'}
          key={Math.floor(Math.random() * 100000)}
        >
          <Image
            src={
              card &&
              encodeURI(`${reactImageUrl}${_homePageImg_}${card?.image}`)
            }
            alt={card?.image_alt}
            className='w-f'
            width={0}
            height={0}
            quality={100}
            sizes='100vw'
          />
        </Link>
      )
  }

  return (
    <>
      {data?.length > 1 ? (
        <div className='brand-slider-wrapper  arrow-relative'>
          <Slider
            slidesPerView={1}
            loop={true}
            pagination={true}
            autoplay={3000}
          >
            {data?.map((card) => (
              <SwiperSlide key={Math.floor(Math.random() * 100000)}>
                {renderComponent(card)}
              </SwiperSlide>
            ))}
          </Slider>
        </div>
      ) : (
        <div>{renderComponent(data[0])}</div>
      )}
    </>
  )
}

export default SingleImage
