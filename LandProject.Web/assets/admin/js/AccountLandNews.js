var _idgrid = "#grid";
var LandNewsModel = {
    ID: '',
    Title: '',
    Alias: '',
    Description: '',
    Image: '',
    Code: '',
    Address: '',
    LandTypeID: '',
    LandCategoryID: '',
    ProvinceID: '',
    DistrictID: '',
    UserID: '',
    AgentID: '',
    WardID: '',
    LProjectID: '',
    Area: '',
    Price: '',
    TotalPrice: '',
    DecimalTotalPrice: '',
    Unit: '',
    Facade: '',
    Entry: '',
    HouseDirection: '',
    BalconyDirection: '',
    NumberFloor: '',
    NumberBedroom: '',
    NumberWC: '',
    Furniture: '',
    LatiLongTude: '',
    IsDelete: false,
    IsPublished: false,
    IsSale: false,
    Status: false,
    CreatedDate: '',
    LandNewsScheduleID: '1',
    Agent: {
        ID: "",
        Name: "",
        Address: "",
        Phone1: "",
        Phone2: "",
        Phone3: "",
        PhoneShow: "",
        Email: "",
        UserId: ""
    }
};


var param = null;
var TypeActionAdd = true;
$(document).ready(function () {
    LoadGrid();

    eventAfterLoadAttributes();
    $('body').on('click', '#saveLandNews', function () {
        if (checkValid()) {
            LandNewsModel.Title = $('#txtTitle').val();
            LandNewsModel.Alias = new commonService().getSeoTitle($('#txtTitle').val());
            LandNewsModel.Description = $('#txtDescription').val();
            LandNewsModel.Address = $('#Address').val();
            LandNewsModel.LandTypeID = $('#cboLandType').val();
            LandNewsModel.LandCategoryID = $('#cboLandCategory').val();
            LandNewsModel.LatiLongTude = $('#LatiLongTude').val();
            LandNewsModel.ProvinceID = $("#cboProvince").val();
            LandNewsModel.DistrictID = $('#cboDistrict').val();
            LandNewsModel.WardID = $('#cboWard').val();
            LandNewsModel.LProjectID = $('#cboLProject').val();
            LandNewsModel.Area = $('#txtArea').val();
            LandNewsModel.Price = $("#txtPrice").val() != "" ? parseFloat(numberReplaceCommas($("#txtPrice").val())) : 0;
            LandNewsModel.TotalPrice = $('#txtTotalPrice').val();
            LandNewsModel.DecimalTotalPrice = $("#txtPrice").val() != "" ? parseFloat(numberReplaceCommas($("#txtPrice").val())) : 0;
            LandNewsModel.Unit = $('#txtUnit').val();
            LandNewsModel.Facade = $('#txtFacade').val();
            LandNewsModel.Entry = $('#txtEntry').val();
            LandNewsModel.HouseDirection = $('#txtHouseDirection').val();
            LandNewsModel.BalconyDirection = $('#txtBalconyDirection').val();
            LandNewsModel.NumberFloor = $('#txtNumberFloor').val();
            LandNewsModel.NumberBedroom = $('#txtNumberBedroom').val();
            LandNewsModel.NumberWC = $('#txtNumberWC').val();
            LandNewsModel.Furniture = $('#txtFurniture').val();
            LandNewsModel.Convenient = $('#txtConvenient').val();
            LandNewsModel.Environment = $('#txtEnvironment').val();
            LandNewsModel.LegalStatus = $('#txtLegalStatus').val();
            LandNewsModel.Agent.Name = $('#txtAgentName').val();
            LandNewsModel.Agent.Phone1 = $('#txtAgentPhone1').val();
            LandNewsModel.Agent.Phone2 = $('#txtAgentPhone2').val();
            LandNewsModel.Agent.Phone3 = $('#txtAgentPhone3').val();
            LandNewsModel.Agent.PhoneShow = $('#txtAgentPhoneShow').val();
            LandNewsModel.Agent.Address = $('#txtAgentAddress').val();

            LandNewsModel.Agent.Email = $('#txtAgentEmail').val();

            console.log(LandNewsModel)
            //File hình ảnh
            var formData = new FormData();

            formData.append('landnews', JSON.stringify(LandNewsModel));
            formData.append('agent', JSON.stringify(LandNewsModel.Agent));

            $.ajax({
                url: _Host + "api/landnews/update",
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                dataType: "json",
                success: function (result) {
                    if (result) {
                        $("#LandNewsModel").modal('hide');
                        swal({
                            title: "Thông báo",
                            text: "thành công",
                            confirmButtonColor: "#EF5350",
                            type: "success"
                        }, function () { $("#model-notify").modal('show'); });
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();

                    }
                },
                error: function (e) {
                    console.log(e)
                }
            });
        }
    })
    $('body').on('click', '#closeLandType', function () {
        $("#LandNewsModel").modal('hide');
    })

    // event rmfile load update
    $('body').on('click', '.rmFileImage', function () {
        var fileId = $(this).attr('data-id-file');
        var fileName = $(this).attr('data-name-file');
        var params = {
            fileId: fileId,
            fileName: fileName
        }
        params = JSON.stringify(parmas);
        console.log(params)
    })


    $('body').on('click', '#saveLandNewsSchedule', function () {
        var landScheduleID = $("#cboLandSchedule").val();
        var param = {
            landScheduleID: landScheduleID,
            landNewsID: $('#landNewsID').val()
        }
        param = JSON.stringify(param)
        var svr = new AjaxCall("api/landnews/publishedlandnewsschedule", param);
        svr.callServicePOST(function (data) {
            console.log(data)
            $("#ActionLandNewsSchedule").modal('hide');
            swal({
                title: "Thông báo",
                text: "thành công",
                confirmButtonColor: "#EF5350",
                type: "success"
            }, function () { $("#model-notify").modal('show'); });
            $('#grid').data('kendoGrid').dataSource.read();
            $('#grid').data('kendoGrid').refresh();
        });
    })

    $('body').on('click', '#closeLandNewsSchedule', function () {
        $("#ActionLandNewsSchedule").modal('hide');
    })

})

