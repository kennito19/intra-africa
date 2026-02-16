import React from 'react'
import DoubleImage from './DoubleImage'

const Grid_col_four = ({ layoutsInfo, section }) => {
  const images = [
    {
      id: 1,
      thumbimg: '/images/grid10.jpg'
    },
    {
      id: 2,
      thumbimg: '/images/grid9.jpg'
    },
    {
      id: 3,
      thumbimg: '/images/grid2.jpg'
    }
  ]

  return (
    <div className='grid_column_four'>
      <DoubleImage data={images} />
      <DoubleImage data={images} />
      <DoubleImage data={images} />
      <DoubleImage data={images} />
    </div>
  )
}

export default Grid_col_four
