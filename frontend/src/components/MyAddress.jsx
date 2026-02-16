'use client'
import React, { useEffect, useState } from 'react'
import MyaccountMenu from './MyaccountMenu'
import IpRadio from './base/IpRadio'
import { useDispatch, useSelector } from 'react-redux'
import {
  _alphabetRegex_,
  _integerRegex_,
  _phoneNumberRegex_
} from '../lib/Regex'
import axiosProvider from '../lib/AxiosProvider'
import Toaster from './base/Toaster'
import { showToast } from '../lib/GetBaseUrl'
import EmptyComponent from './EmptyComponent'
import { addressData } from '@/redux/features/addressSlice'
import AddressModal from './AddressModal'
import Swal from 'sweetalert2'
import { _SwalDelete, _exception } from '../lib/exceptionMessage'
import Head from 'next/head'
import {
  _headDescription_,
  _headKeywords_,
  _headTitle_,
  _ogDescription_,
  _ogLogo_,
  _ogUrl_,
  _projectName_
} from '../lib/ConfigVariables'

const MyAddress = ({ setLoading }) => {
  const dispatch = useDispatch()
  const currentURL = window.location.href
  const { user } = useSelector((state) => state?.user)
  const [data, setData] = useState()
  const [modalShow, setModalShow] = useState({ show: false, data: null })
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })

  const fetchDefaultAddress = async (id) => {
    const data = {
      id: id,
      userId: user?.userId,
      setDefault: true
    }
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'PUT',
        endpoint: 'Address/setDefault',
        data
      })

      setLoading(false)
      if (response?.status === 200) {
        fetchData()
        showToast(toast, setToast, response)
      } else {
        setLoading(false)
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const handleDelete = async (id) => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'DELETE',
        endpoint: 'Address',
        queryString: `?id=${id}`
      })

      setLoading(false)
      if (response?.data?.code === 200) {
        fetchData()
      }
      showToast(toast, setToast, response)
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  const fetchData = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'Address/byUserId',
        queryString: `?userId=${user?.userId}`
      })
      setLoading(false)
      if (response?.status === 200) {
        setData(response)
        dispatch(addressData(response?.data?.data))
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  useEffect(() => {
    fetchData()
  }, [])

  return (
    <>
      <Head>
        <link rel='canonical' href={currentURL} />
        <title>My Address</title>
        <meta name='description' content={_headDescription_} />
        <meta name='keywords' content={_headKeywords_} />
        <meta property='og:locale' content='en_US' />
        <meta
          property='og:title'
          content={`${_projectName_} - ${_headTitle_}`}
        />
        <meta property='og:description' content={_ogDescription_} />
        <meta property='og:url' content={_ogUrl_} />
        <meta property='og:image' content={_ogLogo_} />
      </Head>

      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}

      <div className='wish_main_flex'>
        <div className='wish_inner_20'>
          <MyaccountMenu activeTab={'address'} />
        </div>
        <div className='wish_inner_80'>
          {data && data?.data?.data?.length > 0 ? (
            <>
              <div className='address_main_fl'>
                <div className='address_main_fl_first'>
                  <h1 className='order-menu-title'>My Addresses</h1>
                </div>
                <div className='address_main_fl_second'>
                  <button
                    className='m-btn address_new_add'
                    onClick={() => {
                      setModalShow({ show: !modalShow.show, data: null })
                    }}
                  >
                    <i className='m-icon m-new-address-icon'></i>Add new address
                  </button>
                </div>
              </div>

              <div className='add_fl-main'>
                {data?.data?.data?.map((item) => (
                  <div
                    className='main-mb_3'
                    key={Math.floor(Math.random() * 100000)}
                  >
                    <div className='card_default-address'>
                      <div className='def_fl'>
                        <p className='default_name'>{item?.fullName}</p>

                        <IpRadio
                          id={item?.id}
                          labelText={'Set Default'}
                          MainHeadClass={'mb_none'}
                          name={'setDefalt'}
                          onChange={() => fetchDefaultAddress(item?.id)}
                          checked={item?.setDefault}
                        />
                      </div>
                      <p className='default_add-add'>
                        {item?.addressLine1}, {item?.addressLine2},
                        {item?.cityName} - {item?.stateName} {item?.pincode}
                      </p>
                      <p className='default_add-add'>
                        Mobile: {item?.mobileNo}
                      </p>
                    </div>
                    <div className='addressAccordian-buttons'>
                      <button
                        className='addressAccordian-button'
                        onClick={() => {
                          setModalShow({ show: !modalShow.show, data: item })
                        }}
                      >
                        Edit
                      </button>
                      <div className='addressAccordian-buttonDivider'></div>
                      <button
                        className='addressAccordian-button removeaddress_btn'
                        onClick={() => {
                          Swal.fire({
                            title: '',
                            text: 'Are you sure you want to delete this address?',
                            icon: _SwalDelete?.icon,
                            showCancelButton: _SwalDelete?.showCancelButton,
                            confirmButtonColor: _SwalDelete?.confirmButtonColor,
                            cancelButtonColor: _SwalDelete?.cancelButtonColor,
                            confirmButtonText: 'Yes, Delete',
                            cancelButtonText: 'Cancel',
                            customClass: {
                              title: 'sweet-alert-text'
                            }
                          }).then((result) => {
                            if (result?.isConfirmed) {
                              handleDelete(item?.id, item?.userID)
                            }
                          })
                        }}
                      >
                        Remove
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            </>
          ) : (
            data && (
              <EmptyComponent
                title={'No Addresses found in your account!'}
                description={'Add a delivery address.'}
                src={'/images/myaddresses-empty.webp'}
                alt={'empty_Add'}
                isButton
                btnText={'Add Address'}
                onClick={() =>
                  setModalShow({ show: !modalShow?.show, data: null })
                }
                redirectTo={'#.'}
              />
            )
          )}
        </div>
      </div>
      {modalShow?.show && (
        <AddressModal
          modalShow={modalShow}
          setModalShow={setModalShow}
          fetchAllAddress={fetchData}
          setLoading={setLoading}
        />
      )}
    </>
  )
}

export default MyAddress
