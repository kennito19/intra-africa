import React from 'react'
import SingleImage from './SingleImage'

const Grid1_3_1 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_1-3-1'>
      <SingleImage data={section?.columns?.left?.single ?? []} />
      <SingleImage data={section?.columns?.center?.top ?? []} />
      <SingleImage data={section?.columns?.right?.single ?? []} />
      <SingleImage data={section?.columns?.center?.single ?? []} />
      <SingleImage data={section?.columns?.center?.bottom ?? []} />
    </div>
  )
}

export default Grid1_3_1
