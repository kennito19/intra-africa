import React from 'react'

const AddToCartCheckout = () => {
  return (
    <>
      <div className='checkout-main-compo'>
        <ol className='checkout-steps'>
          <li className='step step1 stepsactive'>BAG</li>
          <li className='divider'></li>
          <li className='step step2'>ADDRESS</li>
          <li className='divider'></li>
          <li className='step step3'>PAYMENT</li>
        </ol>
      </div>
    </>
  )
}

export default AddToCartCheckout
