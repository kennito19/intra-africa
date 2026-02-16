import React from 'react'
import SingleImage from './SingleImage'

const Grid3_1_3 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_3-1-3'>
      <SingleImage data={section?.columns?.left?.top ?? []} />
      <SingleImage data={section?.columns?.center?.single ?? []} />
      <SingleImage data={section?.columns?.left?.single ?? []} />
      <SingleImage data={section?.columns?.left?.bottom ?? []} />
      <SingleImage data={section?.columns?.right?.top ?? []} />
      <SingleImage data={section?.columns?.right?.single ?? []} />
      <SingleImage data={section?.columns?.right?.bottom ?? []} />
    </div>
  )
}

export default Grid3_1_3
