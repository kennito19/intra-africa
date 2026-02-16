import React from 'react'
import Image from 'next/image'
import { currencyIcon } from '../lib/GetBaseUrl'

function ProductGridView() {
  return (
    <div className='pd-list__card-gridview'>
      <div className='pd-list__img'>
        <Image
          src='/images/prd-list.png'
          alt='Product list images'
          width={300}
          height={300}
          className='prd-list-image'
        />
      </div>

      <div>
        <div className=' prd-list__details-gridview'>
          <h2 className='prd-list-title'>BoAt</h2>
          <p className='prd-list-contains'>
            BoAt Bassheads 220 Wired in BoAt Bassheads 220 Wired in
          </p>
          <p className='prd-list-color__title'>
            Color :<span className='prd-list-color__name'> Black</span>
          </p>
          <div className='prd-list-price__wrapper-gridview'>
            <h2 className='prd-total-price'>{currencyIcon} 350.00</h2>
            <p className='prd-check-price'>{currencyIcon} 999.00</p>
            <span className='prd-list-offer'>(13% OFF)</span>
          </div>
        </div>

        <div className='prd-list-btn__wrapper-gridview'>
          <button className='m-btn btn-whishlist'>
            <i className='m-icon m-wishlist-icon'></i>Wishlist
          </button>
          <button className='m-btn btn-add-cart'>
            <i className='m-icon m-cart-icon'></i> Add to cart
          </button>
        </div>
      </div>
    </div>
  )
}

export default ProductGridView
