import { NextResponse } from 'next/server'

export async function middleware(request) {
  const refreshToken = request.cookies.get('refreshToken')?.value
  const userId = request.cookies.get('userId')?.value

  if (!!refreshToken && !!userId) {
    return NextResponse.next()
  } else {
    if (
      request?.nextUrl?.pathname === '/' &&
      request?.nextUrl?.pathname?.length === 1
    ) {
      return NextResponse.next()
    } else {
      return NextResponse.redirect(new URL('/', request?.url))
    }
  }
}

export const config = {
  matcher: [
    '/orderlist/:path*',
    '/myprofile/:path*',
    '/wishlist/:path*',
    '/myaddress/:path*'
    // '/checkoutorder/:path*'
    // '/user/:path*'
  ]
}
