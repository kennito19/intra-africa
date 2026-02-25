import Image from 'next/image'
import Link from 'next/link'
import React from 'react'
import { _homePageImg_ } from '../../lib/ImagePath'
import {
  buildResourceImageUrl,
  checkCase,
  imagePlaceholderUrl
} from '../../lib/GetBaseUrl'

const DoubleImage = ({ data }) => {
  const renderComponent = (card) => {
    if (card)
      return (
        <Link
          href={checkCase(card)}
          target={card?.redirect_to === 'Custom link' ? '_blank' : '_self'}
          key={card?.id}
        >
          <Image
            src={buildResourceImageUrl(card?.image, _homePageImg_)}
            alt={card?.image_alt}
            className='w-f'
            width={0}
            height={0}
            quality={100}
            sizes='100vw'
            onError={(event) => {
              event.currentTarget.src = imagePlaceholderUrl
            }}
          />
        </Link>
      )
  }

  const imagesToRender = data.slice(0, 2)

  return (
    <>
      {imagesToRender.map((card) => (
        <div key={Math.floor(Math.random() * 100000)}>
          {renderComponent(card)}
        </div>
      ))}
    </>
  )
}

export default DoubleImage
