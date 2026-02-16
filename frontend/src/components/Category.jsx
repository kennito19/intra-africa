import React from 'react'
import Image from 'next/image'
import Link from 'next/link'
import Slider from './Slider'
import { _homePageImg_, _lendingPageImg_ } from '../lib/ImagePath'
import { checkCase, reactImageUrl } from '../lib/GetBaseUrl'
import DynamicPositionComponent from './DynamicPositionComponent'

const Categories = ({ layoutsInfo, section, fromLendingPage = false }) => {
  const withSlider = layoutsInfo?.layout_class
    ?.toLowerCase()
    ?.includes('slider')

  const withPadding = layoutsInfo?.layout_class
    ?.toLowerCase()
    ?.includes('with-pd')

  return (
    <div>
      {section?.status?.toLowerCase() === 'active' && (
        <section
          style={{ backgroundColor: section?.background_color?.toLowerCase() }}
        >
          {withSlider ? (
            <div>
              {section?.columns?.left?.single?.length > 0 && (
                <div className='site-container categories-section section_spacing_b'>
                  <div className='categories-wrapper'>
                    <DynamicPositionComponent
                      heading={section?.title}
                      paragraph={section?.sub_title}
                      btnText={section?.link_text}
                      redirectTo={section?.link ?? '#'}
                      headingPosition={section?.title_position?.toLowerCase()}
                      buttonPosition={section?.link_position?.toLowerCase()}
                      buttonPositionDirection={section?.link_in?.toLowerCase()}
                      TitleColor={section?.title_color?.toLowerCase()}
                      TextColor={section?.text_color?.toLowerCase()}
                    >
                      <Slider
                        className='category-slider-main'
                        spaceBetween={10}
                        withPadding={withPadding}
                        slidesPerView={5}
                        loop={false}
                        withArrows={false}
                        showShareBtn={true}
                        autoplay={true}
                        navigation={true}
                        pagination={false}
                        breakpoints={{
                          320: {
                            slidesPerView: 1
                          },

                          768: {
                            slidesPerView: 3
                          },
                          1024: {
                            slidesPerView: 4
                          }
                          // 1280: {
                          //   slidesPerView:
                          //     section?.columns?.left?.single.length <= 7
                          //       ? section?.columns?.left?.single.length
                          //       : 7
                          // }
                        }}
                      >
                        {section?.columns?.left?.single?.length > 0 &&
                          section?.columns?.left?.single.map((imageObj) => {
                            return (
                              <Link
                                href={checkCase(imageObj)}
                                target={
                                  imageObj?.redirect_to === 'Custom link'
                                    ? '_blank'
                                    : '_self'
                                }
                                className='categories-col'
                                key={Math.floor(Math.random() * 100000)}
                              >
                                <Image
                                  src={
                                    imageObj &&
                                    encodeURI(
                                      `${reactImageUrl}${
                                        fromLendingPage
                                          ? _lendingPageImg_
                                          : _homePageImg_
                                      }${imageObj?.image}`
                                    )
                                  }
                                  alt={imageObj?.image_alt ?? 'image-alt'}
                                  className='categories-img'
                                  width={300}
                                  height={300}
                                />
                                <p className='categories-name'>
                                  {imageObj.name}
                                </p>
                              </Link>
                            )
                          })}
                      </Slider>
                    </DynamicPositionComponent>
                  </div>
                </div>
              )}
            </div>
          ) : (
            <div>
              {section?.columns?.left?.single?.length >
              section?.SectionColumns ? (
                <div className='site-container categories-section section_spacing_b'>
                  <div className='categories-wrapper'>
                    <DynamicPositionComponent
                      heading={section?.title}
                      paragraph={section?.sub_title}
                      btnText={section?.link_text}
                      redirectTo={section?.link ?? '#'}
                      headingPosition={section?.title_position?.toLowerCase()}
                      buttonPosition={section?.link_position?.toLowerCase()}
                      buttonPositionDirection={section?.link_in?.toLowerCase()}
                      TitleColor={section?.title_color?.toLowerCase()}
                      TextColor={section?.text_color?.toLowerCase()}
                    >
                      <div
                        className='pv-home-thumbline-main'
                        style={{
                          gridTemplateColumns: `repeat(${
                            section?.SectionColumns > 0
                              ? section?.SectionColumns
                              : section?.columns?.left?.single?.length < 4
                              ? 4
                              : section?.columns?.left?.single?.length > 7
                              ? 7
                              : section?.columns?.left?.single?.length
                          }, minmax(0, 1fr))`,
                          gap: `${withPadding ? '10px' : '0px'}`
                        }}
                      >
                        {section?.columns?.left?.single?.length > 0 &&
                          section?.columns?.left?.single.map((imageObj) => {
                            return (
                              <Link
                                href={checkCase(imageObj)}
                                target={
                                  imageObj?.redirect_to === 'Custom link'
                                    ? '_blank'
                                    : '_self'
                                }
                                className='categories-col'
                                key={Math.floor(Math.random() * 100000)}
                              >
                                <Image
                                  src={
                                    imageObj &&
                                    encodeURI(
                                      `${reactImageUrl}${
                                        fromLendingPage
                                          ? _lendingPageImg_
                                          : _homePageImg_
                                      }${imageObj?.image}`
                                    )
                                  }
                                  alt={imageObj?.image_alt ?? 'image-alt'}
                                  className='categories-img'
                                  width={300}
                                  height={300}
                                />
                                <p className='categories-name'>
                                  {imageObj.name}
                                </p>
                              </Link>
                            )
                          })}
                      </div>
                    </DynamicPositionComponent>
                  </div>
                </div>
              ) : (
                <div className='site-container categories-section section_spacing_b'>
                  <div className='categories-wrapper'>
                    <DynamicPositionComponent
                      heading={section?.title}
                      paragraph={section?.sub_title}
                      btnText={section?.link_text}
                      redirectTo={section?.link ?? '#'}
                      headingPosition={section?.title_position?.toLowerCase()}
                      buttonPosition={section?.link_position?.toLowerCase()}
                      buttonPositionDirection={section?.link_in?.toLowerCase()}
                      TitleColor={section?.title_color?.toLowerCase()}
                      TextColor={section?.text_color?.toLowerCase()}
                    >
                      <Slider
                        spaceBetween={10}
                        slidesPerView={5}
                        withPadding={withPadding}
                        loop={true}
                        withArrows={false}
                        showShareBtn={true}
                        autoplay={true}
                        navigation={true}
                        pagination={false}
                        breakpoints={{
                          0: {
                            slidesPerView: 1
                          },

                          768: {
                            slidesPerView: 3
                          },
                          1024: {
                            slidesPerView: 4
                          },
                          1280: {
                            slidesPerView: 5
                          }
                        }}
                      >
                        {section?.columns?.left?.single?.length > 0 &&
                          section?.columns?.left?.single.map((imageObj) => {
                            return (
                              <Link
                                href={checkCase(imageObj)}
                                target={
                                  imageObj?.redirect_to === 'Custom link'
                                    ? '_blank'
                                    : '_self'
                                }
                                className='categories-col'
                                key={Math.floor(Math.random() * 100000)}
                              >
                                <Image
                                  src={
                                    imageObj &&
                                    encodeURI(
                                      `${reactImageUrl}${
                                        fromLendingPage
                                          ? _lendingPageImg_
                                          : _homePageImg_
                                      }${imageObj?.image}`
                                    )
                                  }
                                  alt={imageObj?.image_alt ?? 'image-alt'}
                                  className='categories-img'
                                  width={300}
                                  height={300}
                                />
                                <p className='categories-name'>
                                  {imageObj.name}
                                </p>
                              </Link>
                            )
                          })}
                      </Slider>
                    </DynamicPositionComponent>
                  </div>
                </div>
              )}
            </div>
          )}
        </section>
      )}
    </div>
  )
}

export default Categories
