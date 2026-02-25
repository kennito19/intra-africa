import Image from 'next/image'
import React from 'react'
import Slider from '../Slider'
import {
  buildResourceImageUrl,
  imagePlaceholderUrl
} from '../../lib/GetBaseUrl'
import { _homePageImg_ } from '../../lib/ImagePath'

const Testimonial = ({ card }) => {
  return (
    <div className='pv-testimonial-main'>
      <div className='site-container '>
        {/* <Slider
          spaceBetween={10}
          withArrows={true}
          loop={true}
          autoplay={false}
          navigation={false}
          pagination={false}
          slidesPerView={2}
          breakpoints={{
            0: {
              slidesPerView: 1
            },
            // 479: {
            //   slidesPerView: 1,
            // },
            768: {
              slidesPerView: 2
            },
            1024: {
              slidesPerView: 3
            }
          }}
        > */}
        <div className='pv-testimonial-inner'>
          <Image
            src={buildResourceImageUrl(card?.image, _homePageImg_)}
            alt={card?.image_alt ? card?.image_alt : 'image'}
            className='testimonial_box-img'
            width={300}
            height={300}
            onError={(event) => {
              event.currentTarget.src = imagePlaceholderUrl
            }}
          />
          <div
            className='testimonial_box-name'
            dangerouslySetInnerHTML={{ __html: card?.title }}
          ></div>
          <div
            className='testimonial_box-job'
            dangerouslySetInnerHTML={{ __html: card?.sub_title }}
          ></div>
          <div
            className='testimonial_box-text'
            dangerouslySetInnerHTML={{ __html: card?.description }}
          ></div>
        </div>
        {/* </Slider> */}
      </div>
    </div>
  )
}

export default Testimonial