checkValid = function () {
    var res = $("#form").validationEngine('validate');
    setTimeout(function () {
        $('#form').validationEngine('hide');
    }, 10000);
    var LandNewsName = $('#txtTitle').val();
    var AgentPhone = $('#txtAgentPhone1').val();
    if (LandNewsName.trim() == "") {
        $('#txtLandNewsName').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#txtLandNewsName").validationEngine('hide');
    }
    if ($("#cboLandType").data("kendoComboBox").selectedIndex == -1) {
        $('#cboLandType').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#cboLandType").validationEngine('hide');
    }
    if ($("#cboLandCategory").data("kendoComboBox").selectedIndex == -1) {
        $('#cboLandCategory').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#cboLandCategory").validationEngine('hide');
    }
    if ($("#cboProvince").data("kendoComboBox").selectedIndex == -1) {
        $('#cboProvince').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#cboProvince").validationEngine('hide');
    }
    if ($("#cboDistrict").data("kendoComboBox").selectedIndex == -1) {
        $('#cboDistrict').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#cboDistrict").validationEngine('hide');
    }
    if (AgentPhone.trim() == "") {
        $('#txtAgentPhone1').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#txtAgentPhone1").validationEngine('hide');
    }
    if ($('#Address').val().trim() == "") {
        $('#Address').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#Address").validationEngine('hide');
    }

    return res;
}

