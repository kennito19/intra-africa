const { nextui } = require('@nextui-org/react')

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './src/pages/**/*.{js,ts,jsx,tsx}',
    './src/components/**/*.{js,ts,jsx,tsx}',
    './src/app/**/*.{js,ts,jsx,tsx}',
    './node_modules/@nextui-org/theme/dist/**/*.{js,ts,jsx,tsx}'
  ],
  theme: {
    extend: {
      container: {
        padding: {
          DEFAULT: '1.5rem'
        }
      },
      screens: {
        Esm: '480px',
        sm: '768px',
        md: '1024px',
        lg: '1280px',
        xl: '1536px'
      },
      colors: {
        transparent: 'transparent',
        current: 'currentColor',
        slategrey: '#707070',
        // primary: '#05215B',
        primary: '#0D4458',
        sectionTitel: '#393D46',
        'primary-hover': '#0a3646',
        'pink-light': '#FEF2F2',
        // secondary: '#1F2937',
        secondary: '#404040',
        themered: '#D0564E',
        themegrey: '#7F7F7F',
        'secondary-hover': '#1F2937',
        bglight: '#EDF3F499',
        headcolor: '#383838',
        yellow: '#FFE869',
        yellowHover: '#F6D415',
        TextTitle: '#2D2D2D',
        grayishBlueWhite: '#F6F7FB',
        classicSilver: '#F6F7FA',
        lightGraytext: '#878787',
        lightGray: '#DBDBDB',
        lighttext: '#535766',
        Gray: '#888888',
        JungleGreen: '#198754',
        darkgreen: '#0F5132',
        bluishgreen: '#D1E7DD',
        Scarlet: '#F33232',
        Darkgrey: '#BDBDBD',
        WhiteSmoke: '#F9F9F9',
        BlueGray: '#EDEFF5',
        SteelBlue: '#4067BC',
        LightSilver: '#F7F8FB',
        CharmPink: '#F8D7DA',
        ReddishBrown: '#842029',
        BluishGrey: '#B2B2B2',
        DodgerBlue: '#333EFF',
        Teal: '#03A685',
        link: '#0000EE'
      },
      fontFamily: {
        poppins: 'Poppins',
        nunitoSans: "'Nunito Sans'"
      },
      fontSize: {
        40: [
          '2.5rem',
          {
            lineHeight: '3rem'
          }
        ],
        48: [
          '3rem',
          {
            lineHeight: '3.75rem'
          }
        ],
        32: [
          '2rem',
          {
            lineHeight: '2.5rem'
          }
        ],
        28: [
          '1.75rem',
          {
            lineHeight: '2.25rem'
          }
        ],
        24: [
          '1.5rem',
          {
            lineHeight: '2rem'
          }
        ],
        22: [
          '1.375rem',
          {
            lineHeight: '1.8756rem'
          }
        ],
        18: [
          '1.125rem',
          {
            lineHeight: '1.75rem'
          }
        ],
        16: [
          '1rem',
          {
            lineHeight: '1rem'
          }
        ],
        14: [
          '0.875rem',
          {
            lineHeight: '1.1394rem'
          }
        ],
        12: [
          '0.75rem',
          {
            lineHeight: '1.1394rem'
          }
        ]
      },
      boxShadow: {
        searchboxshadow: '0 1px 2px 0 rgba(148,150,159,.3)',
        prdboxshadow: '0px 0px 8px 2px #cfcfcfcc',
        prdbuttonshadow: '0px 0px 7px 0px #D5D4D4',
        light: '0 1px 10px rgba(0,0,0,.08)',
        medium: '0 4px 12px 0 rgba(0,0,0,.05)',
        card: '0 2px 16px 4px rgba(40,44,63,.07)',
        productcard: '1px 1px 5px #ccc',
        Popover: '0 4px 16px 0 rgba(0,0,0,.2)'
      }
    }
  },
  plugins: [nextui()]
}
