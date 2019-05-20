var lstCoverPhoto = [];
function onchangeevent() {

    var fileToUpload = getNameFromPath($('#fileUpload').val());
    var filename = fileToUpload.substr(0, (fileToUpload.lastIndexOf('.')));
    if (checkFileExtensionImg(fileToUpload)) {
        //-----------------
        var dataA = new FormData();

        var files = $("#fileUpload").get(0).files;
        var size = 0;
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                size += parseInt(files[i].size);
                dataA.append("UploadedImage" + i, files[i]);
            }
        }
        if ((size / 1024 / 1024) > 15) {
            bootbox.dialog({
                title: "Warning",
                message: "Vui lòng upload ảnh dưới 15M",
                closeButton: true,
                size: 'larger'
            });
            return false;
        }
        $("body").waitMe({
            effect: 'bounce',
            text: '',
            bg: 'rgba(255,255,255,0.7)',
            color: '#000'
        });

        /*
        var ajaxRequest = $.ajax({
            url: serverFile + "Upload/UploadFile?UserInfo=" + getCookie("UserInfo") + "&placeID=" + $("#popup-upload-image").attr("data-placeid-camera") + "&planID=" + planID,
            type: 'POST',
            dataType: 'json',
            data: dataA,
            cache: false,
            contentType: false,
            processData: false,
        });
        ajaxRequest.done(function (responseData, textStatus) {
            if (textStatus == 'success') {
                if (responseData != null) {
                    $("body").waitMe("hide");
                    innerListPhoto();
                }
            } else {
                $("body").waitMe("hide");
            }
        });
        ajaxRequest.error(function (e) {
        })

        */

        $.ajax({
            url: serverFile + "Upload/UploadFile?UserInfo=" + getCookie("UserInfo") + "&placeID=" + $("#popup-upload-image").attr("data-placeid-camera") + "&planID=" + planID,
            type: 'POST',
            dataType: 'json',
            data: dataA,
            cache: false,
            contentType: false,
            processData: false,
            success: function (responseData, textStatus) {
                if (textStatus == 'success') {
                    if (responseData != null) {
                        $("body").waitMe("hide");
                        innerListPhoto();
                    }
                } else {
                    $("body").waitMe("hide");
                }
            },
            error: function (e) {
                $("body").waitMe("hide");
                bootbox.dialog({
                    title: "Warning",
                    message: "Thêm hình ảnh không thành công.",
                    closeButton: true,
                    size: 'larger'
                });
                $("#fileUpload").val('');
                return false;
            }
        });

    } else {
        bootbox.dialog({
            title: "Warning",
            message: "Vui lòng upload ảnh (*.png | *.gif | *.jpg | *.jpeg)",
            closeButton: true,
            size: 'larger'
        });
        $("#fileUpload").val('');
        return false;
    }
}
function innerListPhoto() {
    var planPlaceID = $("#popup-upload-image").attr("data-placeid-camera");
    $.ajax({
        url: serviceBase + "PlanPlace/GetListPhoto?ID=" + planPlaceID + "&IsShared=" + flagIsShared + "&planID=" + planID,
        type: 'GET',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                if (result.placeImage != null) {
                    //trigger new day
                    $("ul.tulieu.list-photo").html("");
                    for (var i = 0; i < result.placeImage.length; i++) {
                        $("ul.tulieu.list-photo").append('<a  data-fancybox="gallery"  href="' + serverFileWeb + "Upload/" + result.placeImage[i].ImagesThumb + '" class="hinhanh" id="20170223011725112">' +
                                                           '<div class="restaurant-detail-multi-img hinhanh_tulieu width_100per" style="background-image:url(' + serverFileWeb + "Upload/" + result.placeImage[i].ImagesThumb + ');"></div>' +
                                                           '<div class="close-button" data-id="' + result.placeImage[i].ID + '"><span class="fa fa-remove"></span></div>' +
                                                       '</li>');
                    }
                    $(".close-button").click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        var id = $(this).attr("data-id");
                        $.ajax({
                            url: serviceBase + "PlacePhoto/UpdatePlacePhoto?PlacePhotoID=" + id,
                            type: 'GET',
                            data: {},
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                if (result != null) {
                                    innerListPhoto();
                                }
                            },
                            error: function (e) {
                            }
                        });

                    })
                }
            }
        },
        error: function (e) {
            $("body").waitMe("hide");
        }
    });
}
function checkFileExtension(file) {
    var flag = true;
    var extension = file.substr((file.lastIndexOf('.') + 1));

    switch (extension) {
        case 'mp4':
        case 'mov':

        case 'MP4':
        case 'MOV':
            flag = true;
            break;
        default:
            flag = false;
    }

    return flag;
}
function checkFileExtensionImg(file) {
    var flag = true;
    var extension = file.substr((file.lastIndexOf('.') + 1));

    switch (extension) {
        case 'jpg':
        case 'JPG':
        case 'png':
        case 'PNG':
        case 'gif':
        case 'GIF':
        case 'jpeg':
        case 'JPEG':
            flag = true;
            break;
        default:
            flag = false;
    }

    return flag;
}
function getNameFromPath(strFilepath) {

    var objRE = new RegExp(/([^\/\\]+)$/);
    var strName = objRE.exec(strFilepath);

    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}

