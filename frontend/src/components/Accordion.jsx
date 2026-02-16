import React, { useState, useEffect, useRef } from 'react'

const Accordion = ({
  accordiontitle,
  accordioncontent,
  activeDefault,
  customParentAccordionclassnm
}) => {
  const [active, setActive] = useState(activeDefault)
  const contentRef = useRef(null)
  const [contentHeight, setContentHeight] = useState(null)

  useEffect(() => {
    if (contentRef.current) {
      setContentHeight(contentRef.current.scrollHeight)
    }
  }, [accordioncontent])

  const toggleAccordion = () => {
    setActive((prevActive) => !prevActive)
  }

  useEffect(() => {
    setActive(activeDefault)
  }, [activeDefault])

  return (
    <div className={customParentAccordionclassnm || 'accordion'}>
      <div className={`accordion_item ${active ? 'active' : ''}`}>
        <button className='accordion_title' onClick={toggleAccordion}>
          <span>{accordiontitle || 'Default Title Name'}</span>
        </button>
        <div
          className={`accordion_content_wrapper ${active ? 'show' : ''}`}
          ref={contentRef}
          style={{
            maxHeight: active ? contentHeight + 'px' : '0'
          }}
        >
          <div className='accordion_content'>{accordioncontent}</div>
        </div>
      </div>
    </div>
  )
}

export default Accordion
