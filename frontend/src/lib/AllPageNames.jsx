export const allPages = {
  category: 'Manage Category',
  size: 'Manage Size',
  color: 'Manage Color',
  assignAttributes: 'Assign Specs to categories',
  seller: 'Manage Seller',
  hsnCode: 'HSN Code',
  assignTaxRateToHSN: 'Assign tax rate to hsn',
  manageTax: 'Manage Tax',
  manageLayout: 'Manage Layout',
  manageMenu: 'Manage Menu',
  manageStaticPage: 'Manage Static Page',
  addFlashSaleCollection: 'Add Flash Sale Collection',
  flashSaleCollectionMapping: 'Flash Sale Collection Mapping',
  createProductCollection: 'Create Product Collection',
  collectionMappingList: 'Collection Mapping List',
  manageCountry: 'Manage Country',
  manageState: 'Manage State',
  manageCity: 'Manage City',
  manageDelivery: 'Manage Delivery',
  manageSpecifications: 'Manage Specifications',
  manageShippingCharges: 'Manage Shipping Charges',
  manageChargesPaidBy: 'Manage Charges Paid By',
  ManageHSNCode: 'Manage HSN Code',
  manageProducts: 'Manage Products',
  manageAttributes: 'Manage Attributes',
  Brand: 'Brand',
  weightSlab: 'Weight Slab',
  manageAdmin: 'Manage Admin',
  manageOrderStatus: 'Manage Order Status',
  manageIssueandRejection: 'Manage Issue and Rejection',
  manageRoles: 'Manage Roles',
  assignPageRole: 'Assign Page Role',
  manageConfig: 'Manage Config',
  manageReturn: 'Manage Return',
  assignReturnToCategory: 'Assign Return To Category',
  manageSeller: 'Manage Seller',
  assignBrandToSeller: 'Assign Brand To Seller',
  manageCoupon: 'Manage Coupon',
  assignManageSpecification: 'Assign Manage Specification',
  product: 'Product'
}

export const allCrudNames = {
  read: 'read',
  write: 'write',
  update: 'update',
  delete: 'delete'
}

export const checkPageAccess = (pageAccess, pageNames, rightName) => {
  if (typeof pageNames === 'string') {
    pageNames = [pageNames]
  }

  const page = pageAccess?.find((page) => pageNames?.includes(page.pageName))

  if (!page) {
    return false
  }

  if (!rightName) {
    return true
  }

  return page?.hasOwnProperty(rightName) && page[rightName]
}