function CheckCoverImage(input) {
    var mess = "";
    if (input.files && input.files[0]) {
        var files = input.files;

        var CheckSizeImg = false;
        var TotalSize = 0;

        //if (files.length > 3) {
        //    mess = "Vui lòng chỉ chọn tối đa 3 hình ảnh cho 01 lần upload";
        //}
        //else {
            
        //}
        for (var i = 0; i < files.length; i++) {
            var size = (files[i].size / 1024 / 1024).toFixed(2);
            if (size > 30) {
                CheckSizeImg = true;
                break;
            }
            else {
                TotalSize += parseFloat(size);
            }
        }

        if (CheckSizeImg) {
            mess = "Vui lòng upload hình ảnh không quá 30MB";
        }
        if (TotalSize > 100) {
            mess = "Tổng dung lượng cho 01 lần upload không vượt quá 90MB";
        }
        return mess;
    }
}

function getListBase64Image(input) {
    return new Promise(function (resolve, reject) {
        if (input.files && input.files[0]) {
            var files = input.files;

            for (var i = 0; i < files.length; i++) {
                if (checkFileExtensionImg(files[i].name)) {
                    let res = {};
                    let file = files[i];
                    res.fileName = file.name;
                    let reader = new FileReader();
                    reader.onload = function (e) {
                        res.fileData = e.target.result;
                        var el = `<div class="photo-item" style="width: 50%; border: 2px solid transparent; box-sizing: padding-box; position: relative">
                                        <img class="img-responsive" style="height: 115px; width: 100%" src="`+ res.fileData + `" alt="` + res.fileName + `" />
                                        <span class ="fa fa-remove" data-remove-filename= "`+ res.fileName + `" style="position: absolute; top: 10px; right: 10px; color: #795548;; cursor: pointer"></span>
                                        <input class="source-photo" placeholder="nguồn hình ảnh" style="height: 18px; width: 100%; line-height: 13px; padding: 0px; font-size: 11px;" type="text">
                                    </div>`;
                        $('[data-id="cover"] .img-preview').append(el);
                        $('.fa-remove').click(function () {
                            var filename = $(this).data('remove-filename');
                            $(this).parent().remove();
                            $.each(lstCoverPhoto, function (i, el) {
                                if (this.fileName == filename) {
                                    lstCoverPhoto.splice(i, 1);
                                }
                            });
                        });
                    };
                    reader.readAsDataURL(file);
                    lstCoverPhoto.push(res);
                }
            }
        }
    });
}