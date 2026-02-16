'use client'

import { useEffect, useState } from "react"
import OrderDetails from "../OrderDetails"
import OrderListDetail from "../OrderListDetail"
import { useSearchParams } from "next/navigation"

const OrdersContainer = ({ orders }) => {
    const orderId = useSearchParams()?.get("orderId")
    const [activeComponent, setActiveComponent] = useState("OrderListDetails")
    useEffect(() => {
        setActiveComponent(orderId ? "OrderDetails" : "OrderListDetails")
    }, [orderId])

    return (
        <>
            {activeComponent === "OrderListDetails" &&
                <OrderListDetail
                    orders={orders}
                    activeComponent={activeComponent}
                    setActiveComponent={setActiveComponent}
                />}
            {activeComponent === "OrderDetails" &&
                <OrderDetails
                    orders={orders}
                    activeComponent={activeComponent}
                    setActiveComponent={setActiveComponent}
                />}
        </>
    )
}

export default OrdersContainer