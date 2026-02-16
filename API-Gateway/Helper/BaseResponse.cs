using API_Gateway.Models.Dto;
using Nancy.Json;
using Newtonsoft.Json;

namespace API_Gateway.Helper
{
    public class BaseResponse<T>
    {
        public int code { get; set; }
        public string Message { get; set; }
        public Pagination pagination { get; set; }
        public object Data { get; set; }

        public T Convertor(string message)
        {
            var res = JsonConvert.DeserializeObject<T>(message);
            return res;
        }

        public T Parser(HttpResponseMessage message)
        {
            var res = JsonConvert.DeserializeObject<T>(message.Content.ReadAsStringAsync().Result);
            return res;
        }

        public BaseResponse<UserSignInResponseModel> ParseSignUpResponse(HttpResponseMessage message)
        {
            BaseResponse<UserSignInResponseModel> baseResponse = JsonConvert.DeserializeObject<BaseResponse<UserSignInResponseModel>>(message.Content.ReadAsStringAsync().Result);
            var data = baseResponse.Data.ToString();
            var values = JsonConvert.DeserializeObject<UserSignInResponseModel>(data);
            baseResponse.Data = values;
            return baseResponse;
        }


        //public BaseResponse<UserSignInResponseModel> ParseSignUpResponse(HttpResponseMessage message)
        //{
        //    var json = message.Content.ReadAsStringAsync().Result;
        //    var baseResponse = JsonConvert.DeserializeObject<BaseResponse<List<List<string>>>>(json);

        //    // Extract the error messages from the 'data' field
        //    //var errorMessages = baseResponse.Data.SelectMany(list => list).ToList();

        //    // Handle the error messages however you need to
        //    // For example, you can check if the error messages contain specific strings and perform appropriate actions

        //    // Create a new BaseResponse<UserSignInResponseModel> instance
        //    var parsedResponse = new BaseResponse<UserSignInResponseModel>
        //    {
        //        code = baseResponse.code,
        //        Message = baseResponse.Message,
        //        pagination = baseResponse.pagination,
        //        Data = baseResponse.Data.ToString(),
        //    };

        //    return parsedResponse;
        //}


        public BaseResponse<T> JsonParseList(HttpResponseMessage message)
        {
            var response = new BaseResponse<T>();

            if (message.IsSuccessStatusCode)
            {
                response = JsonConvert.DeserializeObject<BaseResponse<T>>(message.Content.ReadAsStringAsync().Result);
                if (response.Data != null)
                {
                    var data = response.Data.ToString();
                    List<T> values = JsonConvert.DeserializeObject<List<T>>(data);
                    if (values.Count == 0)
                    {
                        response = response.NotExist();
                    }
                    else
                    {
                        response.code = 200;
                        response.Message = "Record bind successfully.";
                        string s = new JavaScriptSerializer().Serialize(values.FirstOrDefault());
                        var pagination = JsonConvert.DeserializeObject<Pagination>(s);
                        if (pagination.RecordCount > 0)
                        {
                            response.pagination = pagination;
                        }
                        response.Data = values;
                    }
                }
                else
                {
                    response = response.NotExist();
                }
            }
            else
            {
                response = APICallFailed(message);
            }
            return response;
        }



        public BaseResponse<T> JsonParseRecord(HttpResponseMessage message)
        {
            var response = new BaseResponse<T>();
            if (message.IsSuccessStatusCode)
            {
                var temp = response.JsonParseList(message);
                if (temp.code != 204)
                {
                    List<T> temp2 = (List<T>)temp.Data;
                    response.Data = temp2.FirstOrDefault();
                    response.code = 200;
                    response.Message = "Record bind successfully.";
                }
                else
                {
                    response = response.NotExist();
                }
            }
            else
            {
                response = APICallFailed(message);
            }
            return response;
        }

        public BaseResponse<T> JsonParseInputResponse(HttpResponseMessage message)
        {
            var response = new BaseResponse<T>();
            if (message.IsSuccessStatusCode)
            {
                response = JsonConvert.DeserializeObject<BaseResponse<T>>(message.Content.ReadAsStringAsync().Result);
                if (response.Data != null)
                {
                    var data = response.Data.ToString();
                    response.Data = JsonConvert.DeserializeObject<int>(data);
                }
            }
            else
            {
                response = APICallFailed(message);
            }
            return response;
        }

        public BaseResponse<T> AlreadyExists()
        {
            BaseResponse<T> response = new BaseResponse<T>();
            response.code = 201;
            response.Message = "Record Already Exist.";
            response.Data = null;
            return response;
        }

        public BaseResponse<T> NotExist()
        {
            BaseResponse<T> response = new BaseResponse<T>();
            response.code = 204;
            response.Message = "Record does not Exist.";
            response.Data = new List<T>();
            return response;
        }

        public BaseResponse<T> ChildExists()
        {
            BaseResponse<T> response = new BaseResponse<T>();
            response.code = 205;
            response.Message = "Parent Record cannot be deleted First.";
            return response;
        }

        public BaseResponse<T> ChildAlreadyExists(string mode, string DeleteRecord)
        {
            BaseResponse<T> response = new BaseResponse<T>();
            response.code = 205;
            response.Message = "Cannot delete " + DeleteRecord + " because it is associated with " + mode;
            return response;
        }

        public BaseResponse<T> APICallFailed(HttpResponseMessage message)
        {

            BaseResponse<T> response = new BaseResponse<T>();
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                response.code = 404;
                response.Message = "API call failed or Endpoint does not Exist.";
            }
            else if (message.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                response.code = 400;
                response.Message = "Bad Request";
            }
            else if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response.code = 401;
                response.Message = "Unauthorized";
            }
            else if (message.StatusCode == System.Net.HttpStatusCode.BadGateway)
            {
                response.code = 502;
                response.Message = "Bad Gateway";
            }
            return response;
        }

        public BaseResponse<T> InvalidInput(string errorMessage)
        {

            BaseResponse<T> response = new BaseResponse<T>();

            response.code = 400;
            response.Message = errorMessage;

            return response;
        }

    }

    public class Pagination
    {
        public int? PageCount { get; set; }
        public int? RecordCount { get; set; }
    }

    public class SignInResponse
    {
        public bool Succeeded { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsNotAllowed { get; set; }
        public bool RequiresTwoFactor { get; set; }
    }

    public class TokenResponseClass
    {
        public string? Access_Token { get; set; }
        public string? Identity_Token { get; set; }
        public string? Issued_Token_Type { get; set; }
        public string? Token_Type { get; set; }
        public string? Refresh_Token { get; set; }
        public string? Scope { get; set; }
        public int Expires_In { get; set; }
    }

    public class UserAccessClaims
    {
        public string? Resource { get; set; }
        public List<string>? AccessType { get; set; }
    }

    public class UserClaimResponse
    {
        public string UserName { get; set; }

        public string UserId { get; set; }
        public List<string>? Roles { get; set; }


        public List<UserAccessClaims> claims { get; set; }

        public UserClaimResponse(string FullName, string userId, List<string>? roles, List<UserAccessClaims> accessClaims)
        {
            UserName = FullName;
            UserId = userId;
            Roles = roles;
            claims = accessClaims;
        }
    }
}
