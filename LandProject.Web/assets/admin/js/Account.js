var _idgrid = "#grid";
var AccountModel = {
    Id:'',
    UserName: '',
    FullName : '',
    Email : '',
    Address : '',
    PhoneNumber: '',
    EmailConfirmed : '',
    TotalLands: '',
}
$(document).ready(function () {
    LoadGrid();
})
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
            template: '#=data.UserName#',
            field: "UserName",
            title: "Tài khoản",
            filterable: false,
        },
        {
            template: '#if(data.StrGroup != null){##=data.StrGroup##}else{#User#}#',
            field: "StrGroup",
            title: "Nhóm TK",
            filterable: false,
        },
        {
            template: '#=data.Email#',
            field: "Email",
            title: "Email",
            filterable: false,
        },
        {
            template: '#=data.FullName#',
            field: "FullName",
            title: "Họ và Tên",
            filterable: false,
        },
        {
            template: '#if(data.Address != null){##=data.Address##}#',
            field: "Address",
            title: "Địa chỉ",
            filterable: false,
        },
        {
            template: '#if(data.PhoneNumber != null){##=data.PhoneNumber##}#',
            field: "PhoneNumber",
            title: "Số điện thoại",
            filterable: false,
        },
        {
            template: '#=data.TotalLands#',
            field: "TotalLands",
            title: "Số bài tin đăng",
            filterable: false,
        },
        {
            template: '#if(data.EmailConfirmed == true){#<span style="color:green">Đã kích hoạt</span>#} else if(data.EmailConfirmed == false){#<span style="color:red">Chưa kích hoạt</span>#}#',
            field: "EmailConfirmed",
            title: "Trạng thái",
            filterable: false,
        },

];
LoadGrid = function () {
    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/applicationUser/getlistpaging"), null, false, '')
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
        html += '<ul class="dropdown-menu dropdown-white dropdown-menu-right" style="height:50px;">';
        html += '<li>';
        html += '<a href="/Admin/ApplicationUser/AccountLandNews?userId=' + e.Id + '" target="_blank">Xem danh sách tin đăng</a>';
        html += '</li>';
        html += '</ul></div>';
    }
    return html;
}
