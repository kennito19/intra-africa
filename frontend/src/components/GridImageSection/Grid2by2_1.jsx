import React from 'react'
import SingleImage from './SingleImage'
import DoubleImage from './DoubleImage'

const Grid2by2_1 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_2by2-1'>
      <DoubleImage data={section?.columns?.left?.top ?? []} />
      <DoubleImage data={section?.columns?.left?.bottom ?? []} />
      <SingleImage data={section?.columns?.right?.single ?? []} />
    </div>
  )
}

export default Grid2by2_1
