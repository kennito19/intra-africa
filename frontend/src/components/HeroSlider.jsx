import React from 'react'
import Slider from './Slider'
import Link from 'next/link'
import Image from 'next/image'
import { _homePageImg_, _lendingPageImg_ } from '../lib/ImagePath'
import { checkCase, reactImageUrl } from '../lib/GetBaseUrl'
import DynamicPositionComponent from './DynamicPositionComponent'

const Heroslider = ({ layoutsInfo, section, fromLendingPage = false }) => {
  return (
    <section
      style={{ backgroundColor: section?.background_color?.toLowerCase() }}
    >
      {section?.status?.toLowerCase() === 'active' && (
        <div className='site-container categories-section section_spacing_b'>
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
                spaceBetween={10}
                slidesPerView={1}
                loop={false}
                withArrows={false}
                showShareBtn={true}
                autoplay={3000}
                navigation={
                  section?.columns?.left?.single?.length > 1 &&
                  layoutsInfo?.layout_class?.toLowerCase()?.includes('arrows')
                }
                pagination={
                  section?.columns?.left?.single?.length > 1 &&
                  layoutsInfo?.layout_class?.toLowerCase()?.includes('dots') &&
                  true
                }
              >
                {section &&
                  section?.columns?.left?.single?.length > 0 &&
                  section?.columns?.left?.single?.map((imageObj) => {
                    return (
                      <Link
                        href={checkCase(imageObj)}
                        target={
                          imageObj?.redirect_to === 'Custom link'
                            ? '_blank'
                            : '_self'
                        }
                        key={Math.floor(Math.random() * 1000000)}
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
                          className='hero-slider-img'
                          width='0'
                          height='0'
                          sizes='100vw'
                        />
                      </Link>
                    )
                  })}
              </Slider>
            </DynamicPositionComponent>
          </div>
        </div>
      )}
    </section>
  )
}

export default Heroslider
