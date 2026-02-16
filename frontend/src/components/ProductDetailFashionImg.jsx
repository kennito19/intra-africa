import React, { useEffect, useState } from 'react'
import 'lightbox.js-react/dist/index.css'
import { SlideshowLightbox } from 'lightbox.js-react'
import Image from 'next/image'
import Slider from './Slider'

const ProductDetailFashionImg = () => {
  const images = [
    {
      src: '/images/product/product-1.jpg',
      alt: 'product-1'
    },
    {
      src: '/images/product/product-2.jpg',
      alt: 'product-2'
    },
    {
      src: '/images/product/product-3.jpg',
      alt: 'product-3'
    },
    {
      src: '/images/product/product-4.jpg',
      alt: 'product-4'
    },
    {
      src: '/images/product/product-1.jpg',
      alt: 'product-5'
    },
    {
      src: '/images/product/product-3.jpg',
      alt: 'product-6'
    }
  ]

  const [isMobile, setIsMobile] = useState(false)

  const handleResize = () => {
    setIsMobile(window.innerWidth <= 1024)
  }

  useEffect(() => {
    // Check initial screen size
    setIsMobile(window.innerWidth <= 1024)

    // Add event listener for window resize
    window.addEventListener('resize', handleResize)

    // Clean up the event listener on component unmount
    return () => {
      window.removeEventListener('resize', handleResize)
    }
  }, [])

  return (
    <div className='prdct_details_images_wrapper'>
      {isMobile ? (
        <SlideshowLightbox
          lightboxIdentifier='lightbox1'
          framework='next'
          showThumbnails={true}
          images={images}
        >
          <Slider
            spaceBetween={10}
            slidesPerView={1}
            loop={false}
            withArrows={false}
            showShareBtn={true}
            autoplay={3000}
            navigation={false}
            pagination={true}
          >
            {images.map((image, index) => (
              <Image
                src={image.src}
                alt={image.alt}
                height={0}
                width={0}
                data-lightboxjs='lightbox1'
                quality={100}
                sizes='100vw'
                className='prdt_images_lightbox'
                key={Math.floor(Math.random() * 100000)}
              />
            ))}
          </Slider>
        </SlideshowLightbox>
      ) : (
        <SlideshowLightbox
          lightboxIdentifier='lightbox1'
          framework='next'
          images={images}
          showThumbnails={true}
          className='prdct_details_images_gridview'
        >
          {images.map((image, index) => (
            <div key={Math.floor(Math.random() * 100000)}>
              <Image
                src={image.src}
                alt={image.alt}
                height={0}
                width={0}
                data-lightboxjs='lightbox1'
                quality={100}
                sizes='100vw'
                className='prdt_images_lightbox'
              />
            </div>
          ))}
        </SlideshowLightbox>
      )}
    </div>
  )
}

export default ProductDetailFashionImg
