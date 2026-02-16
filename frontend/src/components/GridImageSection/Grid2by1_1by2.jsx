import React from 'react'
import SingleImage from './SingleImage'
import DoubleImage from './DoubleImage'

const Grid2by1_1by2 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_2by1-1by2'>
      <DoubleImage data={section?.columns?.left?.top ?? []} />
      <SingleImage data={section?.columns?.right?.top ?? []} />
      <SingleImage data={section?.columns?.left?.bottom ?? []} />
      <DoubleImage data={section?.columns?.right?.bottom ?? []} />
    </div>
  )
}

export default Grid2by1_1by2
