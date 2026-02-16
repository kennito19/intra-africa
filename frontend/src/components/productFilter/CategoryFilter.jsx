import React from 'react'
import IpRadio from '../base/IpRadio'

const CategoryFilter = ({
  filterList,
  filtersObj,
  toggleOpen,
  handleFilterFunc
}) => {
  return (
    <li className='m-prd-slidebar__item is-open' id='id-category'>
      <a
        className='m-prd-slidebar__name'
        onClick={() => {
          toggleOpen('id-category')
        }}
      >
        Category
        <i className='m-icon m-prdlist-icon'></i>
      </a>

      <ul className='m-sub-prdlist'>
        <li className='m-sub-prditems'>
          {filterList &&
            filterList.length > 0 &&
            filterList?.map((category) => (
              <a className='m-sub-prdname' key={category?.categoryId}>
                <IpRadio
                  name={category?.categoryId}
                  id={category?.categoryId}
                  label={category?.categoryName}
                  labelText={category?.categoryName}
                  value={category?.categoryId}
                  onChange={() => {
                    handleFilterFunc(
                      'CategoryId',
                      category?.categoryId,
                      category?.categoryName
                    )
                  }}
                  checked={category?.categoryId === filtersObj?.CategoryId}
                />
              </a>
            ))}
        </li>
      </ul>
    </li>
  )
}

export default CategoryFilter
