import React from 'react'
import SingleImage from './SingleImage'
import DoubleImage from './DoubleImage'

const Grid1by2_1 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_1by2-1'>
      <SingleImage data={section?.columns?.left?.top ?? []} />
      <SingleImage data={section?.columns?.right?.single ?? []} />
      <DoubleImage data={section?.columns?.left?.bottom ?? []} />
    </div>
  )
}

export default Grid1by2_1