DataSource = function () {
    this.MasterDatasource = function (url) {
        var total = 0;
        var data = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    //console.log(options);
                    //console.log(options.data);
                    //var request = JSON.stringify(options.data);
                    var form = new FormData();
                    form.append('requestFilter', JSON.stringify(options.data))
                    form.append('userID', JSON.stringify($("#userID").val()))
                    $.ajax({
                        url: url,
                        type: "POST",
                        contentType: false,
                        processData: false,
                        cache: false,
                        data: form,
                        beforeSend: function () {
                            //console.log(1);
                            $(block).block({
                                message: '<i class="icon-spinner4 fa fa-spinner fa-pulse spinner"></i>',
                                overlayCSS: {
                                    backgroundColor: '#efeff6',
                                    opacity: 0.8,
                                    cursor: 'wait',
                                    zIndex: 99999999999999
                                },
                                css: {
                                    border: 0,
                                    padding: 0,
                                    backgroundColor: 'transparent',
                                }
                            });
                        },
                        complete: function (e) {
                            $(block).unblock();
                        },
                        success: function (result) {
                            //console.log(result)
                            if (result != null) {
                                if (result.Items != null) {
                                    options.success(result);
                                }
                            } else {
                                options.success([]);
                            }
                        },
                        error: function (result) {
                            options.success([]);
                        }
                    })
                },
            },
            schema: {
                data: function (data) {
                    //console.log(data)
                    if (data != null) {
                        if (data.Items != null)
                            return data.Items;
                    }
                    return [];
                },
                total: function (data) {
                    //console.log(data)
                    if (data != null) {
                        if (data.Items != null)
                            return data.TotalCount;
                    }
                    return 0;
                },
                model: {
                    fields: {
                        CreatedDate: { type: "date" },
                    }
                }
            },
            pageSize: 20,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        });
        return data;
    }
}
var Columns = [{
    title: "Stt",
    template: '#= record++ #',
    filterable: false,
    width: 50,
    attributes: {
        style: "text-align: center"
    },
    headerAttributes: {
        style: "text-align: center"
    },
},
    {
        template: templateForAction,
        filterable: false,
        width: 90,
        title: "Tiện ích",
        attributes: {
            style: "text-align: center; overflow : visible; cursor: pointer",
        },
        headerAttributes: {
            style: "text-align: center"
        },
    },
        {
            template: '#if(data.IsPublished == true){#<span style="color:green">Đã duyệt</span>#} else if(data.IsPublished == false){#<span style="color:red">Đang chờ duyệt</span>#}#',
            field: "ln.IsPublished",
            title: "Trạng thái",
            width: 120,
            filterable: {
                ui: statusFilter
            }
        },
                        {
                            template: '#=data.LandNewsScheduleName#',
                            field: "ls.Name",
                            title: "Loại tin",
                            width: 100,
                        },
            {
                template: '#=data.Code#',
                field: "ln.Code",
                title: "Mã tin",
                width: 100,
            },

                {
                    template: '#if(data.CreatedDate != null){#<div>#=kendo.toString(new Date(data.CreatedDate), "dd/MM/yyyy")#</div>#}#',
                    field: "CreatedDate",
                    title: "Ngày tạo",
                    width: 100,
                    groupable: false,
                    sortable: true,
                    filterable: {
                        extra: false,
                        operators: {
                            date: {
                                eq: "Bằng với",
                                gte: "Lớn hơn",
                                lte: "Nhỏ hơn",

                            }
                        },
                    },

                },
    {
        template: '#=data.Title#',
        field: "ln.Title",
        title: "Tiêu đề",
        width: 250,
    },
    {
        template: '#if(data.Address != null){##=data.Address##}#',
        field: "ln.Address",
        title: "Địa chỉ",
        width: 150,
        filterable: false,
    },
    {
        template: '#if(data.Area != null){##=data.Area##}#',
        field: "ln.Area",
        title: "Diện tích/m2",
        width: 100,
        filterable: false,
    },
    {
        template: '#if(data.Price != null){##=data.Price##}#',
        field: "ln.Price",
        title: "Giá tiền",
        width: 100,
        filterable: false,
    },
    {
        template: '#if(data.Unit != null){##=data.Unit##}#',
        field: "ln.Unit",
        title: "Đơn vị",
        width: 100,
        filterable: false,
    },
    {
        template: '#if(data.TotalPrice != null){##=data.TotalPrice##}#',
        field: "ln.TotalPrice",
        title: "Tổng tiền",
        width: 100,
        filterable: false,
    },
      {
          template: '#if(data.ProvinceName != null){##=data.ProvinceName##}#',
          field: "ln.ProvinceID",
          title: "Tỉnh/Thành phố",
          width: 100,
      },
            {
                template: '#if(data.DistrictName != null){##=data.DistrictName##}#',
                field: "ln.DistrictID",
                title: "Quận/Huyện",
                width: 100,
            },
                  {
                      template: '#if(data.WardName != null){##=data.WardName##}#',
                      field: "ln.WardID",
                      title: "Phường/Xã",
                      width: 100,
                  },

    //{
    //    template: '#=data.LandTypeName#',
    //    field: "ln.LandTypeID",
    //    title: "Thể loại",
    //    filterable: {
    //        ui: landTypeFilter
    //    }
    //},

];
LoadGrid = function () {

    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/landnews/getalltablebyaccount"), null, false, '')
}

