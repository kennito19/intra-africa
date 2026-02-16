import { useRouter } from 'next/navigation'
import { useSelector } from 'react-redux'
import axiosProvider from '../lib/AxiosProvider'
import { showToast } from '../lib/GetBaseUrl'
import { _exception } from '../lib/exceptionMessage'
import { _toaster } from '../lib/tosterMessage'

const AddressSection = ({
  values,
  setValues,
  setFieldValue,
  setModalShow,
  modalShow,
  setActiveAccordion,
  buttonText,
  handleAccordionChange,
  setLoading,
  cartCalculation,
  toast,
  setToast
}) => {
  const router = useRouter()
  const { address } = useSelector((state) => state?.address)
  return (
    <div>
      <div className='d-address-main'>
        {address && address?.length > 0 && (
          <>
            {address?.map((item) => (
              <div className='d-address-name' key={item?.id}>
                <div className='d-address-details'>
                  <input
                    id={`default-radio-${item?.id}`}
                    type='radio'
                    name='default-radio'
                    className='onSubmitAddress'
                    checked={item?.id === values?.addressVal?.id}
                    onChange={() => {
                      if (setFieldValue) {
                        setFieldValue('addressVal', item)
                      } else {
                        setValues({ ...values, addressVal: item })
                      }
                    }}
                  />
                  <label
                    for={`default-radio-${item?.id}`}
                    className='d-adress-title'
                  >
                    {item?.fullName}
                  </label>
                  <p className='delivery-address'>
                    {`${item?.addressLine1}, ${item?.addressLine2}, ${item?.cityName}, ${item?.stateName}`}
                    - <b>{item?.pincode}</b>
                  </p>
                  <button
                    id='deliverHereButton'
                    className='m-btn checkout_btn'
                    onClick={async () => {
                      if (handleAccordionChange) {
                        if (!values?.addressVal) {
                          setValues({ ...values, addressVal: item })
                        } else {
                          setValues({
                            ...values,
                            addressVal: address?.find(
                              (data) => data?.id === values?.addressVal?.id
                            )
                          })
                        }
                      }
                      try {
                        const response = await axiosProvider({
                          method: 'GET',
                          endpoint: `Delivery/byPincode?pincode=${item?.pincode}`
                        })

                        if (response?.status === 200) {
                          if (
                            response?.data?.data?.pincode ===
                              Number(item?.pincode) &&
                            response?.data?.data?.status === 'Active'
                          ) {
                            if (handleAccordionChange) {
                              handleAccordionChange(1)
                              cartCalculation(false, false, false, true, item)
                            } else {
                              if (values?.actionID && values?.actionID !== 1) {
                                const ReplaceValue = {
                                  ...values,
                                  addressLine1: item.addressLine1,
                                  addressLine2: item?.addressLine2,
                                  landmark: item?.landmark,
                                  pincode: Number(item?.pincode),
                                  city: item?.cityName,
                                  state: item?.stateName,
                                  country: item?.countryName
                                }
                                try {
                                  setLoading(true)
                                  const response = await axiosProvider({
                                    method: 'POST',
                                    endpoint: 'ManageOrder/OrderReplace',
                                    data: ReplaceValue
                                  })
                                  setLoading(false)
                                  if (response?.data?.code === 200) {
                                    showToast(toast, setToast, response)
                                    setTimeout(() => {
                                      router?.push('/')
                                    }, 1000)
                                  }
                                } catch (error) {
                                  setLoading(false)
                                  showToast(toast, setToast, {
                                    data: {
                                      code: 204,
                                      message: _exception?.message
                                    }
                                  })
                                }
                              }
                            }
                            setActiveAccordion(2)
                          } else {
                            setActiveAccordion(1)
                            showToast(toast, setToast, {
                              data: {
                                code: 204,
                                message: _toaster?.pinCodeError
                              }
                            })
                          }
                        }
                      } catch {
                        showToast(toast, setToast, {
                          data: {
                            code: 204,
                            message: _toaster?.pinCodeError
                          }
                        })
                      }
                    }}
                    type='button'
                  >
                    {buttonText ? buttonText : 'Deliver Here'}
                  </button>
                </div>
                <div className='d-edit-btn'>
                  <button
                    className='m-icon edit-btn-delivery_responsive'
                    onClick={() => {
                      setModalShow({
                        show: !modalShow?.show,
                        data: item,
                        type: 'address'
                      })
                    }}
                    type='button'
                  >
                    <i className='m-icon m-edit-icon'></i>
                  </button>
                </div>
              </div>
            ))}
          </>
        )}
      </div>
      <div className='add-new-address-btn'>
        <button
          id='AddNewAddress'
          className='m-btn new-address-btn'
          type='button'
          onClick={() => {
            setModalShow({ show: !modalShow.show, data: null, type: 'address' })
          }}
        >
          <i className='m-icon m-new-address-icon'></i>Add new address
        </button>
      </div>
    </div>
  )
}

export default AddressSection
