import React, { useEffect, useRef, useState } from 'react'

const PrdtDetailContent = ({ values }) => {
  const toggleOpen = (id) => {
    const topEl = document.getElementById(`details_${id}`)
    const isOpen = topEl.classList.contains('is-open')

    if (isOpen) {
      topEl.classList.remove('is-open')
    } else {
      topEl.classList.add('is-open')
    }
  }

  const ShowMoreLessHTML = ({ htmlContent, limit = 250 }) => {
    const [showAll, setShowAll] = useState(false)
    const [hasInteracted, setHasInteracted] = useState(false)
    const contentRef = useRef(null)
    const showMoreButtonRef = useRef(null)
    const savedScrollPositionRef = useRef(0)

    const toggleShowAll = () => {
      setHasInteracted(true)
      setShowAll(!showAll)
    }

    useEffect(() => {
      if (!showAll && hasInteracted) {
        window.scrollTo({
          top: savedScrollPositionRef.current,
          behavior: 'smooth'
        })
      } else {
        savedScrollPositionRef.current = window.scrollY
      }
    }, [showAll, hasInteracted])

    const isLongText = htmlContent.length > limit
    const truncatedText = isLongText
      ? htmlContent.substring(0, limit)
      : htmlContent
    const displayedHTML = showAll ? htmlContent : truncatedText

    return (
      <div>
        <div
          ref={contentRef}
          dangerouslySetInnerHTML={{ __html: displayedHTML }}
        />
        {isLongText && (
          <button
            onClick={toggleShowAll}
            className='pv-content-morebtn'
            ref={showMoreButtonRef}
          >
            {showAll ? 'Show Less' : 'Show More'}
          </button>
        )}
      </div>
    )
  }

  return (
    <>
      <ul className='m-prd-sidebar__list'>
        <li className='m-prd-slidebar__item is-open' id={`details_${0}`}>
          <a
            className='m-prd-slidebar__name'
            onClick={() => {
              toggleOpen(0)
            }}
          >
            PRODUCT DETAILS
            <i className='m-icon m-prdlist-icon'></i>
          </a>
          <ul className='m-sub-prdlist m_pad_add'>
            <li className='m-sub-prditems'>
              {values?.description && (
                <div>
                  <h3 className='m-sub-prditems-head'> Description :</h3>
                  {/* <div
                    dangerouslySetInnerHTML={{ __html: values?.description }}
                  ></div> */}
                  <ShowMoreLessHTML htmlContent={values?.description} />
                </div>
              )}
              {values?.highlights && (
                <div>
                  <h3 className='m-sub-prditems-head'> Highlight :</h3>
                  <div
                    dangerouslySetInnerHTML={{ __html: values?.highlights }}
                  ></div>
                </div>
              )}
              <div className='prd_sellar_label'>
                <span className='rd_selectsize_label-seller'>
                  Seller:&nbsp;
                </span>
                {values?.allSizes?.find((item) => item?.isSelected)?.sellerName}
              </div>
            </li>
          </ul>
        </li>
      </ul>
    </>
  )
}

export default PrdtDetailContent
