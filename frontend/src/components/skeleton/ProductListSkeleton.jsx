import React from 'react'
import ProductFilterSkeleton from './ProductFilterSkeleton'
import ProductViewSkeleton from './ProductViewSkeleton'
import ProductCardSkeleton from './ProductCardSkeleton'

const ProductListSkeleton = ({ isView, specificPartRef, isActiveDrawer }) => {
  return (
    <div className='site-container'>
      <div className='p-prdlist__wrapper'>
        <div
          className={
            isActiveDrawer.filterDrawer
              ? 'p-prdlist__sidebar active'
              : 'p-prdlist__sidebar'
          }
          id='p-prdlist__sidebar'
        >
          <div className='m-prd-sidebar' ref={specificPartRef}>
            <ul className='m-prd-sidebar__list'>
              <ProductFilterSkeleton searchBar={false} />
              <ProductFilterSkeleton />
              <ProductFilterSkeleton />
              <ProductFilterSkeleton />
              <ProductFilterSkeleton />
            </ul>
          </div>
          <div className='p-prdlist__products'>
            <ProductViewSkeleton />
            <div
              className={
                isView ? 'p-prdlist-grid__wrapper' : 'p-prdgrid-view__wrapper'
              }
            >
              <ProductCardSkeleton isView={isView} productItem={16} />
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ProductListSkeleton
