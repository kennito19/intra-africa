import React from 'react'

const TextareaComponent = ({ MainHeadClass, TextValue, TextClass, placeholder,  labelClass, labelText, id,  }) => {
  return (
    <div className={`input-wrapper-main ${MainHeadClass || ''}`}>
      <label htmlFor={id} className={`form-c-label ${labelClass || ''}`}>{labelText}</label>
      <textarea value={TextValue} className={`form-c-input ${TextClass || ''}`} placeholder={placeholder} id={id} />
    </div>
  )
}

export default TextareaComponent