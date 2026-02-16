import axiosProvider from './AxiosProvider'
import { showToast } from './GetBaseUrl'
import { _exception } from './exceptionMessage'
import { _toaster } from './tosterMessage'

const convertToNumber = (amount) => {
  if (typeof amount === 'string') {
    return parseFloat(amount.replace(/,/g, ''))
  } else if (typeof amount === 'number') {
    return amount
  } else {
    return 0
  }
}

export const generateSessionId = () => {
  const randomChars =
    'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
  const sessionIdLength = 30
  let sessionId = ''

  for (let i = 0; i < sessionIdLength; i++) {
    sessionId += randomChars.charAt(
      Math.floor(Math.random() * randomChars.length)
    )
  }

  return sessionId
}

export const prepareExtendedWarranty = (item, user) => {
  return item?.item_warranty_charges
    ? [
        {
          id: 0,
          warrantyId: item?.item_warranty_charges?.Id ?? 0,
          productId: item?.productID ?? 0,
          userId: user?.userId ?? '',
          yearId: item?.item_warranty_charges?.YearId ?? 0,
          year: item?.item_warranty_charges?.Year?.toString() ?? '',
          priceFrom: item?.item_warranty_charges?.PriceFrom ?? 0,
          priceTo: item?.item_warranty_charges?.PriceTo ?? 0,
          title: item?.item_warranty_charges?.Title ?? '',
          sortDescription: item?.item_warranty_charges?.SortDescription ?? '',
          description: item?.item_warranty_charges?.Description ?? '',
          chargesIn: item?.item_warranty_charges?.ChargesIn ?? '',
          percentageValue: item?.item_warranty_charges?.PercentageValue ?? 0,
          amountValue: item?.item_warranty_charges?.AmountValue ?? 0,
          isMandatory: item?.item_warranty_charges?.IsMandatory ?? false,
          actualPrice: item?.item_warranty_charges?.Price ?? 0,
          totalActualPrice: item?.item_warranty_charges?.Total ?? 0,
          qty: item?.item_warranty_charges?.Qty ?? 0
        }
      ]
    : []
}

export const prepareExtraCharges = (item) => {
  return Object.keys(item?.taxinfo?.otherCharges)?.map((key) => {
    const extra = item?.taxinfo?.otherCharges[key]
    return {
      chargesType: extra?.charges_type ?? '',
      chargesPaidBy: extra?.charges_paid_by ?? '',
      chargesIn: extra?.charges_in ?? '',
      chargesValueInPercentage: extra?.charges_percentage_value ?? 0,
      chargesValueInAmount: extra?.charges_amount_value ?? 0,
      chargesMaxAmount: extra?.charges_max_value ?? 0,
      taxOnChargesAmount: extra?.tax_on_actual_charges ?? 0,
      chargesAmountWithoutTax: extra?.tax_ex_actual_charges ?? 0,
      totalCharges: extra?.total_charges ?? 0
    }
  })
}

export const prepareTaxInfos = (item) => {
  return [
    {
      commissionAmount: Number(item?.taxinfo?.commissionAmount) ?? 0,
      productID: parseInt(item?.taxinfo?.productID) ?? 0,
      sellerProductID: parseInt(item?.taxinfo?.sellerProductID) ?? 0,
      netEarn: Number(item?.taxinfo?.netEarn) ?? 0,
      orderTaxRateId: parseInt(item?.taxinfo?.orderTaxRateId) ?? 0,
      orderTaxRate: item?.taxinfo?.orderTaxRate ?? '',
      hsnCode: item?.taxinfo?.hsnCode ?? '',
      weightSlab: `${item?.taxinfo?.weightSalb}`,
      shippingCharge: Number(item?.taxinfo?.actualShippingCharges) ?? 0,
      commissionIn: item?.taxinfo?.commissionIn ?? '',
      commissionRate: Number(item?.taxinfo?.commissionRate) ?? 0,
      isSellerWithGSTAtOrderTime: false,
      shippingZone: item?.ItemShippingZone ?? '',
      taxOnShipping: item?.taxinfo?.actualtaxOnShippingCharges ?? 0,
      shipmentBy: item?.taxinfo?.shipmentBy ?? '',
      shipmentPaidBy: item?.taxinfo?.shippingPaidBy ?? ''
    }
  ]
}

