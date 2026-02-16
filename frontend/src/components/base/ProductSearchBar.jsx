'use client'

import { Form, Formik } from 'formik'
import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useRef, useState } from 'react'
import * as Yup from 'yup'
import axiosProvider from '../../lib/AxiosProvider'
import { spaceToDash } from '../../lib/GetBaseUrl'
import useDebounce from '../../lib/useDebounce'

const ProductSearchBar = ({ placeholder, searchBtnVisible }) => {
  const router = useRouter()
  const profileRef = useRef(null)
  const [searchText, setSearchText] = useState()
  const debounceSearchText = useDebounce(searchText, 500)
  const [suggestions, setSuggestions] = useState(false)
  const validationSchema = Yup.object().shape({
    searchTexts: Yup.string().required()
  })

  const handleSearch = async (values) => {
    setSuggestions(false)
    document.activeElement.blur()
    router.push(`/products/search/${spaceToDash(values?.searchTexts)}`)
  }

  const fetchData = async (endpoint, setterFunc) => {
    const response = await axiosProvider({
      method: 'GET',
      endpoint
    })
    if (response?.data?.code === 200) {
      return setterFunc(response)
    } else {
      setSuggestions(false)
    }
  }
  useEffect(() => {
    if (searchText) {
      fetchData(
        `user/Product/searchsuggestion?searchText=${debounceSearchText}`,
        (resp) => {
          setSuggestions(resp?.data?.data)
        }
      )
    }
  }, [debounceSearchText])

  useEffect(() => {
    setSuggestions(false)
  }, [router?.query])

  useEffect(() => {
    const handleOutsideClick = (event) => {
      if (profileRef.current && !profileRef.current.contains(event.target)) {
        setSuggestions(false)
      }
    }

    document.body.addEventListener('click', handleOutsideClick)
    return () => {
      document.body.removeEventListener('click', handleOutsideClick)
    }
  }, [])

  return (
    <div className='search_container' ref={profileRef}>
      <Formik
        enableReinitialize={true}
        initialValues={{
          searchTexts: router?.query?.searchTexts ?? ''
        }}
        validationSchema={validationSchema}
        onSubmit={handleSearch}
      >
        {({ values, setFieldValue }) => (
          <Form className='search_form'>
            <input
              id='product-searchbar'
              autoComplete='off'
              type='search'
              placeholder={placeholder}
              name={'searchTexts'}
              value={values?.searchTexts ?? ''}
              onChange={(e) => {
                setFieldValue('searchTexts', e?.target?.value?.trimStart())
                if (e?.target?.value?.trim()?.length >= 2) {
                  // getSuggestion(e?.target?.value)
                  setSearchText(e?.target?.value)
                } else {
                  setSuggestions(false)
                }
              }}
              onBlur={(e) =>
                setFieldValue('searchTexts', e?.target?.value?.trim())
              }
              onKeyDown={(e) => {
                if (e.key === 'Enter') {
                  setSuggestions(false)
                  setSearchText(false)
                }
              }}
              className='search_textbox'
            />
            {searchBtnVisible && (
              <button
                className='m-btn btn-green pv-nav-serch-btn'
                onClick={() => handleSearch(values?.searchTexts)}
              >
                Search
              </button>
            )}

            {(suggestions?.categories?.length > 0 ||
              suggestions?.brands?.length > 0 ||
              suggestions?.products?.length > 0) && (
              <div className='suggestions_container'>
                <div className='p_indide'>
                  {suggestions?.categories?.length > 0 && (
                    <ul className='suggestions_list'>
                      <li className='static_title'>Categories</li>
                      {suggestions?.categories?.map((categories, index) => (
                        <li key={index} className='suggestion_item'>
                          <Link
                            href={`/products/search/${spaceToDash(
                              categories?.name
                            )}`}
                            onClick={() => {
                              setSuggestions(false)
                              setFieldValue('searchTexts', '')
                            }}
                          >
                            {categories?.name}
                          </Link>
                        </li>
                      ))}
                    </ul>
                  )}
                  {suggestions?.brands?.length > 0 && (
                    <ul className='suggestions_list'>
                      <li className='static_title'>Brands</li>
                      {suggestions?.brands?.map((brand, index) => (
                        <li key={index} className='suggestion_item'>
                          <Link
                            href={`/products/search/${spaceToDash(
                              brand?.name
                            )}`}
                            onClick={() => {
                              setSuggestions(false)
                              setFieldValue('searchTexts', '')
                            }}
                          >
                            {brand?.name}
                          </Link>
                        </li>
                      ))}
                    </ul>
                  )}
                  {suggestions?.products?.length > 0 && (
                    <ul className='suggestions_list'>
                      <li className='static_title'>Products</li>
                      {suggestions?.products?.map((product, index) => (
                        <li key={index} className='suggestion_item'>
                          <Link
                            href={`/products/search/${spaceToDash(
                              product?.productName
                            )}`}
                            onClick={() => {
                              setSuggestions(false)
                              setFieldValue('searchTexts', '')
                            }}
                          >
                            {product?.productName}
                          </Link>
                        </li>
                      ))}
                    </ul>
                  )}
                </div>
              </div>
            )}
          </Form>
        )}
      </Formik>
    </div>
  )
}

export default ProductSearchBar
