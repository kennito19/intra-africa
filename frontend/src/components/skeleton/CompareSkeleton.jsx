import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const CompareSkeleton = () => {
  return (
    <div className='pv-compare-main'>
      <div className='pv-compare-table flex flex-row jsu'>
        <Skeleton
          className='mr-2'
          width='297px'
          height='700px'
          borderRadius={10}
        />
          <Skeleton height='700px' width='1189px' borderRadius={10} />
      </div>
    </div>
  )
}

export default CompareSkeleton