function templateForAction(e) {
    var html = '';
    var permission = {
        IsView: true,
        IsAdd: true,
        IsUpdate: true,
        IsDelete: true,
        IsApprove: true,
    };
    if (permission.IsView || permission.IsAdd || permission.IsUpdate || permission.IsDelete || permission.IsApprove) {
        html += '<div class="btn-group">';
        html += '<i class="ace-icon fa fa-cog icon-only bigger-120 dropdown-toggle" data-toggle="dropdown"></i>';
        html += '<ul class="dropdown-menu dropdown-white dropdown-menu-right">';
        if (permission.IsUpdate) {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').Edit(\'' + e.LandTypeName + '\')">Xem / Chỉnh sửa</a>';
            html += '</li>';
        }
        if (permission.IsUpdate) {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').PickTypeLandNewsSchedule(\'' + e.LandNewsScheduleID + '\')">Chọn loại tin</a>';
            html += '</li>';
        }
        if (!e.IsPublished) {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').Published()">Duyệt tin</a>';
            html += '</li>';
        } else {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').CanclePublished()">Hạ tin</a>';
            html += '</li>';
        }
        if (!e.IsDelete) {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').Delete(\'' + e.Name + '\')">Xóa</a>';
            html += '</li>';
        }
        html += '</ul></div>';
    }
    return html;
}
ForumCatg = function (id) {
    this.Edit = function (landTypeName) {

        var form = new FormData();
        TypeActionAdd = false;
        $("#lTypeTitle").empty().html(landTypeName)
        $('#txtTitle').val('');
        $('#txtArea').val('');
        $('#txtUnit').val('');
        $('#txtPrice').val('');
        $('#txtTotalPrice').val('');
        $('#Address').val('');
        $('#txtDecimalTotalPrice').val('');
        $('#txtMetaKeyword').val('');
        $('#txtMetaDescription').val('');

        $('#txtFacade').val('');
        $('#txtEntry').val('');
        $('#txtHouseDirection').val('');
        $('#txtBalconyDirection').val('');
        $('#txtNumberFloor').val('');
        $('#txtNumberBedroom').val('');
        $('#txtNumberWC').val('');
        $('#txtFurniture').val('');
        $('#txtDescription').val('');

        $('#txtConvenient').val('');
        $('#txtEnvironment').val('');
        $('#txtLegalStatus').val('');

        $('#txtAgentName').val('');
        $('#txtAgentAddress').val('');
        $('#txtAgentPhone1').val('');
        $('#txtAgentPhone2').val('');
        $('#txtAgentPhone3').val('');
        $('#txtAgentPhoneShow').val('');
        $('#txtAgentEmail').val('');

        $("#cboLandType").val('');
        $("#cboLandCategory").val('');
        $("#cboProvince").val('');
        $("#cboDistrict").val('');
        $("#cboWard").val('');
        $("#cboLProject").val('');

        $("#lst-file-image").empty();

        $("#LandNewsModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        var param = {
            LandNewsID: id
        }
        var svr = new AjaxCall("api/landnews/getbyid", param);
        svr.callServiceGET(function (data) {

            console.log(data)
            if (data != undefined) {
                LandNewsModel.ID = data.ID;
                LandNewsModel.IsDelete = data.IsDelete;
                LandNewsModel.IsPublished = data.IsPublished;
                LandNewsModel.IsSale = data.IsSale;
                LandNewsModel.Status = data.Status;
                LandNewsModel.LandNewsScheduleID = data.LandNewsScheduleID;
                LandNewsModel.CreatedDate = data.CreatedDate;
                LandNewsModel.Image = data.Image;
                LandNewsModel.Code = data.Code;
                LandNewsModel.UserID = data.UserID;
                LandNewsModel.LatiLongTude = data.LatiLongTude;
                LandNewsModel.AgentID = data.AgentID;
                $("#LatiLongTude").val(data.LatiLongTude);

                if (LandNewsModel.LatiLongTude != "") {
                    var arrLatLong = LandNewsModel.LatiLongTude.split(',');
                    initLatiLongMap(arrLatLong[0], arrLatLong[1]);
                } else {
                    initMap();
                }

                var url1 = "api/landtype/getall";
                var element = "#cboLandType";
                LoadComboBoxWithServices(element, url1, null, "ID", "Name", data.LandTypeID, "Chọn Hình thức", false, null, function () { }, null);
                var param1 = {
                    lTypeID: data.LandTypeID
                }
                var url2 = "api/landcategory/getall";
                var element = "#cboLandCategory";
                LoadComboBoxWithServices(element, url2, param1, "ID", "Name", data.LandCategoryID, "Chọn Thể Loại", false, null, function () { }, null);

                var url6 = "api/lproject/getall";
                var element = "#cboLProject";
                LoadComboBoxWithServices(element, url6, null, "ID", "Name", data.LProjectID, "Chọn Dự án", false, null, function () { }, null);

                var url4 = "api/address/getdistrict";
                var element4 = "#cboDistrict";

                var url5 = "api/address/getward";
                var element5 = "#cboWard";

                var url3 = "api/address/getprovince";
                var element = "#cboProvince";
                LoadComboBoxWithServices(element, url3, null, "ID", "Name", data.ProvinceID, "Chọn Tỉnh/Thành phố", false, null, function (e) {
                    console.log(e.sender._old);
                    var provinceID = e.sender._old;
                    $("#cboWard").data("kendoComboBox").value(null);
                    if (provinceID == '') {
                        provinceID = 0;
                        $("#cboDistrict").val('');
                        var param5 = {
                            districtID: 0
                        }
                        LoadComboBoxWithServices(element5, url5, param5, "ID", "Name", null, "Chọn Phường/Xã", false, null, function () { }, null);
                    }
                    var param4 = {
                        provinceID: provinceID
                    }
                    LoadComboBoxWithServices(element4, url4, param4, "ID", "Name", null, "Chọn Quận/Huyện", false, null, function (e) {
                        console.log(e.sender._old);
                        $("#cboWard").data("kendoComboBox").value(null);
                        var districtID = e.sender._old;
                        if (districtID == '') {
                            districtID = 0;
                            $("#cboWard").data("kendoComboBox").value(null);
                        }
                        var param5 = {
                            districtID: districtID
                        }
                        LoadComboBoxWithServices(element5, url5, param5, "ID", "Name", null, "Chọn Phường/Xã", false, null, function () { }, null);
                    }, null);
                }, null);

                var param4 = {
                    provinceID: data.ProvinceID
                }
                var url4 = "api/address/getdistrict";
                var element = "#cboDistrict";
                LoadComboBoxWithServices(element, url4, param4, "ID", "Name", data.DistrictID, "Chọn Quận/Huyện", false, null, function () { }, null);
                var param5 = {
                    districtID: data.DistrictID
                }
                var url5 = "api/address/getward";
                var element = "#cboWard";
                LoadComboBoxWithServices(element, url5, param5, "ID", "Name", data.WardID, "Chọn Phường/Xã", false, null, function () { }, null);


                $('#txtTitle').val(data.Title);
                $('#txtArea').val(data.Area);
                $('#txtUnit').val(data.Unit);
                $('#txtPrice').val(data.Price);
                $('#txtTotalPrice').val(data.TotalPrice);
                $('#Address').val(data.Address);
                $('#txtDecimalTotalPrice').val(data.DecimalTotalPrice);
                $('#txtMetaKeyword').val(data.MetaKeyword);
                $('#txtMetaDescription').val(data.MetaDescription);


                $('#txtFacade').val(data.Facade);
                $('#txtEntry').val(data.Entry);
                $('#txtHouseDirection').val(data.HouseDirection);
                $('#txtBalconyDirection').val(data.BalconyDirection);
                $('#txtNumberFloor').val(data.NumberFloor);
                $('#txtNumberBedroom').val(data.NumberBedroom);
                $('#txtNumberWC').val(data.NumberWC);
                $('#txtFurniture').val(data.Furniture);
                $("#txtDescription").val(data.Description.replace(/<br *\/?>/gi, '\n'));

                $('#txtConvenient').val(data.Convenient);
                $('#txtEnvironment').val(data.Environment);
                $('#txtLegalStatus').val(data.LegalStatus);

                if (data.Agent != undefined) {
                    console.log(data.Agent)
                    LandNewsModel.Agent.ID = data.Agent.ID;
                    LandNewsModel.Agent.UserId = data.Agent.UserId;
                    $('#txtAgentName').val(data.Agent.Name);
                    $('#txtAgentAddress').val(data.Agent.Address);
                    $('#txtAgentPhone1').val(data.Agent.Phone1);
                    $('#txtAgentPhone2').val(data.Agent.Phone2);
                    $('#txtAgentPhone3').val(data.Agent.Phone3);
                    $('#txtAgentPhoneShow').val(data.Agent.PhoneShow);
                    $('#txtAgentEmail').val(data.Agent.Email);
                }

                var html = '';
                if (data.LandFiles.length != 0) {
                    $.each(data.LandFiles, function (index, value) {
                        html += '<div class="col-md-55">'
                        html += '<div class="thumbnail">';
                        html += '<div class="image view view-first" data-id-file="' + value.ID + '" data-name-file="' + value.FileName + '">';
                        html += '<img style="width: 100%; display: block;" src="' + _Host + 'fileman/Uploads/LandNews/' + value.FileName + '" alt="image" />';
                        html += '<div class="mask">';
                        html += '<div class="tools tools-bottom">';
                        html += '<a href="#" data-id-file="' + value.ID + '" data-name-file="' + value.FileName + '" class="rmFileImage"><i class="fa fa-times"></i></a>';
                        html += '</div>';
                        html += '</div>';
                        html += '</div>';
                        html += '</div>';
                        html += '</div>';
                    })
                }
                $("#lst-file-image").append(html);

                setTextMoneyAllowDecimal($("#txtPrice"));

                $("#LandNewsModel").modal({
                    backdrop: 'static',
                    keyboard: true,
                    show: true
                });
            }
        });
    }
    this.Published = function () {
        var param = {
            landNewsID: id
        }
        param = JSON.stringify(param)
        var svr = new AjaxCall("api/landnews/published", param);
        svr.callServicePOST(function (data) {
            console.log(data)
            $("#model-notify").modal('hide');
            swal({
                title: "Thông báo",
                text: "Duyệt thành công",
                confirmButtonColor: "#EF5350",
                type: "success"
            }, function () { $("#model-notify").modal('show'); });
            $('#grid').data('kendoGrid').dataSource.read();
            $('#grid').data('kendoGrid').refresh();
        });
    }
    this.CanclePublished = function () {
        var param = {
            landNewsID: id
        }
        param = JSON.stringify(param)
        var svr = new AjaxCall("api/landnews/canclepublished", param);
        svr.callServicePOST(function (data) {
            console.log(data)
            $("#model-notify").modal('hide');
            swal({
                title: "Thông báo",
                text: "Hạ tin thành công",
                confirmButtonColor: "#EF5350",
                type: "success"
            }, function () { $("#model-notify").modal('show'); });
            $('#grid').data('kendoGrid').dataSource.read();
            $('#grid').data('kendoGrid').refresh();
        });
    }
    this.PickTypeLandNewsSchedule = function (landNewsScheduleID) {
        $("#landNewsID").val(id);
        var url1 = "api/schedule/getall";
        var element = "#cboLandSchedule";
        LoadComboBoxWithServices(element, url1, null, "ID", "Name", landNewsScheduleID, "Chọn Loại đăng tin", false, null, function () { }, null);
        $("#ActionLandNewsSchedule").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
    }

    this.Delete = function (title) {
        bootbox.confirm({
            message: "Bạn có chắc muốn xóa tin này",
            buttons: {
                confirm: {
                    label: "Đồng ý",
                    className: 'btn-primary'
                },
                cancel: {
                    label: "Hủy",
                    className: 'btn-default'
                }
            },
            callback: function (result) {
                if (result) {
                    var param = {
                        landNewsID: id
                    }
                    var svr1 = new AjaxCall("api/landnews/delete", JSON.stringify(param));
                    svr1.callServicePOST(function (data) {
                        if (data != undefined) {
                            $("#model-notify").modal('hide');
                            swal({
                                title: "Thông báo",
                                text: "Xóa tin thành công",
                                confirmButtonColor: "#EF5350",
                                type: "success"
                            }, function () { $("#model-notify").modal('show'); });
                            $('#grid').data('kendoGrid').dataSource.read();
                            $('#grid').data('kendoGrid').refresh();
                        }
                    });
                }
            }
        })
    }
}
statusF = new kendo.data.DataSource({
    data: [{ STT: 0, TEXT: "Đã duyệt" }, { STT: 1, TEXT: "Đang chờ duyệt" }]
});

