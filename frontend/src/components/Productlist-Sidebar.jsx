import { useParams, useRouter, useSearchParams } from 'next/navigation'
import { useEffect, useState } from 'react'
import { containsKey } from '../lib/AllGlobalFunction'
import { decryptId, stringToIntegerOrArray } from '../lib/GetBaseUrl'
import MBtn from './base/MBtn'
import FilterBadgesWatching from './misc/FilterBadgesWatching'
import BrandFilter from './productFilter/BrandFilter'
import CategoryFilter from './productFilter/CategoryFilter'
import ColourFilter from './productFilter/ColourFilter'
import DiscountFilter from './productFilter/DiscountFilter'
import PricingFilter from './productFilter/PricingFilter'
import SizeFilter from './productFilter/SizeFilter'
import SpecificationFilter from './productFilter/SpecificationFilter'
import ProductFilterSkeleton from './skeleton/ProductFilterSkeleton'

function ProductlistSidebar({
  productData,
  filtersObj,
  setFiltersObj,
  changeUrl,
  searchText,
  handleSearch,
  setIsActiveDrawer,
  isActiveDrawer,
  redirectTo,
  specificPartRef,
  loading
}) {
  const router = useRouter()
  const searchQuery = useSearchParams()
  const params = useParams()
  const { filterList } = productData?.data
  const discounts = [10, 20, 30, 40, 50, 60, 70, 80]
  const [value, setValue] = useState([
    filterList?.minSellingPrice,
    filterList?.maxSellingPrice
  ])
  const toggleOpen = (id) => {
    const topEl = document.getElementById(id)
    const isOpen = topEl.classList.contains('is-open')

    if (isOpen) {
      topEl.classList.remove('is-open')
    } else {
      topEl.classList.add('is-open')
    }
  }

  const handleFilterFunc = async (objKey, id, categoryName) => {
    let filterObj = filtersObj
    switch (objKey) {
      case 'BrandIds': {
        filterObj = { ...filterObj, fby: 'brand' }
        break
      }

      case 'SizeIds': {
        filterObj = { ...filterObj, fby: 'size' }
        break
      }

      case 'ColorIds': {
        filterObj = { ...filterObj, fby: 'color' }
        break
      }

      case 'SpecTypeValueIds': {
        filterObj = { ...filterObj, fby: 'spec' }
        break
      }

      default:
        break
    }
    if (objKey === 'SpecTypeValueIds') {
      let specifications = filterObj?.specifications ?? []
      if (filterObj?.SpecTypeValueIds?.includes(id?.value)) {
        specifications = specifications?.filter(
          (item) => item?.value !== id?.value
        )
      } else {
        specifications = [...specifications, id]
      }
      filterObj = {
        ...filterObj,
        SpecTypeValueIds: specifications?.map((item) => item?.value),
        specifications
      }
    } else {
      const idExistsInFiltersObj = Object.values(filterObj).some((obj) => {
        if (obj === null) {
          return false
        }
        if (Array.isArray(obj)) {
          return obj.includes(id)
        }
        return obj.id === id
      })
      if (!idExistsInFiltersObj) {
        if (filterObj[objKey]?.length > 0) {
          var pair = [...filterObj[objKey], id]
        }
        if (objKey === 'MinDiscount') {
          filterObj = { ...filterObj, MinDiscount: id }
        } else if (objKey === 'CategoryId') {
          filterObj = { CategoryId: id }
        } else {
          filterObj = { ...filterObj, [objKey]: pair ?? [id] }
        }
      } else {
        if (
          typeof filterObj[objKey] === 'object' &&
          filterObj[objKey]?.length > 0
        ) {
          filterObj = {
            ...filterObj,
            [objKey]: filterObj[objKey]?.filter((item) => item !== id)
          }
        }
      }
    }
    setFiltersObj(filterObj)
    changeUrl(filterObj, categoryName ? categoryName : params?.categoryName)
  }

  function updateVisibility(maxDisc) {
    const visibilityData = []

    for (let i = 0; i < discounts.length; i++) {
      const discount = discounts[i]
      const isVisible = discount >= 10 && discount <= maxDisc
      visibilityData.push({ [discount]: isVisible })
    }

    return visibilityData
  }

  const result = updateVisibility(filterList?.maxDiscount)

  const onSubmit = async () => {
    const query = {
      ...filtersObj,
      MinPrice: parseFloat(value[0]),
      MaxPrice: parseFloat(value[1])
    }
    if (params?.categoryName || searchQuery.get('searchTexts')) {
      changeUrl(query, params?.categoryName)
    } else {
      router.push(
        queryString?.length > 0
          ? `/${redirectTo}?${queryString}`
          : `/${redirectTo}`
      )
    }
  }

  function checkObjectValues(obj) {
    const acceptableKeys = ['CategoryId', 'fby', 'searchTexts']

    for (const key in obj) {
      if (!acceptableKeys.includes(key)) {
        const value = obj[key]
        if (value && value !== null) {
          if (Array.isArray(value) && value.length === 0) {
            continue
          } else if (typeof value === 'string' && value.trim() === '') {
            continue
          } else {
            return false
          }
        }
      }
    }

    return true
  }

  useEffect(() => {
    if (searchQuery.get('MaxPrice') && searchQuery.get('MinPrice')) {
      setValue([
        decryptId(searchQuery.get('MinPrice')),
        decryptId(searchQuery.get('MaxPrice'))
      ])
    } else {
      setValue([filterList?.minSellingPrice, filterList?.maxSellingPrice])
    }
  }, [result?.query, filterList])

  return (
    <div className='m-prd-sidebar' ref={specificPartRef}>
      {(containsKey(filtersObj, filterList)?.status ||
        containsKey(filtersObj, filterList)?.initStatus ||
        containsKey(filtersObj, filterList)?.match) && (
        <div className='filtered_checked_list'>
          <h2 className='filtered_title'>You are watching</h2>
          <div className='filtered_badges'>
            {filtersObj?.BrandIds?.length > 0 &&
              filtersObj?.BrandIds?.map((bId) => (
                <FilterBadgesWatching
                  text={
                    filterList?.filteredBrand?.find(
                      (item) => item?.brandId === bId
                    )?.brandName
                  }
                  key={Math.floor(Math.random() * 100000)}
                  onClick={async () => {
                    let BrandIds = filtersObj?.BrandIds
                    let indexToRemove = BrandIds.indexOf(bId)

                    let updatedFiltersObj = { ...filtersObj }

                    if (indexToRemove !== -1) {
                      BrandIds.splice(indexToRemove, 1)
                    }

                    updatedFiltersObj = { ...updatedFiltersObj, BrandIds }
                    setFiltersObj(updatedFiltersObj)

                    changeUrl(updatedFiltersObj, params?.categoryName)
                  }}
                />
              ))}
            {filtersObj?.SizeIds?.length > 0 &&
              filtersObj?.SizeIds?.map((sizeId) => (
                <FilterBadgesWatching
                  text={
                    filterList?.size_filter?.find(
                      (item) => item?.sizeID === sizeId
                    )?.size
                  }
                  key={Math.floor(Math.random() * 100000)}
                  onClick={async () => {
                    let SizeIds = filtersObj?.SizeIds
                    let indexToRemove = SizeIds.indexOf(sizeId)

                    let updatedFiltersObj = { ...filtersObj }

                    if (indexToRemove !== -1) {
                      SizeIds.splice(indexToRemove, 1)
                    }

                    updatedFiltersObj = { ...updatedFiltersObj, SizeIds }
                    setFiltersObj(updatedFiltersObj)
                    changeUrl(updatedFiltersObj, params?.categoryName)
                  }}
                />
              ))}
            {filtersObj?.ColorIds?.length > 0 &&
              filtersObj?.ColorIds?.map((colorId) => (
                <FilterBadgesWatching
                  text={
                    filterList?.color_filter?.find(
                      (item) => item?.colorId === colorId
                    )?.colorName
                  }
                  key={Math.floor(Math.random() * 100000)}
                  onClick={async () => {
                    let ColorIds = filtersObj?.ColorIds
                    let indexToRemove = ColorIds.indexOf(Number(colorId))

                    let updatedFiltersObj = { ...filtersObj }

                    if (indexToRemove !== -1) {
                      ColorIds.splice(indexToRemove, 1)
                    }
                    updatedFiltersObj = { ...updatedFiltersObj, ColorIds }
                    setFiltersObj(updatedFiltersObj)
                    changeUrl(updatedFiltersObj, params?.categoryName)
                  }}
                />
              ))}
            {searchQuery.get('SpecTypeValueIds')?.length > 0 &&
              searchQuery
                .get('SpecTypeValueIds')
                ?.split(/[|,]/)
                .map((item) => Number(decryptId(item)))
                .map((id) => (
                  <>
                    <FilterBadgesWatching
                      text={
                        filtersObj?.specifications?.find(
                          (check) => check?.value === id
                        )?.valueName
                      }
                      key={Math.floor(Math.random() * 100000)}
                      onClick={async () => {
                        let updatedFiltersObj = { ...filtersObj }
                        let specifications = filtersObj?.specifications?.filter(
                          (spec) => spec?.value !== id
                        )
                        updatedFiltersObj = {
                          ...updatedFiltersObj,
                          specifications,
                          SpecTypeValueIds: specifications
                        }
                        setFiltersObj(updatedFiltersObj)
                        changeUrl(updatedFiltersObj, params?.categoryName)
                      }}
                    />
                  </>
                ))}
            {!checkObjectValues(filtersObj) && (
              <span
                role='button'
                className='clearall_filtertag'
                onClick={() => {
                  let updatedFiltersObj = {
                    ...filtersObj,
                    CategoryId:
                      stringToIntegerOrArray(
                        decryptId(searchQuery.get('CategoryId')),
                        'category'
                      ) ?? null,
                    BrandIds: [],
                    SizeIds: [],
                    ColorIds: [],
                    SpecTypeValueIds: '',
                    specifications: null,
                    MinDiscount: '',
                    fby: ''
                  }
                  setFiltersObj(updatedFiltersObj)
                  changeUrl(updatedFiltersObj, params?.categoryName)
                }}
              >
                clear all
              </span>
            )}
          </div>
        </div>
      )}
      <ul className='m-prd-sidebar__list'>
        {loading && !productData ? (
          <ProductFilterSkeleton searchBar={false} />
        ) : (
          filterList &&
          filterList?.category_filter?.length > 0 && (
            <CategoryFilter
              filterList={filterList?.category_filter}
              filtersObj={filtersObj}
              toggleOpen={toggleOpen}
              handleFilterFunc={handleFilterFunc}
            />
          )
        )}
        {loading && !productData ? (
          <ProductFilterSkeleton />
        ) : (
          filterList &&
          filterList?.brand_filter?.length > 0 && (
            <BrandFilter
              filterList={filterList?.filteredBrand}
              filtersObj={filtersObj}
              toggleOpen={toggleOpen}
              handleSearch={handleSearch}
              searchText={searchText}
              handleFilterFunc={handleFilterFunc}
            />
          )
        )}

        {loading && !productData ? (
          <ProductFilterSkeleton />
        ) : (
          filterList &&
          filterList?.color_filter?.length > 0 && (
            <ColourFilter
              productData={productData}
              filterList={filterList?.filteredColor}
              filtersObj={filtersObj}
              toggleOpen={toggleOpen}
              handleSearch={handleSearch}
              searchText={searchText}
              handleFilterFunc={handleFilterFunc}
              loading={loading}
            />
          )
        )}

        {loading && !productData ? (
          <ProductFilterSkeleton />
        ) : (
          filterList &&
          filterList?.size_filter?.length > 0 && (
            <SizeFilter
              productData={productData}
              filterList={filterList?.filteredSize}
              filtersObj={filtersObj}
              toggleOpen={toggleOpen}
              handleSearch={handleSearch}
              searchText={searchText}
              handleFilterFunc={handleFilterFunc}
              loading={loading}
            />
          )
        )}
        {filterList &&
          ((filterList?.minSellingPrice && filterList.minSellingPrice !== 0) ||
            (filterList?.maxSellingPrice &&
              filterList.maxSellingPrice !== 0)) && (
            <PricingFilter
              productData={productData?.data?.products}
              filterList={filterList}
              value={value}
              setFiltersObj={setFiltersObj}
              changeUrl={changeUrl}
              setValue={setValue}
              onSubmit={onSubmit}
              filtersObj={filtersObj}
              toggleOpen={toggleOpen}
            />
          )}
        {filterList &&
          filterList?.minDiscount &&
          filterList.maxDiscount >= 10 && (
            <DiscountFilter
              toggleOpen={toggleOpen}
              filtersObj={filtersObj}
              setFiltersObj={setFiltersObj}
              changeUrl={changeUrl}
              result={result}
              handleFilterFunc={handleFilterFunc}
            />
          )}
        {filterList && filterList?.filter_types?.length > 0 && (
          <>
            <SpecificationFilter
              filterList={filterList?.filter_types}
              toggleOpen={toggleOpen}
              handleFilterFunc={handleFilterFunc}
              filtersObj={filtersObj}
            />
          </>
        )}
      </ul>
      {isActiveDrawer?.filterDrawer && (
        <div className='pv-filter-main'>
          <MBtn
            buttonClass={'pv-filter-btn'}
            btnText='Clear All'
            onClick={() => {
              let updatedFiltersObj = {
                ...filtersObj,
                CategoryId:
                  stringToIntegerOrArray(
                    decryptId(searchQuery.get('CategoryId')),
                    'category'
                  ) ?? null,
                BrandIds: [],
                SizeIds: [],
                ColorIds: [],
                SpecTypeValueIds: '',
                specifications: null,
                MinDiscount: '',
                fby: ''
              }
              setFiltersObj(updatedFiltersObj)
              changeUrl(updatedFiltersObj, params?.categoryName)
              setIsActiveDrawer(false)
            }}
          />
          <MBtn
            buttonClass={'pv-filter-btn'}
            btnText='Apply'
            onClick={() => setIsActiveDrawer(false)}
          />
        </div>
      )}
    </div>
  )
}

export default ProductlistSidebar
