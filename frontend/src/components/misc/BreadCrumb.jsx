import Link from 'next/link'
import React from 'react'

const BreadCrumb = ({ items, brandName }) => {
  return (
    <div className='breadcrumb_wrapper'>
      {items &&
        items?.length > 0 &&
        items?.map((item, index) => (
          <span key={index}>
            {item?.link ? (
              <Link href={item?.link}>{item?.text}</Link>
            ) : (
              <span>{item?.text}</span>
            )}
            {items?.length - 1 !== index && (
              <span className='breadcrumb_seprator'>/</span>
            )}
          </span>
        ))}
      {brandName && <span className='breadcrumb_seprator'>/ {brandName}</span>}
    </div>
  )
}

export default BreadCrumb
