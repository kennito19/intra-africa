import React, { useState } from 'react'

function IpCheckBox({
  id,
  label,
  colorClass,
  value,
  onChange,
  showColor,
  checked,
  changeListener,
  isDisabled
}) {
  // const [checked, setChecked] = useState(false)
  // const emitVal = (e) => {
  //   const val = {
  //     checked: e.target.checked,
  //     value: e.target.value
  //   }
  //   setChecked(!checked)
  //   // changeListener(val)
  // }

  return (
    <div className='ip-checkbox__wrapper'>
      <input
        type='checkbox'
        className='ip-checknox__input'
        id={id}
        onChange={onChange}
        value={value}
        checked={checked}
        disabled={isDisabled}
      />
      <label htmlFor={id} className='ip-chekbox__lable'>
        {showColor ? (
          <span
            className='ip_checkbox-color'
            style={{ backgroundColor: colorClass }}
          ></span>
        ) : (
          <></>
        )}
        {label}
      </label>
    </div>
  )
}

export default IpCheckBox
