import React from 'react'
import IpCheckBox from '../base/IpCheckBox'
import SearchBar from '../base/SearchBar'

const ColourFilter = ({
  filterList,
  filtersObj,
  toggleOpen,
  handleSearch,
  searchText,
  handleFilterFunc
}) => {
  return (
    <li className='m-prd-slidebar__item is-open' id='id-color'>
      <a
        className='m-prd-slidebar__name'
        onClick={() => {
          toggleOpen('id-color')
        }}
      >
        Color
        <i className='m-icon m-prdlist-icon'></i>
      </a>
      <ul className='m-sub-prdlist'>
        <li className='m-sub-prditems'>
          <a className='m-sub-prdname'>
            <SearchBar
              onChange={(e) => {
                handleSearch(
                  e?.target?.value,
                  'colorName',
                  'color_filter',
                  'filteredColor'
                )
              }}
              id='colour'
              value={searchText}
              placeholder={'Search colors'}
            />
          </a>

          {filterList?.length > 0 ? (
            filterList?.map((color) => (
              <a className='m-sub-prdname' key={color?.colorId}>
                <IpCheckBox
                  id={color?.colorId}
                  label={color?.colorName}
                  showColor={true}
                  colorClass={color?.colorCode}
                  onChange={() => {
                    handleFilterFunc('ColorIds', color?.colorId)
                  }}
                  checked={
                    filtersObj?.ColorIds?.length > 0 &&
                    Array.isArray(filtersObj?.ColorIds) &&
                    filtersObj?.ColorIds?.find(
                      (item) => item === color?.colorId
                    )
                  }
                />
              </a>
            ))
          ) : (
            <span className='pv-m-subprdctnobrand'>No Color available</span>
          )}
        </li>
      </ul>
    </li>
  )
}

export default ColourFilter
