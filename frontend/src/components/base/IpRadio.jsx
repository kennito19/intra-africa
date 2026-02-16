import React from 'react'

const IpRadio = ({
  MainHeadClass,
  name,
  id,
  value,
  rdclass,
  labelText,
  checked,
  labelClass,
  onChange,
  ...rest
}) => {
  return (
    <div className={`input-wrapper-radio ${MainHeadClass || ''}`}>
      <input
        type='radio'
        name={name}
        value={value}
        className={`RadioClass ${rdclass || ''}`}
        id={id}
        checked={checked}
        onChange={() => onChange(value)}
        {...rest}
      />
      <label htmlFor={id} className={`form-c-radio ${labelClass || ''}`}>
        {labelText}
      </label>
    </div>
  )
}

export default IpRadio
