import React from 'react'
import SingleImage from './SingleImage'
import DoubleImage from './DoubleImage'

const Grid1by2_2by1 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_1by2-2by1'>
      <SingleImage data={section?.columns?.left?.top ?? []} />
      <DoubleImage data={section?.columns?.right?.top ?? []} />
      <DoubleImage data={section?.columns?.left?.bottom ?? []} />
      <SingleImage data={section?.columns?.right?.bottom ?? []} />
    </div>
  )
}

export default Grid1by2_2by1
