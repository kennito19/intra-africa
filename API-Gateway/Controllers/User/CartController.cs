using API_Gateway.Common.cart;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Nancy.Json;
using Nancy.Session;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Xml;
using Users = API_Gateway.Models.Entity.User.Users;

namespace API_Gateway.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string IdServer = string.Empty;
        public static string Catalougeurl = string.Empty;
        BaseResponse<Cart> baseResponse = new BaseResponse<Cart>();
        BaseResponse<cartresponse> baseResponse1 = new BaseResponse<cartresponse>();
        private ApiHelper helper;
        private readonly GetCartDetails getCart;
        public CartController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            Catalougeurl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdServer = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<ApiHelper> Save(Cart model)
        {
            var temp = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + model.SessionId + "&SellerProductMasterId=" + model.SellerProductMasterId + "&sizeId=" + model.SizeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Cart> tmp = (List<Cart>)baseResponse.Data;

            if (tmp.Any())
            {

                foreach (var item in tmp)
                {
                    if (model.Type != null && model.Type.ToLower() == "buynow")
                    {
                        item.Quantity = 1;
                    }
                    else
                    {
                        item.Quantity = item.Quantity + model.Quantity;
                    }
                    Update(item);
                }
            }
            else
            {
                GetCartDetails getCart = new GetCartDetails(_configuration, _httpContext);
                List<SellerProductDetails> lstsellerProduct = getCart.GetSellerproductDetails(model.SellerProductMasterId, Catalougeurl);
                SellerProductDetails sellerProduct = new SellerProductDetails();
                int _qty = 0;



                if (lstsellerProduct.Count > 0)
                {
                    lstsellerProduct = lstsellerProduct.Where(p => p.Status.ToLower() == "active" && p.LiveStatus == true).ToList();
                    if (lstsellerProduct.Count > 0)
                    {
                        if (model.SizeId != null && model.SizeId != 0)
                        {
                            sellerProduct = lstsellerProduct.Where(p => p.SizeId == model.SizeId).FirstOrDefault();
                        }
                        else
                        {
                            sellerProduct = lstsellerProduct.FirstOrDefault();
                        }
                        _qty = Convert.ToInt32(sellerProduct.Quantity);

                        if (_qty == 0)
                        {
                            baseResponse1.code = 204;
                            baseResponse1.Message = "Out of stock";
                            baseResponse1.Data = null;

                        }
                        else if (_qty != 0 && model.Quantity > _qty)
                        {
                            baseResponse1.code = 204;
                            baseResponse1.Message = "Maximum quantity reached";
                            baseResponse1.Data = null;
                        }
                        else
                        {
                            string session = Guid.NewGuid().ToString();
                            Cart cart = new Cart();
                            cart.UserId = model.UserId;
                            cart.SessionId = model.SessionId;
                            cart.SellerProductMasterId = model.SellerProductMasterId;
                            cart.SizeId = model.SizeId;
                            cart.Quantity = model.Quantity;
                            cart.TempMRP = model.TempMRP;
                            cart.TempSellingPrice = model.TempSellingPrice;
                            cart.TempDiscount = model.TempDiscount;
                            cart.SubTotal = model.TempSellingPrice * model.Quantity;
                            //cart.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                            cart.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault() != null ? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value : null;
                            cart.CreatedAt = DateTime.Now;

                            var response = helper.ApiCall(URL, EndPoints.Cart, "POST", cart);
                            baseResponse = baseResponse.JsonParseInputResponse(response);

                            BaseResponse<Cart> baseCart = new BaseResponse<Cart>();
                            var tempCartCount = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + model.SessionId, "GET", null);
                            baseCart = baseCart.JsonParseList(tempCartCount);
                            List<Cart> tmpCart = (List<Cart>)baseCart.Data;
                            JObject jobj = new JObject();
                            jobj["id"] = baseResponse.Data.ToString();
                            jobj["cartCount"] = tmpCart.Count();


                            cartresponse cartresponse = new cartresponse
                            {
                                id = jobj["id"].Value<int>(),
                                cartCount = jobj["cartCount"].Value<int>()
                            };

                            baseResponse1.code = baseResponse.code;
                            baseResponse1.Message = baseResponse.Message;
                            baseResponse1.pagination = baseResponse.pagination;
                            baseResponse1.Data = cartresponse;

                        }
                    }
                    else
                    {
                        baseResponse1 = baseResponse1.NotExist();
                    }
                }
                else
                {
                    baseResponse1 = baseResponse1.NotExist();
                }

            }
            return Ok(baseResponse1);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<ApiHelper> Update(Cart model)
        {
            GetCartDetails getCart = new GetCartDetails(_configuration, _httpContext);
            //var temp = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + model.SessionId + "&SellerProductMasterId=" + model.SellerProductMasterId+"&Id=", "GET", null);
            //baseResponse = baseResponse.JsonParseList(temp);
            //List<Cart> tmp = (List<Cart>)baseResponse.Data;
            //if (tmp.Where(x => x.Id != model.Id).Any())
            //{
            //    baseResponse = baseResponse.AlreadyExists();
            //}
            //else
            //{

            int cartQty = 0;
            cartQty = model.Quantity;


            var response = helper.ApiCall(URL, EndPoints.Cart + "?Id=" + model.Id, "GET", null);
            List<SellerProductDetails> lstsellerProduct = getCart.GetSellerproductDetails(model.SellerProductMasterId, Catalougeurl);
            SellerProductDetails sellerProduct = new SellerProductDetails();
            int _qty = 0;

            baseResponse = baseResponse.JsonParseRecord(response);
            Cart cart = (Cart)baseResponse.Data;

            if (lstsellerProduct.Count > 0)
            {
                lstsellerProduct = lstsellerProduct.Where(p => p.Status.ToLower() == "active" && p.LiveStatus == true).ToList();
                if (lstsellerProduct.Count > 0)
                {
                    if (cart.SizeId != null && cart.SizeId != 0)
                    {
                        sellerProduct = lstsellerProduct.Where(p => p.SizeId == cart.SizeId).FirstOrDefault();
                    }
                    else
                    {
                        sellerProduct = lstsellerProduct.FirstOrDefault();
                    }
                    _qty = Convert.ToInt32(sellerProduct.Quantity);

                    if (_qty == 0)
                    {
                        baseResponse1.code = 204;
                        baseResponse1.Message = "Out of stock";
                        baseResponse1.Data = null;

                    }
                    else if (_qty != 0 && cartQty > _qty)
                    {
                        baseResponse1.code = 204;
                        baseResponse1.Message = "Maximum quantity reached";
                        baseResponse1.Data = null;
                    }
                    else
                    {


                        cart.Id = model.Id;
                        cart.UserId = model.UserId;
                        cart.SessionId = model.SessionId;

                        cart.Quantity = model.Quantity;

                        cart.SubTotal = model.TempSellingPrice * model.Quantity;
                        cart.ModifiedBy = HttpContext != null ? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault() != null ? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value : null : null;

                        cart.ModifiedAt = DateTime.Now;

                        response = helper.ApiCall(URL, EndPoints.Cart, "PUT", cart);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                        baseResponse.Data = model.Id;

                        BaseResponse<Cart> baseCart = new BaseResponse<Cart>();
                        var tempCartCount = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + model.SessionId, "GET", null);
                        baseCart = baseCart.JsonParseList(tempCartCount);
                        List<Cart> tmpCart = (List<Cart>)baseCart.Data;
                        //baseResponse.CartCount = tmpCart.Count();
                        JObject jobj = new JObject();
                        jobj["id"] = model.Id;
                        jobj["cartCount"] = tmpCart.Count();

                        cartresponse cartresponse = new cartresponse
                        {
                            id = jobj["id"].Value<int>(),
                            cartCount = jobj["cartCount"].Value<int>()
                        };

                        baseResponse1.code = baseResponse.code;
                        baseResponse1.Message = baseResponse.Message;
                        baseResponse1.pagination = baseResponse.pagination;
                        baseResponse1.Data = cartresponse;
                    }
                }
                else
                {
                    baseResponse1 = baseResponse1.NotExist();
                }
            }
            else
            {
                baseResponse1 = baseResponse1.NotExist();
            }
            //}
            return Ok(baseResponse1);
        }

        [HttpPut("AddUserIdinCart")]
        [Authorize]
        public ActionResult<ApiHelper> AddUserIdinCart(AddCartUserId cart)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "/AddUserIdinCart", "PUT", cart);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize]
        public ActionResult<ApiHelper> Delete(string sessionId, int? id = null, string? userId = null, int? sellerProductMasterId = null, string? sellerProductIds = null)
        {
            string url = string.Empty;
            if (id != null && id != 0)
            {
                url += "&id=" + id;
            }
            if (!string.IsNullOrEmpty(userId))
            {
                url += "&userId=" + userId;
            }
            if (sellerProductMasterId != null && sellerProductMasterId != 0)
            {
                url += "&SellerProductMasterID=" + sellerProductMasterId;
            }
            if (!string.IsNullOrEmpty(sellerProductIds))
            {
                url += "&sellerProductIds=" + sellerProductIds;
            }

            var temp = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + sessionId + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Cart> templist = (List<Cart>)baseResponse.Data;
            if (templist.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + sessionId + url, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }


        [HttpGet("byId")]
        [Authorize]
        public ActionResult<ApiHelper> ByID(int? id = 0)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("bysessionId")]
        [Authorize]
        public ActionResult<ApiHelper> bysessionId(string sessionId)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "?SessionID=" + sessionId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byuserId")]
        [Authorize]
        public ActionResult<ApiHelper> byuserId(string userId)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "?UserId=" + userId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("bySessionIdandUserId")]
        [Authorize]
        public ActionResult<ApiHelper> bySessionIdandUserId(string sessionId, string userId)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "?SessionID=" + sessionId + "&UserId=" + userId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("sessionCartBySizeandProductId")]
        [Authorize]
        public ActionResult<ApiHelper> sessionCartBySizeandProductId(string sessionId, int sizeId, int sellerProductMasterId)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "?SessionID=" + sessionId + "&SizeId=" + sizeId + "&SellerProductMasterID=" + sellerProductMasterId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("userCartBySizeandProductId")]
        [Authorize]
        public ActionResult<ApiHelper> userCartBySizeandProductId(string sessionId, string userId, int sizeId, int sellerProductMasterId)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "?SessionID=" + sessionId + "&UserId=" + userId + "&SizeId=" + sizeId + "&SellerProductMasterID=" + sellerProductMasterId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("bySessionId&ProductId")]
        [Authorize]
        public ActionResult<ApiHelper> BySessionIdAndProductID(string sessionId, int sellerProductMasterId)
        {
            var response = helper.ApiCall(URL, EndPoints.Cart + "?SessionID=" + sessionId + "&SellerProductMasterId=" + sellerProductMasterId, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpPost("CartCalculation")]
        [Authorize]
        public ActionResult<string> CartCaculation(CartCalculationDTO cartCalculation)
        {
            CartCalculation _cartCalculation = new CartCalculation(_configuration, _httpContextAccessor);
            //NewCartCalculation _cartCalculation = new NewCartCalculation(_configuration, _httpContextAccessor);
            //ClsCartCalcultion _cartCalculation = new ClsCartCalcultion(_configuration, _httpContextAccessor);
            JObject _netSellerEarn = _cartCalculation.GetCartCalculation(cartCalculation);
            string val = _netSellerEarn.ToString();
            return val;
        }

        //string Userurl, string Catalougeurl, string IdentityUrl, string OrderUrl

        [HttpPut("UpdateSession")]
        [Authorize]
        public ActionResult<ApiHelper> UpdateSession(string UserId, string SessionId, int? CartId = 0)
        {
            GetCartDetails getCart = new GetCartDetails(_configuration, _httpContext);
            var temp = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + SessionId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Cart> tmp = (List<Cart>)baseResponse.Data;

            foreach (var item in tmp)
            {
                Cart cart = item;

                cart.Id = item.Id;
                cart.UserId = UserId;
                cart.SessionId = UserId;


                var response = helper.ApiCall(URL, EndPoints.Cart, "PUT", cart);
                baseResponse = baseResponse.JsonParseInputResponse(response);

            }

            BaseResponse<Cart> getCartbaseResponse = new BaseResponse<Cart>();
            var tempcart = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + UserId, "GET", null);
            getCartbaseResponse = getCartbaseResponse.JsonParseList(tempcart);
            List<Cart> tmpcart = (List<Cart>)getCartbaseResponse.Data;
            //List<Cart> rmCart = tmpcart.DistinctBy(p => p.SellerProductMasterId).ToList();

            //var duplicateCounts = tmpcart.GroupBy(pair => new { Key1 = pair.SizeId, Key2 = pair.SellerProductMasterId }).Where(group => group.Count() > 1).Select(s => new { s.First().Id });

            //var rmCart1 = tmpcart.GroupBy(pair => new { Key1 = pair.SizeId, Key2 = pair.SellerProductMasterId }).Where(group => group.Count() > 1).Select(s => new { s.First().Id }).ToList();

            tmpcart = tmpcart.OrderBy(p => p.Id).ToList();

            var rmCart1 = tmpcart.GroupBy(pair => new { Key1 = pair.SizeId, Key2 = pair.SellerProductMasterId }).Where(group => group.Count() > 1).ToList();
            List<Cart> resultCartList = rmCart1.SelectMany(group => group).ToList();
            if (CartId != 0 && CartId != null)
            {
                var data = resultCartList.Where(p => p.Id != CartId).ToList();
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        var response = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + UserId + "&id=" + item.Id, "DELETE", null);
                    }

                }

            }
            else
            {
                var data = rmCart1.Where(group => group.Count() > 1).Select(s => new { s.First().Id, s.First().Quantity }).ToList();
                foreach (var item in data)
                {
                    Cart rec = resultCartList.Where(p => p.Id != item.Id).FirstOrDefault();
                    if (rec != null)
                    {
                        rec.Quantity = rec.Quantity + item.Quantity;
                        var response = helper.ApiCall(URL, EndPoints.Cart, "PUT", rec);
                    }

                    var res = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + UserId + "&id=" + item.Id, "DELETE", null);
                }
            }



            //var s = from ss in tmpcart
            //        group ss in ss.siz
            //        select ss ;



            //if (rmCart1.Any())
            //{
            //    foreach (var item in rmCart1)
            //    {
            //        var response = helper.ApiCall(URL, EndPoints.Cart + "?SessionId=" + UserId + "&id=" + item.Id, "DELETE", null);
            //    }

            //}


            return Ok(baseResponse);
        }

        [HttpGet("AbandonedCart")]
        [Authorize]
        public ActionResult<ApiHelper> GetAbandonedCart(int? pageIndex = 1, int? pageSize = 50)
        {
            BaseResponse<AbandonedCartDTO> baseResponse1 = new BaseResponse<AbandonedCartDTO>();
            var abandonedCartDTOs = new List<AbandonedCartDTO>();

            ProductDetailsDTO product = new ProductDetailsDTO();

            BaseResponse<Cart> baseResponsecart = new BaseResponse<Cart>();

            var response = helper.ApiCall(URL, EndPoints.Cart + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            baseResponsecart = baseResponse.JsonParseList(response);
            List<Cart> cart = new List<Cart>();
            cart = (List<Cart>)baseResponsecart.Data;

            var groupedCarts = cart.Where(c => c.UserId != null).DistinctBy(c => c.UserId).ToList();

            foreach (var ca in groupedCarts)
            {


                CustomerListModel customer = null;
                if (ca.UserId != null)
                {
                    var res = helper.ApiCall(IdServer, EndPoints.CustomerById + "?ID=" + ca.UserId, "GET", null);
                    var baseResponsecustomer = new BaseResponse<CustomerListModel>();
                    baseResponsecustomer = baseResponsecustomer.JsonParseRecord(res);

                    if (baseResponsecustomer.Data != null)
                    {
                        customer = (CustomerListModel)baseResponsecustomer.Data;
                    }
                    var dto = new AbandonedCartDTO
                    {
                        Id = ca.Id,
                        UserId = ca.UserId,
                        //Quantity = ca.Quantity,
                        // SellerProductMasterId = ca.SellerProductMasterId,
                        // TempSellingPrice = ca.TempSellingPrice,
                        // SubTotal = ca.SubTotal,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        UserName = customer.UserName,
                        ProfileImage = customer.ProfileImage,
                        MobileNo = customer.MobileNo,
                        Productdetail = Productdetails(ca.UserId, cart)
                    };
                    abandonedCartDTOs.Add(dto);
                }
            }
            baseResponse1.Data = abandonedCartDTOs;
            baseResponse1.code = baseResponsecart.code;
            baseResponse1.Message = baseResponsecart.Message;
            baseResponse1.pagination = baseResponsecart.pagination;

            return Ok(baseResponse1);
        }

        /*   public ActionResult<ApiHelper> GetAbandonedCart(int? pageIndex = 1, int? pageSize = 50)
           {
               BaseResponse<AbandonedCartDTO> baseResponse1 = new BaseResponse<AbandonedCartDTO>();
               var abandonedCartDTOs = new List<AbandonedCartDTO>();

               ProductDetailsDTO product = new ProductDetailsDTO();

               BaseResponse<Cart> baseResponsecart = new BaseResponse<Cart>();

               var response = helper.ApiCall(URL, EndPoints.Cart + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
               baseResponsecart = baseResponse.JsonParseList(response);
               List<Cart> cart = new List<Cart>();
               cart = (List<Cart>)baseResponsecart.Data;

               var groupedCarts = cart.Where(c => c.UserId != null).GroupBy(c => c.UserId).ToList();

               foreach (var userCarts in groupedCarts)
               {
                   int totalQuantity = 0;

                   // Iterate through carts for this user
                   foreach (var ca in userCarts)
                   {
                       totalQuantity += ca.Quantity;
                   }

                   CustomerListModel customer = null;

                   if (userCarts.Key != null)
                   {
                       var res = helper.ApiCall(IdServer, EndPoints.CustomerById + "?ID=" + userCarts.Key, "GET", null);
                       var baseResponsecustomer = new BaseResponse<CustomerListModel>();
                       baseResponsecustomer = baseResponsecustomer.JsonParseRecord(res);

                       if (baseResponsecustomer.Data != null)
                       {
                           customer = (CustomerListModel)baseResponsecustomer.Data;
                       }
                       var dto = new AbandonedCartDTO
                       {
                           Id = userCarts.First().Id, // Assuming all carts for the same user have the same ID
                           UserId = userCarts.Key,
                           Quantity = totalQuantity,
                           FirstName = customer.FirstName,
                           LastName = customer.LastName,
                           UserName = customer.UserName,
                           ProfileImage = customer.ProfileImage,
                           MobileNo = customer.MobileNo,
                           Productdetail = Productdetails(userCarts.Key, cart)
                       };
                       abandonedCartDTOs.Add(dto);
                   }
               }
               baseResponse1.Data = abandonedCartDTOs;
               baseResponse1.code = baseResponsecart.code;
               baseResponse1.Message = baseResponsecart.Message;
               baseResponse1.pagination = baseResponsecart.pagination;

               return Ok(baseResponse1);
           }
   */

        [NonAction]
        public List<ProductDetailsDTO> Productdetails(string UserId, List<Cart> ca)
        {

            GetCartDetails getCart = new GetCartDetails(_configuration, _httpContext);

            List<ProductDetailsDTO> DTOList = new List<ProductDetailsDTO>();

            ca = ca.Where(p => p.UserId == UserId).ToList();

            // int totalQuantity = 0;

            foreach (var item in ca)
            {
                //  totalQuantity += item.Quantity;

                List<SellerProductDetails> lstsellerProduct = getCart.GetSellerproductDetails(item.SellerProductMasterId, Catalougeurl);

                foreach (var lst in lstsellerProduct)
                {
                    var dto = new ProductDetailsDTO
                    {
                        ProductId = lst.ProductId,
                        SellerProductId = lst.SellerProductId,
                        SellerId = lst.SellerId,
                        ProductName = lst.ProductName,
                        ProductGuid = lst.ProductGuid,
                        MRP = lst.MRP,
                        SellingPrice = lst.SellingPrice,
                        ProductImage = lst.ProductImage,

                        Quantity = item.Quantity,
                        //  Quantity =totalQuantity,
                        CategoryId = lst.CategoryId
                    };

                    DTOList.Add(dto);
                }
            }

            return DTOList;
        }

        public class cartresponse
        {
            public int id { get; set; }
            public int cartCount { get; set; }
        }

    }
}
