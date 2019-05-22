var _idgrid = "#grid";
var LandNewsModel = {
    ID: '',
    Name: '',
    Alias: '',
    LandTypeID: '',
    IsDelete: false
}
var param = null;
var TypeActionAdd = true;

$(document).ready(function () {
    LoadGrid();
    //$('body').on('click', '#form-create-landNews', function () {
    //    TypeActionAdd = true;
    //    var urlLandType = "api/landtype/getall";
    //    var element = "#cboLandType";
    //    LoadComboBoxWithServices(element, urlLandType, param, "ID", "Name", null, "Chọn Thể Loại", false, null, function () { }, null);
    //    $('#txtLandNewsName').val('');
    //    $("#LandNewsModel").modal({
    //        backdrop: 'static',
    //        keyboard: true,
    //        show: true
    //    });
    //})
    //$('body').on('click', '#saveLandNews', function () {
    //    if (checkValid()) {
    //        LandNewsModel.Name = $('#txtLandNewsName').val();
    //        LandNewsModel.Alias = new commonService().getSeoTitle($('#txtLandNewsName').val());
    //        LandNewsModel.IsDelete = false;
    //        LandNewsModel.LandTypeID = $("#cboLandType").val();
    //        if (TypeActionAdd) { //add          
    //            var svr = new AjaxCall("api/landnews/create", JSON.stringify(LandNewsModel));
    //            svr.callServicePOST(function (data) {
    //                //console.log(data)
    //                if (data != null) {
    //                    $("#LandNewsModel").modal('hide');
    //                    $('#grid').data('kendoGrid').dataSource.read();
    //                    $('#grid').data('kendoGrid').refresh();
    //                }
    //            });
    //        } else { //update
    //            var svr = new AjaxCall("api/landnews/update", JSON.stringify(LandNewsModel));
    //            svr.callServicePOST(function (data) {
    //                //console.log(data)
    //                if (data != null) {
    //                    $("#LandNewsModel").modal('hide');
    //                    $('#grid').data('kendoGrid').dataSource.read();
    //                    $('#grid').data('kendoGrid').refresh();
    //                }
    //            });
    //        }
    //    }
    //})
    //$('body').on('click', '#closeLandNews', function () {
    //    $("#LandNewsModel").modal('hide');
    //})
})


checkValid = function () {
    var res = $("#form").validationEngine('validate');
    setTimeout(function () {
        $('#form').validationEngine('hide');
    }, 10000);
    var LandNewsName = $('#txtLandNewsName').val();
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
                    form.append('requestFilter',JSON.stringify(options.data))
                    form.append('lTypeID', JSON.stringify($("#landTypeID").val()))
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
    console.log(e)
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
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').Edit(\'' + e.LandTypeName + '\')">Xem/Chỉnh sửa</a>';
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
        //if (permission.IsDelete && e.IsDelete == "1") {
        //    html += '<li>';
        //    html += '<a href="javascript:new ForumCatg(' + e.ID + ').Delete(\'' + e.Name + '\')">Xóa</a>';
        //    html += '</li>';
        //}
        html += '</ul></div>';
    }
    return html;
}
ForumCatg = function (id) {
    console.log(id)
    this.Edit = function (landTypeName) {
        var form = new FormData();
        TypeActionAdd = false;
        $("#lTypeTitle").empty().html(landTypeName)
        $("#LandNewsModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        //initMap();
        
        var param = {
            LandNewsID: id
        }
        var svr = new AjaxCall("api/landnews/getbyid", param);
        svr.callServiceGET(function (data) {
            console.log(data)
            if (data != undefined) {
                LandNewsModel.ID = data.ID;
                $('#txtLandNewsName').val(data.Name);
                var urlLandType = "api/landtype/getall";
                var element = "#cboLandType";
                LoadComboBoxWithServices(element, urlLandType, param, "ID", "Name", data.LandTypeID, "Chọn Thể Loại", false, null, function () { }, null);

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
    this.Delete = function (title) {
        ConfirmDialog("Xác nhận thông tin", "Bạn muốn xóa loại văn bản '" + title + "' ?", function (e) {
            //if (result) {
            //Logic to delete the item
            var url = _Host + "Ajax/DeleteDocumentType";
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                data: {
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
                                AlertDialog("Thông báo", "Bạn không có quyền xóa loại văn bản", function () { });
                                break;
                        }
                    }
                },
                error: function (result) { }
            })
        });
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
    document.getElementById("Latitude").value = place.geometry.location.lat();
    document.getElementById("Longitude").value = place.geometry.location.lng();
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
        document.getElementById("Latitude").value = this.getPosition().lat();
        document.getElementById("Longitude").value = this.getPosition().lng();
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