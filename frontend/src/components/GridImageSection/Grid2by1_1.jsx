import React from 'react'
import SingleImage from './SingleImage'
import DoubleImage from './DoubleImage'

const Grid2by1_1 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_2by1-1'>
      <DoubleImage data={section?.columns?.left?.top ?? []} />
      <SingleImage data={section?.columns?.right?.single ?? []} />
      <SingleImage data={section?.columns?.left?.bottom ?? []} />
    </div>
  )
}

export default Grid2by1_1
