'use client'
import React, { useState, useEffect, useRef } from 'react'

const AccordionCheckout = ({
  id,
  accordiontitle,
  accordioncontent,
  isActive,
  toggleAccordion,
  customParentAccordionclass,
  Content,
  Name,
  index,
  change,
  activeAccordion
}) => {
  const contentRef = useRef(null)
  const [contentHeight, setContentHeight] = useState()
  useEffect(() => {
    if (contentRef.current) {
      setContentHeight(contentRef.current.scrollHeight)
    }
  }, [accordioncontent])

  return (
    <div
      className={customParentAccordionclass || 'accordion-check'}
      id={id && id}
    >
      <div className={`accordion_item_check ${isActive ? 'active' : ''}`}>
        <div className='accordion_title_check'>
          <div>
            <div className='acc_t_fl'>
              <span className='acc_span'>
                {accordiontitle || 'Default Title Name'}
              </span>
              {!isActive && index <= activeAccordion && (
                <i className='m-icon m_checked'></i>
              )}
            </div>
            {!isActive && index <= activeAccordion && (
              <p className='acc_p_c'>
                {Name} <span>{Content}</span>
              </p>
            )}
          </div>

          {!change && !isActive && index <= activeAccordion && (
            <button className='m-btn' onClick={toggleAccordion}>
              Change
            </button>
          )}
        </div>
        <div
          className={`accordion_content_wrapper_check ${
            isActive ? 'show' : ''
          }`}
          ref={contentRef}
          style={{
            maxHeight: isActive ? contentHeight + 200 + 'px' : '0'
          }}
        >
          <div className='accordion_content_check'>{accordioncontent}</div>
        </div>
      </div>
    </div>
  )
}

export default AccordionCheckout
