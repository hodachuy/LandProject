﻿
    LoadLandType();
    LoadProvince();
    LoadDistricts();
    LoadLandProjectByDistrict();
    LoadCaptchaCode();
    getUserInfo();

function LoadCaptchaCode() {
    var url = _Host + 'PostingNews/CaptchaIndex'; // _Host trong file jquery.js
    $('#imgCaptcha').html("Đang tải Captcha....");
    $.ajax({
        url: url,
        cache: false,
        success: function (data) {
            var imag = "<img src='" + "data:image/jpg;base64," + data + "'/>";
            $('#imgCaptcha').html(imag);
        }
    });
    }
// get userSession
function getUserInfo() {
    var userName = $("#UserName").val(),
        userAddress = $("#UserAddress").val(),
        userMobile = $("#UserMobile").val(),
        userEmail = $("#UserEmail").val(),
        userId = $("#UserId").val();

    $("#AgentName").val(userName);
    $("#AgentAddress").val(userAddress);
    $("#AgentPhone").val('');
    $("#AgentMobilePhone").val(userMobile);
    $("#AgentEmail").val(userEmail);
    $("#UserId").val(userId);
}


var arrImageAsUrl = [];
var arrTotalImage = [];
var indexImg = 0;

$('body').on("click", '#multi-file', function (e) { e.target.value = null; }); // sự kiện reset input file

$('body').on('change', '#multi-file', function (event) {
    var files = event.target.files;
    var maxSize = 3072; // 3MB
    for (var i = 0; i < files.length; i++) {
        indexImg++;
        var reader = new FileReader();
        if (files[i].type.indexOf("image") == 0) {
            reader.onload = (function (index) {
                return function (e) {
                    var fileSize = (e.total / 1024).toFixed(0);
                    console.log(fileSize)
                    if (fileSize > maxSize) {
                        toastr.error('Kích thước ảnh lớn hơn 3MB', null, { timeOut: 5000 });
                        return;
                    }
                    var fileSrc = e.target.result;
                    var fileName = e.target.fileName;
                    var temp = '';
                    temp += '<div class="photo-item" style="width: 50%; border: 2px solid transparent; box-sizing: padding-box; position: relative">';
                    temp += '<img class="img-responsive" style="height: 115px; width: 100 %" src="' + fileSrc + '" data-index="' + index + '" alt="' + fileName + '"/>';
                    temp += '<span class="fa fa-remove rmFile" data-index="' + index + '" data-remove-filename="' + fileName + '" style="position: absolute; top: 10px; right: 10px; color: #795548;; cursor: pointer"></span>';
                    temp += '</div>';
                    $(".img-preview").append(temp);
                    var image = {
                        index: index,// parseInt(i),
                        file: e
                    }
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
        arrTotalImage.push(image);
        //formData.append('file', files[i] )

    };
    console.log("arrimageAsUrl:" + arrImageAsUrl)
    console.log("arrimageFile:" + arrTotalImage)

});
$('body').on('click', '.rmFile', function () {
    var indexImage = $(this).attr('data-index');
    for (var i = 0; i < arrImageAsUrl.length; i++) {
        if (arrImageAsUrl[i].index == indexImage) {
            arrImageAsUrl.splice(i, 1);
            arrTotalImage.splice(i, 1);
            $(this).parent().remove();
            break;
        }
    }
    console.log("arrimageAsUrl:" + arrImageAsUrl)
    console.log("arrimageFile:" + arrTotalImage)
});

//Loại BDS
function LoadLandType() {
    GetLandType(function (err, result, msg) {
        var data1 = $.map(result, function (obj) {
            var newObje = {};
            newObje.id = obj.ID;
            newObje.text = obj.Name;
            return newObje;
        });
        data1.unshift({
            id: 0,
            text: '-- Hình thức --',
        });
        $("#LandTypeID").select2({
            data: data1
        });
        //$("#TypePlan").select2({
        //    data: data1
        //});
    });

};
function GetLandType(callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/landtype/getallbuyrent",
        success: function (data) {
            //var objLandTypeSale = [];
            //if (data.length != 0) {
            //    objLandTypeSale = data.filter(function (x) { return x.SortOrder < 3 });//sort 1,2 type bán cho thuê
            //} 
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        },
    });
};
function LoadLandCategory() {
    GetLandCategory($("#LandTypeID").val(), function (err, result, msg) {
        console.log(result)
        $('#LandCategoryID').empty().trigger('change');
        // $("#LandCategoryID").select2("val", "");
        if (result != undefined) {
            var data1 = $.map(result, function (obj) {
                var newObje = {};
                newObje.id = obj.ID;
                newObje.text = obj.Name;
                return newObje;
            });
            data1.unshift({
                id: 0,
                text: '-- Loại --',
            });
            $("#LandCategoryID").select2({
                data: data1
            });
        }
    });

};
function GetLandCategory(lTypeId, callback) {
    if (lTypeId != "" || lTypeId != null) {
        $.ajax({
            type: 'GET',
            data: {},
            contentType: 'application/json',
            dataType: "json",
            url: _Host + "api/landcategory/getall?lTypeID=" + lTypeId,
            success: function (data) {
                callback(false, data, null);
            },
            error: function (xhr, error) {
                callback(true, null, error);
            },
        });
    }
};
//Danh sách tỉnh thành
function LoadProvince() {
    GetProvince(function (err, result, msg) {
        var data1 = $.map(result, function (obj) {
            var newObje = {};
            newObje.id = obj.ID;
            newObje.text = obj.Name;
            return newObje;
        });
        data1.unshift({
            id: 0,
            text: '-- Tỉnh/thành phố --',
        });
        $("#ProvinceID").select2({
            data: data1
        });
        //$("#Province").select2({
        //    data: data1
        //});
    });

};
function GetProvince(callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/address/getprovince",
        success: function (data) {
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        }
    });
};
//Load Quận huyện
function LoadDistricts() {
    GetDistricts($("#ProvinceID").val(), function (err, result, msg) {
        $('#DistrictID').empty().trigger('change');
        if (result != undefined) {
            var data1 = $.map(result, function (obj) {
                var newObje = {};
                newObje.id = obj.ID;
                newObje.text = obj.Name;
                return newObje;
            });
            data1.unshift({
                id: 0,
                text: '-- Quận/Huyện --',
            });
            $("#DistrictID").select2({
                data: data1
            });
        }
    });
}

