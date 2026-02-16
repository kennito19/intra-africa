import React from 'react'
import SingleImage from './SingleImage'

const Grid2_1_2 = ({ layoutsInfo, section }) => {
  return (
    <div className='grid_2-1-2'>
      <SingleImage data={section?.columns?.left?.top ?? []} />
      <SingleImage data={section?.columns?.center?.single ?? []} />
      <SingleImage data={section?.columns?.left?.bottom ?? []} />
      <SingleImage data={section?.columns?.right?.top ?? []} />
      <SingleImage data={section?.columns?.right?.bottom ?? []} />
    </div>
  )
}

export default Grid2_1_2
