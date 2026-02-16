import React from 'react'
import InputComponent from './base/InputComponent'
import axiosProvider from '../lib/AxiosProvider'

const PincodeCheck = ({ values, setValues }) => {
  return (
    <>
      <div className='pincode_fl-main'>
        <InputComponent
          type={'text'}
          placeholder={'Enter Pincode'}
          labelClass={'dl_fl-fle'}
          maxLength={6}
          labelText={
            <>
              DELIVERY OPTIONS <i className='m-icon m-icon-delevery-cart'></i>
            </>
          }
          value={values?.pinCodeValue}
          onChange={(event) => {
            const inputValue = event?.target?.value
            const regex = /^[0-9\b]+$/
            if (inputValue === '' || regex.test(inputValue)) {
              setValues({ ...values, pinCodeValue: inputValue })
            }
          }}
          onKeyDown={(e) => {
            if (e.key === 'Enter' && values?.pinCodeValue?.length === 6) {
              document.getElementById('check-pincode').click()
            }
          }}
          // onBlur={() => {
          //   setValues({
          //     ...values,
          //     pinCodeValue: values?.pinCodeCheckValue?.trim()
          //   })
          // }}
        />
        <button
          id='check-pincode'
          type='button'
          onClick={async () => {
            if (values?.pinCodeValue?.length === 6) {
              try {
                const response = await axiosProvider({
                  method: 'GET',
                  endpoint: `Delivery/byPincode?pincode=${values?.pinCodeValue}`
                })
                if (response?.data?.code === 200) {
                  setValues({
                    ...values,
                    deliveryDays: response?.data?.data?.deliveryDays,
                    pincodeError: ''
                  })
                } else {
                  setValues({
                    ...values,
                    deliveryDays: null,
                    pincodeError: 'Not a valid PIN code.'
                  })
                }
              } catch (error) {
                setValues({
                  ...values,
                  deliveryDays: null,
                  pincodeError: 'Not a valid PIN code.'
                })
              }
            } else {
              setValues({
                ...values,
                deliveryDays: null,
                pincodeError: 'Please enter PIN code.'
              })
            }
          }}
          className='btn-check'
        >
          Check
        </button>
        {values?.deliveryDays ? (
          <p className='success_msg-succ'>
            Faster delivery by {values?.deliveryDays} Days
          </p>
        ) : (
          values?.pincodeError && (
            <p className='err_msg-msg'>{values?.pincodeError}</p>
          )
        )}
      </div>
    </>
  )
}

export default PincodeCheck
