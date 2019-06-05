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
        ID: '',
        Phone: '',
        Address: '',
        Mobile: '',
        Name: '',
        UserId:''
    }
};


var param = null;
var TypeActionAdd = true;
$(document).ready(function () {
    LoadGrid();

    $('body').on('click', '#saveLandNews', function () {
        if (checkValid()) {
            LandNewsModel.Title = $('#txtTitle').val();
            LandNewsModel.Alias = new commonService().getSeoTitle($('#txtTitle').val());
            LandNewsModel.Description = $('#txtDescription').val();
            LandNewsModel.Address = $('#txtAddress').val();
            LandNewsModel.LandTypeID = $('#cboLandType').val();
            LandNewsModel.LandCategoryID = $('#cboLandCategory').val();
            LandNewsModel.DistrictID = $('#cboDistrict').val();
            LandNewsModel.WardID = $('#cboWard').val();
            LandNewsModel.LProjectID = $('#cboLProject').val();
            LandNewsModel.Area = $('#txtArea').val();
            LandNewsModel.Price = $("#txtPrice").val();
            LandNewsModel.TotalPrice = $('#txtTotalPrice').val();
            LandNewsModel.DecimalTotalPrice = $('#txtDecimalTotalPrice').val();
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
            LandNewsModel.Agent.UserId = $('#txtUserId').val();


            if (TypeActionAdd) {//add          
                var svr = new AjaxCall("api/landtype/create", JSON.stringify(LandNewsModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != undefined) {
                        $("#LandNewsModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            } else {//update
                var svr = new AjaxCall("api/landtype/update", JSON.stringify(LandNewsModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != undefined) {
                        $("#LandNewsModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            }
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
            landNewsID :$('#landNewsID').val()
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
    var LandNewsName = $('#txtLandNewsName').val();
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
    if ($('#txtAddress').val().trim() == "") {
        $('#txtAddress').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#txtAddress").validationEngine('hide');
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
                    form.append('typeExchange', JSON.stringify($("#typeExchange").val()))
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

    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/landnews/getalltable"), null, false, '')
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
        $('#txtAddress').val('');
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

                if (LandNewsModel.LatiLongTude != undefined) {
                    var arrLatLong = LandNewsModel.LatiLongTude.split(',');
                    initMap(arrLatLong[0], arrLatLong[1]);
                }

                var url1 = "api/landtype/getall";
                var element = "#cboLandType";
                LoadComboBoxWithServices(element, url1, null, "ID", "Name", data.LandTypeID, "Chọn Hình thức", false, null, function () { }, null);
                var param1 = {
                    lTypeID: data.LandTypeID
                }
                var url2 = "api/landcategory/getall";
                var element = "#cboLandCategory";
                LoadComboBoxWithServices(element, url2,param1, "ID", "Name", data.LandCategoryID, "Chọn Thể Loại", false, null, function () { }, null);

                var url3 = "api/address/getprovince";
                var element = "#cboProvince";
                LoadComboBoxWithServices(element, url3, null, "ID", "Name", data.ProvinceID, "Chọn Tỉnh/Thành phố", false, null, function () { }, null);

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

                var url6 = "api/lproject/getall";
                var element = "#cboLProject";
                LoadComboBoxWithServices(element, url6, null, "ID", "Name", data.LProjectID, "Chọn Dự án", false, null, function () { }, null);


                $('#txtTitle').val(data.Title);
                $('#txtArea').val(data.Area);
                $('#txtUnit').val(data.Unit);
                $('#txtPrice').val(data.Price);
                $('#txtTotalPrice').val(data.TotalPrice);
                $('#txtAddress').val(data.Address);
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
                $('#txtDescription').val(data.Description);

                $('#txtConvenient').val(data.Convenient);
                $('#txtEnvironment').val(data.Environment);
                $('#txtLegalStatus').val(data.LegalStatus);

                if (data.Agent != undefined) {
                    LandNewsModel.AgentID = data.Agent.AgentID;
                    LandNewsModel.Agent.AgentID = data.Agent.AgentID;
                    $('#txtAgentName').val(data.Agent.Name);
                    $('#txtAgentAddress').val(data.Agent.Address);
                    $('#txtAgentPhone1').val(data.Agent.Phone1);
                    $('#txtAgentPhone2').val(data.Agent.Phone2);
                    $('#txtAgentPhone3').val(data.Agent.Phone3);
                    $('#txtAgentPhoneShow').val(data.Agent.PhoneShow);
                    $('#txtAgentEmail').val(data.Agent.Email);
                    $('#txtUserId').val(data.Agent.UserId);
                }

                var html = '';
                if (data.LandFiles.length != 0) {
                    $.each(data.LandFiles, function (index, value) {
                        html +='<div class="col-md-55">'
                        html +=                '<div class="thumbnail">';
                        html +=                     '<div class="image view view-first" data-id-file="' + value.ID + '" data-name-file="' + value.FileName + '">';
                        html +=                         '<img style="width: 100%; display: block;" src="' + _Host + 'fileman/Uploads/LandNews/' + value.FileName + '" alt="image" />';
                        html +=                             '<div class="mask">';
                        html +=                                 '<div class="tools tools-bottom">';
                        html +=                                     '<a href="#" data-id-file="' + value.ID + '" data-name-file="' + value.FileName + '" class="rmFileImage"><i class="fa fa-times"></i></a>';
                        html +=                                 '</div>';
                        html +=                             '</div>';
                        html +=                    '</div>';
                        html +=                '</div>';
                        html += '</div>';
                    })
                }
                $("#lst-file-image").append(html);

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
function initMap(latitude, longitude) {
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
    //var input = document.getElementById('address');
    //autocomplete = new google.maps.places.Autocomplete(input);
    //autocomplete.addListener('place_changed', fillInAddress);
}