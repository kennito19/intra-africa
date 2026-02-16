import IpCheckBox from '../base/IpCheckBox'
import SearchBar from '../base/SearchBar'

const BrandFilter = ({
  filterList,
  filtersObj,
  toggleOpen,
  handleSearch,
  searchText,
  handleFilterFunc
}) => {
  return (
    <li className='m-prd-slidebar__item is-open' id='id-brands'>
      <a
        className='m-prd-slidebar__name'
        onClick={() => {
          toggleOpen('id-brands')
        }}
      >
        Brands
        <i className='m-icon m-prdlist-icon'></i>
      </a>
      <ul className='m-sub-prdlist'>
        <li className='m-sub-prditems'>
          <a className='m-sub-prdname'>
            <SearchBar
              onChange={(e) => {
                handleSearch(
                  e?.target?.value,
                  'brandName',
                  'brand_filter',
                  'filteredBrand'
                )
              }}
              id='brand'
              value={searchText}
              placeholder={'Search brands'}
            />
          </a>
          {filterList?.length > 0 ? (
            filterList?.map((brand) => (
              <a className='m-sub-prdname' key={brand?.brandId}>
                <IpCheckBox
                  id={brand?.brandId}
                  label={brand?.brandName}
                  onChange={() => {
                    if (brand?.brandId) {
                      handleFilterFunc('BrandIds', brand?.brandId)
                    }
                  }}
                  checked={
                    filtersObj?.BrandIds?.length > 0 &&
                    Array.isArray(filtersObj?.BrandIds) &&
                    filtersObj?.BrandIds?.find(
                      (item) => item === brand?.brandId
                    )
                  }
                />
              </a>
            ))
          ) : (
            <span className='pv-m-subprdctnobrand'>No brands available</span>
          )}
        </li>
      </ul>
    </li>
  )
}

export default BrandFilter
