import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const ProductFilterSkeleton = ({ searchBar = true }) => {
  const SkeletonRows = () => {
    const skeletonRows = new Array(6)
      .fill()
      .map((_, index) => (
        <Skeleton key={index} height={12} width={120} borderRadius={5} />
      ))

    return <>{skeletonRows}</>
  }
  return (
    <li className='m-prd-slidebar__item is-open' id='id-brands'>
      <a className='m-prd-slidebar__name'>
        <Skeleton height={25} width={170} borderRadius={5} />
      </a>
      <ul className='m-sub-prdlist'>
        <li className='m-sub-prditems'>
          {searchBar && (
            <a className='m-sub-prdname'>
              <Skeleton height={25} width='100%' borderRadius={5} />
            </a>
          )}
          <SkeletonRows />
        </li>
      </ul>
    </li>
  )
}

export default ProductFilterSkeleton
