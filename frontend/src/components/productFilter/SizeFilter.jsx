import React from 'react'
import SearchBar from '../base/SearchBar'
import IpCheckBox from '../base/IpCheckBox'

const SizeFilter = ({
  filterList,
  filtersObj,
  toggleOpen,
  handleSearch,
  searchText,
  handleFilterFunc
}) => {
  return (
    <li className='m-prd-slidebar__item is-open' id='id-size'>
      <a
        className='m-prd-slidebar__name'
        onClick={() => {
          toggleOpen('id-size')
        }}
      >
        Size
        <i className='m-icon m-prdlist-icon'></i>
      </a>
      <ul className='m-sub-prdlist'>
        <li className='m-sub-prditems'>
          <a className='m-sub-prdname'>
            <SearchBar
              onChange={(e) => {
                handleSearch(
                  e?.target?.value,
                  'size',
                  'size_filter',
                  'filteredSize'
                )
              }}
              id={'size'}
              placeholder={'search sizes'}
              value={searchText}
            />
          </a>

          {filterList?.length > 0 ? (
            filterList?.map((size) => (
              <a className='m-sub-prdname' key={size?.sizeID}>
                <IpCheckBox
                  id={size?.sizeID}
                  label={size?.size}
                  onChange={() => {
                    handleFilterFunc('SizeIds', size?.sizeID)
                  }}
                  checked={
                    filtersObj?.SizeIds?.length > 0 &&
                    Array.isArray(filtersObj?.SizeIds) &&
                    filtersObj?.SizeIds?.find((item) => item === size?.sizeID)
                  }
                />
              </a>
            ))
          ) : (
            <span className='pv-m-subprdctnobrand'>No Size available</span>
          )}
        </li>
      </ul>
    </li>
  )
}

export default SizeFilter
