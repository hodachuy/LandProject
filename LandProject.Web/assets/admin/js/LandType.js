﻿var _idgrid = "#grid";
var LandTypeModel = {
    ID:'',
    Name: '',
    Alias: '',
    TypeExchange:'',
    IsDelete:false,
    SortOrder:'',
}
var TypeActionAdd = true;
$(document).ready(function () {
    LoadGrid();
    $('body').on('click', '#form-create-landType', function () {
        $('#form').validationEngine('hide');
        TypeActionAdd = true;
        $('#txtLandTypeName').val('');
        $('#typeExchange').val('');
        $('#txtLandTypeSortOrder').val('0');
        $("#LandTypeModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
    })
    $('body').on('click', '#saveLandType', function () {
        if (checkValid()) {
            LandTypeModel.Name = $('#txtLandTypeName').val();
            LandTypeModel.Alias = new commonService().getSeoTitle($('#txtLandTypeName').val());
            LandTypeModel.IsDelete = false;
            LandTypeModel.TypeExchange = $('#typeExchange').val();
            LandTypeModel.SortOrder = $('#txtLandTypeSortOrder').val();
            if (TypeActionAdd) {//add     
                LandTypeModel.ID = 0;
                var svr = new AjaxCall("api/landtype/create", JSON.stringify(LandTypeModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != null) {
                        $("#LandTypeModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            } else {//update
                var svr = new AjaxCall("api/landtype/update", JSON.stringify(LandTypeModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != null) {
                        $("#LandTypeModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            }
        }
    })
    $('body').on('click', '#closeLandType', function () {
        $("#LandTypeModel").modal('hide');
    })
})

checkValid = function () {
    var res = $("#form").validationEngine('validate');;
    setTimeout(function () {
        $('#form').validationEngine('hide');
    }, 10000);
    var landTypeName = $('#txtLandTypeName').val();
    if (landTypeName.trim() == "") {
        $('#txtLandTypeName').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#txtLandTypeName").validationEngine('hide');
    }

    if ($('#typeExchange').val() != "") {
        $("#typeExchange").validationEngine('hide');
    } else {
        $('#typeExchange').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    }
    return res;
}

DataSource = function () {
    this.MasterDatasource = function (url) {
        var total = 0;
        var data = new kendo.data.DataSource({
            transport: {
                read: function (options) {
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
                    if (data != null) {
                        if(data.Items != null)
                            return data.Items;
                    }
                    return [];
                },
                total: function (data) {
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
            template: '#=data.Name#',
            field: "Name",
            title: "Thể loại",
            filterable: false,
        },
        {
            template: '#if(data.Description != null){##=data.Description##}#',
            field: "Description",
            title: "Mô tả",
            filterable: false,
        },
        {
            template: '#:getUrl(data.ID,data.Alias)#',
            field: "Name",
            title: "Đường dẫn",
        },
        {
            template: '#if(data.TypeExchange == 1){#Bán#} else if(data.TypeExchange == 2){#Cho thuê#}#',
            field: "TypeExchange",
            title: "Loại hình",
            filterable: false,
        },
        {
            template: '#=data.SortOrder#',
            field: "SortOrder",
            title: "Thứ tự",
            filterable: false,
        },

];
LoadGrid = function () {
    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/landtype/getalltable"), null, false, '')
}
function getUrl(id, alias) {
    if (id == null) return "";
    else return kendo.toString("/nha-dat/" + alias + ".lt-" + id + ".html");
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
        html += '<li>';
        html += '<a href="javascript:new ForumCatg(' + e.ID + ').Edit()">Chỉnh sửa</a>';
        html += '</li>';
        html += '<li>';
        html += '<a href="javascript:new ForumCatg(' + e.ID + ').View(\'' + e.Alias + '\')">Xem trước</a>';
        html += '</li>';
        html += '<li>';
        html += '<a href="javascript:new ForumCatg(' + e.ID + ').Delete(\'' + e.Name + '\')">Xóa</a>';
        html += '</li>';
        //if (permission.IsUpdate) {
        //    html += '<li>';
        //    html += '<a href="javascript:new ForumCatg(' + e.DocTypeID + ').Edit()">Chỉnh sửa</a>';
        //    html += '</li>';
        //}
        //if (permission.IsDelete && e.IsDelete == "1") {
        //    html += '<li>';
        //    html += '<a href="javascript:new ForumCatg(' + e.DocTypeID + ').Delete(\'' + e.DocName + '\')">Xóa</a>';
        //    html += '</li>';
        //}
        html += '</ul></div>';
    }
    return html;
}
ForumCatg = function (id) {
    this.View = function (alias) {
        var url = _Host + "nha-dat/" + alias + ".lt-" + id + ".html";
        window.open(url, '_blank');
    }
    this.Edit = function () {
        TypeActionAdd = false;
        $("#LandTypeModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        var param = {
            lTypeId: id
        }
        var svr = new AjaxCall("api/landtype/getbyid", param);
        svr.callServiceGET(function (data) {
            console.log(data)
            if (data != undefined) {
                $('#txtLandTypeName').val(data.Name);
                $('#typeExchange').val(data.TypeExchange)
                $('#txtLandTypeSortOrder').val(data.SortOrder);
                LandTypeModel.ID = data.ID;
            }
        });

    }
    this.Delete = function (title) {
        ConfirmDialog("Xác nhận thông tin", "Bạn muốn xóa thể loại '" + title + "' ?", function (e) {
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