function GetDistricts(provinceId, callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/address/getdistrict?provinceID=" + provinceId,
        success: function (data) {
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        }
    });
}
//Load phường xã
function LoadWards() {
    GetWards($("#DistrictID").val(), function (err, result, msg) {
        $('#WardID').empty().trigger('change');
        if (result != undefined) {
            var data1 = $.map(result, function (obj) {
                var newObje = {};
                newObje.id = obj.ID;
                newObje.text = obj.Name;
                return newObje;
            });
            data1.unshift({
                id: 0,
                text: '-- Phường/xã --',
            });
            $("#WardID").select2({
                data: data1
            });
        }
    });
}
function GetWards(districtId, callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/address/getward?districtID=" + districtId,
        success: function (data) {
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        }
    });
}

function LoadLandProjectByDistrict() {
    GetLandProjectByDistrict($("#DistrictID").val(), function (err, result, msg) {
        $('#LProjectID').empty().trigger('change');
        if (result != undefined) {
            var data1 = $.map(result, function (obj) {
                var newObje = {};
                newObje.id = obj.ID;
                newObje.text = obj.Name;
                return newObje;
            });
            data1.unshift({
                id: 0,
                text: '-- Dự án --',
            });
            $("#LProjectID").select2({
                data: data1
            });
        }
    });

}
function GetLandProjectByDistrict(districtId, callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/lproject/getbydistrict?districtID=" + districtId,
        success: function (data) {
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        }
    });
}

$(document).on('keypress', '.price', function (event) {
    event = (event) ? event : window.event;
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    else {
        return true;
    }
});
$(document).on('keypress', '.number', function (event) {
    event = (event) ? event : window.event;
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    else {
        return true;
    }
});


