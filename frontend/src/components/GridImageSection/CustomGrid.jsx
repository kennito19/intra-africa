import React from 'react'
import Image from 'next/image'
import Link from 'next/link'
import { _homePageDtlImg_, _homePageImg_ } from '../../lib/ImagePath'
import { checkCase, reactImageUrl } from '../../lib/GetBaseUrl'
import Testimonial from '../testimonial/Testimonial'

const CustomGrid = ({ layoutsInfo, section, data = [] }) => {
  return (
    <div className='row-grid' key={section?.section_id}>
      {Object.keys(section?.innerColumnClass || {}).map((innerColumnName) => {
        const columnKey = section?.innerColumnClass[innerColumnName]
        const column = section?.columns && section?.columns[innerColumnName]

        const columnNumberMatch = innerColumnName.match(/\d+/)
        const columnNumber = columnNumberMatch
          ? parseInt(columnNumberMatch[0])
          : null
        if (column && column?.single) {
          return (
            <React.Fragment key={Math.floor(Math.random() * 100000)}>
              {column?.single?.map((card) => (
                <div
                  className={`${card?.col_class}`}
                  key={Math.floor(Math.random() * 100000)}
                >
                  {card?.image && card?.option_name === 'Image' ? (
                    <Link
                      href={checkCase(card)}
                      target={
                        card?.redirect_to === 'Custom link' ? '_blank' : '_self'
                      }
                    >
                      <Image
                        src={
                          card &&
                          encodeURI(
                            `${reactImageUrl}${_homePageImg_}${card?.image}`
                          )
                        }
                        alt={card?.image_alt ?? 'image-alt'}
                        className='custom_gridimg'
                        width={0}
                        height={0}
                        quality={100}
                        sizes='100vw'
                      />
                    </Link>
                  ) : (
                    <Testimonial
                      // fromLendingPage={fromLendingPage}
                      card={card}
                    />
                  )}
                </div>
              ))}
            </React.Fragment>
          )
        }
      })}
    </div>
  )
}

export default CustomGrid
