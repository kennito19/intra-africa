using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json.Linq;
using Swashbuckle.Swagger;

namespace API_Gateway.Common
{
    public class getHomePageSections
    {
        private readonly IConfiguration _configuration;
        public string CatelogueURL = string.Empty;
        public string IdServerURL = string.Empty;
        public string UserURL = string.Empty;
        public string UserId = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public getHomePageSections(IConfiguration configuration, HttpContext httpContext)
        {
            _configuration = configuration;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            _httpContext = httpContext;
            helper = new ApiHelper(_httpContext);
        }
        public List<ManageHomePageSectionsLibrary> GetHomePageSection(string? status = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                url += "&Status=" + status;
            }
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageHomePageSections + "?PageIndex=0&PageSize=0" + url, "GET", null);
            BaseResponse<ManageHomePageSectionsLibrary> baseResponse = new BaseResponse<ManageHomePageSectionsLibrary>();
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageHomePageSectionsLibrary> homepageSection =
                baseResponse?.Data as List<ManageHomePageSectionsLibrary> ?? new List<ManageHomePageSectionsLibrary>();
            homepageSection = homepageSection.OrderBy(x => x.Sequence).ToList();

            return homepageSection;
        }

        public List<ManageHomePageDetailsLibrary> GetHomePageDetails(int SectionId, string? status = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                url += "&Status=" + status;
            }
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageHomePageDetails + "?PageIndex=0&PageSize=0&SectionId=" + SectionId + url, "GET", null);
            BaseResponse<ManageHomePageDetailsLibrary> baseResponse = new BaseResponse<ManageHomePageDetailsLibrary>();
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageHomePageDetailsLibrary> homepageSectionDetails =
                baseResponse?.Data as List<ManageHomePageDetailsLibrary> ?? new List<ManageHomePageDetailsLibrary>();
            homepageSectionDetails = homepageSectionDetails.OrderBy(x => x.Sequence).ToList();

            return homepageSectionDetails;
        }

        public ManageLayoutsLibrary GetLayouts(int id)
        {
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageLayouts + "?Id=" + id, "GET", null);
            BaseResponse<ManageLayoutsLibrary> baseResponse = new BaseResponse<ManageLayoutsLibrary>();
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageLayoutsLibrary manageLayouts = baseResponse.Data as ManageLayoutsLibrary;

            return manageLayouts;
        }

        public ManageLayoutTypesLibrary GetLayoutType(int id)
        {
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageLayoutTypes + "?Id=" + id, "GET", null);
            BaseResponse<ManageLayoutTypesLibrary> baseResponse = new BaseResponse<ManageLayoutTypesLibrary>();
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageLayoutTypesLibrary manageLayoutType = baseResponse.Data as ManageLayoutTypesLibrary;

            return manageLayoutType;
        }

        public List<ManageLayoutTypesDetails> GetLayoutTypesDetails(int LayoutTypeId)
        {
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageLayoutTypesDetails + "?LayoutTypeId=" + LayoutTypeId, "GET", null);
            BaseResponse<ManageLayoutTypesDetails> baseResponse = new BaseResponse<ManageLayoutTypesDetails>();
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageLayoutTypesDetails> manageLayoutTypeDetails = baseResponse.Data as List<ManageLayoutTypesDetails>;

            return manageLayoutTypeDetails;
        }

        public List<FrontHomepageDetailsDto> GetFrontHomepageDetails(string? status = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                url += "?Status=" + status;
            }
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageHomePageDetails+ "/getFrontHomepageDetails" + url, "GET", null);
            BaseResponse<FrontHomepageDetailsDto> baseResponse = new BaseResponse<FrontHomepageDetailsDto>();
            baseResponse = baseResponse.JsonParseList(response);
            List<FrontHomepageDetailsDto> homepageSectionDetails =
                baseResponse?.Data as List<FrontHomepageDetailsDto> ?? new List<FrontHomepageDetailsDto>();
            homepageSectionDetails = homepageSectionDetails.ToList();

            return homepageSectionDetails;
        }

        public JObject setSections(string? status = null)
        {
            List<FrontHomepageDetailsDto> HomePageSections = GetFrontHomepageDetails(status);
            
            
            
            
            
            //List<ManageHomePageSectionsLibrary> HomePageSections = GetHomePageSection(status);
            JObject sectionslst = new JObject();

            if (HomePageSections.Count > 0)
            {
                var distinctSections = HomePageSections.GroupBy(dto => dto.HomePageSectionId).Select(group => group.First()).ToList();


                int count = 0;
                bool hasProductListSection = false;
                JObject Data = new JObject();
                foreach (var section in distinctSections)
                {
                    JObject sectionsObj = new JObject();
                    count = count + 1;
                    if (string.Equals(section.LayoutName, "Product List", StringComparison.OrdinalIgnoreCase))
                    {
                        hasProductListSection = true;
                    }
                    List<FrontHomepageDetailsDto> HomePagedata= HomePageSections.Where(p=>p.HomePageSectionId == section.HomePageSectionId).ToList();
                    List<FrontHomepageDetailsDto> HomePageDetails = HomePageSections.Where(p => p.HomePageSectionId == section.HomePageSectionId && p.HomePageSectionDetailsId != null).OrderBy(p=>p.HomePageSectionDetailsSequence).ToList();
                    //List<ManageHomePageDetailsLibrary> HomePageDetails = GetHomePageDetails(section.Id, status);
                    //ManageLayoutsLibrary layouts = GetLayouts(section.LayoutId);
                    //ManageLayoutTypesLibrary layoutTypes = GetLayoutType(section.LayoutTypeId);
                    //List<ManageLayoutTypesDetails> layoutTypesDetails = GetLayoutTypesDetails(section.LayoutTypeId);
                    List<FrontHomepageDetailsDto> layoutTypesDetails = HomePagedata.Where(p => p.LayoutTypeId == section.LayoutTypeId && p.LayoutTypeDetailsId !=null).GroupBy(dto => dto.LayoutTypeDetailsId).Select(group => group.First()).OrderByDescending(p=>p.LayoutTypeDetailsId).ToList();

                    //if (!string.IsNullOrEmpty(layouts.Name) && !string.IsNullOrEmpty(layoutTypes.Name))
                    if (!string.IsNullOrEmpty(section.LayoutName) && !string.IsNullOrEmpty(section.LayoutTypeName))
                    {

                        JObject innerData = new JObject();
                        JObject layoutData = new JObject();
                        JObject _collayoutData = new JObject();
                        JObject _mainobj = new JObject();
                        JObject _leftobj = new JObject();
                        JObject _rightobj = new JObject();
                        JObject _centerobj = new JObject();

                        layoutData["layout_id"] = section.LayoutId;
                        layoutData["layout_type_id"] = section.LayoutTypeId;
                        layoutData["layout_name"] = section.LayoutName;
                        layoutData["layout_type_name"] = section.LayoutTypeName;
                        layoutData["layout_class"] = section.ClassName;
                        layoutData["layout_options"] = section.Options;
                        layoutData["layout_inner_columns"] = section.LayoutTypeColumns;
                        layoutData["layout_has_inner_columns"] = section.HasInnerColumns;
                        layoutData["layout_min_images"] = section.MinImage;
                        layoutData["layout_max_images"] = section.MaxImage;
                        JArray layoutdetailsArary = new JArray();

                        innerData["section_id"] = section.HomePageSectionId;
                        innerData["name"] = section.HomePageSectionName;
                        innerData["sequence"] = section.HomePageSectionSequence;
                        innerData["SectionColumns"] = section.SectionColumns;
                        innerData["title"] = section.HomePageSectionTitle;
                        innerData["sub_title"] = section.HomePageSectionSubTitle;
                        innerData["link_text"] = section.HomePageSectionLinkText;
                        innerData["link"] = section.HomePageSectionLink;
                        innerData["status"] = section.HomePageSectionStatus;
                        innerData["list_type"] = section.ListType;
                        innerData["top_products"] = section.TopProducts;
                        innerData["title_position"] = section.HomePageSectionTitlePosition;
                        innerData["link_in"] = section.HomePageSectionLinkIn;
                        innerData["link_position"] = section.HomePageSectionLinkPosition;
                        innerData["background_color"] = section.BackgroundColor;
                        innerData["title_color"] = section.TitleColor;
                        innerData["text_color"] = section.TextColor;
                        innerData["in_container"] = section.InContainer;
                        innerData["is_title_visible"] = section.HomePageSectionIsTitleVisible;

                        if (section.LayoutTypeName == "Custom Grid" || section.LayoutTypeName == "Custom Row Grid" || section.LayoutTypeName == "Custom Row grid Slider")
                        {
                            innerData["totalRowsInSection"] = section.TotalRowsInSection;
                        }
                        if (section.LayoutTypeName == "Custom Grid")
                        {
                            innerData["isCustomGrid"] = section.IsCustomGrid;
                            innerData["numberOfImages"] = section.NumberOfImages;

                            if (section.NumberOfImages == 1)
                            {
                                _collayoutData["column1"] = "col_" + section.Column1;
                            }
                            else if (section.NumberOfImages == 2)
                            {
                                _collayoutData["column1"] = "col_" + section.Column1;
                                _collayoutData["column2"] = "col_" + section.Column2;
                            }
                            else if (section.NumberOfImages == 3)
                            {
                                _collayoutData["column1"] = "col_" + section.Column1;
                                _collayoutData["column2"] = "col_" + section.Column2;
                                _collayoutData["column3"] = "col_" + section.Column3;
                            }
                            else if (section.NumberOfImages == 4)
                            {
                                _collayoutData["column1"] = "col_" + section.Column1;
                                _collayoutData["column2"] = "col_" + section.Column2;
                                _collayoutData["column3"] = "col_" + section.Column3;
                                _collayoutData["column4"] = "col_" + section.Column4;
                            }
                            
                            innerData["innerColumnClass"] = _collayoutData;
                        }
                        innerData["category_id"] = section.CategoryId;

                        
                        if (layoutTypesDetails.Count > 0)
                        {

                            foreach (var items in layoutTypesDetails)
                            {
                                JObject layoutdetails = new JObject();
                                //JObject mainobj = new JObject();

                                layoutdetails["layout_type_detail_id"] = items.LayoutTypeDetailsId;
                                layoutdetails["layout_type_detail_name"] = items.LayoutTypeDetailsName;
                                layoutdetails["section_type"] = items.SectionType;
                                layoutdetails["inner_columns"] = items.InnerColumns;

                                layoutdetailsArary.Add(layoutdetails);

                                var HomepageData = HomePageDetails.Where(x => x.LayoutTypeDetailsId == items.LayoutTypeDetailsId).ToList();

                                JArray arrayobj = new JArray();
                                foreach (var itemlst in HomepageData)
                                {
                                    JObject obj = new JObject();

                                    obj["section_details_id"] = itemlst.HomePageSectionDetailsId;
                                    obj["layout_type_details_Id"] = itemlst.LayoutTypeDetailsId;
                                    obj["image"] = itemlst.Image;
                                    obj["image_alt"] = itemlst.ImageAlt;
                                    obj["is_title_visible"] = itemlst.IsTitleVisible;
                                    obj["title"] = itemlst.HomePageSectionDetailsTitle;
                                    obj["sub_title"] = itemlst.HomePageSectionDetailsSubTitle;
                                    obj["title_position"] = itemlst.HomePageSectionDetailsTitlePosition;
                                    obj["sequence"] = itemlst.HomePageSectionDetailsSequence;
                                    obj["status"] = itemlst.HomePageSectionDetailsStatus;
                                    obj["redirect_to"] = itemlst.RedirectTo;
                                    obj["title_color"] = itemlst.HomePageSectionDetailsTitleColor;
                                    obj["sub_title_color"] = itemlst.HomePageSectionDetailsSubTitleColor;
                                    obj["title_size"] = itemlst.TitleSize;
                                    obj["sub_title_size"] = itemlst.SubTitleSize;
                                    obj["italic_sub_title"] = itemlst.ItalicSubTitle;
                                    obj["italic_title"] = itemlst.ItalicTitle;
                                    obj["description"] = itemlst.Description;
                                    obj["slider_type"] = itemlst.SliderType;
                                    obj["video_link_type"] = itemlst.VideoLinkType;
                                    obj["video_id"] = itemlst.VideoId;
                                    obj["name"] = itemlst.Name;
                                    obj["option_name"] = itemlst.LayoutOptionName;
                                    obj["option_id"] = itemlst.OptionId;
                                    if (itemlst.HomePageSectionDetailsCategoryId != null)
                                    {
                                        obj["categoryId"] = itemlst.HomePageSectionDetailsCategoryId;
                                        obj["categoryPathName"] = itemlst.CategoryName;
                                        obj["categoryName"] = itemlst.CategoryPathName;
                                    }
                                    if (itemlst.BrandIds != null)
                                    {
                                        obj["brandIds"] = itemlst.BrandIds;
                                    }
                                    if (itemlst.SizeIds != null)
                                    {
                                        obj["sizeids"] = itemlst.SizeIds;
                                    }
                                    if (itemlst.SpecificationIds != null)
                                    {
                                        obj["specificationIds"] = itemlst.SpecificationIds;
                                    }
                                    if (itemlst.ColorIds != null)
                                    {
                                        obj["colorIds"] = itemlst.ColorIds;
                                    }
                                    if (itemlst.CollectionId != null)
                                    {
                                        obj["collectionId"] = itemlst.CollectionId;
                                    }
                                    if (itemlst.ProductId != null)
                                    {
                                        obj["productId"] = itemlst.ProductId;
                                        obj["productName"] = itemlst.ProductName;
                                    }
                                    if (itemlst.StaticPageId != null)
                                    {
                                        obj["staticPageId"] = itemlst.StaticPageId;
                                    }
                                    if (itemlst.LendingPageId != null)
                                    {
                                        obj["lendingPageId"] = itemlst.LendingPageId;
                                    }
                                    if (itemlst.CustomLinks != null)
                                    {
                                        obj["customLinks"] = itemlst.CustomLinks;
                                    }
                                    if (itemlst.AssignCity != null)
                                    {
                                        obj["assignCities"] = itemlst.AssignCity;
                                    }
                                    if (itemlst.AssignState != null)
                                    {
                                        obj["assignStates"] = itemlst.AssignState;
                                    }
                                    if (itemlst.AssignCountry != null)
                                    {
                                        obj["assignCountry"] = itemlst.AssignCountry;
                                    }

                                    arrayobj.Add(obj);


                                }
                                string positionName = string.Empty;
                                //if (layoutTypes.Columns != null && layoutTypes.Columns == 2)
                                if (section.LayoutTypeColumns != null && section.LayoutTypeColumns == 2)
                                {
                                    if (items.LayoutTypeDetailsName.ToLower() == "column1")
                                    {
                                        positionName = "left";
                                        _leftobj[items.SectionType.ToLower()] = arrayobj;
                                        _mainobj[positionName] = _leftobj;
                                    }
                                    //else if (items.Name.ToLower() == "column2")
                                    else if (items.LayoutTypeDetailsName.ToLower() == "column2")
                                    {
                                        positionName = "right";
                                        _rightobj[items.SectionType.ToLower()] = arrayobj;
                                        _mainobj[positionName] = _rightobj;
                                    }

                                }
                                //else if (layoutTypes.Columns != null && layoutTypes.Columns == 3)
                                else if (section.LayoutTypeColumns != null && section.LayoutTypeColumns == 3)
                                {
                                    //if (items.Name.ToLower() == "column1")
                                    if (items.LayoutTypeDetailsName.ToLower() == "column1")
                                    {
                                        positionName = "left";
                                        _leftobj[items.SectionType.ToLower()] = arrayobj;
                                        _mainobj[positionName] = _leftobj;


                                    }
                                    //else if (items.Name.ToLower() == "column2")
                                    else if (items.LayoutTypeDetailsName.ToLower() == "column2")
                                    {
                                        positionName = "center";
                                        _centerobj[items.SectionType.ToLower()] = arrayobj;
                                        _mainobj[positionName] = _centerobj;
                                    }
                                    else
                                    {
                                        positionName = "right";
                                        _rightobj[items.SectionType.ToLower()] = arrayobj;
                                        _mainobj[positionName] = _rightobj;
                                    }

                                }
                                else
                                {
                                    _leftobj[items.SectionType.ToLower()] = arrayobj;
                                    _mainobj["left"] = _leftobj;
                                }
                            }
                            layoutData["layout_details"] = layoutdetailsArary;

                        }
                        //else if (layoutTypes.Name == "Custom Grid")
                        else if (section.LayoutTypeName == "Custom Grid")
                        {
                            var HomepageData = HomePageDetails.Where(x => x.LayoutTypeDetailsId == null && (x.HomePageSectionDetailsId!=null || x.HomePageSectionDetailsId==0)).ToList();
                            //JObject mainobj = new JObject();

                            string name = string.Empty;
                            foreach (var itemlst in HomepageData)
                            {
                                JArray arrayobj = new JArray();
                                JObject obj = new JObject();
                                JObject _Cleftobj = new JObject();
                                
                                obj["section_details_id"] = itemlst.HomePageSectionDetailsId;
                                obj["layout_type_details_Id"] = itemlst.LayoutTypeDetailsId;
                                obj["image"] = itemlst.Image;
                                obj["image_alt"] = itemlst.ImageAlt;
                                obj["is_title_visible"] = itemlst.IsTitleVisible;
                                obj["title"] = itemlst.HomePageSectionDetailsTitle;
                                obj["sub_title"] = itemlst.HomePageSectionDetailsSubTitle;
                                obj["title_position"] = itemlst.HomePageSectionDetailsTitlePosition;
                                obj["sequence"] = itemlst.HomePageSectionDetailsSequence;
                                obj["status"] = itemlst.HomePageSectionDetailsStatus;
                                obj["redirect_to"] = itemlst.RedirectTo;
                                obj["title_color"] = itemlst.HomePageSectionDetailsTitleColor;
                                obj["sub_title_color"] = itemlst.HomePageSectionDetailsSubTitleColor;
                                obj["title_size"] = itemlst.TitleSize;
                                obj["sub_title_size"] = itemlst.SubTitleSize;
                                obj["italic_sub_title"] = itemlst.ItalicSubTitle;
                                obj["italic_title"] = itemlst.ItalicTitle;
                                obj["description"] = itemlst.Description;
                                obj["slider_type"] = itemlst.SliderType;
                                obj["video_link_type"] = itemlst.VideoLinkType;
                                obj["video_id"] = itemlst.VideoId;
                                obj["name"] = itemlst.Name;
                                obj["option_name"] = itemlst.LayoutOptionName;
                                obj["option_id"] = itemlst.OptionId;
                                if (itemlst.HomePageSectionDetailsCategoryId != null)
                                {
                                    obj["categoryId"] = itemlst.HomePageSectionDetailsCategoryId;
                                    obj["categoryPathName"] = itemlst.CategoryName;
                                    obj["categoryName"] = itemlst.CategoryPathName;
                                }
                                if (itemlst.BrandIds != null)
                                {
                                    obj["brandIds"] = itemlst.BrandIds;
                                }
                                if (itemlst.SizeIds != null)
                                {
                                    obj["sizeids"] = itemlst.SizeIds;
                                }
                                if (itemlst.SpecificationIds != null)
                                {
                                    obj["specificationIds"] = itemlst.SpecificationIds;
                                }
                                if (itemlst.ColorIds != null)
                                {
                                    obj["colorIds"] = itemlst.ColorIds;
                                }
                                if (itemlst.CollectionId != null)
                                {
                                    obj["collectionId"] = itemlst.CollectionId;
                                }
                                if (itemlst.ProductId != null)
                                {
                                    obj["productId"] = itemlst.ProductId;
                                    obj["productName"] = itemlst.ProductName;
                                }
                                if (itemlst.StaticPageId != null)
                                {
                                    obj["staticPageId"] = itemlst.StaticPageId;
                                }
                                if (itemlst.LendingPageId != null)
                                {
                                    obj["lendingPageId"] = itemlst.LendingPageId;
                                }
                                if (itemlst.CustomLinks != null)
                                {
                                    obj["customLinks"] = itemlst.CustomLinks;
                                }
                                if (itemlst.AssignCity != null)
                                {
                                    obj["assignCities"] = itemlst.AssignCity;
                                }
                                if (itemlst.AssignState != null)
                                {
                                    obj["assignStates"] = itemlst.AssignState;
                                }
                                if (itemlst.AssignCountry != null)
                                {
                                    obj["assignCountry"] = itemlst.AssignCountry;
                                }
                                if (itemlst.Columns==1)
                                {
                                    name = "column1";
                                    obj["col_class"] = "col_" + section.Column1;
                                }
                                else if (itemlst.Columns == 2)
                                {
                                    name = "column2";
                                    obj["col_class"] = "col_" + section.Column2;
                                }
                                else if (itemlst.Columns == 3)
                                {
                                    name = "column3";
                                    obj["col_class"] = "col_" + section.Column3;
                                }
                                else if (itemlst.Columns == 4)
                                {
                                    name = "column4";
                                    obj["col_class"] = "col_" + section.Column4;
                                }
                                arrayobj.Add(obj);
                                _Cleftobj["single"] = arrayobj;
                                _mainobj[name] = _Cleftobj;
                            }
                        }

                        else
                        {
                            var HomepageData = HomePageDetails.Where(x => x.LayoutTypeDetailsId == null && (x.HomePageSectionDetailsId != null || x.HomePageSectionDetailsId == 0)).ToList();
                            //JObject mainobj = new JObject();

                            JArray arrayobj = new JArray();
                            foreach (var itemlst in HomepageData)
                            {
                                JObject obj = new JObject();

                                obj["section_details_id"] = itemlst.HomePageSectionDetailsId;
                                obj["layout_type_details_Id"] = itemlst.LayoutTypeDetailsId;
                                obj["image"] = itemlst.Image;
                                obj["image_alt"] = itemlst.ImageAlt;
                                obj["is_title_visible"] = itemlst.IsTitleVisible;
                                obj["title"] = itemlst.HomePageSectionDetailsTitle;
                                obj["sub_title"] = itemlst.HomePageSectionDetailsSubTitle;
                                obj["title_position"] = itemlst.HomePageSectionDetailsTitlePosition;
                                obj["sequence"] = itemlst.HomePageSectionDetailsSequence;
                                obj["status"] = itemlst.HomePageSectionDetailsStatus;
                                obj["redirect_to"] = itemlst.RedirectTo;
                                obj["title_color"] = itemlst.HomePageSectionDetailsTitleColor;
                                obj["sub_title_color"] = itemlst.HomePageSectionDetailsSubTitleColor;
                                obj["title_size"] = itemlst.TitleSize;
                                obj["sub_title_size"] = itemlst.SubTitleSize;
                                obj["italic_sub_title"] = itemlst.ItalicSubTitle;
                                obj["italic_title"] = itemlst.ItalicTitle;
                                obj["description"] = itemlst.Description;
                                obj["slider_type"] = itemlst.SliderType;
                                obj["video_link_type"] = itemlst.VideoLinkType;
                                obj["video_id"] = itemlst.VideoId;
                                obj["name"] = itemlst.Name;
                                obj["option_name"] = itemlst.LayoutOptionName;
                                obj["option_id"] = itemlst.OptionId;
                                if (itemlst.HomePageSectionDetailsCategoryId != null)
                                {
                                    obj["categoryId"] = itemlst.HomePageSectionDetailsCategoryId;
                                    obj["categoryPathName"] = itemlst.CategoryName;
                                    obj["categoryName"] = itemlst.CategoryPathName;
                                }
                                if (itemlst.BrandIds != null)
                                {
                                    obj["brandIds"] = itemlst.BrandIds;
                                }
                                if (itemlst.SizeIds != null)
                                {
                                    obj["sizeids"] = itemlst.SizeIds;
                                }
                                if (itemlst.SpecificationIds != null)
                                {
                                    obj["specificationIds"] = itemlst.SpecificationIds;
                                }
                                if (itemlst.ColorIds != null)
                                {
                                    obj["colorIds"] = itemlst.ColorIds;
                                }
                                if (itemlst.CollectionId != null)
                                {
                                    obj["collectionId"] = itemlst.CollectionId;
                                }
                                if (itemlst.ProductId != null)
                                {
                                    obj["productId"] = itemlst.ProductId;
                                    obj["productName"] = itemlst.ProductName;
                                }
                                if (itemlst.StaticPageId != null)
                                {
                                    obj["staticPageId"] = itemlst.StaticPageId;
                                }
                                if (itemlst.LendingPageId != null)
                                {
                                    obj["lendingPageId"] = itemlst.LendingPageId;
                                }
                                if (itemlst.CustomLinks != null)
                                {
                                    obj["customLinks"] = itemlst.CustomLinks;
                                }
                                if (itemlst.AssignCity != null)
                                {
                                    obj["assignCities"] = itemlst.AssignCity;
                                }
                                if (itemlst.AssignState != null)
                                {
                                    obj["assignStates"] = itemlst.AssignState;
                                }
                                if (itemlst.AssignCountry != null)
                                {
                                    obj["assignCountry"] = itemlst.AssignCountry;
                                }

                                arrayobj.Add(obj);


                            }
                            _leftobj["single"] = arrayobj;
                            _mainobj["left"] = _leftobj;
                        }
                        innerData["columns"] = _mainobj;
                        sectionsObj["layoutsInfo"] = layoutData;
                        sectionsObj["section"] = innerData;
                        Data["section" + count] = sectionsObj;

                        //JArray jData = new JArray();
                        //jData.Add(Data);
                        sectionslst["code"] = 200;
                        sectionslst["message"] = "Record bind successfully.";
                        sectionslst["data"] = Data;
                        //sectionslst["data"] = jData;
                        //sectionslst = Data;
                    }
                    else
                    {
                        sectionslst["code"] = 204;
                        sectionslst["message"] = "Record does not exist";
                        sectionslst["data"] = null;
                    }

                }

                if (!hasProductListSection && HasAnyCatalogueProducts())
                {
                    count = count + 1;
                    Data["section" + count] = BuildAutoFeaturedProductSection();
                    sectionslst["code"] = 200;
                    sectionslst["message"] = "Record bind successfully.";
                    sectionslst["data"] = Data;
                }
            }
            else
            {
                if (HasAnyCatalogueProducts())
                {
                    JObject data = new JObject();
                    data["section1"] = BuildAutoFeaturedProductSection();

                    sectionslst["code"] = 200;
                    sectionslst["message"] = "Record bind successfully.";
                    sectionslst["data"] = data;
                }
                else
                {
                    sectionslst["code"] = 204;
                    sectionslst["message"] = "Record does not exist";
                    sectionslst["data"] = null;
                }
            }
            return sectionslst;
        }

        private bool HasAnyCatalogueProducts()
        {
            var response = helper.ApiCall(
                CatelogueURL,
                EndPoints.ManageProductHomePageSections + "?categoryId=0&topProduct=1&productId=",
                "GET",
                null
            );
            BaseResponse<ProductHomePageSectionLibrary> productResponse = new BaseResponse<ProductHomePageSectionLibrary>();
            productResponse = productResponse.JsonParseList(response);
            List<ProductHomePageSectionLibrary> products =
                productResponse?.Data as List<ProductHomePageSectionLibrary> ?? new List<ProductHomePageSectionLibrary>();

            return products.Count > 0;
        }

        private JObject BuildAutoFeaturedProductSection()
        {
            JObject sectionObj = new JObject();
            JObject layoutObj = new JObject();
            JObject sectionDataObj = new JObject();

            layoutObj["layout_id"] = 0;
            layoutObj["layout_type_id"] = 0;
            layoutObj["layout_name"] = "Product List";
            layoutObj["layout_type_name"] = "Auto Generated";
            layoutObj["layout_class"] = "";
            layoutObj["layout_options"] = "";

            sectionDataObj["section_id"] = 0;
            sectionDataObj["name"] = "Auto Featured Products";
            sectionDataObj["sequence"] = 9999;
            sectionDataObj["SectionColumns"] = 1;
            sectionDataObj["title"] = "Featured Products";
            sectionDataObj["sub_title"] = "Top picks from our product catalog";
            sectionDataObj["link_text"] = "View All";
            sectionDataObj["link"] = "/products/all";
            sectionDataObj["status"] = "Active";
            sectionDataObj["list_type"] = "top products";
            sectionDataObj["top_products"] = 1;
            sectionDataObj["title_position"] = "left";
            sectionDataObj["link_in"] = "right";
            sectionDataObj["link_position"] = "right";
            sectionDataObj["background_color"] = "#ffffff";
            sectionDataObj["title_color"] = "#0f172a";
            sectionDataObj["text_color"] = "#475569";
            sectionDataObj["in_container"] = true;
            sectionDataObj["is_title_visible"] = true;
            sectionDataObj["category_id"] = 0;

            sectionObj["layoutsInfo"] = layoutObj;
            sectionObj["section"] = sectionDataObj;

            return sectionObj;
        }
         

        public JObject NewSetSections(int? homepageId= null, string homepageStatus = "Active", string? status = null , int pageIndex= 1, int pageSize= 10)
        {


            return null;
        }

        // Bind Menu

        public List<HomePageSubMenu> getSubmenuList()
        {
            BaseResponse<HomePageSubMenu> Sub_baseResponse = new BaseResponse<HomePageSubMenu>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageSubMenu + "?pageIndex=0&pageSize=0", "GET", null);
            Sub_baseResponse = Sub_baseResponse.JsonParseList(response);
            List<HomePageSubMenu> subMenus =
                Sub_baseResponse?.Data as List<HomePageSubMenu> ?? new List<HomePageSubMenu>();
            List<HomePageSubMenu> submenulst = new List<HomePageSubMenu>();
            submenulst = subMenus.Where(x => x.ParentId == null)
                .Select(x => new HomePageSubMenu
                {
                    Id = x.Id,
                    MenuType = x.MenuType,
                    HeaderId = x.HeaderId,
                    ParentId = x.ParentId,
                    Name = x.Name,
                    Image = x.Image,
                    ImageAlt = x.ImageAlt,
                    HasLink = x.HasLink,
                    RedirectTo = x.RedirectTo,
                    LendingPageId = x.LendingPageId,
                    CategoryId = x.CategoryId,
                    StaticPageId = x.StaticPageId,
                    CollectionId = x.CollectionId,
                    CustomLink = x.CustomLink,
                    Sizes = x.Sizes,
                    Colors = x.Colors,
                    Specifications = x.Specifications,
                    Brands = x.Brands,
                    Sequence = x.Sequence,
                    HeaderColor = x.HeaderColor,
                    ChildMenu = subMenus.Where(detail => detail.ParentId == x.Id)
                .Select(y => new HomePageSubMenu
                {
                    Id = y.Id,
                    MenuType = y.MenuType,
                    HeaderId = y.HeaderId,
                    ParentId = y.ParentId,
                    Name = y.Name,
                    Image = y.Image,
                    ImageAlt = y.ImageAlt,
                    HasLink = y.HasLink,
                    RedirectTo = y.RedirectTo,
                    LendingPageId = y.LendingPageId,
                    CategoryId = y.CategoryId,
                    StaticPageId = y.StaticPageId,
                    CollectionId = y.CollectionId,
                    CustomLink = y.CustomLink,
                    Sizes = y.Sizes,
                    Colors = y.Colors,
                    Specifications = y.Specifications,
                    Brands = y.Brands,
                    Sequence = y.Sequence,
                    HeaderColor = y.HeaderColor
                }).OrderBy(p => p.Sequence).ToList(),
                }).OrderBy(p => p.Sequence).ToList();

            return submenulst;
        }

        public List<HomepageMenu> getmenuList()
        {
            BaseResponse<HomepageMenu> Sub_baseResponse = new BaseResponse<HomepageMenu>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageHeaderMenu + "?pageIndex=0&pageSize=0", "GET", null);
            Sub_baseResponse = Sub_baseResponse.JsonParseList(response);
            List<HomepageMenu> Menus =
                Sub_baseResponse?.Data as List<HomepageMenu> ?? new List<HomepageMenu>();
            List<HomepageMenu> menulst = new List<HomepageMenu>();
            List<HomePageSubMenu> Submenulst = new List<HomePageSubMenu>();
            Submenulst = getSubmenuList().ToList();
            menulst = Menus
                .Select(x => new HomepageMenu
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image,
                    ImageAlt = x.ImageAlt,
                    HasLink = x.HasLink,
                    RedirectTo = x.RedirectTo,
                    LendingPageId = x.LendingPageId,
                    CategoryId = x.CategoryId,
                    StaticPageId = x.StaticPageId,
                    CollectionId = x.CollectionId,
                    CustomLink = x.CustomLink,
                    color = x.color,
                    Sequence = x.Sequence,
                    SubMenu = Submenulst.Where(p => p.HeaderId == x.Id).ToList()
                }).OrderBy(p => p.Sequence).ToList();

            return menulst;
        }


    }
}