var TotalPrice;
$(document).on('keyup', '.price', function (event) {
    if (event.which >= 37 && event.which <= 40) {
        event.preventDefault();
    }

    var currentVal = $(this).val();
    var testDecimal = testDecimals(currentVal);
    if (testDecimal.length > 1) {
        console.log("You cannot enter more than one decimal point");
        currentVal = currentVal.slice(0, -1);
    }
    $(this).val(replaceCommas(currentVal));
});

function CalculatePrice() {
    //đơn vị
    var Unit = $("#Unit").val();
    //diện tích
    var area = numberReplaceCommas($("#Area").val());
    var price = numberReplaceCommas($("#Price").val());
        TotalPrice = price + " vnd";
    var strTotalPrice = "";
    var strFirstTotalPrice = "";
    var totalPrice = 0;
    if (Unit == "0") //thỏa thuận
    {        
        var strTotalPrice = "Thỏa thuận";
        var totalPrice = 0;
    }
    else if (Unit == "1") //triệu
    {
       
        var totalPrice = area * price*1000000;
        strFirstTotalPrice = replaceCommas(totalPrice.toString());
        var strTotalPrice = strFirstTotalPrice + " VNĐ";
    }
    else if (Unit == "2") //Tỷ
    {
        var totalPrice = area * price*1000000000;
        strFirstTotalPrice = replaceCommas(totalPrice.toString());
        var strTotalPrice = strFirstTotalPrice + " VNĐ";
    }
    else if (Unit == "3") //trăm nghìn/m2
    {
        var totalPrice = area * price*100000;
        strFirstTotalPrice = replaceCommas(totalPrice.toString());
        var strTotalPrice = strFirstTotalPrice + "VNĐ";
    }
    else if (Unit == "4")//triệu/m2
    {
        var totalPrice = area * price*1000000;
        strFirstTotalPrice = replaceCommas(totalPrice.toString());
        var strTotalPrice = strFirstTotalPrice + "VNĐ";
    }

    $("#TotalPrice").val(strTotalPrice);
    $("#DecimalTotalPrice").val(totalPrice);
}

function CheckValid() {
    if ($("#Title").val() == "") {
        //$("#Title").parent().find(".errorMessage").html("Vui lòng nhập tiêu đề!");
        //$("#Title").parent().find(".errorMessage").show();
        $("#Title").focus();
        toastr.error("", "Vui lòng nhập tiêu đề", 30000);
        return false;
    } else if ($("#Description").val() == "") {
        $("#Description").focus();
        toastr.error("", "Vui lòng nhập thông tin mô tả ", 30000);
        return false;
    }
    else if ($("#LandTypeID").val() == 0) {
        $("#LandTypeID").focus();
        toastr.error("","Vui lòng chọn hình thức",30000);
        return false;
    } else if ($("#LandCategoryID").val() == 0) {
        $("#LandTypeID").focus();
        toastr.error("", "Vui lòng chọn loại", 30000);
        return false;
    } else if ($("#ProvinceID").val() == 0) {
        $("#LandTypeID").focus();
        toastr.error("", "Vui lòng chọn tỉnh thành", 30000);
        return false;
    } else if ($("#DistrictID").val() == 0) {
        $("#DistrictID").focus();
        toastr.error("", "Vui lòng chọn quận/huyện", 30000);
        return false;
    } else if ($("#Address").val() == "") {
        $("#Address").focus();
        toastr.error("", "Vui lòng nhập địa chỉ", 30000);
        return false;
    }  else if ($("#AgentMobilePhone").val() == "") {
        $("#AgentMobilePhone").focus();
        toastr.error("", "Vui lòng nhập số điện thoại liên hệ ", 30000);
        return false;
    } else if ($("#Captcha").val() == "") {
        $("#Captcha").focus();
        toastr.error("", "Vui lòng nhập mã captcha ", 30000);
        return false;
    }else {
        return true;
    } 
    
}

