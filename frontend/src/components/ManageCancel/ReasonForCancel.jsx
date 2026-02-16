import React from 'react'
import axiosProvider from '../../lib/AxiosProvider'
import { _exception } from '../../lib/exceptionMessage'
import { showToast } from '../../lib/GetBaseUrl'
import IpTextarea from '../base/IpTextarea'
import { ErrorMessage } from 'formik'
import TextError from '../base/TextError'
import * as Yup from 'yup'

const ReasonForReturn = ({
  values,
  setFieldValue,
  allState,
  setAllState,
  setActiveAccordion
}) => {
  const validateForm = async (values) => {
    try {
      const validationSchema = Yup.object().shape({
        comment: Yup.string()
          .trim()
          .required('This field is required.')
          .test(
            'is-not-empty',
            'This field is required.',
            (value) => value.trim() !== ''
          ),
        issue:
          allState?.issueTypes?.length &&
          Yup.string().min(1, 'Select issue').required('Select issue'),
        reason:
          allState?.reasonList?.length &&
          Yup.string().min(1, 'Select reason').required('Select reason')
      })

      await validationSchema.validate(values, { abortEarly: false })
      return {}
    } catch (error) {
      const errors = {}
      error.inner.forEach((err) => {
        errors[err.path] = err.message
      })
      return errors
    }
  }

  return (
    <div className='pv-or-rfr'>
      <div className='pv-or-items'>
        <div className='pv-order-status'>Action</div>
        <div>
          <select
            name='actionID'
            id='actionID'
            className='pv-or-select'
            value={values?.actionID}
            onChange={() => {
              setFieldValue("actionId",values?.actionID)
            }}
          >
            <option value='' selected disabled>
              select action
            </option>
            {allState?.returnAction?.length > 0 &&
              allState?.returnAction?.map((item) => (
                <option
                  value={item?.id}
                  key={item?.id}
                  data-label={item?.returnAction}
                >
                  {item?.returnAction}
                </option>
              ))}
          </select>
          <ErrorMessage name='actionID' component={TextError} />
        </div>
      </div>
      {values?.actionID && allState?.issueTypes?.length > 0 && (
        <div className='pv-or-items'>
          <div className='pv-order-status'>
            Reason for Cancellation
            <span className='pv-label-red-required'>*</span>
          </div>
          <div>
            <select
              name='issue'
              id=''
              value={values?.issueId}
              className='pv-or-select'
              onChange={async (e) => {
                try {
                  setFieldValue('issueId', Number(e?.target?.value))
                  setFieldValue('validation', {
                    ...values?.validation,
                    issue: ''
                  })
                  const issueName =
                    e.target.options[e.target.selectedIndex].getAttribute(
                      'data-label'
                    )
                  setFieldValue('issue', issueName)
                  setFieldValue('reason', '')
                  setFieldValue('reasonId', '')
                  const reasonList = await axiosProvider({
                    method: 'GET',
                    endpoint: `IssueReason/ByIssueTypeId?issueTypeId=${e?.target?.value}`
                  })
                  if (reasonList?.status === 200) {
                    setAllState((draft) => {
                      draft.reasonList = reasonList?.data?.data
                    })
                  }
                } catch (error) {}
              }}
            >
              <option value='' selected disabled>
                select reason
              </option>
              {allState?.issueTypes?.map((item) => (
                <option
                  value={item?.id}
                  key={item?.id}
                  data-label={item?.issue}
                >
                  {item?.issue}
                </option>
              ))}
            </select>
            {values?.validation?.issue && (
              <div className={'input-error-msg validation-error-message'}>
                {values?.validation?.issue}
              </div>
            )}
          </div>
        </div>
      )}
      {values?.issue && allState?.reasonList?.length > 0 && (
        <div className='pv-or-items'>
          <div className='pv-order-status'>
            Cancellation Details<span className='pv-label-red-required'>*</span>
          </div>
          <div>
            <select
              name='reason'
              id=''
              value={values?.reasonId}
              className='pv-or-select'
              onChange={(e) => {
                const reasondetails =
                  e.target.options[e.target.selectedIndex].getAttribute(
                    'data-label'
                  )
                setFieldValue('reasonId', Number(e?.target?.value))
                setFieldValue('reason', reasondetails)
                setFieldValue('validation', {
                  ...values?.validation,
                  reason: ''
                })
              }}
            >
              <option value='' selected disabled>
                select reason details
              </option>
              {allState?.reasonList?.map((item) => (
                <option
                  value={item?.id}
                  key={item?.id}
                  data-label={item?.reasons}
                >
                  {item?.reasons}
                </option>
              ))}
            </select>
            {values?.validation?.reason && (
              <div className={'input-error-msg validation-error-message'}>
                {values?.validation?.reason}
              </div>
            )}
          </div>
        </div>
      )}

      {values?.actionID && (
        <div className='pv-or-items'>
          <div className='pv-order-status'>
            Comments<span className='pv-label-red-required'>*</span>
          </div>
          <div>
            <IpTextarea
              id={'comment'}
              value={values?.comment}
              onChange={(e) => {
                setFieldValue('validation', {
                  ...values?.validation,
                  comment: ''
                })
                setFieldValue('comment', e?.target?.value)
              }}
            />
            {values?.validation?.comment && (
              <div className={'input-error-msg validation-error-message'}>
                {values?.validation?.comment}
              </div>
            )}
          </div>
        </div>
      )}
      {values?.actionID && (
        <div>
          <button
            className='m-btn checkout_btn'
            onClick={async () => {
              try {
                const errors = await validateForm(values)
                if (Object.keys(errors).length === 0) {
                  setFieldValue('validation', '')
                  setActiveAccordion(1)
                } else {
                  setFieldValue('validation', errors)
                }
              } catch (validationError) {
                console.error('Validation error:', validationError.errors)
              }
            }}
            id='reasonCancel'
            type='button'
          >
            continue
          </button>
        </div>
      )}
    </div>
  )
}

export default ReasonForReturn