function statusFilter(element) {
    element.kendoDropDownList({
        dataSource: statusF,//statusF,
        dataTextField: "TEXT",
        dataValueField: "STT",
        optionLabel: "--Chọn trạng thái--",
        //template: '#="<span>"+TEXT+"</span>" #',
    });
}

function landTypeFilter(element) {
    element.kendoDropDownList({
        dataSource: lstLanType,
        dataTextField: "Name",
        dataValueField: "ID",
        optionLabel: "--Chọn thể loại--"
    });
}
var lstLanType = new kendo.data.DataSource({
    transport: {
        read: function (options) {
            $.ajax({
                url: _Host + "api/landtype/getall",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    //console.log(result)
                    if (result != "" && result != null) {
                        options.success(result);
                    } else {
                        options.success([]);
                    }
                },
                error: function (result) {
                    options.success([]);
                }
            })
        }
    }
});

//init map
function initLatiLongMap(latitude, longitude) {
    var posVietNam = { lat: 16.4498, lng: 107.5624 };
    var zoomSize = 5;
    if (latitude && longitude) {
        posVietNam = { lat: parseFloat(latitude), lng: parseFloat(longitude) };
        zoomSize = 18;
    }
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: zoomSize,
        center: posVietNam
    });
    marker = new google.maps.Marker({
        map: map,
        draggable: true,
        animation: google.maps.Animation.DROP,
        position: posVietNam
    });
    google.maps.event.addListener(marker, 'dragend', function (event) {
        document.getElementById("Latitude").value = this.getPosition().lat();
        document.getElementById("Longitude").value = this.getPosition().lng();
    });
    //autocomplete = new google.maps.places.Autocomplete(
    //    (document.getElementById('address')), { types: ['geocode'] });
    var input = document.getElementById('Address');
    autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.addListener('place_changed', fillInAddress);
}

