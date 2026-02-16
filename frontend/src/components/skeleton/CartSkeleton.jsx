import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const CartSkeleton = ({ modalShow, setModalShow }) => {
  return (
    <div className='small-container'>
      <div className='add-cart-compont'>
        <div className='d-flex'>
          <div className={''}>
          <Skeleton width='764px' height='594px' borderRadius={10} />
          </div>
        </div>
        <div className='cart-side-compont'>
          <Skeleton width='437px' height='94px' borderRadius={10} />
          <div className='price_details'>
            <Skeleton width='437px' height='330px' borderRadius={10} />

            <Skeleton
              className='my-2'
              width='437px'
              height='38px'
              borderRadius={10}
            />
            <Skeleton width='437px' height='42px' borderRadius={10} />
          </div>
        </div>
      </div>
    </div>
  )
}

export default CartSkeleton
