import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const ProductDetailSkeleton = () => {
  return (
    <div className='site-container'>
      <div className='breadcrumb_wrapper'>
        <Skeleton />
      </div>
      <div className='product_details_wrapper'>
        <div className='product_images_wrapper'>
          <Skeleton height={600} />
        </div>

        <div className='product_contents_details'>
          <div className='products_pricing_details'>
            <div className='product_brands_wishlist_icon'>
              <p className='prdct__brands_nm'>
                <Skeleton width={150} />
              </p>
            </div>
            <h1 className='product_name'>
              <Skeleton />
            </h1>
            <div className='product_pricong_offer_deliverychrg'>
              <span className='total_pricing_product'>
                <Skeleton width={90} />
              </span>
              <span className='actual_pricing_product_mrp'>
                <Skeleton width={90} />
              </span>
              <span className='actual_pricing_product_dis'>
                <Skeleton width={70} />
              </span>
            </div>
            <div className='prd_clr_varients_img'>
              <div className='prd_clr_varients'>
                <span className='rd_selectsize_label-seller'>
                  <Skeleton width={130} />
                </span>
                <div className='prd_varients_imgs'>
                  <Skeleton height={30} width={700} />
                </div>
              </div>
            </div>
            <div className='prd_clr_varients_img'>
              <div className='prd_clr_varients'>
                <span className='rd_selectsize_label-seller'>
                  <Skeleton width={130} />
                </span>
                <div className='prd_varients_imgs'>
                  <Skeleton height={30} width={700} />
                </div>
              </div>
            </div>
            <div className='prdt_btns_atc_byn_wrapper'>
              <Skeleton width={130} height={35} />

              <Skeleton width={130} height={35} />
            </div>
          </div>

          <div className='products_pricing_details'>
            <Skeleton height={60} />
          </div>

          <div className='prdt_best_offers_wrapper'>
            <p className='prdt_best_offer_lable'>
              <Skeleton width={200} />
            </p>
            <div className='prdt_best_offers_wrapper-inner'>
              <Skeleton height={50} />
            </div>
          </div>

          <div className='prdt_other_sellers_wrapper'>
            <h3 className='title_other_sellers'>
              <Skeleton width={200} />
            </h3>
            <div className='prdt_other_seller_card'>
              <Skeleton height={70} />
            </div>
          </div>
          <ul className='m-prd-sidebar__list'>
            <li className='m-prd-slidebar__item is-open' id='id-p-detail'>
              <a className='m-prd-slidebar__name'>
                <Skeleton width={700} height={100} />
              </a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  )
}

export default ProductDetailSkeleton
