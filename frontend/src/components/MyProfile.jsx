'use client'
import React, { useEffect, useState } from 'react'
import MyaccountMenu from './MyaccountMenu'
import Image from 'next/image'
import { useDispatch, useSelector } from 'react-redux'
import ChangePassword from './auth/ChangePassword'
import axiosProvider from '../lib/AxiosProvider'
import UpdateProfile from './UpdateProfile'
import Toaster from './base/Toaster'
import { _exception } from '../lib/exceptionMessage'
import { _productImg_, _userProfileImg_ } from '../lib/ImagePath'
import { reactImageUrl } from '../lib/GetBaseUrl'
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
import { addUser } from '@/redux/features/userSlice'
import ProfileSkeleton from './skeleton/ProfileSkeleton'
import { useRouter } from 'next/navigation'

const MyProfile = () => {
  const [modalOpen, setModalOpen] = useState({
    updateProfile: false,
    changePassword: false
  })
  const router = useRouter()
  const [loading, setLoading] = useState(true)
  const [currentURL, setCurrentURL] = useState(false)
  const [data, setData] = useState()
  const dispatch = useDispatch()
  const { user, userToken, refreshToken, deviceId } = useSelector(
    (state) => state?.user
  )
  const [toast, setToast] = useState({
    show: false,
    text: null,
    variation: null
  })
  const defaultPicture =
    'https://img.freepik.com/premium-vector/man-avatar-profile-round-icon_24640-14044.jpg?w=2000'

  const closeModal = () => {
    setModalOpen({ updateProfile: false, changePassword: false })
  }

  const fetchUser = async () => {
    try {
      setLoading(true)
      const response = await axiosProvider({
        method: 'GET',
        endpoint: 'Account/Customer/ById',
        queryString: `?Id=${user?.userId}`
      })
      setLoading(false)
      if (response?.status === 200) {
        setData(response?.data?.data)
        let userObj = {
          ...response?.data?.data,
          userId: response?.data?.data?.id,
          fullName:
            response?.data?.data?.firstName +
            ' ' +
            response?.data?.data?.lastName
        }
        dispatch(
          addUser({
            user: userObj,
            userToken: userToken,
            refreshToken: refreshToken,
            deviceId: deviceId
          })
        )
      }
    } catch {
      setLoading(false)
      showToast(toast, setToast, {
        data: { code: 204, message: _exception?.message }
      })
    }
  }

  useEffect(() => {
    fetchUser()
    setCurrentURL(window?.location?.href)
  }, [])

  useEffect(() => {
    if (!user?.userId) {
      router.push('/')
    }
  }, [user])
  return (
    <>
      <Head>
        <link rel='canonical' href={currentURL} />
        <title>My Profile</title>
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

      {modalOpen?.changePassword && (
        <ChangePassword
          onClose={closeModal}
          setToast={setToast}
          toast={toast}
        />
      )}

      {modalOpen?.updateProfile && (
        <UpdateProfile
          data={data}
          onClose={closeModal}
          fetchUser={fetchUser}
          setToast={setToast}
          toast={toast}
          setLoading={setLoading}
        />
      )}

      {toast?.show && (
        <Toaster text={toast?.text} variation={toast?.variation} />
      )}
      <div className='wish_main_flex'>
        <div className='wish_inner_20'>
          <MyaccountMenu activeTab={'profile'} />
        </div>

        {loading ? (
          <ProfileSkeleton />
        ) : (
          <div className='profile-main-info'>
            <div className='wish_inner_80 profile-main'>
              <div className='p-head-image-main'>
                <div className='profile-head'>
                  <h2 className='profile-label'>Profile Details</h2>
                </div>
                <div className='profile-image'>
                  <div className='profile-pic'>
                    <label className='-label' htmlFor='file'>
                      <span className='glyphicon glyphicon-camera'></span>
                      <span>Change Image</span>
                    </label>
                    <input id='file' type='file' />
                    {data?.profileImage && data?.profileImage !== 'null' ? (
                      <Image
                        src={`${reactImageUrl}${_userProfileImg_}${data?.profileImage}`}
                        alt='Selected Profile Picture'
                        id='output'
                        width={100}
                        height={100}
                      />
                    ) : (
                      data && (
                        <Image
                          src={defaultPicture}
                          alt='Selected Profile Picture'
                          id='output'
                          width={100}
                          height={100}
                        />
                      )
                    )}
                    {/* <label htmlFor='file' className='user-lable-pro'>
                    <div className='user-profile-icon'>
                      <i className='m-icon user-icon-p'></i>
                    </div>
                  </label> */}
                  </div>
                </div>
              </div>
              <div className='profile-data'>
                {data && (
                  <table>
                    <tbody>
                      <tr>
                        <td>Full Name:</td>
                        <td className='pv-profile-data'>
                          {data?.firstName} {data?.lastName}
                        </td>
                      </tr>

                      <tr>
                        <td>Mobile Number:</td>
                        <td className='pv-profile-data'> {data?.mobileNo}</td>
                      </tr>

                      <tr>
                        <td>Email ID:</td>
                        <td className='pv-profile-data'>
                          {' '}
                          {data?.userName?.toLowerCase()}
                        </td>
                      </tr>

                      <tr>
                        <td>Gender:</td>
                        <td className='pv-profile-data'>
                          {data?.gender ?? `-`}
                        </td>
                      </tr>

                      {/* <tr>
                      <td>Date of Birth</td>
                      <td>23 MAY 2019</td>
                    </tr> */}
                    </tbody>
                  </table>
                )}
              </div>
            </div>
            <div className='profile-edit'>
              <button
                className='m-btn btn-primary'
                onClick={() => {
                  setModalOpen({ ...modalOpen, updateProfile: true })
                }}
              >
                Update Profile
              </button>
              <button
                onClick={() => {
                  setModalOpen({ ...modalOpen, changePassword: true })
                }}
                className='m-btn btn-edit-myprofile'
              >
                change password
              </button>
            </div>
          </div>
        )}
      </div>
    </>
  )
}

export default MyProfile