//init map
function initMap() {
    var posVietNam = { lat: 16.4498, lng: 107.5624 };
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 5,
        center: posVietNam
    });
    var image = {
        url: "/assets/client/img/gmap_marker.png",
        anchor: new google.maps.Point(25, 25),
        scaledSize: new google.maps.Size(25, 25)
    };
    marker = new google.maps.Marker({
        map: map,
        draggable: true,
        animation: google.maps.Animation.DROP,
        position: posVietNam,
        //icon: image
    });
    google.maps.event.addListener(marker, 'dragend', function (event) {
        document.getElementById("Latitude").value = this.getPosition().lat();
        document.getElementById("Longitude").value = this.getPosition().lng();
    });
    //autocomplete = new google.maps.places.Autocomplete((document.getElementById('address')), {types: ['geocode'] });
    var input = document.getElementById('Address');
    autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.addListener('place_changed', fillInAddress);
}

function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();
    document.getElementById("LatiLongTude").value = place.geometry.location.lat() + "," + place.geometry.location.lng();
    var pos = { lat: place.geometry.location.lat(), lng: place.geometry.location.lng() };
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 18,
        center: pos
    }); var image = {
        url: "/assets/client/img/gmap_marker_active.png",
        anchor: new google.maps.Point(25, 25),
        scaledSize: new google.maps.Size(45, 45)
    };
    marker = new google.maps.Marker({
        map: map,
        draggable: true,
        animation: google.maps.Animation.DROP,
        position: pos,
        //icon: image
    });
    google.maps.event.addListener(marker, 'dragend', function (event) {
        document.getElementById("LatiLongTude").value = this.getPosition().lat() + "," + this.getPosition().lng();
    });
}

