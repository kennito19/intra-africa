import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const ProductCardSkeleton = ({ isView, productItem }) => {
  const SkeletonRows = () => {
    const skeletonRows = new Array(productItem).fill().map((_, index) => (
      <div
        className={
          isView
            ? isView
              ? 'pd-list__card'
              : 'pd-list__card-gridview'
            : 'pd-list__card'
        }
        key={Math.floor(Math.random() * 100000)}
      >
        <div className='pd-list__img'>
          <Skeleton width={400} height={300} />
        </div>

        <div
          className={
            isView
              ? isView
                ? 'main_prd_fl'
                : 'main_prd_fl active'
              : 'main_prd_fl'
          }
        >
          <div
            className={
              isView
                ? isView
                  ? 'prd-list__details'
                  : 'prd-list__details-gridview'
                : 'prd-list__details'
            }
          >
            <h2 className='prd-list-title'>
              <Skeleton width={115} />
            </h2>
            <p className='prd-list-contains'>
              {' '}
              <Skeleton />
            </p>

            <div className='prd-list-price__wrapper'>
              <h2 className='prd-total-price'>
                <Skeleton width={80} />
              </h2>
              <p className='prd-check-price'>
                <Skeleton width={80} />
              </p>
              <span className='prd-list-offer'>
                <Skeleton width={60} />
              </span>
            </div>

            {!isView && (
              <div className='jp_prdlist_content'>
                <Skeleton height={80} />
              </div>
            )}
          </div>
        </div>
      </div>
    ))

    return <>{skeletonRows}</>
  }
  return <SkeletonRows />
}

export default ProductCardSkeleton
