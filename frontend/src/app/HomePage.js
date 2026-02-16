'use client'
import actionHandler from '@/utils/actionHandler'
import Head from 'next/head'
import { useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'
import Categories from '../components/Category'
import AllGridFile from '../components/GridImageSection/AllGridFile'
import Heroslider from '../components/HeroSlider'
import ProductlistHomepage from '../components/ProductlistHomepage'
import Toaster from '../components/base/Toaster'
import {
  _headDescription_,
  _headKeywords_,
  _headTitle_,
  _ogDescription_,
  _ogLogo_,
  _ogUrl_,
  _projectName_
} from '../lib/ConfigVariables'

const HomePage = ({ homePageData }) => {
  const [loading, setLoading] = useState(false)
  const router = useRouter()
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })

  const renderComponent = (data) => {
    return Object.entries(data)?.map(([key, value]) => {
      switch (value?.layoutsInfo?.layout_name) {
        case 'Banners':
          return (
            <div
              className='hero-slider-wrapper'
              key={value?.section?.section_id}
            >
              <Heroslider
                layoutsInfo={value?.layoutsInfo}
                section={value?.section}
              />
            </div>
          )
        case 'Thumbnail':
          return (
            <div key={value?.section?.section_id}>
              <Categories
                layoutsInfo={value?.layoutsInfo}
                section={value?.section}
              />
            </div>
          )
        case 'Product List':
          return (
            <ProductlistHomepage
              layoutsInfo={value?.layoutsInfo}
              section={value?.section}
              key={value?.section?.section_id}
              setLoading={setLoading}
              setToast={setToast}
              toast={toast}
            />
          )
        case 'Gallery':
          return (
            <div key={value?.section?.section_id}>
              <AllGridFile
                layoutsInfo={value?.layoutsInfo}
                section={value?.section}
              />
            </div>
          )
        default:
          return null
      }
    })
  }
  const components = homePageData?.data && renderComponent(homePageData?.data)

  useEffect(() => {
    if (homePageData?.action) {
      actionHandler(homePageData?.action, router)
    }
  }, [])

  return (
    <>
      <Head>
        <link
          rel='canonical'
          href=''
        />
        <title>
          {_projectName_} - {_headTitle_}
        </title>
        <meta name='description' content={_headDescription_} />
        <meta name='keywords' content={_headKeywords_} />
        <meta property='og:locale' content='en_US' />
        <meta
          property='og:title'
          content={`${_projectName_} - ${_headTitle_}`}
        />
        <meta property='og:description' content={_ogDescription_} />
        <meta property='og:url' content={_ogUrl_} />
        <meta property='og:image' content={_ogLogo_} />
      </Head>

      {components?.length > 0 && components}

      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
    </>
  )
}

export default HomePage
