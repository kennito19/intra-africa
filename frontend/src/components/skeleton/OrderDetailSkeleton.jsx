import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const OrderDetailSkeleton = () => {
  const SkeletonRows = () => {
    const skeletonRows = new Array(2).fill().map((_, index) => (
      <div
        className='order-product-detail'
        key={Math.floor(Math.random() * 100000)}
      >
        <div className='order-product-detail-image'>
          <Skeleton height={150} />
        </div>
        <div className='order-productdetail-info'>
          <p>
            <Skeleton />
          </p>

          <span className='product-size-color'>
            <Skeleton width={100} />
          </span>

          <span className=''>
            <Skeleton width={50} />
          </span>

          <div className='pv-order-mrp'>
            <span>
              <Skeleton width={70} />
            </span>
            <span>
              <div className='prd-list-price__wrapper'>
                <h2 className='prd-total-price'>
                  {' '}
                  <Skeleton width={40} />
                </h2>
                <p className='prd-check-price'>
                  {' '}
                  <Skeleton width={70} />
                </p>
                <span className='prd-list-offer'>
                  <Skeleton width={40} />
                </span>
              </div>
            </span>
          </div>
          <dl>
            <Skeleton width={100} />
          </dl>
        </div>
      </div>
    ))

    return <>{skeletonRows}</>
  }
  return (
    <div>
      <div className='order-details-main'>
        <div className='order-detail-title-main'>
          <div className='breadcrumb_wrapper'>
            <Skeleton />
          </div>
          <div className='order-detail-title'>
            <h1 className='od-title'>
              <Skeleton width={150} />
            </h1>
            <div className='order-date'>
              <span>
                <Skeleton width={200} />
              </span>
              <p>
                <Skeleton width={350} />
              </p>
            </div>
          </div>
        </div>
        <div className='pv-order-listmain'>
          <div className='order-seller-Items'>
            <SkeletonRows />
          </div>
        </div>

        <div className='shipping-main'>
          <div className='payment'>
            <h2 className='payment-title'>
              <Skeleton width={150} />
            </h2>
            <Skeleton width={70} />
          </div>
          <div className='order-summary'>
            <h2 className='order-title'>
              <Skeleton width={130} />
            </h2>
            <table>
              <tbody>
                <tr>
                  <Skeleton />
                </tr>
                <tr>
                  <Skeleton />
                </tr>
                <tr>
                  <Skeleton />
                </tr>
                <tr>
                  <Skeleton />
                </tr>
              </tbody>
            </table>
          </div>
          <div className='ship-address'>
            <h2 className='ship-title'>
              <Skeleton width={130} />
            </h2>
            <span>
              <Skeleton width={90} />
            </span>
            <p>
              <Skeleton height={40} />
            </p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default OrderDetailSkeleton
