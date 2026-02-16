import PriceDetails from '@/components/PriceDetails'
import MixedCaptcha from '@/components/base/MixedCaptcha'
import ModalComponent from '@/components/base/ModalComponent'
import { generateCaptcha } from '@/lib/AllGlobalFunction'
import Image from 'next/image'
import { useSelector } from 'react-redux'
import AccordionCheckout from '../../../../components/AccordionCheckout'
import AddressModal from '../../../../components/AddressModal'
import AddressSection from '../../../../components/AddressSection'
import AddToCartProduct from '../../../../components/misc/AddToCartProduct'
import {
  convertToNumber,
  currencyIcon,
  maximumOrderValue,
  minimumOrderValue,
  showToast
} from '../../../../lib/GetBaseUrl'
import { _toaster } from '../../../../lib/tosterMessage'
import Skeleton from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

const UserCheckout = ({
  data,
  setData,
  activeAccordion,
  setActiveAccordion,
  modalShow,
  setModalShow,
  values,
  setValues,
  cartCalculation,
  setLoading,
  toast,
  setToast,
  handleAccordionChange,
  fetchAddress,
  onSubmit,
  fetchPinCodeAndCheckCart
}) => {
  const { cart } = useSelector((state) => state?.cart)
  const { user } = useSelector((state) => state?.user)
  const inStockCart = cart?.items?.filter((item) => item?.status === 'In stock')
  return (
    <>
      <AccordionCheckout
        accordiontitle={'Login'}
        isActive={activeAccordion === 0}
        activeAccordion={activeAccordion}
        Name={user?.fullName}
        Content={user?.mobileNo}
        index={0}
        change
        toggleAccordion={() => setActiveAccordion(0)}
      />

      <AccordionCheckout
        id={'delivery-address'}
        accordiontitle={'Delivery address'}
        isActive={activeAccordion === 1}
        activeAccordion={activeAccordion}
        toggleAccordion={() => {
          fetchAddress(values?.addressVal?.id, true)
          setActiveAccordion(1)
        }}
        Name={values?.addressVal?.fullName ?? ''}
        index={1}
        Content={
          Object?.keys(values?.addressVal).length > 0 ? (
            `${values?.addressVal?.addressLine1}, ${values?.addressVal?.addressLine2}, ${values?.addressVal?.cityName}, ${values?.addressVal?.stateName} - ${values?.addressVal?.pincode}`
          ) : (
            <Skeleton width='400px' />
          )
        }
        accordioncontent={
          <AddressSection
            values={values}
            setValues={setValues}
            setActiveAccordion={setActiveAccordion}
            setModalShow={setModalShow}
            modalShow={modalShow}
            handleAccordionChange={handleAccordionChange}
            cartCalculation={cartCalculation}
            toast={toast}
            setToast={setToast}
          />
        }
      />

      <AccordionCheckout
        id={'order-summary'}
        accordiontitle={'Order summary'}
        isActive={activeAccordion === 2}
        activeAccordion={activeAccordion}
        toggleAccordion={() => setActiveAccordion(2)}
        Name={`${cart?.items?.length} ${
          cart?.items?.length === 1 ? 'Item' : 'Items'
        }`}
        index={2}
        accordioncontent={
          <>
            {cart && cart?.items?.length > 0 && (
              <AddToCartProduct
                data={data}
                stateValues={values}
                cartCalculation={cartCalculation}
                setLoading={setLoading}
                toast={toast}
                setToast={setToast}
                setData={setData}
              />
            )}
            <div className='summary-continue'>
              <button
                className='checkout_btn m-btn'
                onClick={() => {
                  if (inStockCart?.length === cart?.items?.length) {
                    handleAccordionChange(activeAccordion)
                    setActiveAccordion(3)
                    setValues({ ...values, captchaValue: generateCaptcha() })
                  } else {
                    showToast(toast, setToast, {
                      data: {
                        code: 204,
                        message: _toaster?.OutOfstockProduct
                      }
                    })
                  }
                }}
                type='button'
              >
                Continue
              </button>
            </div>
          </>
        }
      />

      <AccordionCheckout
        accordiontitle={'Payment Options'}
        isActive={activeAccordion === 3}
        activeAccordion={activeAccordion}
        toggleAccordion={() => setActiveAccordion(3)}
        index={3}
        accordioncontent={
          <div className='payment_options_all'>
            {convertToNumber(values?.CartAmount?.paid_amount) <=
              maximumOrderValue && (
              <div
                className='cash_ondelivery'
                onClick={() => {
                  if (
                    convertToNumber(values?.CartAmount?.paid_amount) >=
                    minimumOrderValue
                  ) {
                    setActiveAccordion(3)
                    fetchPinCodeAndCheckCart(values?.addressVal, false, 'cod')
                    setValues({
                      ...values,
                      paymentMode: 'cod',
                      captchaValue: generateCaptcha()
                    })
                  } else {
                    showToast(toast, setToast, {
                      data: {
                        message: `The minimum purchase Value is ${currencyIcon}${minimumOrderValue}.`,
                        code: 204
                      }
                    })
                  }
                }}
              >
                <Image
                  src='/images/payment/pg-cod.png'
                  alt='COD Paymemt'
                  className='images_paymentgateway'
                  width='0'
                  height='0'
                  quality={100}
                  sizes='100vw'
                />
              </div>
            )}
            <div
              className='online_delivery'
              onClick={() => {
                setActiveAccordion(3)
                setValues({ ...values, paymentMode: 'online' })
                onSubmit({
                  ...values,
                  paymentMode: 'online',
                  codCharge: 0,
                  codMessage: ''
                })
              }}
            >
              <Image
                src='/images/payment/pg-net-banking.png'
                alt='Net banking Paymemt'
                className='images_paymentgateway'
                width='0'
                height='0'
                quality={100}
                sizes='100vw'
              />
            </div>
          </div>
        }
      />

      {modalShow?.show && modalShow?.type === 'address' && (
        <AddressModal
          modalShow={modalShow}
          setModalShow={setModalShow}
          fetchAllAddress={fetchAddress}
          setLoading={setLoading}
          stateValues={values}
          setStateValues={setValues}
          setActiveAccordion={setActiveAccordion}
        />
      )}
      {modalShow?.show && modalShow?.type === 'cod' && (
        <ModalComponent
          isOpen={true}
          modalSize={'modal-sm'}
          headClass={'HeaderText'}
          headingText={'Cash On Delivery'}
          onClose={() => {
            setModalShow({ show: false, type: '' })
            setValues({
              ...values,
              captchaValue: generateCaptcha(),
              paymentMode: null,
              captchaError: ''
            })
          }}
        >
          <div className='codpayment_modalwrapper'>
            {Boolean(values?.codCharge) && (
              <div className='paymentmode-cod-badge'>
                {values?.codMessage} :&nbsp;
                <span className='cod-badge-charge'>
                  {currencyIcon}
                  {values?.codCharge}
                </span>
              </div>
            )}

            <MixedCaptcha
              values={values}
              setValues={setValues}
              onSubmit={onSubmit}
            />
            <PriceDetails cart={data?.data} />
          </div>
        </ModalComponent>
      )}
    </>
  )
}

export default UserCheckout