export const prepareOrderItems = (values, cartId, user, allVal) => {
  let data = values?.Items?.map((item) => {
    return {
      guid: '',
      subOrderNo: '',
      productGUID: item?.ProductGuid ?? '',
      sellerID: item?.sellerId ?? '',
      brandID: parseInt(item?.brandid) ?? 0,
      categoryId: parseInt(item?.categoryId) ?? 0,
      productID: parseInt(item?.taxinfo?.productID) ?? '',
      sellerProductID: parseInt(item?.taxinfo?.sellerProductID) ?? '',
      productName: item?.productName ?? '',
      productSKUCode: item?.productSKU ?? '',
      mrp: Number(item?.itemPrice?.mrp) ?? '',
      sellingPrice: Number(item?.itemPrice?.selling_price) ?? '',
      discount: item?.itemPrice?.discount ?? '',
      qty: Number(item?.qty) ?? '',
      totalAmount: convertToNumber(item?.ItemTotal) ?? '',
      priceTypeID: 0,
      priceType: '',
      sizeID: parseInt(item?.sizeId) ?? '',
      sizeValue: item?.size ?? '',
      isCouponApplied: !cartId && item?.coupon_status === 'success',
      coupon: cartId
        ? ''
        : item?.coupon_status === 'success'
        ? allVal?.couponCode
        : '',
      coupontDiscount: item?.coupon_amount ?? 0,
      coupontDetails: cartId
        ? ''
        : item?.coupon_details
        ? item?.coupon_details
        : '',
      subTotal: convertToNumber(item?.ItemSubTotal) ?? 0,
      status: 'Initiate',
      wherehouseId: parseInt(allVal?.addressVal?.id) ?? '',
      isReplace: false,
      parentID: 0,
      returnPolicyName: '',
      returnPolicyTitle: '',
      returnPolicyCovers: '',
      returnPolicyDescription: '',
      returnValidDays: 0,
      returnValidTillDate: null,
      brandName: item?.brandName ?? '',
      productImage: item?.Image ?? '',
      sellerName: item?.sellerName ?? '',
      sellerPhoneNo: '',
      sellerEmailId: '',
      sellerStatus: '',
      sellerKycStatus: '',
      packageId: 0,
      packageNo: '',
      packageItemIds: '',
      totalPakedItems: 0,
      noOfPackage: 0,
      packageAmount: 0,
      orderTaxInfos: prepareTaxInfos(item),
      orderWiseExtraCharges: prepareExtraCharges(item),
      orderWiseExtendedWarranty: prepareExtendedWarranty(item, user),
      shippingZone: item?.ItemShippingZone ?? '',
      shippingCharge: Number(item?.taxinfo?.actualShippingCharges) ?? 0,
      shippingChargePaidBy: item?.ItemShippingPaidBy ?? '',
      color: item?.color ?? '',
      extraDetails: item?.Extradetails ?? '',
      productVariant: item?.ProductVariant ?? ''
    }
  })
  return data ?? []
}

