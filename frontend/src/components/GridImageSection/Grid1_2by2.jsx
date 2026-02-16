import React from 'react'
import SingleImage from './SingleImage'
import DoubleImage from './DoubleImage'

const Grid1_2by2 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_1-2by2'>
      <SingleImage data={section?.columns?.left?.single ?? []} />
      <DoubleImage data={section?.columns?.right?.top ?? []} />
      <DoubleImage data={section?.columns?.right?.bottom ?? []} />
    </div>
  )
}

export default Grid1_2by2
