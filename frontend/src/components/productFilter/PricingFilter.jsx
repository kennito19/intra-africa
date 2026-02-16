import React, { useState } from 'react'
import { useParams, useSearchParams } from 'next/navigation'
import { Slider } from '@nextui-org/react'

const PricingFilter = ({
  productData,
  filterList,
  filtersObj,
  toggleOpen,
  setFiltersObj,
  changeUrl,
  value,
  setValue,
  onSubmit
}) => {
  const params = useParams()
  const searchQuery = useSearchParams()

  return (
    <li className='m-prd-slidebar__item is-open' id='id-price'>
      <a
        className='m-prd-slidebar__name'
        onClick={() => {
          toggleOpen('id-price')
        }}
      >
        Price
        <i className='m-icon m-prdlist-icon'></i>
      </a>
      <ul className='m-sub-prdlist'>
        <li className='m-sub-prditems'>
          {searchQuery.get('MinPrice') && searchQuery.get('MaxPrice') && (
            <div className='any-discount-value'>
              <button
                onClick={() => {
                  let updatedFiltersObj = {
                    ...filtersObj,
                    MinPrice: '',
                    MaxPrice: ''
                  }
                  setFiltersObj(updatedFiltersObj)
                  changeUrl(updatedFiltersObj, params?.categoryName)
                }}
                className='badge-danger'
              >
                Clear
              </button>
            </div>
          )}

          <div className='m-price-range__wrapper'>
            <div className='pv-price-range'>
              <Slider
                aria-label={`Select a value between ${filterList?.minSellingPrice} and ${filterList?.maxSellingPrice}`}
                formatOptions={{ style: 'currency', currency: 'INR' }}
                step={1}
                size='sm'
                maxValue={filterList?.maxSellingPrice}
                minValue={filterList?.minSellingPrice}
                value={[value[0] ? value[0] : 0, value[1] ? value[1] : 0]}
                onChange={(newValue) => {
                  const findSecondMin = (data) => {
                    const minPrice = Math.min(
                      ...data.map((item) => item.sellingPrice)
                    )
                    const valuesWithoutMinPrice = data.filter(
                      (item) => item.sellingPrice !== minPrice
                    )
                    return Math.min(
                      ...valuesWithoutMinPrice.map((item) => item.sellingPrice)
                    )
                  }

                  const findSecondMax = (data) => {
                    const maxPrice = Math.max(
                      ...data.map((item) => item.sellingPrice)
                    )
                    const valuesWithoutMaxPrice = data.filter(
                      (item) => item.sellingPrice !== maxPrice
                    )
                    return Math.max(
                      ...valuesWithoutMaxPrice.map((item) => item.sellingPrice)
                    )
                  }
                  const secondMinPrice = findSecondMin(productData)
                  const secondMaxPrice = findSecondMax(productData)

                  if (newValue[1] < secondMinPrice) {
                    setValue([newValue[0], secondMinPrice])
                  } else {
                    setValue(newValue)
                  }
                }}
                isDisabled={
                  filterList?.minSellingPrice === filterList?.maxSellingPrice
                }
              />
            </div>
            <div className='m-price-range-input__wrapper'>
              <input
                type='text'
                placeholder='min'
                name='minPrice'
                className='m-price-range__ip'
                value={value[0]}
                onChange={(e) => {
                  const inputValue = e?.target?.value
                  const isValidInput = /^\d*\.?\d*$/.test(inputValue)
                  const minValue = isValidInput ? inputValue : ''
                  if (
                    minValue !== '' &&
                    parseFloat(minValue) >= filterList?.minSellingPrice &&
                    parseFloat(minValue) <= filterList?.maxSellingPrice
                  ) {
                    setValue([minValue, value[1]])
                  }
                }}
              />

              <input
                type='text'
                placeholder='max'
                name='maxPrice'
                className='m-price-range__ip'
                value={value[1]}
                onChange={(e) => {
                  const inputValue = e?.target?.value
                  const isValidInput = /^\d*\.?\d*$/.test(inputValue)
                  const maxValue = isValidInput ? inputValue : ''
                  if (
                    maxValue !== '' &&
                    parseFloat(maxValue) >= filterList?.minSellingPrice &&
                    parseFloat(maxValue) <= filterList?.maxSellingPrice
                  ) {
                    setValue([value[0], maxValue])
                  }
                }}
              />
            </div>

            <div className='m-price-range-btn'>
              <button
                type='submit'
                onClick={onSubmit}
                className='btn-price-save'
              >
                GO
              </button>
            </div>
          </div>
        </li>
      </ul>
    </li>
  )
}

export default PricingFilter
