import React from 'react'
import SingleImage from './SingleImage'
import DoubleImage from './DoubleImage'

const Grid1_1by2 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_1-1by2'>
      <SingleImage data={section?.columns?.left?.single ?? []} />
      <DoubleImage data={section?.columns?.right?.top ?? []} />
      <SingleImage data={section?.columns?.right?.bottom ?? []} />
    </div>
  )
}

export default Grid1_1by2
