var _idgrid = "#grid";
var PostModel = {
    ID: '',
    Name: '',
    Alias: '',
    Descriptipn: '',
    PostCategoryID:'',
    Content:'',
    Status: false,
    IsDelete:false
}
var TypeActionAdd = true;

var arrImageAsUrl = [];
var arrTotalImage = [];
var indexImg = 0;

$(document).ready(function () {
    LoadGrid();

    $('body').on('click', '#form-create-post', function () {
        arrImageAsUrl = [];
        arrTotalImage = [];
        indexImg = 0;

        $('#form').validationEngine('hide');
        TypeActionAdd = true;

        var urlPostCategory = "api/postcategory/getall";
        var element = "#cboPostCategory";
        LoadComboBoxWithServices(element, urlPostCategory, null, "ID", "Name", null, "Chọn Thể Loại", false, null, function () { }, null);

        $('#txtPostName').val('');
        $('#txtDescription').val('');
        tinymce.get("txtContent").getBody().innerHTML = "";

        $("#PostModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
    })
    $('body').on('click', '#savePost', function () {
        if (checkValid()) {
            var formData = new FormData();

            PostModel.Name = $('#txtPostName').val();
            PostModel.Alias = new commonService().getSeoTitle($('#txtPostName').val());
            PostModel.Content = tinymce.get("txtContent").getContent();
            PostModel.Description = $('#txtDescription').val();
            PostModel.PostCategoryID = $('#cboPostCategory').val();
            PostModel.IsDelete = false;
            if (TypeActionAdd) {//add      
                PostModel.ID = 0;

                $.each(arrTotalImage, function (index, value) {
                    formData.append('file' + index, value.file);
                });

                formData.append('post', JSON.stringify(PostModel));

                $.ajax({
                    url: _Host + "api/post/create",
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    dataType: "json",
                    success: function (result) {
                        if (result) {
                            $("#PostModel").modal('hide');
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

                formData.append('post', JSON.stringify(PostModel));

                $.ajax({
                    url: _Host + "api/post/update",
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    dataType: "json",
                    success: function (result) {
                        if (result) {
                            $("#PostModel").modal('hide');
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
    $('body').on('click', '#closePost', function () {
        $("#PostModel").modal('hide');
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
                        var html = '<div class="col-md-55"><div class="thumbnail"><div class="image view view-first"  data-id="0" data-index="' + index + '"><img style="width: 100%; display: block;" src="' + fileSrc + '" alt="image"><div class="mask"><div class="tools tools-bottom"><a href="#"  data-id="0" data-index="' + index + '"  class="rmFileImage"><i class="fa fa-times"></i></a></div></div></div></div></div>';

                        //var temp = '';
                        //temp += '<div class="photo-item" style="width: 50%; border: 2px solid transparent; box-sizing: padding-box; position: relative">';
                        //temp += '<img class="img-responsive" style="height: 115px; width: 100 %" src="' + fileSrc + '" data-index="' + index + '" alt="' + fileName + '"/>';
                        //temp += '<span class="fa fa-remove rmFile" data-index="' + index + '" data-remove-filename="' + fileName + '" style="position: absolute; top: 10px; right: 10px; color: #795548;; cursor: pointer"></span>';
                        //temp += '</div>';
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
            var param = {
                fileID: idImage
            }
            param = JSON.stringify(param);
            var svr1 = new AjaxCall("api/file/delete", param);
            svr1.callServicePOST(function (data) {
                console.log(data)
                if (data) {
                    $(this).closest('.col-md-55').remove();
                }
            });
        } else {
            for (var i = 0; i < arrImageAsUrl.length; i++) {
                if (arrImageAsUrl[i].index == indexImage) {
                    arrImageAsUrl.splice(i, 1);
                    arrTotalImage.splice(i, 1);
                    $(this).closest('.col-md-55').remove();
                    break;
                }
            }
            console.log("arrimageAsUrl:" + arrImageAsUrl)
            console.log("arrimageFile:" + arrTotalImage)
        }
    });

    //tinymce
    tinymce.init({
        selector: '.editorTinyMce',
        cleanup: true,
        verify_html: false,
        entity_encoding: "raw",
        cleanup_on_startup: true,
        // ket thuc performance
        height: '200px',
        inline: false,
        plugins: 'advlist autolink link image lists print preview code media table textcolor hr searchreplace wordcount ',
        //toolbar: "forecolor backcolor table image autolink preview charmap link searchreplace fontsizeselect fontselect ",
        toolbar: "fontselect | bold italic underline | forecolor | fontsizeselect | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | image searchreplace ",
        fontsize_formats: "8pt 10pt 12pt 13pt 14pt 18pt 24pt 36pt",
        skin: 'lightgray',
        theme: 'modern',
        languague: 'vi',
        valid_elements: '+*[*]',
        file_browser_callback: RoxyFileBrowser,

        media_url_resolver: function (data, resolve/*, reject*/) {
            if (data.url.indexOf('YOUR_SPECIAL_VIDEO_URL') !== -1) {
                var embedHtml = '<iframe src="' + data.url +
                '" width="400" height="400" ></iframe>';
                resolve({ html: embedHtml });
            } else {
                resolve({ html: '' });
            }
        }
    })
})

checkValid = function () {
    var res = $("#form").validationEngine('validate');;
    setTimeout(function () {
        $('#form').validationEngine('hide');
    }, 10000);
    var postName = $('#txtPostName').val();
    if (postName.trim() == "") {
        $('#txtPostName').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#txtPostName").validationEngine('hide');
    }
    if ($("#cboPostCategory").data("kendoComboBox").selectedIndex == -1) {
        $('#cboPostCategory').validationEngine('showPrompt', '* Trường này bắt buộc', 'red', 'topRight', true);
        res = false;
    } else {
        $("#cboPostCategory").validationEngine('hide');
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
            width: 100,
        },
        {
            template: '#=data.Name#',
            field: "Name",
            title: "Tên bài tin",
        },
         {
             template: '#=data.PostCategory.Name#',
             field: "PostCategoryID",
             title: "Thể loại tin",
             filterable: {
                 ui: postCategoryFilter
             }
         },
];
LoadGrid = function () {
    InitKendoGrid(_idgrid, Columns, new DataSource().MasterDatasource("" + _Host + "api/post/getalltable"), null, false, '')
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
        arrImageAsUrl = [];
        arrTotalImage = [];
        indexImg = 0;

        var urlPostCategory = "api/postcategory/getall";
        var element = "#cboPostCategory";
        LoadComboBoxWithServices(element, urlPostCategory, null, "ID", "Name", null, "Loại bài viết", false, null, function () {
        }, null);
        $('#txtPostName').val('');
        $('#txtDescription').val('');
        tinymce.get("txtContent").getBody().innerHTML = '';

        TypeActionAdd = false;
        $("#PostModel").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        var param = {
            postID: id
        }
        var svr = new AjaxCall("api/post/getbyid", param);
        svr.callServiceGET(function (data) {
            console.log(data)
            if (data != undefined) {
                var urlPostCategory = "api/postcategory/getall";
                var element = "#cboPostCategory";
                LoadComboBoxWithServices(element, urlPostCategory, null, "ID", "Name", data.PostCategoryID, "Loại bài viết", false, null, function () {
                }, null);
                $('#txtPostName').val(data.Name);
                $('#txtDescription').val(data.Description);
                tinymce.get("txtContent").getBody().innerHTML = data.Content;
                PostModel.ID = data.ID;
                PostModel.IsDelete = data.IsDelete;
                PostModel.IsPublished = data.IsPublished;
                PostModel.PostCategoryID = data.PostCategoryID;

                var param1 = {
                    postID: id
                }
                var svr1 = new AjaxCall("api/post/getfilelpost", param1);
                svr1.callServiceGET(function (data) {
                    if (data.length != 0) {
                        var html = '';
                        $.each(data, function (index, value) {
                            html += '<div class="col-md-55"><div class="thumbnail"><div class="image view view-first" data-id="' + value.ID + '" data-index="' + index + '"><img style="width: 100%; display: block;" src="' + _Host + 'fileman/Uploads/LProject/' + value.FileName + '" alt="image"><div class="mask"><div class="tools tools-bottom"><a href="#" data-index="' + index + '"   data-id="' + value.ID + '" class="rmFileImage"><i class="fa fa-times"></i></a></div></div></div></div></div>';
                        })
                        $('#lst-file-image').append(html);
                    }
                });
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

function postCategoryFilter(element) {
    element.kendoDropDownList({
        dataSource: lstPostCategory,
        dataTextField: "Name",
        dataValueField: "ID",
        optionLabel: "--Chọn thể loại--"
    });
}
var lstPostCategory = new kendo.data.DataSource({
    transport: {
        read: function (options) {
            $.ajax({
                url: _Host + "api/postcategory/getall",
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