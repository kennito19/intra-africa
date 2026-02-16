import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const WishlistSkeleton = () => {
  const skeleton = () => {
    let test = []
    for (let index = 0; index < 11; index++) {
      test?.push(<Skeleton width='100%' height='330px' />)
    }
    return test
  }
  return (
    <div className='wish_inner_80'>
      <div className='order_grid_wishlist'>
        <div className='index-headingDiv'>
          <span className='index-heading'>My Wishlist: </span>
          <span className='index-count'>
            <Skeleton width={55} />
          </span>
        </div>
        <div className='p-prdlist-wishlist-wrapper'>{skeleton()}</div>
      </div>
    </div>
  )
}

export default WishlistSkeleton
