import Image from 'next/image'
import { useEffect, useRef, useState } from 'react'
import { useSelector } from 'react-redux'
import { getUserId, reactImageUrl, showToast } from '../lib/GetBaseUrl'
import { _productImg_ } from '../lib/ImagePath'
import Slider from './Slider'
// import "react-inner-image-zoom/lib/InnerImageZoom/styles.min.css";
import { useRouter } from 'next/navigation'
import {
  getEmbeddedUrlFromYouTubeUrl,
  handleWishlistClick
} from '../lib/AllGlobalFunction'
import { checkTokenAuthentication } from '../lib/checkTokenAuthentication'

const ProductDetail = ({
  values,
  toast,
  setToast,
  setValues,
  setModalShow,
  fetchProduct,
  setLoading,
  onMouseEnter,
  onMouseLeave,
  onMouseMove,
  selectedMedia,
  setSelectedMedia
}) => {
  const router = useRouter()
  const { user } = useSelector((state) => state?.user)
  const userIdCookie = getUserId()
  const [isMobile, setIsMobile] = useState(false)
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [showScrollButtons, setShowScrollButtons] = useState({
    showTopButton: false,
    showBottomButton: false
  })
  const thumbnailsRef = useRef(null)

  useEffect(() => {
    setSelectedMedia((values?.productImage && values?.productImage[0]) || null)
  }, [values])

  useEffect(() => {
    setIsMobile(window.innerWidth <= 1024)

    // Update the showScrollButtons state based on the current screen size.
    setShowScrollButtons({
      showTopButton:
        thumbnailsRef.current && thumbnailsRef.current.scrollTop > 0,
      showBottomButton:
        thumbnailsRef.current &&
        thumbnailsRef.current.scrollTop <
          thumbnailsRef.current.scrollHeight -
            thumbnailsRef.current.clientHeight
    })

    window.addEventListener('resize', () => {
      setIsMobile(window.innerWidth <= 1024)
      setShowScrollButtons({
        showTopButton:
          thumbnailsRef.current && thumbnailsRef.current.scrollTop > 0,
        showBottomButton:
          thumbnailsRef.current &&
          thumbnailsRef.current.scrollTop <
            thumbnailsRef.current.scrollHeight -
              thumbnailsRef.current.clientHeight
      })
    })

    thumbnailsRef.current.addEventListener('scroll', () => {
      const scrollTop = parseFloat(thumbnailsRef.current.scrollTop)
      const scrollHeight = thumbnailsRef.current.scrollHeight
      const clientHeight = thumbnailsRef.current.clientHeight
      const epsilonPercentage = 0.05

      const epsilon = clientHeight * epsilonPercentage

      setShowScrollButtons({
        showTopButton: scrollTop > epsilon,
        showBottomButton: scrollTop < scrollHeight - clientHeight - epsilon
      })
    })

    return () => {
      window.removeEventListener('resize', () => {})
      thumbnailsRef.current &&
        thumbnailsRef.current.removeEventListener('scroll', () => {})
    }
  }, [thumbnailsRef])

  const scrollToTop = () => {
    const container = thumbnailsRef.current
    container.scrollTo({ top: container.scrollTop - 200, behavior: 'smooth' })
  }

  const scrollToBottom = () => {
    const container = thumbnailsRef.current
    container.scrollTo({
      top: container.scrollTop + 200,
      behavior: 'smooth'
    })
  }

  const handleThumbnailHover = (media) => {
    setSelectedMedia(media)
  }

  const handlePosterClick = (video) => {
    setSelectedMedia(video)
    setIsModalOpen(true)
  }

  const closeModal = () => {
    setIsModalOpen(false)
  }

  return (
    <div className='product-details'>
      {!isMobile && (
        <div className='thumbnails-container'>
          {showScrollButtons?.showTopButton && (
            <button className='thumbs_btn thumbs_top_btn' onClick={scrollToTop}>
              <i className='m-icon icon-up-arrow'></i>
            </button>
          )}
          <div
            className='thumbnails'
            ref={thumbnailsRef}
            style={!isMobile && { 'max-height': '450px' }}
          >
            {values?.productImage &&
              values?.productImage?.map((media, index) => (
                <div
                  key={Math.floor(Math.random() * 100000)}
                  className={`thumbnail ${
                    selectedMedia?.url === media?.url ? 'active' : ''
                  }`}
                  onMouseOver={() => handleThumbnailHover(media)}
                >
                  <Image
                    src={
                      media?.type === 'Image'
                        ? `${reactImageUrl}${_productImg_}${media?.url}`
                        : `https://img.youtube.com/vi/${getEmbeddedUrlFromYouTubeUrl(
                            media?.url
                          )}/0.jpg`
                    }
                    alt={`Image ${media?.url}`}
                    height={0}
                    width={0}
                    quality={100}
                    sizes='100vw'
                  />
                </div>
              ))}
          </div>
          {showScrollButtons?.showBottomButton && (
            <button
              className='thumbs_btn thumbs_btm_btn'
              onClick={scrollToBottom}
            >
              <i className='m-icon icon-down-arrow'></i>
            </button>
          )}
        </div>
      )}
      <div className={`main-media ${isMobile ? 'slider-mode' : ''}`}>
        {selectedMedia && !isMobile && (
          <div className='imageshover_m'>
            {selectedMedia?.type?.toLowerCase() === 'video' ? (
              <div className='image-wrapper'>
                <iframe
                  src={`https://www.youtube.com/embed/${getEmbeddedUrlFromYouTubeUrl(
                    selectedMedia?.url
                  )}`}
                  title='Video Player'
                  width='100%'
                  height='450'
                ></iframe>
              </div>
            ) : (
              <div
                className='image-wrapper'
                onMouseMove={onMouseMove}
                onMouseLeave={onMouseLeave}
                onMouseEnter={onMouseEnter}
              >
                <img
                  className='sampleImage'
                  src={
                    selectedMedia?.url &&
                    encodeURI(
                      `${reactImageUrl}${_productImg_}${selectedMedia?.url}`
                    )
                  }
                  alt={`Image ${selectedMedia?.url}`}
                />
              </div>
            )}
          </div>
        )}

        {isMobile && (
          <Slider
            spaceBetween={10}
            slidesPerView={1}
            className='product-slider mobile_viewimages'
            pagination={true}
          >
            {values.productImage?.map((media, index) => (
              <div key={index}>
                {media?.type?.toLowerCase() === 'image' ? (
                  <Image
                    src={encodeURI(
                      `${reactImageUrl}${_productImg_}${media?.url}`
                    )}
                    alt={`Image - ${media?.url}`}
                    height={0}
                    width={0}
                    quality={100}
                    sizes='100vw'
                  />
                ) : (
                  <div
                    className='video-thumbnail slider-video-thumbnail'
                    onClick={() => handlePosterClick(media)}
                  >
                    <Image
                      src={`https://img.youtube.com/vi/${getEmbeddedUrlFromYouTubeUrl(
                        media?.url
                      )}/0.jpg`}
                      alt={`Video - ${media?.url}`}
                      height={0}
                      width={0}
                      quality={100}
                      sizes='100vw'
                    />
                    <span className='play-icon'>
                      <i className='m-icon play_video_icon'></i>
                    </span>
                  </div>
                )}
              </div>
            ))}
          </Slider>
        )}
      </div>

      <button
        type='button'
        className='m-btn btn-whishlist wishlist_icon'
        onClick={async () => {
          if (user?.userId) {
            setLoading(true)
            const response = await handleWishlistClick(
              values,
              values,
              'specificProduct',
              toast,
              setToast
            )
            setLoading(false)
            if (response?.wishlistResponse?.data?.code === 200) {
              setValues(response)
            } else if (response?.code === 500) {
              router?.push('/')
            } else {
              setValues(values)
            }
            response?.wishlistResponse &&
              showToast(toast, setToast, response?.wishlistResponse)
          } else {
            if (userIdCookie) {
              const authenticatedUser = await checkTokenAuthentication(
                toast,
                setToast
              )
              if (authenticatedUser === userIdCookie) {
                if (fetchProduct) {
                  await fetchProduct(product)
                }
              }
            } else {
              setLoading(false)
              setModalShow({
                show: true,
                data: values,
                module: { type: 'wishlist' }
              })
            }
          }
        }}
      >
        <i
          className={`m-icon m-wishlist-icon ${
            values?.isWishlistProduct ? 'wishlist-checked' : ''
          }`}
        ></i>
      </button>

      {isModalOpen &&
        isMobile &&
        selectedMedia?.type?.toLowerCase() === 'video' && (
          <div className='modal-video_md' onClick={closeModal}>
            <div className='video-container'>
              <iframe
                src={`https://www.youtube.com/embed/${getEmbeddedUrlFromYouTubeUrl(
                  selectedMedia?.url
                )}?autoplay=1`}
                title='Video Player'
                width='100%'
                height='450'
              ></iframe>
            </div>
          </div>
        )}
    </div>
  )
}

export default ProductDetail
