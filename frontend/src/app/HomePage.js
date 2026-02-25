'use client'
import actionHandler from '@/utils/actionHandler'
import { useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'
import Categories from '../components/Category'
import AllGridFile from '../components/GridImageSection/AllGridFile'
import Heroslider from '../components/HeroSlider'
import ProductlistHomepage from '../components/ProductlistHomepage'
import Toaster from '../components/base/Toaster'
import Slider from '../components/Slider'
import axiosProvider from '../lib/AxiosProvider'
import {
  buildResourceImageUrl,
  imagePlaceholderUrl,
  reactImageUrl
} from '../lib/GetBaseUrl'
import { _brandImg_, _homePageImg_, _productImg_ } from '../lib/ImagePath'

const extractDbSlides = (homePageData) => {
  const sectionMap = homePageData?.data
  if (!sectionMap || typeof sectionMap !== 'object') return []

  return Object.values(sectionMap)
    .flatMap((entry) => {
      const section = entry?.section ?? {}
      const singles = entry?.section?.columns?.left?.single ?? []
      return singles
        .filter((item) => item?.image)
        .map((item) => ({
          image: item.image,
          imageAlt: item.image_alt || section?.title || 'slide-image',
          title: item?.title || section?.title || '',
          text: item?.sub_title || section?.sub_title || '',
          link: section?.link || '/'
        }))
    })
    .slice(0, 8)
}

const FallbackHomepage = ({
  products = [],
  isLoadingProducts = false,
  brands = [],
  isLoadingBrands = false,
  slides = []
}) => (
  <section className='fallback-home'>
    <div className='fallback-home__inner'>
      {slides?.length > 0 && (
        <div className='fallback-hero'>
          <Slider
            spaceBetween={0}
            slidesPerView={1}
            loop={true}
            withArrows={false}
            autoplay={true}
            navigation={false}
            pagination={true}
          >
            {slides.map((slide, idx) => (
              <a className='fallback-hero__slide' key={`${slide?.image}-${idx}`} href={slide?.link || '/'}>
                <img
                  src={buildResourceImageUrl(slide.image, _homePageImg_)}
                  alt={slide.imageAlt}
                  className='fallback-hero__image'
                  onError={(event) => {
                    event.currentTarget.src = imagePlaceholderUrl
                  }}
                />
                {(slide?.title || slide?.text) && (
                  <div className='fallback-hero__content'>
                    {slide?.title && <h2>{slide.title}</h2>}
                    {slide?.text && <p>{slide.text}</p>}
                  </div>
                )}
              </a>
            ))}
          </Slider>
        </div>
      )}

      <div className='fallback-products'>
        <div className='fallback-products__head'>
          <h3>Featured Products</h3>
          <a href='/products/all'>View All</a>
        </div>
        <div className='fallback-products__grid'>
          {isLoadingProducts ? (
            <div className='fallback-empty'>Loading products...</div>
          ) : products?.length > 0 ? (
            products.map((product, idx) => (
              <article className='fallback-product' key={`${product?.id ?? idx}`}>
                <img
                  src={`${reactImageUrl}${_productImg_}${
                    product?.image1 ?? product?.productImage ?? ''
                  }`}
                  alt={product?.productName ?? 'Product'}
                />
                <h4>{product?.productName ?? '-'}</h4>
                <p>
                  ${Number(product?.sellingPrice ?? product?.mrp ?? 0).toFixed(2)}
                </p>
                <a href='/products/all'>View Product</a>
              </article>
            ))
          ) : (
            <div className='fallback-empty'>
              No products available in database to display.
            </div>
          )}
        </div>
      </div>

      <div className='fallback-brands'>
        {isLoadingBrands ? (
          <div className='fallback-empty'>Loading brands...</div>
        ) : brands?.length > 0 ? (
          brands.slice(0, 14).map((brand, idx) => (
            <div className='fallback-brand' key={`${brand?.id ?? idx}`}>
              {brand?.logo || brand?.Logo ? (
                <img
                  src={`${reactImageUrl}${_brandImg_}${brand?.logo ?? brand?.Logo ?? ''}`}
                  alt={brand?.name ?? brand?.Name ?? 'Brand'}
                />
              ) : (
                <span className='fallback-brand__name'>
                  {brand?.name ?? brand?.Name ?? 'Brand'}
                </span>
              )}
            </div>
          ))
        ) : (
          <div className='fallback-empty'>No brands available in database.</div>
        )}
      </div>
    </div>

    <style jsx>{`
      .fallback-home {
        padding: 24px 0 48px;
        background: linear-gradient(180deg, #f8fafc 0%, #ffffff 35%);
      }
      .fallback-home__inner {
        max-width: 1240px;
        margin: 0 auto;
        padding: 0 16px;
      }
      .fallback-hero {
        border-radius: 16px;
        overflow: hidden;
        box-shadow: 0 16px 34px rgba(16, 24, 40, 0.15);
      }
      .fallback-hero__slide {
        display: block;
        text-decoration: none;
        background: #fff;
      }
      .fallback-hero__image {
        width: 100%;
        height: 360px;
        object-fit: cover;
        display: block;
      }
      .fallback-hero__content {
        padding: 16px 18px 20px;
      }
      .fallback-hero__content h2 {
        font-size: 30px;
        line-height: 1.2;
        margin: 0 0 8px;
        color: #0f172a;
      }
      .fallback-hero__content p {
        font-size: 15px;
        line-height: 1.5;
        margin: 0;
        color: #475569;
      }
      .fallback-grid {
        margin-top: 24px;
        display: grid;
        gap: 20px;
        grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
      }
      .fallback-grid__image {
        width: 100%;
        height: 220px;
        object-fit: cover;
        display: block;
      }
      .fallback-grid__body {
        padding: 14px 16px 16px;
      }
      .fallback-grid__body h5 {
        margin: 0;
        font-size: 18px;
        font-weight: 700;
      }
      .fallback-grid__body p {
        margin: 8px 0 0;
        color: #56616f;
        line-height: 1.45;
      }
      .fallback-products {
        margin-top: 28px;
      }
      .fallback-products__head {
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: 14px;
      }
      .fallback-products__head h3 {
        margin: 0;
        font-size: 26px;
      }
      .fallback-products__head a {
        color: #0b5f7a;
        text-decoration: none;
        font-weight: 600;
      }
      .fallback-products__grid {
        display: grid;
        gap: 18px;
        grid-template-columns: repeat(auto-fit, minmax(210px, 1fr));
      }
      .fallback-product {
        background: #fff;
        border-radius: 12px;
        border: 1px solid #e7ecf2;
        padding: 14px;
      }
      .fallback-product img {
        width: 100%;
        height: 180px;
        object-fit: contain;
        background: #f5f7fa;
        border-radius: 8px;
      }
      .fallback-product h4 {
        margin: 12px 0 6px;
        font-size: 17px;
      }
      .fallback-product p {
        margin: 0 0 10px;
        color: #0f172a;
        font-weight: 700;
      }
      .fallback-product button {
        width: 100%;
        border: 1px solid #0b5f7a;
        background: #fff;
        color: #0b5f7a;
        border-radius: 8px;
        padding: 9px 10px;
        font-weight: 600;
        cursor: pointer;
      }
      .fallback-product a {
        display: inline-block;
        text-decoration: none;
        color: #0b5f7a;
        font-weight: 600;
      }
      .fallback-empty {
        grid-column: 1 / -1;
        background: #fff;
        border: 1px dashed #ccd5df;
        border-radius: 12px;
        padding: 24px;
        color: #4e5b6a;
      }
      .fallback-brands {
        margin-top: 28px;
        background: #fff;
        border: 1px solid #e7ecf2;
        border-radius: 12px;
        padding: 14px;
        display: grid;
        grid-template-columns: repeat(7, minmax(0, 1fr));
        gap: 10px;
        align-items: center;
      }
      .fallback-brand {
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 48px;
      }
      .fallback-brand img {
        max-width: 100%;
        max-height: 40px;
        object-fit: contain;
      }
      .fallback-brand__name {
        color: #334155;
        font-size: 13px;
        font-weight: 600;
        text-align: center;
      }
      @media (max-width: 900px) {
        .fallback-hero__image {
          height: 300px;
        }
        .fallback-hero__content h2 {
          font-size: 24px;
        }
        .fallback-brands {
          grid-template-columns: repeat(4, minmax(0, 1fr));
        }
      }
      @media (max-width: 520px) {
        .fallback-hero__image {
          height: 240px;
        }
        .fallback-hero__content h2 {
          font-size: 20px;
        }
        .fallback-hero__content p {
          font-size: 13px;
        }
        .fallback-brands {
          grid-template-columns: repeat(3, minmax(0, 1fr));
        }
      }
    `}</style>
  </section>
)

const HomePage = ({ homePageData }) => {
  const [loading, setLoading] = useState(false)
  const router = useRouter()
  const [fallbackProducts, setFallbackProducts] = useState([])
  const [isLoadingProducts, setIsLoadingProducts] = useState(false)
  const [fallbackBrands, setFallbackBrands] = useState([])
  const [isLoadingBrands, setIsLoadingBrands] = useState(false)
  const [fallbackSlides, setFallbackSlides] = useState([])
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
  const hasRenderableComponents =
    Array.isArray(components) && components.filter(Boolean).length > 0

  useEffect(() => {
    if (homePageData?.action) {
      actionHandler(homePageData?.action, router)
    }
    setFallbackSlides(extractDbSlides(homePageData))

    const fetchFallbackProducts = async () => {
      setIsLoadingProducts(true)
      try {
        const response = await axiosProvider({
          method: 'GET',
          endpoint: 'ManageHomePageSection/GetFeaturedProducts',
          queryString: '?topProduct=8'
        })
        if (response?.status === 200) {
          setFallbackProducts(response?.data?.data ?? [])
        } else {
          setFallbackProducts([])
        }
      } catch {
        setFallbackProducts([])
      } finally {
        setIsLoadingProducts(false)
      }
    }

    const fetchFallbackBrands = async () => {
      setIsLoadingBrands(true)
      try {
        const response = await axiosProvider({
          method: 'GET',
          endpoint: 'ManageHomePageSection/GetBrands',
          queryString: '?pageIndex=0&pageSize=0&status=Active'
        })
        if (response?.status === 200) {
          setFallbackBrands(response?.data?.data ?? [])
        } else {
          setFallbackBrands([])
        }
      } catch {
        setFallbackBrands([])
      } finally {
        setIsLoadingBrands(false)
      }
    }

    if (!hasRenderableComponents) {
      fetchFallbackProducts()
      fetchFallbackBrands()
    }
  }, [])

  return (
    <>
      {hasRenderableComponents ? (
        components
      ) : (
        <FallbackHomepage
          products={fallbackProducts}
          isLoadingProducts={isLoadingProducts}
          brands={fallbackBrands}
          isLoadingBrands={isLoadingBrands}
          slides={fallbackSlides}
        />
      )}

      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
    </>
  )
}

export default HomePage
