import React from 'react'
import MBtn from '../base/MBtn'
import Link from 'next/link'
import { encryptId, reactImageUrl } from '../../lib/GetBaseUrl'
import { _productImg_ } from '../../lib/ImagePath'
import Image from 'next/image'

const ComparePopover = ({ compareData, setCompareData }) => {
  return (
    <div className='pv-compare-Popovermain'>
      <Link
        href={`/product/compare?sp_id=${compareData
          ?.map((item) => encryptId(item?.sellerProductId))
          .join(',')}`}
        className='pv-compare-btn'
      >
        Compare <span>{compareData?.length}</span>
      </Link>
      <div className='pv-compare-Popoverinner'>
        {compareData?.map((item) => (
          <div
            key={Math.floor(Math.random() * 1000000)}
            className='pv-compare-itemcard'
          >
            <MBtn
              buttonClass={'pv-compare-itembtn'}
              withIcon
              iconClass={'closetoggle-icon'}
              onClick={() => {
                let filteredCompareData = compareData?.filter(
                  (data) => data?.id !== item?.id
                )
                localStorage.setItem(
                  'hk-compare-data',
                  JSON.stringify(filteredCompareData)
                )
                setCompareData(filteredCompareData)
              }}
            />
            <Image
              className='pv-compare-popupimg'
              src={encodeURI(`${reactImageUrl}${_productImg_}${item?.image1}`)}
              alt={item?.productName}
              width={100}
              height={100}
            />
            <p className='pv-compr-itemname'>{item?.productName}</p>
          </div>
        ))}
        {compareData?.length > 1 && (
          <div className='pv-compare-btn-main'>
            <MBtn
              buttonClass={'pv-compare-removebtn'}
              btnText={'Remove all'}
              withIcon
              iconClass={'closetoggle-icon'}
              onClick={() => {
                localStorage.removeItem('hk-compare-data')
                setCompareData([])
              }}
            />
          </div>
        )}
      </div>
    </div>
  )
}

export default ComparePopover
