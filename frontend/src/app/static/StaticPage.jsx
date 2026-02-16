// import React from 'react'
'use client'

const StaticPage = ({ staticData }) => {
  return (
    staticData?.data && (
      <div className='site-container section_spacing_b'>
        <div
          dangerouslySetInnerHTML={{ __html: staticData?.data?.pageContent }}
          className='pv-static-pagemain'
        ></div>
      </div>
    )
  )
}

export default StaticPage