//Lưu thông tin bán dự án
function SubmitBuyPlan() {
    if (CheckValid() == true) {
     
        var landnews = {
            "ID":0,
            "Title": $("#Title").val(),
            "Alias": stringToSlug($("#Title").val()),
            "Description": $("#Description").val(),
            "Image": "",
            "Code": "",
            "Address": $("#Address").val(),
            "LandTypeID": $("#LandTypeID").val() != "" ? parseInt($("#LandTypeID").val()) : 0,
            "LandTypeName": $("#LandTypeID option:selected").text(),
            "LandCategoryID": $("#LandCategoryID").val() != "" ? parseInt($("#LandCategoryID").val()) : 0,
            "ProvinceID": $("#ProvinceID").val() != "" ? parseInt($("#ProvinceID").val()) : 0,
            "DistrictID": $("#DistrictID").val() != "" ? parseInt($("#DistrictID").val()) : 0,
            "UserID": $("#UserId").val() != "" ? $("#UserId").val() : "",
            "LandNewsScheduleID": 2,
            "AgentID":0,
            "WardID": $("#WardID").val() != "" ? parseInt($("#WardID").val()) : 0,
            "LProjectID": $("#LProjectID").val() != "" ? parseInt($("#LProjectID").val()) : 0,
            "Area": $("#Area").val()!=""?parseInt($("#Area").val()):0,
            "Price": $("#Price").val()!=""?parseFloat($("#Price").val()):0,
            "TotalPrice": TotalPrice,
            "DecimalTotalPrice": $("#DecimalTotalPrice").val()!=""?parseFloat($("#DecimalTotalPrice").val()):0,
            "Unit": $("#Unit option:selected").text(),//$("#Unit").val(),
            "Facade": $("#Facade").val()!=""?parseInt($("#Facade").val()):0,
            "Entry": $("#Entry").val() != "" ?parseInt($("#Entry").val()):0,
            "HouseDirection": $("#HouseDirection").val(),
            "BalconyDirection": $("#BalconyDirection").val(),
            "NumberFloor": $("#NumberFloor").val() != "" ? parseInt($("#NumberFloor").val()) : 0,
            "NumberBedroom": $("#NumberBedroom").val() != "" ? parseInt($("#NumberBedroom").val()) : 0, 
            "NumberWC": $("#NumberWC").val() != "" ? parseInt($("#NumberWC").val()) : 0, 
            "Furniture": $("#Furniture").val(),
            "LatiLongTude": $("#LatiLongTude").val(),
            "IsDelete": false,
            "IsPublished": false,
            "IsSale": false,
            "Status": false,
        };       
        var agent = {
            "ID": 0,
            "Name": $("#AgentName").val(),
            "Address": $("#AgentAddress").val(),
            "Phone": $("#AgentPhone").val(),
            "Mobile": $("#AgentMobilePhone").val(),
            "Email": $("#AgentEmail").val(),
            "UserId": $("#UserId").val()
        };
            //File hình ảnh
        var formData = new FormData();

        $.each(arrTotalImage, function (index, value) {
            debugger
            console.log(value.file)
            formData.append('file' + index, value.file);
        });
        formData.append('captcha', JSON.stringify($("#Captcha").val()));
        formData.append('landnews', JSON.stringify(landnews)); 
        formData.append('agent', JSON.stringify(agent));     
        
        $.ajax({
            url: _Host + "PostingNews/Create",
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            dataType: "json",
            success: function (result) {
                if (result.status == true) {
                    //alert(result.code);
                    bootbox.confirm({
                        title: "Thông báo",
                        message: "Mã xác nhận đăng tin của bạn là: " + result.code,
                        buttons: {
                            cancel: {
                                label: 'Đóng',
                                className: 'btn-primary'
                            },
                            confirm: {
                                label: 'Tin tục đăng Tin',
                                className: 'btn-success'
                            }
                        },
                        callback: function (result) {
                            if (result) {
                                location.reload();
                            };
                        }
                    });
                    $("#btnSalePlan").attr('disabled', 'disabled');
                    toastr.info("", "Đăng tin thành công", 3000);
                } else {
                    toastr.info("", result.message, 3000);
                }
            },
            error: function (e) {
                toastr.info("", "Đăng tin thất bại", 3000);
            }
        });
    }
}
