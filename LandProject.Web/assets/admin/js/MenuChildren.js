var _idgrid = "#grid";
var MenuChildModel = {
    ID: '',
    Name: '',
    Alias: '',
    URL: '',
    DisplayOrder: '',
    ParentID:'',
    MenuGroupID:'',
    Target: '',
    Status: true
}
var TypeActionAdd = true;
$(document).ready(function () {
    LoadGrid();
    $('body').on('click', '#form-create-menuChild', function () {
        $('#form').validationEngine('hide');
        TypeActionAdd = true;

        var urlMenuGroup = "api/menugroup/getall";
        var element = "#cboMenuGroup";
        LoadComboBoxWithServices(element, urlMenuGroup, null, "ID", "Name", null, "Chọn danh mục cha", false, null, function () { }, null);

        var urlMenuGroup = "api/menu/getparent";
        var element = "#cboMenuParent";
        LoadComboBoxWithServices(element, urlMenuGroup, null, "ID", "Name", null, "Chọn danh mục đa cấp", false, null, function () { }, null);

        $('#txtMenuChildName').val('');
        $('#txtMenuChildDisplayOrder').val();
        $('#txtMenuChildTarget').val();
        $('#txtMenuChildURL').val();
        $("#MenuChildModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
    })
    $('body').on('click', '#saveMenuChild', function () {
        if (checkValid()) {
            MenuChildModel.Name = $('#txtMenuChildName').val();
            MenuChildModel.Alias = new commonService().getSeoTitle($('#txtMenuChildName').val());
            MenuChildModel.DisplayOrder = $('#txtMenuChildDisplayOrder').val();
            MenuChildModel.Target = $('#txtMenuChildTarget').val();
            MenuChildModel.URL = $('#txtMenuChildURL').val();
            MenuChildModel.MenuGroupID = $("#cboMenuGroup").val();
            MenuChildModel.ParentID = $("#cboMenuParent").val();
            if (TypeActionAdd) {//add          
                var svr = new AjaxCall("api/menu/create", JSON.stringify(MenuChildModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != null) {
                        $("#MenuChildModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            } else {//update
                var svr = new AjaxCall("api/menu/update", JSON.stringify(MenuChildModel));
                svr.callServicePOST(function (data) {
                    console.log(data)
                    if (data != null) {
                        $("#MenuChildModel").modal('hide');
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            }
        }
    })
    $('body').on('click', '#closeMenuChild', function () {
        $("#MenuChildModel").modal('hide');
    })
})

checkValid = function () {
    var res = $("#form").validationEngine('validate');;
    setTimeout(function () {
        $('#form').validationEngine('hide');
    }, 10000);
    var MenuChildName = $('#txtMenuChildName').val();
    if (MenuChildName.trim() == "") {
        $('#txtMenuChildName').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#txtMenuChildName").validationEngine('hide');
    }
    if ($("#cboMenuGroup").data("kendoComboBox").selectedIndex == -1) {
        $('#cboMenuGroup').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#cboMenuGroup").validationEngine('hide');
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
            template: '#if(data.Status == true){#<span style="color:green">Đã kích hoạt</span>#} else if(data.Status == false){#<span style="color:orange">Chưa kích hoạt</span>#}#',
            field: "Status",
            title: "Trạng thái",
        },
        {
            template: '#=data.Name#',
            field: "Name",
            title: "Tên danh mục",
        },
        {
            template: '#=data.URL#',
            field: "URL",
            title: "Đường dẫn liên kết",
        },
        {
            template: '#if(data.DisplayOrder != null){##=data.DisplayOrder##}#',
            field: "Description",
            title: "Thứ tự hiển thị",
            filterable: false,
        },
        {
            template: '#=data.MenuGroupName#',
            field: "MenuGroupID",
            title: "Thuộc danh mục cha",
            filterable: {
                ui: menuGroupFilter
            }
        },

];
LoadGrid = function () {
    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/menu/getalltable"), null, false, '')
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
        if (e.Status) {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').Active()">Hiện danh mục</a>';
            html += '</li>';
        } else {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').UnActive()">Ẩn danh mục</a>';
            html += '</li>';
        }
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
        $("#MenuChildModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        var param = {
            menuID: id
        }
        var svr = new AjaxCall("api/menu/getbyid", param);
        svr.callServiceGET(function (data) {
            console.log(data)
            if (data != undefined) {
                var urlMenuGroup = "api/menugroup/getall";
                var element = "#cboMenuGroup";
                LoadComboBoxWithServices(element, urlMenuGroup, null, "ID", "Name",  data.MenuGroupID, "Chọn danh mục cha", false, null, function () { }, null);

                var urlMenuGroup = "api/menu/getparent";
                var element = "#cboMenuParent";
                LoadComboBoxWithServices(element, urlMenuGroup, null, "ID", "Name", data.MenuGroupID, "Chọn danh mục đa cấp", false, null, function () { }, null);

                $('#txtMenuChildName').val(data.Name);
                $('#txtMenuChildDisplayOrder').val(data.DisplayOrder);
                $('#txtMenuChildTarget').val(data.Target);
                $('#txtMenuChildURL').val(data.URL);
                MenuChildModel.ID = data.ID;
                MenuChildModel.Status = data.Status
            }
        });
    }
    this.Active = function () {
        TypeActionAdd = false;
        var param = {
            menuID: id
        }
        var svr = new AjaxCall("api/menu/getbyid", param);
        svr.callServiceGET(function (data) {
            if (data != undefined) {
                MenuChildModel.Name = data.Name
                MenuChildModel.Alias = data.Alias
                MenuChildModel.DisplayOrder = data.DisplayOrder;
                MenuChildModel.Target = data.Target;
                MenuChildModel.URL = data.URL;
                MenuChildModel.ID = data.ID;
                MenuChildModel.Status = true;
                MenuChildModel.MenuGroupID = data.MenuGroupID;
                MenuChildModel.ParentID = data.ParentID;
                var svr1 = new AjaxCall("api/menu/update", JSON.stringify(MenuChildModel));
                svr1.callServicePOST(function (data) {
                    if (data != undefined) {
                        $("#model-notify").modal('hide');
                        swal({
                            title: "Thông báo",
                            text: "Hiện danh mục thành công",
                            confirmButtonColor: "#EF5350",
                            type: "success"
                        }, function () { $("#model-notify").modal('show'); });
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            }
        });

    }
    this.UnActive = function () {
        TypeActionAdd = false;
        var param = {
            menuID: id
        }
        var svr = new AjaxCall("api/menu/getbyid", param);
        svr.callServiceGET(function (data) {
            if (data != undefined) {
                MenuChildModel.Name = data.Name
                MenuChildModel.Alias = data.Alias
                MenuChildModel.DisplayOrder = data.DisplayOrder;
                MenuChildModel.Target = data.Target;
                MenuChildModel.URL = data.URL;
                MenuChildModel.ID = data.ID;
                MenuChildModel.MenuGroupID = data.MenuGroupID;
                MenuChildModel.ParentID = data.ParentID;
                MenuChildModel.Status = false;
                var svr1 = new AjaxCall("api/menu/update", JSON.stringify(MenuChildModel));
                svr1.callServicePOST(function (data) {
                    if (data != undefined) {
                        $("#model-notify").modal('hide');
                        swal({
                            title: "Thông báo",
                            text: "Ẩn danh mục thành công",
                            confirmButtonColor: "#EF5350",
                            type: "success"
                        }, function () { $("#model-notify").modal('show'); });
                        $('#grid').data('kendoGrid').dataSource.read();
                        $('#grid').data('kendoGrid').refresh();
                    }
                });
            }
        });

    }
    this.Delete = function (title) {
        bootbox.confirm({
            message: "Bạn có chắc muốn xóa danh mục này",
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
                        menuID: id
                    }
                    var svr1 = new AjaxCall("api/menu/delete", JSON.stringify(param));
                    svr1.callServicePOST(function (data) {
                        if (data != undefined) {
                            $("#model-notify").modal('hide');
                            swal({
                                title: "Thông báo",
                                text: "Xóa danh mục thành công",
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

function menuGroupFilter(element) {
    element.kendoDropDownList({
        dataSource: lstMenuGroup,
        dataTextField: "Name",
        dataValueField: "ID",
        optionLabel: "--Chọn danh mục cha--"
    });
}
var lstMenuGroup = new kendo.data.DataSource({
    transport: {
        read: function (options) {
            $.ajax({
                url: _Host + "api/menugroup/getall",
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