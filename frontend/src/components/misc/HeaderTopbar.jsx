const HeaderTopbar = () => {
  return (
    <div className='h__topbar'>
      <div className='site-container-max'>
        <div className='h__topbar-content'>
          <div className='pv-topbar-leftpara'>
            <p> Welcome to Intra-Africa Corporation </p>
          </div>
          <div className='pv-viewcollection-main'>
            <p>New Summer collection is available with 20% off</p>{' '}
            <button className='m-btn btn-red'>View Collection</button>
          </div>
          <div className='pv-topbar-contact-main'>
            <ul className='pv-topbar-call-main'>
              <li>
                <a href='tel:+12 345 67890'>+12 345 67890</a>{' '}
              </li>
            </ul>
            <ul className='pv-topbar-mail-main'>
              <li>
                <a href='mailto:support@intraafrica.com'>
                  support@intraafrica.com
                </a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  )
}

export default HeaderTopbar
