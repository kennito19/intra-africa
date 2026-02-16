import React from 'react'
import Image from 'next/image'

const MainGrid = () => {
  return (
    <>
      <div className='grid_1-2by1 '>
        <div className='grid-item-image'>
          <Image
            src='/images/grid-img1.png'
            alt='image'
            className='grid-img'
            width='0'
            height='0'
            sizes='100vw'
          />
        </div>
        <div className='grid-item-image'>
          <Image
            src='/images/grid-img2.png'
            alt='image'
            className='grid-img'
            width='0'
            height='0'
            sizes='100vw'
          />
        </div>
        <div className='grid-item-image'>
          <Image
            src='/images/grid-img3.png'
            alt='image'
            className='grid-img'
            width='0'
            height='0'
            sizes='100vw'
          />
        </div>
        <div className='grid-item-image'>
          <Image
            src='/images/grid-img4.png'
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

export default MainGrid
