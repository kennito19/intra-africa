import Link from 'next/link'

function DynamicPositionComponent({
  heading,
  paragraph,
  headingPosition,
  buttonPosition,
  btnText,
  redirectTo,
  buttonPositionDirection,
  children,
  TitleColor,
  TextColor,
  ...props
}) {
  const calculateButtonPosition = () => {
    if (buttonPositionDirection === 'title') {
      if (headingPosition === 'left') {
        return 'right'
      } else if (headingPosition === 'right') {
        return 'left'
      } else {
        return 'center'
      }
    } else {
      return buttonPosition
    }
  }

  const renderOptionheading = () => {
    switch (headingPosition) {
      case 'left':
        return 'justify-content-between'
      case 'center':
        return 'flex-column'
      case 'right':
        return 'flex-r-reverse justify-content-between'
      default:
        return null // Render nothing for unknown options
    }
  }

  const renderOptionbutton = () => {
    // Calculate the button position based on the logic above
    const calculatedButtonPosition = calculateButtonPosition()

    switch (calculatedButtonPosition) {
      case 'left':
        return 'me-au'
      case 'center':
        return 'm-au'
      case 'right':
        return 'ms-au'
      default:
        return null // Render nothing for unknown options
    }
  }

  return (
    <div className={`site-container ${headingPosition}`}>
      {(heading || paragraph || buttonPositionDirection === 'title') && (
        <div
          className={`heading-main ${
            buttonPositionDirection === 'section' ? 'gp-0' : ''
          } ${renderOptionheading()}`}
        >
          <div>
            <h2
              style={{
                textAlign: headingPosition,
                color: TitleColor
              }}
              className='flex-column titleHeadingH1'
            >
              {heading}
            </h2>
            <p
              className='subtitleHeadingP'
              style={{
                textAlign: headingPosition,
                color: TextColor
              }}
            >
              {paragraph}
            </p>
          </div>
          <div className={`${renderOptionbutton()} button_pos`}>
            {buttonPositionDirection === 'title' && (
              <Link
                href={redirectTo}
                style={{
                  borderColor: TextColor,
                  color: TextColor
                }}
                target='_blank'
                className='m-btn heading_button'
                {...props}
              >
                {btnText}
              </Link>
            )}
          </div>
        </div>
      )}
      {children}
      {buttonPositionDirection === 'section' && (
        <div className={`fl btn-ch jc-${buttonPosition}`}>
          <Link
            href={redirectTo}
            style={{
              borderColor: TextColor,
              color: TextColor
            }}
            target='_blank'
            className='m-btn heading_button'
            {...props}
          >
            {btnText}
          </Link>
        </div>
      )}
    </div>
  )
}

export default DynamicPositionComponent
