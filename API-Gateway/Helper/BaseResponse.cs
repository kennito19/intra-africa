using API_Gateway.Models.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                var payload = message.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<BaseResponse<T>>(payload) ?? new BaseResponse<T>();
                if (response.Data != null)
                {
                    var dataToken = response.Data as JToken ?? JToken.FromObject(response.Data);
                    List<T> values = new List<T>();

                    if (dataToken.Type == JTokenType.Array)
                    {
                        values = dataToken.ToObject<List<T>>() ?? new List<T>();
                    }
                    else if (dataToken.Type == JTokenType.Object)
                    {
                        var single = dataToken.ToObject<T>();
                        if (single != null)
                        {
                            values.Add(single);
                        }
                    }
                    else if (dataToken.Type == JTokenType.Null)
                    {
                        values = new List<T>();
                    }

                    if (values.Count == 0)
                    {
                        response = response.NotExist();
                    }
                    else
                    {
                        response.code = 200;
                        response.Message = "Record bind successfully.";
                        string s = JsonConvert.SerializeObject(values.FirstOrDefault());
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
                var payload = message.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<BaseResponse<T>>(payload) ?? new BaseResponse<T>();

                if (response.Data == null)
                {
                    return response.NotExist();
                }

                var dataToken = response.Data as JToken ?? JToken.FromObject(response.Data);
                T? record = default;

                if (dataToken.Type == JTokenType.Array)
                {
                    var list = dataToken.ToObject<List<T>>() ?? new List<T>();
                    record = list.FirstOrDefault();
                }
                else if (dataToken.Type == JTokenType.Object)
                {
                    record = dataToken.ToObject<T>();
                }
                else if (dataToken.Type != JTokenType.Null)
                {
                    record = JsonConvert.DeserializeObject<T>(dataToken.ToString());
                }

                if (record == null)
                {
                    return response.NotExist();
                }

                response.Data = record;
                response.code = 200;
                response.Message = "Record bind successfully.";
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
                response = JsonConvert.DeserializeObject<BaseResponse<T>>(message.Content.ReadAsStringAsync().Result) ?? new BaseResponse<T>();
                if (response.Data != null)
                {
                    var dataToken = response.Data as JToken ?? JToken.FromObject(response.Data);
                    if (dataToken.Type == JTokenType.Integer)
                    {
                        response.Data = dataToken.ToObject<int>();
                    }
                    else if (dataToken.Type == JTokenType.Float)
                    {
                        response.Data = dataToken.ToObject<double>();
                    }
                    else if (dataToken.Type == JTokenType.Boolean)
                    {
                        response.Data = dataToken.ToObject<bool>();
                    }
                    else if (dataToken.Type == JTokenType.String)
                    {
                        response.Data = dataToken.ToObject<string>();
                    }
                    else if (dataToken.Type == JTokenType.Object || dataToken.Type == JTokenType.Array)
                    {
                        response.Data = dataToken;
                    }
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
            else if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                response.code = 500;
                response.Message = "Internal Server Error";
            }
            else if (message.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                response.code = 503;
                response.Message = "Service Unavailable";
            }
            else
            {
                response.code = (int)message.StatusCode;
                response.Message = !string.IsNullOrWhiteSpace(message.ReasonPhrase) ? message.ReasonPhrase : "Request failed";
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
