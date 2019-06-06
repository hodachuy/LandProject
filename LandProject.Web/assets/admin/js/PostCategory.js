var _idgrid = "#grid";
var PostCategoryModel = {
    ID: '',
    Name: '',
    Alias: '',
    IsDelete: false,
    DisplayOrder: '',
}
var TypeActionAdd = true;
$(document).ready(function () {
    LoadGrid();

    $('body').on('click', '#form-create-postcategory', function () {
        $('#form').validationEngine('hide');
        TypeActionAdd = true;
        $('#txtPostCategoryName').val('');
        $('#txtPostCategorySortOrder').val('0');
        $("#PostCategoryModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
    })
    $('body').on('click', '#savePostCategory', function () { 
        if (checkValid()) {
            PostCategoryModel.Name = $('#txtPostCategoryName').val();
            PostCategoryModel.Alias = new commonService().getSeoTitle($('#txtPostCategoryName').val());
            PostCategoryModel.IsDelete = false;
            PostCategoryModel.DisplayOrder = $('#txtPostCategorySortOrder').val();
            if (TypeActionAdd) {//add      
                PostCategoryModel.ID = 0;
                var svr = new AjaxCall("api/postcategory/create", JSON.stringify(PostCategoryModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != null) {
                        $("#PostCategoryModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            } else {//update
                var svr = new AjaxCall("api/postcategory/update", JSON.stringify(PostCategoryModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != null) {
                        $("#PostCategoryModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            }
        }
    })
    $('body').on('click', '#closePostCategory', function () {
        $("#PostCategoryModel").modal('hide');
    })
})

checkValid = function () {
    var res = $("#form").validationEngine('validate');;
    setTimeout(function () {
        $('#form').validationEngine('hide');
    }, 10000);
    var postCategoryName = $('#txtPostCategoryName').val();
    if (postCategoryName.trim() == "") {
        $('#txtPostCategoryName').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#txtPostCategoryName").validationEngine('hide');
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
                        if (data.Items != null)
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
            template: '#=data.ID#',
            field: "ID",
            title: "ID",
        },
        {
            template: '#=data.Name#',
            field: "Name",
            title: "Thể loại",
        },
        {
            template: '#=data.DisplayOrder#',
            field: "DisplayOrder",
            title: "Thứ tự",
        },

];
LoadGrid = function () {
    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/postcategory/getalltable"), null, false, '')
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
    this.Edit = function () {
        TypeActionAdd = false;
        $("#PostCategoryModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        var param = {
            postCategoryID: id
        }
        var svr = new AjaxCall("api/postcategory/getbyid", param);
        svr.callServiceGET(function (data) {
            console.log(data)
            if (data != undefined) {
                $('#txtPostCategoryName').val(data.Name);
                $('#txtPostCategorySortOrder').val(data.DisplayOrder);
                PostCategoryModel.ID = data.ID;
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