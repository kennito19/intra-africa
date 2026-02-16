import { useEffect, useRef } from "react";
import SwiperCore, { Autoplay, Navigation, Pagination } from "swiper";
import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import { Swiper, SwiperSlide } from "swiper/react";

SwiperCore.use([Navigation, Pagination, Autoplay]);

function Slider({
  children,
  spaceBetween,
  loop,
  slidesPerView,
  speed,
  autoplay,
  breakpoints,
  navigation,
  pagination,
  className,
  activeSlideIndex,
  withPadding,
}) {
  const swiperRef = useRef(null);

  useEffect(() => {
    const handleMouseEnter = () => {
      if (swiperRef.current && swiperRef.current.swiper && swiperRef.current.swiper.autoplay) {
        swiperRef.current.swiper.autoplay.stop();
      }
    };

    const handleMouseLeave = () => {
      if (swiperRef.current && swiperRef.current.swiper && swiperRef.current.swiper.autoplay) {
        swiperRef.current.swiper.autoplay.start();
      }
    };

    const swiper = swiperRef.current?.swiper;

    if (swiper) {
      swiper.el.addEventListener('mouseenter', handleMouseEnter);
      swiper.el.addEventListener('mouseleave', handleMouseLeave);
    }
  }, []);

  return (
    <Swiper
    ref={swiperRef}

      modules={[Navigation, Pagination]}
      spaceBetween={
        withPadding ? spaceBetween : spaceBetween ? spaceBetween : 0
      }
      slidesPerView={slidesPerView}
      loop={loop || false}
      speed={speed || 500}
      className={className}
      autoplay={
        autoplay
          ? {
              delay: 3000,
            }
          : false
      }
      navigation={navigation ? true : false}
      pagination={pagination ? { clickable: true } : false}
      breakpoints={breakpoints}
      onSlideChange={(swiper) => {
        if (activeSlideIndex) {
          activeSlideIndex(swiper.activeIndex);
        }
      }}
    >
      {children &&
        children?.map((child, index) => (
          <SwiperSlide key={Math.floor(Math.random() * 100000)}>
            {child}
          </SwiperSlide>
        ))}
    </Swiper>
  );
}

export default Slider;
