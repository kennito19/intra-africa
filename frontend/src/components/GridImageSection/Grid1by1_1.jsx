import React from 'react'
import SingleImage from './SingleImage'

const Grid1by1_1 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid1by1-1'>
      <SingleImage data={section?.columns?.left?.top ?? []} />
      <SingleImage data={section?.columns?.right?.single ?? []} />
      <SingleImage data={section?.columns?.left?.bottom ?? []} />
    </div>
  )
}

export default Grid1by1_1
