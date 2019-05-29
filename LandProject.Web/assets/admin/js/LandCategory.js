var _idgrid = "#grid";
var LandCategoryModel = {
    ID:'',
    Name: '',
    Alias: '',
    LandTypeID:'',
    IsDelete:false
}
var param = null;
var TypeActionAdd = true;
$(document).ready(function () {
    LoadGrid();
    $('body').on('click', '#form-create-landCategory', function () {
        TypeActionAdd = true;
        var urlLandType = "api/landtype/getall";
        var element = "#cboLandType";
        LoadComboBoxWithServices(element, urlLandType, param, "ID", "Name", null, "Chọn Thể Loại", false, null, function () { }, null);
        $('#txtLandCategoryName').val('');
        $("#LandCategoryModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
    })
    $('body').on('click', '#saveLandCategory', function () {
        if (checkValid()) {
            LandCategoryModel.ID = 0;
            LandCategoryModel.Name = $('#txtLandCategoryName').val();
            LandCategoryModel.Alias = new commonService().getSeoTitle($('#txtLandCategoryName').val());
            LandCategoryModel.IsDelete = false;
            LandCategoryModel.LandTypeID = $("#cboLandType").val();
            if (TypeActionAdd) {//add          
                var svr = new AjaxCall("api/landcategory/create", JSON.stringify(LandCategoryModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != null) {
                        $("#LandCategoryModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            } else {//update
                var svr = new AjaxCall("api/landcategory/update", JSON.stringify(LandCategoryModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != null) {
                        $("#LandCategoryModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            }
        }
    })
    $('body').on('click', '#closeLandCategory', function () {
        $("#LandCategoryModel").modal('hide');
    })
})

checkValid = function () {
    var res = $("#form").validationEngine('validate');
    setTimeout(function () {
        $('#form').validationEngine('hide');
    }, 10000);
    var landCategoryName = $('#txtLandCategoryName').val();
    if (landCategoryName.trim() == "") {
        $('#txtLandCategoryName').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
            $("#txtLandCategoryName").validationEngine('hide');
    }
    if ($("#cboLandType").data("kendoComboBox").selectedIndex == -1) {
        $('#cboLandType').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#cboLandType").validationEngine('hide');
    }
    return res;
}

DataSource = function () {
    this.MasterDatasource = function (url) {
        var total = 0;
        var data = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    console.log(options);
                    console.log(options.data);
                    var request = JSON.stringify(options.data);
                    $.ajax({
                        url: url,
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: request,
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
                            //$("body").append('<div class="wt-waiting wt-fixed wt-large"></div>');
                        },
                        complete: function (e) {
                            //$(".wt-waiting").remove();
                            $(block).unblock();
                        },
                        success: function (result) {
                            console.log(result)
                            if (result != null) {
                                if (result.Items != null) {
                                    options.success(result);
                                }
                            }
                            else {
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
                    console.log(data)
                    if (data != null) {
                        if(data.Items != null)
                            return data.Items;
                    }
                    return [];
                },
                total: function (data) {
                    console.log(data)
                    if (data != null) {
                        if (data.Items != null)
                            return data.TotalCount;
                    }
                    return 0;
                },
            },
            pageSize: 20,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        });
        return data;
    }
}
var Columns = [
        {
            title: "Stt",
            template: '#= record++ #',
            filterable: false,
            width: 50,
            attributes: { style: "text-align: center" },
            headerAttributes: { style: "text-align: center" },
        },
        {
            template: templateForAction,
            filterable: false,
            width: 100,
            title: "Tiện ích",
            attributes: { style: "text-align: center; overflow : visible; cursor: pointer", },
            headerAttributes: { style: "text-align: center" },
        },
                {
            template: '#=data.ID#',
            field: "ID",
            title: "ID",
        },
        {
            template: '#=data.Name#',
            field: "Name",
            title: "Phân mục",
        },
        {
            template: '#if(data.Description != null){##=data.Description##}#',
            field: "Description",
            title: "Mô tả",
            filterable: false,
        },
        {
            template: '#=data.LandType.Name#',
            field: "LandTypeID",
            title: "Thể loại",
            filterable: {
                ui: landTypeFilter
            }
        },

];
LoadGrid = function () {
    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/landcategory/getalltable"), null, false, '')
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
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').Edit()">Chỉnh sửa</a>';
            html += '</li>';
        }
        if (permission.IsDelete && e.IsDelete == "1") {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').Delete(\'' + e.Name + '\')">Xóa</a>';
            html += '</li>';
        }
        html += '</ul></div>';
    }
    return html;
}
ForumCatg = function (id) {
    this.Edit = function () {
        TypeActionAdd = false;
        $("#LandCategoryModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        var param = {
            landCategoryID: id
        }
        var svr = new AjaxCall("api/landcategory/getbyid", param);
        svr.callServiceGET(function (data) {
            console.log(data)
            if (data != undefined) {
                $('#txtLandCategoryName').val(data.Name);
                LandCategoryModel.ID = data.ID;
                var urlLandType = "api/landtype/getall";
                var element = "#cboLandType";
                LoadComboBoxWithServices(element, urlLandType, param, "ID", "Name", data.LandTypeID, "Chọn Thể Loại", false, null, function () { }, null);
            }
        });
    }
    this.Delete = function (title) {
        ConfirmDialog("Xác nhận thông tin", "Bạn muốn xóa loại văn bản '" + title + "' ?", function (e) {
            //if (result) {
            //Logic to delete the item
            var url = _Host + "Ajax/DeleteDocumentType";
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                data:
                {
                    DocTypeID: id
                },
                success: function (result) {
                    //console.log(result);
                    if (result) {
                        switch (result.Table[0].ReturnCode) {
                            case "0":
                                AlertDialog("Kết quả thao tác", "Xóa thành công", function () {
                                    $('#grid').data('kendoGrid').dataSource.read();
                                    $('#grid').data('kendoGrid').refresh();
                                });
                                break;
                            case "2":
                                AlertDialog("Thông báo", "Bạn không có quyền xóa loại văn bản", function () {
                                });
                                break;
                        }
                    }
                },
                error: function (result) {
                }
            })
        });
    }
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
                    }
                    else {
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