function getLatitudeLongitude(callback, address) {
    address = address || 'Ferrol, Galicia, Spain';
    geocoder = new google.maps.Geocoder();
    if (geocoder) {
        geocoder.geocode({
            'address': address
        }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                callback(results[0]);
            }
        });
    }
}

function eventAfterLoadAttributes() {
    $('#txtPrice').keypress(function (event) {
        var $this = $(this);
        if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
           ((event.which < 48 || event.which > 57) &&
           (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();
        if ((event.which == 46) && (text.indexOf('.') == -1)) {
            setTimeout(function () {
                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                }
            }, 1);
        }

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });

    $("#txtPrice").on('input', function () {
        setTextMoneyAllowDecimal($(this));
    });
}

function setTextMoneyAllowDecimal(elementObject) {
    var trueValue;
    if (elementObject == null || elementObject.val() == "") trueValue = 0;
    else trueValue = elementObject.val().replace(/,/g, '');

    $('#hidXPrice').val(parseFloat(trueValue));
    $('#hidXPriceTemp').val(parseFloat(trueValue));
    var textMoney;
    var displayValue;
    var displayValueInput = trueValue;
    if (parseFloat(trueValue) < 1000) {
        // ok
        textMoney = parseFloat(trueValue) + " Triệu";
    } else {
        var pricePrecision = trueValue.toString().split('.')[0];
        var priceScale = trueValue.toString().split('.')[1];
        displayValue = (parseFloat(pricePrecision) / 1000).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        if (typeof priceScale === "undefined") {
            // ok
            textMoney = displayValue + " Tỷ";
            displayValueInput = pricePrecision.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        } else {
            textMoney = displayValue + '.000' + priceScale + " Tỷ";
            displayValueInput = pricePrecision.replace(/\B(?=(\d{3})+(?!\d))/g, ",") + '.' + priceScale;
        }
    }

    var divPrivatePrice = $('#ck_13845').closest('.row').closest('.col-lg-6');
    if ($('.dividePricePrivate').length == 0) {
        $(divPrivatePrice).before('<div class="clearfix dividePricePrivate"></div>');
        $('.dividePricePrivate').before($('.money-text').remove().clone());
    }
    $('#txtTotalPrice').val('');
    $('#txtTotalPrice').val(textMoney);
    if ($(elementObject).val() == 0 || $(elementObject).val() == "") {
        $('#txtTotalPrice').val("Thỏa thuận");
    }
    $('#hidXPriceDisplay').val(textMoney);

    if (displayValueInput != null && displayValueInput.length > 1) {
        displayValueInput = displayValueInput.replace(/^0+/, '');
    }
    elementObject.val(displayValueInput);
}

function setTextMoney(elementObject) {
    $(elementObject).parseNumber({ format: "#,###", locale: "vn" });
    $(elementObject).formatNumber({ format: "#,###", locale: "vn" });
    $('#hidXPrice').val($(elementObject).parseNumber({ format: "#,###.00", locale: "vn" }, false));
    $('#hidXPriceTemp').val($(elementObject).val());
    var divPrivatePrice = $('#ck_13845').closest('.row').closest('.col-lg-6');
    if ($('.dividePricePrivate').length == 0) {
        $(divPrivatePrice).before('<div class="clearfix dividePricePrivate"></div>');
        $('.dividePricePrivate').before($('.money-text').remove().clone());
    }

    $('#txtTotalPrice').text('');
    var textMoney;
    var trueValue;
    var displayValue;
    displayValue = convertPointNumber($(elementObject).val().replace(/[^a-z0-9]/gi, ','));
    trueValue = $(elementObject).parseNumber({ format: "#,###.00", locale: "vn" }, false);

    if (trueValue < 1000) {
        textMoney = displayValue + " Triệu";
    } else {
        textMoney = trueValue / 1000 + " Tỷ";
    }

    $('#txtTotalPrice').text(textMoney);
    if ($(elementObject).val() == 0) {
        $('#txtTotalPrice').text("Thỏa thuận");
    }
    $('#hidXPriceDisplay').val(textMoney);
}

function formatNumber(yourNumber) {
    if (yourNumber) {
        var components = yourNumber.toString().split(".");
        components[0] = components[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return components.join(".");
    } else {
        return "0";
    }
}
function numberReplaceCommas(x) {
    return x.toString().replace(/\,/g, '');
}

/**
 * Format Money
 * Number.prototype.format(n, x, s, c)
 * 
 * @param integer n: length of decimal
 * @param integer x: length of whole part
 * @param mixed   s: sections delimiter
 * @param mixed   c: decimal delimiter
 */
Number.prototype.format = function (n, x, s, c) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~~n));

    return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
};
function convertPointNumber(n) { var t, i; return (n.match(/,/g) || []).length == 1 && (t = n.split(",")[1], n = t.length > 2 ? n.split(",")[0] + "," + t.slice(0, -1) : n.split(",")[0] + "," + t), (n.match(/,/g) || []).length > 1 && (i = n.slice(-1), i == "," && (n = n.slice(0, -1))), n }

