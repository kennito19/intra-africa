import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const ProductViewSkeleton = () => {
  return (
    <div className='p-prdlist-right-header__wrapper'>
      <Skeleton height={25} width={1150} />
    </div>
  )
}

export default ProductViewSkeleton
