import React, { useEffect, useState } from 'react'
import axiosProvider from '../lib/AxiosProvider'
import DynamicPositionComponent from './DynamicPositionComponent'
import Slider from './Slider'
import ProductList from './ProductList'
import { useSelector } from 'react-redux'
import { showToast } from '../lib/GetBaseUrl'
import { _exception } from '../lib/exceptionMessage'
import LoginSignup from './LoginSignup'
import { handleWishlistClick } from '../lib/AllGlobalFunction'

const ProductlistHomepage = ({
  layoutsInfo,
  section,
  setLoading,
  toast,
  setToast
}) => {
  const [data, setData] = useState({ data: [] })
  const { user } = useSelector((state) => state?.user)
  const [modalShow, setModalShow] = useState({
    show: false,
    data: null
  })

  const fetchFallbackProducts = async () => {
    const fallbackResponse = await axiosProvider({
      method: 'GET',
      endpoint: 'ManageHomePageSection/GetFeaturedProducts',
      queryString: `?topProduct=${section?.top_products || 8}`
    })
    if (fallbackResponse?.status === 200) {
      const fallbackData = fallbackResponse?.data?.data ?? []
      setData({ data: Array.isArray(fallbackData) ? fallbackData : [] })
      return true
    }
    return false
  }

  const fetchProduct = async (isWishlistClicked) => {
    try {
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'ManageHomePageSections/GetProductHomePageSection',
        queryString: `?categoryId=${section?.category_id}&topProduct=${
          section?.top_products
        }&productId=${''}`
      })
      if (response?.status === 200) {
        setData(response?.data)
        if (isWishlistClicked) {
          const wishListRes = await handleWishlistClick(
            isWishlistClicked,
            response?.data,
            'productList',
            toast,
            setToast
          )
          if (wishListRes?.wishlistResponse?.data?.code === 200) {
            setData(wishListRes)
          } else {
            setData(response?.data)
          }
          wishListRes?.wishlistResponse &&
            showToast(toast, setToast, wishListRes?.wishlistResponse)
        }
      } else {
        const isFallbackLoaded = await fetchFallbackProducts()
        if (!isFallbackLoaded) {
          setData({ data: [] })
        }
      }
    } catch (error) {
      try {
        const isFallbackLoaded = await fetchFallbackProducts()
        if (!isFallbackLoaded) {
          setData({ data: [] })
        }
      } catch {
        setData({ data: [] })
      }
      showToast(toast, setToast)
    }
  }

  const onClose = () => {
    setModalShow({ ...modalShow, show: false })

    if (user?.userId) {
      setTimeout(() => {
        fetchProduct(modalShow?.data)
      }, 500)
    }
  }

  useEffect(() => {
    setTimeout(() => {
      fetchProduct()
    }, 500)
  }, [])

  const withoutPrice = layoutsInfo?.layout_class === 'without-price'

  return (
    <>
      {modalShow?.show && (
        <LoginSignup
          modal={modalShow}
          modalOpen={setModalShow}
          onClose={onClose}
          toast={toast}
          setToast={setToast}
        />
      )}
      <div
        className='site-container categories-section section_spacing_b'
        key={section?.section_id}
      >
        <div className='categories-wrapper'>
          <DynamicPositionComponent
            heading={section?.title}
            paragraph={section?.sub_title}
            btnText={section?.link_text}
            redirectTo={section?.link ?? '#.'}
            headingPosition={section?.title_position?.toLowerCase()}
            buttonPosition={section?.link_position?.toLowerCase()}
            buttonPositionDirection={section?.link_in?.toLowerCase()}
            TitleColor={section?.title_color?.toLowerCase()}
            TextColor={section?.text_color?.toLowerCase()}
          >
            <Slider
              className='pv-productcard-main'
              spaceBetween={10}
              slidesPerView={5}
              loop={true}
              withArrows={false}
              showShareBtn={true}
              autoplay={true}
              navigation={false}
              pagination={false}
              breakpoints={{
                0: {
                  slidesPerView: 1
                },
                479: {
                  slidesPerView: 2
                },
                768: {
                  slidesPerView: 3
                },
                1024: {
                  slidesPerView: 4
                },
                1280: { slidesPerView: 5 }
              }}
            >
              {data?.data?.length > 0 &&
                data?.data.map((product) => (
                  <ProductList
                    key={`${product.id}${Math.floor(Math.random() * 10000000)}`}
                    product={product}
                    withoutPrice={withoutPrice}
                    wishlistShow
                    isView={true}
                    toast={toast}
                    setToast={setToast}
                    setModalShow={setModalShow}
                    modalShow={modalShow}
                    setLoading={setLoading}
                    setProductData={setData}
                    productData={data}
                    fetchProductList={fetchProduct}
                  />
                ))}
            </Slider>
          </DynamicPositionComponent>
        </div>
      </div>
    </>
  )
}

export default ProductlistHomepage