export const prepareOrderPlacingObject = async (values, cartId) => {
  const { store } = await import('../redux/store')
  const { user } = store.getState().user
  const { cart } = store.getState().cart
  const saveOrderAPIPayload = []
  for (const sellerId in values?.sellarViseItems) {
    if (values?.sellarViseItems.hasOwnProperty(sellerId)) {
      const sellerItems = values?.sellarViseItems[sellerId]
      const transformedSellerData = sellerItems.map((order) => {
        const data = {
          orderId: 0,
          orderNo: '',
          sellerID: order?.Items[0]?.sellerId,
          userId: values?.userId ? values?.userId : user?.userId,
          userName: values?.addressVal?.fullName ?? '',
          UserEmail: user?.userName ? user?.userName : values?.userEmail,
          userPhoneNo: values?.addressVal?.mobileNo ?? '',
          userAddressLine1: values?.addressVal?.addressLine1 ?? '',
          userAddressLine2: values?.addressVal?.addressLine2 ?? '',
          userLendMark: values?.addressVal?.landmark ?? '',
          userPincode: values?.addressVal?.pincode ?? '',
          userCity: values?.addressVal?.cityName ?? '',
          userState: values?.addressVal?.stateName ?? '',
          userCountry: values?.addressVal?.countryName ?? '',
          paymentMode: values?.paymentMode ?? '',
          userGSTNo: '',
          totalShippingCharge:
            convertToNumber(order?.SellerCartAmount?.actual_shipping_charges) ??
            0,
          totalExtraCharges:
            convertToNumber(order?.SellerCartAmount?.total_extra_charges) ?? 0,
          totalAmount:
            convertToNumber(order?.SellerCartAmount?.total_amount) ?? '',
          isCouponApplied: !cartId && cart?.coupon_code?.length > 0,
          coupon: cartId ? '' : cart?.coupon_code ?? '',
          coupontDiscount: order?.SellerCartAmount?.total_extradiscount ?? 0,
          coupontDetails: cartId
            ? null
            : cart?.items?.[0]?.coupon_details ?? null,
          codCharge: order?.SellerCartAmount?.cod_charges ?? 0,
          paidAmount:
            convertToNumber(order?.SellerCartAmount?.paid_amount) ?? '',
          isSale: false,
          saleType: '',
          orderDate: null,
          deliveryDate: null,
          status: 'Initiate',
          paymentInfo: '',
          orderBy: 'customer',
          isRetailer: false,
          isVertualRetailer: false,
          isReplace: false,
          sellerName: order?.Items[0]?.sellerName,
          parentId: 0,
          rowNumber: 0,
          pageCount: 0,
          recordCount: 0,
          deliverydays: Number(values?.deliverydays) + 1
        }
        return {
          ...data,
          orderItems: prepareOrderItems(order, cartId, user, values)
        }
      })
      saveOrderAPIPayload.push(...transformedSellerData)
    }
  }
  return saveOrderAPIPayload
}

export const generateCaptcha = () => {
  let characters =
    'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'

  let result = ''
  const length = 6
  for (let i = 0; i < length; i++) {
    const randomIndex = Math.floor(Math.random() * characters.length)
    result += characters.charAt(randomIndex)
  }
  return result
}

export const handleWishlistSetter = async (
  productGuid,
  isAdd,
  data,
  type,
  toast,
  setToast
) => {
  const { store } = await import('../redux/store')
  const { user } = store.getState().user
  if (user?.userId) {
    const values = {
      CreatedBy: user?.userId,
      userId: user?.userId,
      productId: productGuid
    }
    try {
      const response = await axiosProvider({
        method: isAdd ? 'POST' : 'DELETE',
        endpoint: isAdd
          ? 'Wishlist'
          : `Wishlist?userId=${user.userId}&productId=${productGuid}`,
        data: isAdd && values
      })
      if (response?.data?.code === 200) {
        let newData = { ...data }

        if (type === 'productList') {
          if (newData?.data?.products) {
            const productIndex = newData?.data?.products?.findIndex(
              (item) => item?.guid === productGuid
            )

            if (productIndex !== -1) {
              newData.data.products[productIndex].isWishlistProduct = isAdd
            }
          } else {
            const productIndex = newData?.data?.findIndex(
              (item) => item?.guid === productGuid
            )

            if (productIndex !== -1) {
              newData.data[productIndex].isWishlist = isAdd
            }
          }
        } else if ('specificProduct') {
          newData.isWishlistProduct = isAdd
        }
        return { ...newData, wishlistResponse: response }
      } else if (response?.data?.code) {
        return { wishlistResponse: response, code: 204 }
      } else {
        return { code: 500 }
      }
    } catch (error) {
      showToast(toast, setToast, {
        data: _exception?.message,
        code: 204
      })
    }
  } else {
    showToast(toast, setToast, {
      data: { code: 204, message: _toaster?.wishlistAuth }
    })
  }
}

