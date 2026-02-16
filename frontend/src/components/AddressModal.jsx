'use client'
import { focusInput } from '@/lib/AllGlobalFunction'
import { addressData } from '@/redux/features/addressSlice'
import { ErrorMessage, Form, Formik } from 'formik'
import { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useImmer } from 'use-immer'
import * as Yup from 'yup'
import axiosProvider from '../lib/AxiosProvider'
import { _exception } from '../lib/exceptionMessage'
import { fetchData, fetchDataFromApis, showToast } from '../lib/GetBaseUrl'
import { _alphabetRegex_, _phoneNumberRegex_ } from '../lib/Regex'
import InputComponent from './base/InputComponent'
import IpRadio from './base/IpRadio'
import ModalComponent from './base/ModalComponent'
import TextError from './base/TextError'

const AddressModal = ({
  modalShow,
  setModalShow,
  fetchAllAddress,
  // fetchPinCodeAndCheck,
  setLoading,
  stateValues
}) => {
  const dispatch = useDispatch()
  const { user } = useSelector((state) => state?.user)
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const [allState, setAllState] = useImmer({
    country: [],
    state: [],
    country: []
  })
  const initVal = {
    userID: user?.userId,
    addressType: 'home',
    fullName: '',
    mobileNo: '',
    addressLine1: '',
    addressLine2: '',
    landmark: '',
    pincode: '',
    stateId: '',
    cityId: '',
    countryId: '',
    status: 'Active',
    setDefault: false,
    countryName: '',
    cityName: '',
    stateName: ''
  }
  const [initialValues, setInitialValues] = useState(
    modalShow?.data ? modalShow?.data : initVal
  )

  const validationSchema = Yup.object().shape({
    fullName: Yup.string().required('Please enter Name'),
    mobileNo: Yup.string()
      .required('Mobile number is required')
      .matches(/^\d{10}$/, 'Mobile number must be a 10-digit number'),
    addressLine1: Yup.string().required(
      'Please enter your Flat No, House No, Building, Company, Apartment.'
    ),
    addressLine2: Yup.string().required(
      'Please enter your Area, Street, Sector, or Village.'
    ),
    countryName: Yup.string().required('Select country'),
    cityName: Yup.string().min(1, 'Select city').required('Select city'),
    stateName: Yup.string().min(1, 'Select state').required('Select state'),
    pincode: Yup.string()
      .required('Pincode is required')
      .matches(/^\d{6}$/, 'Pincode must be a 6-digit number'),
    addressType: Yup.string().required('please choose one address type')
  })

  const onSubmit = async (values) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: !values?.id ? 'POST' : 'PUT',
        endpoint: 'Address',
        data: values
      })
      setLoading(false)

      if (response?.data?.code === 200) {
        if (fetchAllAddress) {
          await fetchAllAddress(values?.id ? values?.id : response?.data?.data)
        } else {
          setLoading(true)
          fetchData('Address/byUserId', { userId: user?.userId }, (resp) => {
            dispatch(addressData(resp?.data?.data))
          })
          setLoading(false)
        }
        setModalShow({ show: !modalShow?.show, data: null })
      }
      showToast(toast, setToast, response)
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  useEffect(() => {
    if (!allState?.country?.length) {
      fetchDropDownData()
    }
  }, [])

  const fetchDropDownData = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'Country/search',
        queryString: `?pageindex=0&PageSize=0`
      })
      setLoading(false)
      if (response?.status === 200) {
        setAllState((draft) => {
          draft.country = response?.data?.data
        })
      }
    } catch {
      setLoading(false)
    }
  }

  const fetchStateCity = (countryId = false, stateId = false) => {
    if (countryId) {
      fetchData(
        'State/ByCountryId',
        { id: countryId, pageIndex: 0, pageSize: 0 },
        (resp) => {
          if (resp?.status === 200) {
            setAllState((draft) => {
              draft.state = resp?.data?.data
            })

            if (stateId) {
              fetchData(
                'City/ByStateId',
                { id: stateId, pageIndex: 0, pageSize: 0 },
                (resp) => {
                  if (resp?.status === 200) {
                    setAllState((draft) => {
                      draft.city = resp?.data?.data
                    })
                  }
                }
              )
            }
          }
        }
      )
    } else if (!countryId && stateId) {
      fetchData(
        'City/ByStateId',
        { id: stateId, pageIndex: 0, pageSize: 0 },
        (resp) => {
          if (resp?.status === 200) {
            setAllState((draft) => {
              draft.city = resp?.data?.data
            })
          }
        }
      )
    }
  }

  useEffect(() => {
    if (initialValues?.countryId) {
      fetchStateCity(initialValues?.countryId, initialValues?.stateId)
    }
  }, [initialValues])

  return (
    <div>
      <ModalComponent
        isOpen={true}
        onClose={() => {
          setModalShow({ show: !modalShow.show, data: null })
        }}
        modalSize={'modal-lg'}
        headingText={!initialValues?.id ? 'Add a new address' : 'Edit address'}
        headClass={'HeaderText'}
        bodyClass={'modal-body'}
      >
        <Formik
          enableReinitialize={true}
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={onSubmit}
        >
          {({ values, setFieldValue, validateForm }) => (
            <Form>
              <div className='input_form'>
                <InputComponent
                  id='fullName'
                  name='fullName'
                  labelText={'Full Name'}
                  value={values?.fullName}
                  type={'text'}
                  autoFocus
                  maxLength={'40'}
                  MainHeadClass={'input_filed_50'}
                  required
                  placeholder={'Enter your name'}
                  onChange={(e) => {
                    const inputText = e?.target?.value
                    const fieldName = e?.target?.name
                    const isValid = _alphabetRegex_?.test(inputText)
                    if (isValid || !inputText) {
                      setFieldValue([fieldName], inputText)
                    }
                  }}
                  onBlur={(e) => {
                    let fieldName = e?.target?.name
                    setFieldValue(fieldName, values[fieldName]?.trim())
                  }}
                />

                <InputComponent
                  labelText={'Mobile Number'}
                  type={'text'}
                  maxLength={10}
                  id='mobileNo'
                  name='mobileNo'
                  MainHeadClass={'input_filed_50'}
                  value={values?.mobileNo}
                  required
                  placeholder={'Enter your Mobile number'}
                  onChange={(event) => {
                    const inputValue = event.target.value
                    const fieldName = event?.target?.name
                    const isValid = _phoneNumberRegex_?.test(inputValue)
                    if (!inputValue || isValid) {
                      setFieldValue([fieldName], inputValue)
                    }
                  }}
                  onBlur={(e) => {
                    let fieldName = e?.target?.name
                    setFieldValue(fieldName, values[fieldName]?.trim())
                  }}
                />

                <InputComponent
                  name='addressLine1'
                  id='addressLine1'
                  value={values?.addressLine1}
                  required
                  type='text'
                  maxLength={45}
                  labelText={'Flat No, House No, Building, Company, Apartment'}
                  placeholder={'Enter your Address line'}
                  MainHeadClass={'input_filed_50'}
                  onChange={(e) => {
                    setFieldValue('addressLine1', e?.target?.value)
                  }}
                  onBlur={(e) => {
                    let fieldName = e?.target?.name
                    setFieldValue(fieldName, values[fieldName]?.trim())
                  }}
                />

                <InputComponent
                  id='addressLine2'
                  name='addressLine2'
                  value={values?.addressLine2}
                  required
                  type='text'
                  maxLength={45}
                  labelText={'Area, Street, Sector, Village'}
                  MainHeadClass={'input_filed_50'}
                  placeholder={'Enter your Address line'}
                  onChange={(e) => {
                    setFieldValue('addressLine2', e?.target?.value)
                  }}
                  onBlur={(e) => {
                    let fieldName = e?.target?.name
                    setFieldValue(fieldName, values[fieldName]?.trim())
                  }}
                />

                <InputComponent
                  id='pincode'
                  name='pincode'
                  labelText={'Pincode'}
                  value={values?.pincode}
                  required
                  type={'text'}
                  maxLength={6}
                  MainHeadClass={'input_filed_50'}
                  placeholder={'Enter your Pincode'}
                  onChange={async (event) => {
                    const inputValue = event?.target?.value
                    const regex = /^[0-9]\d{0,5}$/

                    if (inputValue === '' || regex.test(inputValue)) {
                      setFieldValue('pincode', inputValue)
                    }

                    if (inputValue?.length === 6) {
                      try {
                        const response = await axiosProvider({
                          method: 'GET',
                          endpoint: `Delivery/byPincode?pincode=${inputValue}`
                        })

                        if (response?.status === 200) {
                          if (response?.data?.data?.countryID) {
                            let { data } = response?.data
                            if (!allState?.country?.length) {
                              fetchDataFromApis(
                                'Country/search',
                                `?pageindex=0&PageSize=0`,
                                (data) => {
                                  setAllState((draft) => {
                                    draft.country = data
                                    draft.state = []
                                    draft.city = []
                                  })
                                }
                              )
                            }

                            if (data?.countryID) {
                              fetchDataFromApis(
                                'State/ByCountryId',
                                `?id=${data?.countryID}&pageindex=0&PageSize=0`,
                                (data) => {
                                  setAllState((draft) => {
                                    draft.state = data
                                    draft.city = []
                                  })
                                }
                              )
                            }

                            if (data?.stateID) {
                              fetchDataFromApis(
                                'City/ByStateId',
                                `?id=${data?.stateID}&pageindex=0&PageSize=0`,
                                (data) => {
                                  setAllState((draft) => {
                                    draft.city = data
                                  })
                                }
                              )
                            }

                            setFieldValue(
                              'registeredCountryId',
                              data?.countryID
                            )
                            setFieldValue('countryId', data?.countryID)
                            setFieldValue('countryName', data?.countryName)
                            setFieldValue('stateId', data?.stateID)
                            setFieldValue('stateName', data?.stateName)
                            setFieldValue('cityId', data?.cityID)
                            setFieldValue('cityName', data?.cityName)
                          } else {
                            if (!allState?.country?.length) {
                              fetchDataFromApis(
                                'Country/search',
                                `?pageindex=0&PageSize=0`,
                                (data) => {
                                  setAllState((draft) => {
                                    draft.country = data
                                  })
                                }
                              )
                            }
                            setFieldValue('countryId', '')
                            setFieldValue('countryName', '')
                            setFieldValue('stateId', '')
                            setFieldValue('stateName', '')
                            setFieldValue('cityId', '')
                            setFieldValue('cityName', '')
                            setAllState((draft) => {
                              draft.state = []
                              draft.city = []
                            })
                          }
                        }
                      } catch (error) {
                        console.log(error)
                      }
                    }
                  }}
                />

                <InputComponent
                  id='landmark'
                  name='landmark'
                  value={values?.landmark}
                  labelText={'Landmark (Optional)'}
                  type={'text'}
                  MainHeadClass={'input_filed_50'}
                  placeholder={'Enter your Landmark'}
                  onChange={(e) => setFieldValue('landmark', e?.target?.value)}
                />

                <div className='input-wrapper-main input_filed_33'>
                  <label htmlFor='countryName'>
                    Country<span className='pv-label-red-required'>*</span>
                  </label>
                  <select
                    id='countryName'
                    name='countryName'
                    value={values?.countryId}
                    className='select-country'
                    onChange={(e) => {
                      if (e) {
                        const countryName =
                          e.target.options[
                            e.target.selectedIndex
                          ]?.getAttribute('data-label')
                        setFieldValue('countryId', e?.target?.value)
                        setFieldValue('countryName', countryName)
                        setFieldValue('stateId', null)
                        setFieldValue('stateName', null)
                        setFieldValue('cityId', null)
                        setFieldValue('cityName', null)
                        setAllState((draft) => {
                          draft.state = []
                          draft.city = []
                        })
                        fetchStateCity(e?.target?.value)
                      }
                    }}
                  >
                    <option value='country'>Choose a country</option>
                    {allState?.country?.length > 0 &&
                      allState?.country?.map(({ id, name }) => (
                        <>
                          <option key={id} value={id} data-label={name}>
                            {name}
                          </option>
                        </>
                      ))}
                  </select>
                  <ErrorMessage name='countryName' component={TextError} />
                </div>

                <div className='input-wrapper-main input_filed_33'>
                  <label htmlFor='stateName'>
                    State<span className='pv-label-red-required'>*</span>
                  </label>
                  <select
                    id='stateName'
                    name='stateName'
                    className='select-country'
                    value={values?.stateId}
                    onChange={async (e) => {
                      if (e) {
                        const stateName =
                          e.target.options[
                            e.target.selectedIndex
                          ]?.getAttribute('data-label')
                        setFieldValue('cityId', null)
                        setFieldValue('cityName', null)
                        setAllState((draft) => {
                          draft.city = []
                        })
                        setFieldValue('stateId', e?.target?.value)
                        setFieldValue('stateName', stateName)
                        fetchStateCity(false, e?.target?.value)
                      }
                    }}
                  >
                    <option value=''>Choose a state</option>
                    {allState?.state?.length > 0 &&
                      allState?.state?.map(({ id, name }) => (
                        <>
                          <option value={id} key={id} data-label={name}>
                            {name}
                          </option>
                        </>
                      ))}
                  </select>
                  <ErrorMessage name='stateName' component={TextError} />
                </div>
                <div className='input-wrapper-main input_filed_33'>
                  <label htmlFor='cityName'>
                    City/District/Town
                    <span className='pv-label-red-required'>*</span>
                  </label>
                  <select
                    id='cityName'
                    name='cityName'
                    value={values?.cityId}
                    className='select-country'
                    onChange={async (e) => {
                      if (e) {
                        const cityName =
                          e.target.options[
                            e.target.selectedIndex
                          ]?.getAttribute('data-label')
                        setFieldValue('cityId', e?.target?.value)
                        setFieldValue('cityName', cityName)
                      }
                    }}
                  >
                    <option value=''>Choose a city</option>
                    {allState?.city?.length > 0 &&
                      allState?.city?.map(({ id, name }) => (
                        <>
                          <option value={id} data-label={name} key={id}>
                            {name}
                          </option>
                        </>
                      ))}
                  </select>
                  <ErrorMessage name='cityName' component={TextError} />
                </div>

                <div className='input_col my_add_select'>
                  <div className='radio_box'>
                    <IpRadio
                      MainHeadClass={'main_rd_gender'}
                      labelClass={'ico_fl_mn'}
                      rdclass={'gendar-man'}
                      labelText={
                        <span>
                          <i className='m-icon home-icon'></i>Home
                        </span>
                      }
                      id={'home'}
                      name={'addressType'}
                      onChange={(e) => {
                        setFieldValue('addressType', 'home')
                      }}
                      checked={values?.addressType === 'home' ? true : false}
                    />

                    <IpRadio
                      MainHeadClass={'main_rd_gender'}
                      rdclass={'gendar-female'}
                      labelClass={'ico_fl_mn'}
                      labelText={
                        <span>
                          <i className='m-icon office-icon'></i>Office
                        </span>
                      }
                      onChange={(e) => {
                        setFieldValue('addressType', 'office')
                      }}
                      id={'office'}
                      name={'addressType'}
                      checked={values?.addressType === 'office' ? true : false}
                    />

                    <IpRadio
                      MainHeadClass={'main_rd_gender'}
                      rdclass={'gendar-female'}
                      labelClass={'ico_fl_mn'}
                      labelText={
                        <span>
                          <i className='m-icon office-icon'></i>Others
                        </span>
                      }
                      onChange={(e) => {
                        setFieldValue('addressType', 'others')
                      }}
                      id={'others'}
                      name={'addressType'}
                      checked={values?.addressType === 'others' ? true : false}
                    />
                  </div>
                  <ErrorMessage name='addressType' component={TextError} />
                </div>
              </div>

              <button
                type='submit'
                id='onSubmitAddress'
                onClick={() => {
                  validateForm()?.then((focusError) =>
                    focusInput(Object?.keys(focusError)?.[0])
                  )
                }}
                className='m-btn btn-primary'
              >
                Save Address
              </button>
            </Form>
          )}
        </Formik>
      </ModalComponent>
    </div>
  )
}

export default AddressModal
