'use client'
import { generateSessionId } from "@/lib/AllGlobalFunction"
import { fetchNoAuthToken } from "@/utils/authUtils"
import { useRouter } from "next/navigation"
import { parseCookies, setCookie } from 'nookies'
import { useEffect } from "react"

export default function ClientProvider({ children }) {

    const router = useRouter();
    const cookies = parseCookies();

    useEffect(() => {
        
        const checkAndSetUserToken = async () => {
            try {
                const userToken = cookies.userToken

                if (!userToken) {

                    const result = await fetchNoAuthToken()
                    const sessionId = generateSessionId()

                    if (result && result?.userToken) {
                        setCookie(null, 'userToken', result.userToken, {
                            maxAge: 1 * 24 * 60 * 60,
                            path: '/'
                        })
                        setCookie(null, 'deviceId', result.deviceId, {
                            maxAge: 1 * 24 * 60 * 60,
                            path: '/'
                        })
                        setCookie(null, 'sessionId', sessionId, {
                            maxAge: 1 * 24 * 60 * 60,
                            path: '/'
                        })
                        
                    } else {
                        console.error('Failed to generate userToken')
                    }
                }
                
            } catch (error) {
                console.error(
                    'An error occurred while checking/setting userToken:',
                    error
                )
            }
        }

        const initializePage = async () => {
            await checkAndSetUserToken()
        }

        initializePage();

    }, [router])    

    return (
        <>
            <main>
                {children}
            </main>
        </>
    )
}
