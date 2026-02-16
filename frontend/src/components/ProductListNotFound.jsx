import Link from 'next/link'
import { useRouter } from 'next/router'
import React from 'react'

const ProductListNotFound = () => {
  const router = useRouter()
  const { searchTexts } = router?.query
  return (
    <div className='site-container'>
      <div className='prdt_not__found_main'>
        <div>
          <h1>No Products </h1>
          {searchTexts && (
            <>
              <p>Your search did not match any documents. Suggestions:</p>
              <p>Make sure all words are spelled correctly.</p>
              <p>Try more general keywords.</p>
            </>
          )}
          <Link href='/'>Go to the homepage</Link>
        </div>
      </div>
    </div>
  )
}

export default ProductListNotFound
