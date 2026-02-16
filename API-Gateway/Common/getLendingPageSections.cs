using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using Newtonsoft.Json.Linq;

namespace API_Gateway.Common
{
    public class getLendingPageSections
    {
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        public string CatelogueURL = string.Empty;
        private ApiHelper helper;
        public string Token { get; }
        public getLendingPageSections(IConfiguration configuration, HttpContext httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }


        public List<LendingPageSections> GetLendingPageSection(int LendingPageId, string? status = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                url += "&Status=" + status;
            }
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageLendingPageSection + "?LendingPageId=" + LendingPageId + "&PageIndex=0&PageSize=0" + url, "GET", null);
            BaseResponse<LendingPageSections> baseResponse = new BaseResponse<LendingPageSections>();
            baseResponse = baseResponse.JsonParseList(response);
            List<LendingPageSections> lendingpageSection = (List<LendingPageSections>)baseResponse.Data;
            lendingpageSection = lendingpageSection.OrderBy(x => x.Sequence).ToList();

            return lendingpageSection;
        }


        public List<LendingPageSectionDetails> GetLendingPageDetails(int LendingPageSectionId, string? status = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                url += "&Status=" + status;
            }
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageLendingPageSectionsDetail + "?PageIndex=0&PageSize=0&LendingPageSectionId=" + LendingPageSectionId + url, "GET", null);
            BaseResponse<LendingPageSectionDetails> baseResponse = new BaseResponse<LendingPageSectionDetails>();
            baseResponse = baseResponse.JsonParseList(response);
            List<LendingPageSectionDetails> lendingpageSectionDetails = (List<LendingPageSectionDetails>)baseResponse.Data;
            lendingpageSectionDetails = lendingpageSectionDetails.OrderBy(x => x.Sequence).ToList();

            return lendingpageSectionDetails;
        }

        public ManageLayoutsLibrary GetLayouts(int id)
        {
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageLayouts + "?Id=" + id, "GET", null);
            BaseResponse<ManageLayoutsLibrary> baseResponse = new BaseResponse<ManageLayoutsLibrary>();
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageLayoutsLibrary manageLayouts = (ManageLayoutsLibrary)baseResponse.Data;

            return manageLayouts;
        }

        public ManageLayoutTypesLibrary GetLayoutType(int id)
        {
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageLayoutTypes + "?Id=" + id, "GET", null);
            BaseResponse<ManageLayoutTypesLibrary> baseResponse = new BaseResponse<ManageLayoutTypesLibrary>();
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageLayoutTypesLibrary manageLayoutType = (ManageLayoutTypesLibrary)baseResponse.Data;

            return manageLayoutType;
        }

        public List<ManageLayoutTypesDetails> GetLayoutTypesDetails(int LayoutTypeId)
        {
            var response = helper.ApiCall(CatelogueURL, EndPoints.ManageLayoutTypesDetails + "?LayoutTypeId=" + LayoutTypeId, "GET", null);
            BaseResponse<ManageLayoutTypesDetails> baseResponse = new BaseResponse<ManageLayoutTypesDetails>();
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageLayoutTypesDetails> manageLayoutTypeDetails = (List<ManageLayoutTypesDetails>)baseResponse.Data;

            return manageLayoutTypeDetails;
        }

        public JObject setSections(int LendingPageId, string? status=null)
        {
            List<LendingPageSections> LendingPageSections = GetLendingPageSection(LendingPageId, status);
            JObject sectionslst = new JObject();

            if (LendingPageSections.Count > 0)
            {
                int count = 0;
                JObject Data = new JObject();
                foreach (var section in LendingPageSections)
                {
                    JObject sectionsObj = new JObject();
                    count = count + 1;
                    List<LendingPageSectionDetails> HomePageDetails = GetLendingPageDetails(section.Id, status);
                    ManageLayoutsLibrary layouts = GetLayouts(section.LayoutId);
                    ManageLayoutTypesLibrary layoutTypes = GetLayoutType(section.LayoutTypeId);
                    List<ManageLayoutTypesDetails> layoutTypesDetails = GetLayoutTypesDetails(section.LayoutTypeId);

                    if (!string.IsNullOrEmpty(layouts.Name) && !string.IsNullOrEmpty(layoutTypes.Name))
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
                        layoutData["layout_name"] = layouts.Name;
                        layoutData["layout_type_name"] = layoutTypes.Name;
                        layoutData["layout_class"] = layoutTypes.ClassName;
                        layoutData["layout_options"] = layoutTypes.Options;
                        layoutData["layout_inner_columns"] = layoutTypes.Columns;
                        layoutData["layout_has_inner_columns"] = layoutTypes.HasInnerColumns;
                        layoutData["layout_min_images"] = layoutTypes.MinImage;
                        layoutData["layout_max_images"] = layoutTypes.MaxImage;
                        JArray layoutdetailsArary = new JArray();



                        innerData["section_id"] = section.Id;
                        innerData["lending_page_id"] = section.LendingPageId;
                        innerData["lending_page_name"] = section.LendingPageName;
                        innerData["name"] = section.Name;
                        innerData["sequence"] = section.Sequence;
                        innerData["SectionColumns"] = section.SectionColumns;
                        innerData["title"] = section.Title;
                        innerData["sub_title"] = section.SubTitle;
                        innerData["link_text"] = section.LinkText;
                        innerData["link"] = section.Link;
                        innerData["status"] = section.Status;
                        innerData["title_position"] = section.TitlePosition;
                        innerData["link_in"] = section.LinkIn;
                        innerData["link_position"] = section.LinkPosition;
                        innerData["background_color"] = section.BackgroundColor;
                        innerData["title_color"] = section.TitleColor;
                        innerData["text_color"] = section.TextColor;
                        innerData["in_container"] = section.InContainer;
                        innerData["is_title_visible"] = section.IsTitleVisible;
                        //JObject itemData = new JObject();

                        if (layoutTypes.Name == "Custom Grid" || layoutTypes.Name == "Custom Row Grid" || layoutTypes.Name == "Custom Row grid Slider")
                        {
                            innerData["totalRowsInSection"] = section.TotalRowsInSection;
                        }
                        if (layoutTypes.Name == "Custom Grid")
                        {
                            innerData["isCustomGrid"] = section.IsCustomGrid;
                            innerData["numberOfImages"] = section.NumberOfImages;

                            if (section.NumberOfImages == 1)
                            {
                                //innerData["Column1"] = "col_" + section.Column1;
                                _collayoutData["column1"] = "col_" + section.Column1;
                                //innerData["innerColumnClass"] = new JArray("col_" + section.Column1);
                            }
                            else if (section.NumberOfImages == 2)
                            {
                                //innerData["Column1"] = section.Column1;
                                //innerData["Column2"] = section.Column2;
                                _collayoutData["column1"] = "col_" + section.Column1;
                                _collayoutData["column2"] = "col_" + section.Column2;
                                //innerData["Inner_column_class"] = new JArray("col_" + section.Column1, "col_" + section.Column2);
                            }
                            else if (section.NumberOfImages == 3)
                            {
                                //innerData["Column1"] = section.Column1;
                                //innerData["Column2"] = section.Column2;
                                //innerData["Column3"] = section.Column3;
                                _collayoutData["column1"] = "col_" + section.Column1;
                                _collayoutData["column2"] = "col_" + section.Column2;
                                _collayoutData["column3"] = "col_" + section.Column3;
                                //innerData["Inner_column_class"] = new JArray("col_" + section.Column1, "col_" + section.Column2, "col_" + section.Column3);
                            }
                            else if (section.NumberOfImages == 4)
                            {
                                //innerData["Column1"] = section.Column1;
                                //innerData["Column2"] = section.Column2;
                                //innerData["Column3"] = section.Column3;
                                //innerData["Column4"] = section.Column4;
                                _collayoutData["column1"] = "col_" + section.Column1;
                                _collayoutData["column2"] = "col_" + section.Column2;
                                _collayoutData["column3"] = "col_" + section.Column3;
                                _collayoutData["column4"] = "col_" + section.Column4;
                                //innerData["Inner_column_class"] = new JArray("col_" + section.Column1, "col_" + section.Column2, "col_" + section.Column3, "col_" + section.Column4);

                            }

                            //JArray _collayoutDataArary = new JArray();
                            //_collayoutDataArary.Add(_collayoutData);
                            innerData["innerColumnClass"] = _collayoutData;
                        }

                        if (layoutTypesDetails.Count > 0)
                        {

                            foreach (var items in layoutTypesDetails)
                            {
                                JObject layoutdetails = new JObject();
                                //JObject mainobj = new JObject();

                                layoutdetails["layout_type_detail_id"] = items.Id;
                                layoutdetails["layout_type_detail_name"] = items.Name;
                                layoutdetails["section_type"] = items.SectionType;
                                layoutdetails["inner_columns"] = items.InnerColumns;

                                layoutdetailsArary.Add(layoutdetails);

                                var HomepageData = HomePageDetails.Where(x => x.LayoutTypeDetailsId == items.Id).ToList();

                                JArray arrayobj = new JArray();
                                foreach (var itemlst in HomepageData)
                                {
                                    JObject obj = new JObject();

                                    obj["section_details_id"] = itemlst.Id;
                                    obj["layout_type_details_Id"] = itemlst.LayoutTypeDetailsId;
                                    obj["image"] = itemlst.Image;
                                    obj["image_alt"] = itemlst.ImageAlt;
                                    obj["is_title_visible"] = itemlst.IsTitleVisible;
                                    obj["title"] = itemlst.Title;
                                    obj["sub_title"] = itemlst.SubTitle;
                                    obj["title_position"] = itemlst.TitlePosition;
                                    obj["sequence"] = itemlst.Sequence;
                                    obj["status"] = itemlst.Status;
                                    obj["redirect_to"] = itemlst.RedirectTo;
                                    obj["title_color"] = itemlst.TitleColor;
                                    obj["sub_title_color"] = itemlst.SubTitleColor;
                                    obj["title_size"] = itemlst.TitleSize;
                                    obj["sub_title_size"] = itemlst.SubTitleSize;
                                    obj["italic_sub_title"] = itemlst.ItalicSubTitle;
                                    obj["italic_title"] = itemlst.ItalicTitle;
                                    obj["description"] = itemlst.Description;
                                    obj["slider_type"] = itemlst.SliderType;
                                    obj["video_link_type"] = itemlst.VideoLinkType;
                                    obj["video_id"] = itemlst.VideoId;
                                    obj["name"] = itemlst.Name;
                                    obj["option_name"] = itemlst.OptionName;
                                    obj["option_id"] = itemlst.OptionId;
                                    if (itemlst.CategoryId != null)
                                    {
                                        obj["categoryId"] = itemlst.CategoryId;
                                        obj["categoryPathName"] = itemlst.CategoryPathName;
                                        obj["categoryName"] = itemlst.CategoryName;
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
                                if (layoutTypes.Columns != null && layoutTypes.Columns == 2)
                                {
                                    if (items.Name.ToLower() == "column1")
                                    {
                                        positionName = "left";
                                        _leftobj[items.SectionType.ToLower()] = arrayobj;
                                        _mainobj[positionName] = _leftobj;
                                    }
                                    else if (items.Name.ToLower() == "column2")
                                    {
                                        positionName = "right";
                                        _rightobj[items.SectionType.ToLower()] = arrayobj;
                                        _mainobj[positionName] = _rightobj;
                                    }

                                }
                                else if (layoutTypes.Columns != null && layoutTypes.Columns == 3)
                                {
                                    if (items.Name.ToLower() == "column1")
                                    {
                                        positionName = "left";
                                        _leftobj[items.SectionType.ToLower()] = arrayobj;
                                        _mainobj[positionName] = _leftobj;


                                    }
                                    else if (items.Name.ToLower() == "column2")
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
                        else if (layoutTypes.Name == "Custom Grid")
                        {
                            var HomepageData = HomePageDetails.Where(x => x.LayoutTypeDetailsId == null).ToList();
                            //JObject mainobj = new JObject();

                            string name = string.Empty;
                            foreach (var itemlst in HomepageData)
                            {
                                JArray arrayobj = new JArray();
                                JObject obj = new JObject();
                                JObject _Cleftobj = new JObject();

                                obj["section_details_id"] = itemlst.Id;
                                obj["layout_type_details_Id"] = itemlst.LayoutTypeDetailsId;
                                obj["image"] = itemlst.Image;
                                obj["image_alt"] = itemlst.ImageAlt;
                                obj["is_title_visible"] = itemlst.IsTitleVisible;
                                obj["title"] = itemlst.Title;
                                obj["sub_title"] = itemlst.SubTitle;
                                obj["title_position"] = itemlst.TitlePosition;
                                obj["sequence"] = itemlst.Sequence;
                                obj["status"] = itemlst.Status;
                                obj["redirect_to"] = itemlst.RedirectTo;
                                obj["title_color"] = itemlst.TitleColor;
                                obj["sub_title_color"] = itemlst.SubTitleColor;
                                obj["title_size"] = itemlst.TitleSize;
                                obj["sub_title_size"] = itemlst.SubTitleSize;
                                obj["italic_sub_title"] = itemlst.ItalicSubTitle;
                                obj["italic_title"] = itemlst.ItalicTitle;
                                obj["description"] = itemlst.Description;
                                obj["slider_type"] = itemlst.SliderType;
                                obj["video_link_type"] = itemlst.VideoLinkType;
                                obj["video_id"] = itemlst.VideoId;
                                obj["name"] = itemlst.Name;
                                obj["option_name"] = itemlst.OptionName;
                                obj["option_id"] = itemlst.OptionId;
                                if (itemlst.CategoryId != null)
                                {
                                    obj["categoryId"] = itemlst.CategoryId;
                                    obj["categoryPathName"] = itemlst.CategoryPathName;
                                    obj["categoryName"] = itemlst.CategoryName;
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
                                if (itemlst.Columns == 1)
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
                            var HomepageData = HomePageDetails.Where(x => x.LayoutTypeDetailsId == null).ToList();
                            //JObject mainobj = new JObject();
                            JArray arrayobj = new JArray();
                            foreach (var itemlst in HomepageData)
                            {
                                JObject obj = new JObject();

                                obj["section_details_id"] = itemlst.Id;
                                obj["layout_type_details_Id"] = itemlst.LayoutTypeDetailsId;
                                obj["image"] = itemlst.Image;
                                obj["image_alt"] = itemlst.ImageAlt;
                                obj["is_title_visible"] = itemlst.IsTitleVisible;
                                obj["title"] = itemlst.Title;
                                obj["sub_title"] = itemlst.SubTitle;
                                obj["title_position"] = itemlst.TitlePosition;
                                obj["sequence"] = itemlst.Sequence;
                                obj["status"] = itemlst.Status;
                                obj["redirect_to"] = itemlst.RedirectTo;
                                obj["title_color"] = itemlst.TitleColor;
                                obj["sub_title_color"] = itemlst.SubTitleColor;
                                obj["title_size"] = itemlst.TitleSize;
                                obj["sub_title_size"] = itemlst.SubTitleSize;
                                obj["italic_sub_title"] = itemlst.ItalicSubTitle;
                                obj["italic_title"] = itemlst.ItalicTitle;
                                obj["description"] = itemlst.Description;
                                obj["slider_type"] = itemlst.SliderType;
                                obj["video_link_type"] = itemlst.VideoLinkType;
                                obj["video_id"] = itemlst.VideoId;
                                obj["name"] = itemlst.Name;
                                obj["option_name"] = itemlst.OptionName;
                                obj["option_id"] = itemlst.OptionId;

                                if (itemlst.CategoryId != null)
                                {
                                    obj["categoryId"] = itemlst.CategoryId;
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
                            //mainobj["single"] = arrayobj;
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
            }
            return sectionslst;
        }
    }
}
