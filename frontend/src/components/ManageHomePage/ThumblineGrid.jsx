import React from 'react'
import Image from 'next/image'
import Link from 'next/link'

const ThumblineGrid = (props) => {
  // Destructuring props
  const { data, hideExtra } = props

  // Calculate the number of columns dynamically based on the image length
  const calculatedCols = Math.min(Math.max(data.length), 7)

  const gridClassName = `rowgrid_${calculatedCols}`

  const visibleImages = hideExtra ? data.slice(0, calculatedCols) : data

  return (
    <div className={`thumbline_imggrid ${gridClassName}`}>
      {visibleImages.map((data) => (
        <Link
          href={data?.hrefLink}
          className='thumblineLink_grid'
          key={Math.floor(Math.random() * 100000)}
        >
          <Image
            src={data?.thumbimg}
            alt={data?.alt}
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

export default ThumblineGrid
