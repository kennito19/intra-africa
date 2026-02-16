import React from 'react'
import IpCheckBox from '../base/IpCheckBox'

const SpecificationFilter = ({
  filterList,
  toggleOpen,
  handleFilterFunc,
  filtersObj
}) => {
  return filterList?.map((item) => (
    <li
      className='m-prd-slidebar__item is-open'
      id={`id-${item?.filterTypeName}`}
      key={Math.floor(Math.random() * 100000)}
    >
      <a
        className='m-prd-slidebar__name'
        onClick={() => {
          toggleOpen(`id-${item?.filterTypeName}`)
        }}
      >
        {item?.filterTypeName}
        <i className='m-icon m-prdlist-icon'></i>
      </a>
      <ul className='m-sub-prdlist'>
        <li className='m-sub-prditems'>
          {item && item?.filterValues?.length > 0 && (
            <div>
              {item?.filterValues?.map((filterValue) => (
                <a
                  className='m-sub-prdname'
                  key={Math.floor(Math.random() * 100000)}
                >
                  <IpCheckBox
                    id={filterValue?.filterValueId}
                    label={filterValue?.filterValueName}
                    onChange={() => {
                      handleFilterFunc('SpecTypeValueIds', {
                        specId: item?.filterTypeId,
                        value: filterValue?.filterValueId,
                        valueName: filterValue?.filterValueName
                      })
                    }}
                    checked={
                      filtersObj?.SpecTypeValueIds &&
                      filtersObj?.SpecTypeValueIds?.includes(
                        filterValue?.filterValueId
                      )
                    }
                  />
                </a>
              ))}
            </div>
          )}
        </li>
      </ul>
    </li>
  ))
}

export default SpecificationFilter