export const handleWishlistClick = async (
  product,
  data,
  type,
  toast,
  setToast
) => {
  const isAdd = product?.isWishlistProduct
    ? !product?.isWishlistProduct
    : !product?.isWishlist
  const response = await handleWishlistSetter(
    product?.guid ?? product?.products?.guid ?? product?.productGuid,
    isAdd,
    data,
    type,
    toast,
    setToast
  )
  return response
}

const getKeyArray = (obj, key) => {
  return key in obj ? (Array.isArray(obj[key]) ? obj[key] : [obj[key]]) : []
}

export const containsKey = (filterList, productDataFilter) => {
  const keys = ['BrandIds', 'ColorIds', 'SizeIds', 'specifications']
  let allIdsMatch = { brand: false, color: false, size: false, spec: false }

  for (let key of keys) {
    switch (key) {
      case 'BrandIds':
        const brandIdArray = getKeyArray(filterList, key)
        if (brandIdArray?.length > 0) {
          const isBrandIdMatch = productDataFilter?.brand_filter?.some(
            (productBrand) => brandIdArray?.includes(productBrand?.brandId)
          )
          if (!isBrandIdMatch) {
            allIdsMatch.brand = 'brand'
          }
        } else {
          allIdsMatch.brand = true
        }
        break

      case 'ColorIds':
        const colorIdArray = getKeyArray(filterList, key)
        if (colorIdArray.length > 0) {
          const isColorIdMatch = productDataFilter?.color_filter?.some(
            (productColor) => colorIdArray?.includes(productColor?.colorId)
          )

          if (!isColorIdMatch) {
            allIdsMatch.color = 'color'
          }
        } else {
          allIdsMatch.color = true
        }
        break

      case 'SizeIds':
        const sizeIdArray = getKeyArray(filterList, key)
        if (sizeIdArray.length > 0) {
          const isSizeIdMatch = productDataFilter?.size_filter?.some(
            (productSize) => sizeIdArray?.includes(productSize?.sizeID)
          )

          if (!isSizeIdMatch) {
            allIdsMatch.size = 'size'
          }
        } else {
          allIdsMatch.size = true
        }
        break
      case 'specifications':
        const specificationsArray = getKeyArray(filterList, key)

        if (specificationsArray.length > 0) {
          const isSpecificationsMatch = specificationsArray?.some((spec) =>
            productDataFilter?.filter_types?.some(
              (productFilter) =>
                productFilter?.filterTypeId === spec?.specId &&
                productFilter?.filterValues?.some(
                  (value) => value?.filterValueId === spec?.specValue
                )
            )
          )
          if (!isSpecificationsMatch) {
            allIdsMatch.spec = 'spec'
          }
        } else {
          allIdsMatch.spec = true
        }
        break
    }
  }
  return Object.values(allIdsMatch).some((value) => value === false) ||
    allIdsMatch?.brand === 'brand' ||
    allIdsMatch?.color === 'color' ||
    allIdsMatch?.size === 'size' ||
    allIdsMatch?.spec === 'spec'
    ? allIdsMatch?.brand === 'brand' ||
      allIdsMatch?.color === 'color' ||
      allIdsMatch?.size === 'size' ||
      allIdsMatch?.spec === 'spec'
      ? { status: true }
      : allIdsMatch?.brand && allIdsMatch?.color && allIdsMatch?.size
      ? false
      : { match: true }
    : { initStatus: false }
}

export const focusInput = (elem) => {
  const inputField = document.getElementById(elem)
  if (inputField) {
    inputField.focus()
  }
}

export const setPageIndexOne = (searchQuery, callback, label) => {
  const searchString = searchQuery || ''
  if (label) {
    callback((draft) => {
      draft[label] = searchString
      draft.pageIndex = 1
    })
  } else {
    callback((draft) => {
      draft.searchString = searchString
      draft.pageIndex = 1
    })
  }
}

export const getEmbeddedUrlFromYouTubeUrl = (url) => {
  try {
    if (url?.length === 11) {
      return url
    }

    const regex =
      /^(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})/
    const match = url.match(regex)
    if (match && match[1]) {
      const videoId = match[1]
      return videoId
    } else {
      return null
    }
  } catch (error) {
    return null
  }
}
