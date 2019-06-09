var _idgrid = "#grid";
var SlideModel = {
    ID: '',
    Name: '',
    Description: '',
    Url: '',
    DisplayOrder: '',
    Image:'',
    Content:'',
    Status: false,
}
var TypeActionAdd = true;

var arrImageAsUrl = [];
var arrTotalImage = [];
var indexImg = 0;

$(document).ready(function () {
    LoadGrid();

    $('body').on('click', '#form-create-slide', function () {
        arrImageAsUrl = [];
        arrTotalImage = [];
        indexImg = 0;
        $('#lst-file-image').empty()
        $('#form').validationEngine('hide');
        TypeActionAdd = true;

        $('#txtTitle').val('');
        $('#txtDescription').val('');
        $('#txtURL').val('');
        $('#txtDisplayOrder').val('');

        $("#SlideModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
    })
    $('body').on('click', '#saveSlide', function () {
        if (checkValid()) {
            var formData = new FormData();

            SlideModel.Name = $('#txtTitle').val();
            SlideModel.Description = $('#txtDescription').val();
            SlideModel.Url = $('#txtURL').val();
            SlideModel.DisplayOrder = $('#txtDisplayOrder').val();
            if (TypeActionAdd) {//add      
                SlideModel.ID = 0;

                $.each(arrTotalImage, function (index, value) {
                    formData.append('file' + index, value.file);
                });

                formData.append('slide', JSON.stringify(SlideModel));

                $.ajax({
                    url: _Host + "api/slide/create",
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    dataType: "json",
                    success: function (result) {
                        if (result) {
                            $("#SlideModel").modal('hide');
                            $('#grid').data('kendoGrid').dataSource.read();
                            $('#grid').data('kendoGrid').refresh();
                        }
                    },
                    error: function (e) {
                        console.log(e)
                    }
                });

            } else {//update
                $.each(arrTotalImage, function (index, value) {
                    formData.append('file' + index, value.file);
                });

                formData.append('slide', JSON.stringify(SlideModel));

                $.ajax({
                    url: _Host + "api/slide/edit",
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    dataType: "json",
                    success: function (result) {
                        if (result) {
                            $("#SlideModel").modal('hide');
                            $('#grid').data('kendoGrid').dataSource.read();
                            $('#grid').data('kendoGrid').refresh();
                        }
                    },
                    error: function (e) {
                        console.log(e)
                    }
                });
            }
        }
    })
    $('body').on('click', '#closeSlide', function () {
        $("#SlideModel").modal('hide');
    })


    $('body').on("click", '#multi-file', function (e) { e.target.value = null; }); // sự kiện reset input file
    $('body').on('change', '#multi-file', function (event) {
        var files = event.target.files;
        var maxSize = 3072; // 3MB
        for (var i = 0; i < 1; i++) {
            indexImg++;
            var reader = new FileReader();
            if (files[i].type.indexOf("image") == 0) {
                reader.onload = (function (index) {
                    return function (e) {
                        var fileSize = (e.total / 1024).toFixed(0);
                        console.log(fileSize)
                        if (fileSize > maxSize) {
                            alert('Kích thước ảnh lớn hơn 3MB');
                            return;
                        }
                        var fileSrc = e.target.result;
                        var fileName = e.target.fileName;
                        var html = '<div class="col-md-8"><div class="thumbnail"><div class="image view view-first"  data-id="0" data-index="' + index + '"><img style="width: 100%; display: block;" src="' + fileSrc + '" alt="image"><div class="mask"><div class="tools tools-bottom"><a href="#"  data-id="0" data-index="' + index + '"  class="rmFileImage"><i class="fa fa-times"></i></a></div></div></div></div></div>';

                        $("#lst-file-image").empty().append(html);
                        var image = {
                            index: index,// parseInt(i),
                            file: e
                        }
                        arrImageAsUrl = [];
                        arrImageAsUrl.push(image);
                    };
                })(indexImg);// input index
            } else {
                toastr.error('Vui lòng chọn ảnh đúng định dạng (*.png | *.gif | *.jpg | *.jpeg)', null, { timeOut: 5000 });
            }
            reader.readAsDataURL(files[i]);
            var image = {
                index: indexImg,// parseInt(i+1),
                file: files[i]
            }
            arrTotalImage = [];
            arrTotalImage.push(image);
            //formData.append('file', files[i] )

        };
        console.log("arrimageAsUrl:" + arrImageAsUrl)
        console.log("arrimageFile:" + arrTotalImage)

    });
    $('body').on('click', '.rmFileImage', function () {
        var indexImage = $(this).attr('data-index');
        var idImage = $(this).attr('data-id');
        if (idImage != 0) {
            $(this).closest('.col-md-8').remove();
            SlideModel.Image = '';
        } else {
            for (var i = 0; i < arrImageAsUrl.length; i++) {
                if (arrImageAsUrl[i].index == indexImage) {
                    arrImageAsUrl.splice(i, 1);
                    arrTotalImage.splice(i, 1);
                    $(this).closest('.col-md-8').remove();
                    break;
                }
            }
            console.log("arrimageAsUrl:" + arrImageAsUrl)
            console.log("arrimageFile:" + arrTotalImage)
        }
    });

 
})

checkValid = function () {
    var res = $("#form").validationEngine('validate');;
    setTimeout(function () {
        $('#form').validationEngine('hide');
    }, 10000);
    var Name = $('#txtTitle').val();
    if (Name.trim() == "") {
        $('#txtTitle').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#txtTitle").validationEngine('hide');
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
            template:'#if(data.Status == true){#<span style="color:green">Đã hiển thị</span>#} else if(data.Status == false){#<span style="color:red">Không hiển thị</span>#}#',
            field: "Status",
            title: "Trạng thái",
            width: 100,
        },
        {
            template: '#=data.Name#',
            field: "Name",
            title: "Tiêu đề",
        },
        {
            template: '#=data.Url#',
            field: "Url",
            title: "Liên kết tới đường dẫn",
        },
        {
            template: '#=data.Description#',
            field: "Description",
            title: "Mô tả ngắn",
        },
];
LoadGrid = function () {
    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/slide/getalltable"), null, false, '')
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
        if (!e.Status) {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').Show()">Hiển thị</a>';
            html += '</li>';
        } else {
            html += '<li>';
            html += '<a href="javascript:new ForumCatg(' + e.ID + ').UnShow()">Gỡ hiển thị</a>';
            html += '</li>';
        }
        html += '<li>';
        html += '<a href="javascript:new ForumCatg(' + e.ID + ').Edit()">Chỉnh sửa</a>';
        html += '</li>';
        html += '<li>';
        html += '<a href="javascript:new ForumCatg(' + e.ID + ').Delete(\'' + e.Name + '\')">Xóa</a>';
        html += '</li>';
        html += '</ul></div>';
    }
    return html;
}
ForumCatg = function (id) {
    this.Edit = function () {
        arrImageAsUrl = [];
        arrTotalImage = [];
        indexImg = 0;
        $('#lst-file-image').empty()
        $('#txtTitle').val('');
        $('#txtDescription').val('');
        $('#txtURL').val('');
        $('#txtDisplayOrder').val('');

        TypeActionAdd = false;
        $("#SlideModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        var param = {
            slideID: id
        }
        var svr = new AjaxCall("api/slide/getbyid", param);
        svr.callServiceGET(function (data) {
            console.log(data)
            if (data != undefined) {
                $('#txtTitle').val(data.Name);
                $('#txtDescription').val(data.Description);
                $('#txtURL').val(data.Url);
                $('#txtDisplayOrder').val(data.DisplayOrder);

                SlideModel.ID = data.ID;
                SlideModel.Status = data.Status;
                SlideModel.Image = data.Image;
                html = '';
                if (SlideModel.Image != '') {
                    html += '<div class="col-md-8"><div class="thumbnail"><div class="image view view-first" data-id="1" data-index="1"><img style="width: 100%; display: block;" src="' + _Host + 'fileman/Uploads/Slide/' + SlideModel.Image + '" alt="image"><div class="mask"><div class="tools tools-bottom"><a href="#" data-index="1"   data-id="1" class="rmFileImage"><i class="fa fa-times"></i></a></div></div></div></div></div>';
                }
                $('#lst-file-image').empty().append(html);
            }
        });

    }
    this.Show = function () {
        var param = {
            slideID: id
        }
        param = JSON.stringify(param)
        var svr = new AjaxCall("api/slide/show", param);
        svr.callServicePOST(function (data) {
            console.log(data)
            $("#model-notify").modal('hide');
            swal({
                title: "Thông báo",
                text: "Hiển thị thành công",
                confirmButtonColor: "#EF5350",
                type: "success"
            }, function () { $("#model-notify").modal('show'); });
            $('#grid').data('kendoGrid').dataSource.read();
            $('#grid').data('kendoGrid').refresh();
        });
    }
    this.Unshow = function () {
        var param = {
            slideID: id
        }
        param = JSON.stringify(param)
        var svr = new AjaxCall("api/slide/unshow", param);
        svr.callServicePOST(function (data) {
            console.log(data)
            $("#model-notify").modal('hide');
            swal({
                title: "Thông báo",
                text: "Hạ hiển thị thành công",
                confirmButtonColor: "#EF5350",
                type: "success"
            }, function () { $("#model-notify").modal('show'); });
            $('#grid').data('kendoGrid').dataSource.read();
            $('#grid').data('kendoGrid').refresh();
        });
    }

    this.Delete = function (title) {
        bootbox.confirm({
            message: "Bạn có chắc muốn xóa slide này",
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
                        slideID: id
                    }
                    var svr1 = new AjaxCall("api/slide/delete", JSON.stringify(param));
                    svr1.callServicePOST(function (data) {
                        if (data) {
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
