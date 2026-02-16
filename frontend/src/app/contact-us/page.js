'use client'
import { Form, Formik } from 'formik'
import InputComponent from '../../components/base/InputComponent'
import MBtn from '../../components/base/MBtn'
import TextareaComponent from '../../components/base/TextareaComponent'

function page() {

  return (
    <div className='site-container section_spacing_b'>
      <div className='pv-contactus-main'>
        <span className='pv-contactus-line'></span>

        <div className='pv-contactus-inner-inputs'>
          <h2>Contact Us</h2>
          <p className='pv-contact-desc'>
            Feel Free to contact us any time. We will get back to you as soon as
            we can!.
          </p>
          <Formik
            enableReinitialize={true}
            // initialValues={initialValues}
            // validationSchema={validationSchema}
            // onSubmit={onSubmit}
          >
            <Form>
              <div className='input_form'>
                <InputComponent
                  id='firstName'
                  name='first name'
                  labelText={'First Name'}
                  // value={values?.fullName}
                  type={'text'}
                  maxLength={'40'}
                  MainHeadClass={'input_filed_50'}
                  required
                  placeholder={'Enter your name'}
                />
                <InputComponent
                  id='LastName'
                  name='Last name'
                  labelText={'Last Name'}
                  // value={values?.fullName}
                  type={'text'}
                  maxLength={'40'}
                  MainHeadClass={'input_filed_50'}
                  required
                  placeholder={'Enter your name'}
                />
                <InputComponent
                  labelText={'Email Address'}
                  required
                  id={'userName'}
                  type={'text'}
                  labelClass={'sign-com-label'}
                  //   value={values?.userName}
                  name='userName'
                  MainHeadClass={'input_filed_100'}
                />

                <TextareaComponent
                  id={'comment'}
                  labelText='Textarea'
                  MainHeadClass={'input_filed_100'}

                  //   value={values?.comment}
                />
              </div>

              <MBtn
                buttonClass={'m-btn btn-primary'}
                btnText={'Submit'}
                btnPosition='right'
              />
            </Form>
          </Formik>
        </div>
        <div className='pv-contactus-innercol'>
          <div className='pv-contact_info_sec'>
            <h2>Contact Info</h2>
            <div className='pv-continfo-col'>
              <i className='m-icon pv-headset'></i>
              <span>+91 8009 054294</span>
            </div>
            <div className='pv-continfo-col'>
              <i className='m-icon pv-emailicon'></i>
              <span>info@flightmantra.com</span>
            </div>
            <div className='pv-continfo-col'>
              <i className='m-icon pv-address'></i>
              <span>
                1000+ Travel partners and 65+ Service city across India, USA,
                Canada & UAE
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default page
