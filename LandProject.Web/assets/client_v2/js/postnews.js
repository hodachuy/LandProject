$(document).ready(function () {
    getUserInfo();
    eventAfterLoadAttributes();
    LoadLandType(1);
    LoadProvince();
    LoadLandCategory();
    LoadDistricts();
    initMap();
    $('#txtPhone1').val($('#UserMobile').val());

    $('#radioBan').change(function () {
        rakey = 1;
        LoadLandType(1);
        LoadLandCategory
    });
    $('#radioChothue').change(function () {
        rakey = 2;
        LoadLandType(2);
        LoadLandCategory
    });
})


    //LoadLandType();
    //LoadProvince();
    //LoadDistricts();
    //LoadLandProjectByDistrict();
    //LoadCaptchaCode();



function LoadCaptchaCode() {
    var url = _Host + 'PostingNews/CaptchaIndex';
    $('#imgCaptcha').html("Đang tải Captcha....");
    $.ajax({
        url: url,
        cache: false,
        success: function (data) {
            var imag = "<img src='" + "data:image/jpg;base64," + data + "'/>";
            $('#imgCaptcha').html(imag);
        },
    });
}

// get userSession
function getUserInfo() {
    var userName = $("#UserName").val(),
        userAddress = $("#UserAddress").val(),
        userMobile = $("#UserMobile").val(),
        userEmail = $("#UserEmail").val(),
        userId = $("#UserId").val()
}



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
    document.getElementById("LatiLongTude").value = place.geometry.location.lat() + "," + place.geometry.location.lng();
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
        document.getElementById("LatiLongTude").value = this.getPosition().lat() + "," + this.getPosition().lng();
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
//});

//$('#multi-file').change(function () {
//    var el = $('.img-input[data-id="cover"] .img-preview div');

//    // Danh sách hình ảnh mới thêm
//    var t = 0;
//    for (var i = 0; i < el.length; i++) {
//        if (($(el[i]).find('.img-responsive').attr('src')).indexOf('base64') > -1) {
//            t++;
//        }
//    }
//    if (checkFileExtensionImg($('#multi-file').val())) {
//        var mess = CheckCoverImage(this);

//        if (mess) {
//            countPlaceImg = 0;
//            toastr.error(mess, null, { timeOut: 5000 });
//        }
//        else {
//            getListBase64Image(this)
//                .then((listData) => {
//                    loadPreviewImage(listData);
//                });
//        }
//    } else {
//        countPlaceImg = 0;
//        toastr.error('Vui lòng chọn ảnh đúng định dạng (*.png | *.gif | *.jpg | *.jpeg)', null, { timeOut: 5000 });
//    }
//});


//Loại BDS
function LoadLandType(typeExchange) {
    if (typeExchange == 1) {
        GetLandTypeBuy(function (err, result, msg) {
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
    }else{
        GetLandTypeSale(function (err, result, msg) {
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
}


};
function GetLandTypeBuy(callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/landtype/getall",
        success: function (data) {
            var objLandTypeSale = [];
            if (data.length != 0) {
                objLandTypeSale = data.filter(function (x) { return x.TypeExchange = 1 });//sort 1,2 type bán cho thuê
            } 
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        },
    });
};
function GetLandTypeSale(callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/landtype/getall",
        success: function (data) {
            var objLandTypeSale = [];
            if (data.length != 0) {
                objLandTypeSale = data.filter(function (x) { return x.TypeExchange = 2 });//sort 1,2 type bán cho thuê
            }
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
        if (result != null) {
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
        else {
            var data = {
                id: 0,
                text: '-- Loại --',
            };

            var newOption = new Option(data.text, data.id, false, false);
            $('#LandCategoryID').append(newOption).trigger('change');
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
        console.log(result)
        $('#DistrictID').empty().trigger('change');
        if (result != null) {
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
        } else {
            var data = {
                id: 0,
                text: '-- Quận/Huyện --',
            };

            var newOption = new Option(data.text, data.id, false, false);
            $('#DistrictID').append(newOption).trigger('change');
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
    LoadLandProjectByDistrict();
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

//$(document).on('keypress', '.price', function (event) {
//    event = (event) ? event : window.event;
//    var charCode = (event.which) ? event.which : event.keyCode;
//    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
//        return false;
//    }
//    else {
//        return true;
//    }
//});
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

//$(document).on('keyup', '.price', function (event) {
//    if (event.which >= 37 && event.which <= 40) {
//        event.preventDefault();
//    }

//    var currentVal = $(this).val();
//    var testDecimal = testDecimals(currentVal);
//    if (testDecimal.length > 1) {
//        console.log("You cannot enter more than one decimal point");
//        currentVal = currentVal.slice(0, -1);
//    }
//    $(this).val(replaceCommas(currentVal));
//});

function CalculatePrice() {
    //đơn vị
    var Unit = $("#Unit").val();
    //diện tích
    var area = numberReplaceCommas($("#Area").val());
    var price = numberReplaceCommas($("#Price").val());
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
       
        var totalPrice = price*1000000;
        strFirstTotalPrice = replaceCommas(totalPrice.toString());
        var strTotalPrice = strFirstTotalPrice + " VNĐ";
    }
    else if (Unit == "2") //Tỷ
    {
        var totalPrice = price*1000000000;
        strFirstTotalPrice = replaceCommas(totalPrice.toString());
        var strTotalPrice = strFirstTotalPrice + " VNĐ";
    }
    else if (Unit == "3") //trăm nghìn/m2
    {
        var totalPrice = area * price * 100000;
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
    } else if ($("#LatiLongTude").val() == "") {
        $("#Address").focus();
        toastr.error("", "Vui lòng chọn vị trí trên bản đồ", 30000);
        return false;
    } else if ($("#Description").val() == "") {
        $("#Description").focus();
        toastr.error("", "Vui lòng nhập thông tin mô tả ", 30000);
        return false;
    } else if ($("#AgentMobilePhone").val() == "") {
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

function hideErrorMessage(e) {
    if ($(e).val() != "" || $(e).val() != 0) {
        $(e).parent().find(".errorMessage").hide();
    }
    
}


//Lưu thông tin bán dự án
function SubmitSalePlan() {
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
            "TotalPrice": $('.money-text').html(),
            "DecimalTotalPrice": $("#Price").val() != "" ? parseFloat($("#Price").val()) : 0,
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
            "Name": $("#UserName").val(),
            "Address": $("#UserAddress").val(),
            "Phone": $("#UserMobile").val(),
            "Mobile": $("#UserMobile").val(),
            "Email": $("#UserEmail").val(),
            "UserId":  $("#UserId").val()
        };
            //File hình ảnh
        var formData = new FormData();

        $.each(arrTotalImage, function (index, value) {
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
                                label: 'Tiếp tục đăng tin',
                                className: 'btn-success'
                            }
                        },
                        callback: function (result) {
                            if (result) {
                                location.reload();
                            }else{
                                location.reload();
                            }
                        }
                    });
                    $("#btnSalePlan").attr('disabled', 'disabled');
                    toastr.info("", "Đăng tin thành công", 3000);
                } else {
                    toastr.info("", "Đăng tin thất bại", 3000);
                }
            },
            error: function (e) {
                console.log(e)
                toastr.info("", "Đăng tin thất bại", 3000);
            }
            });
    }
}







//$(function () {

  
//    $('.savestep1').on('click', function () {
//        var t = 0;
//        if ($('#ddlCategory').val() == "0") {
//            toastr.error('Chọn danh mục ');
//            t++;
//        }

//        if ($('#ddlSubCategory').val() == "0") {
//            toastr.error('Chọn danh mục ');
//            t++;
//        }

//        if ($('#ddlGeoState').val() == "0") {
//            toastr.error('Chọn thành phố/ Tỉnh thành');
//            t++;
//        }

//        if ($('#ddlGeoDistrict').val() == "0") {
//            toastr.error('Chọn Quận/ Huyện ');
//            t++;
//        }

//        if ($('#ddlGeoWard').val() == "0") {
//            toastr.error('Chọn Phường ');
//            t++;
//        }

//        if ($('#txtTitle').val() == "") {
//            toastr.error('Vui lòng nhập tiêu đề!');
//            t++;
//        }

//        if ($('#txtShortDescription').val() == "") {
//            toastr.error('Vui lòng nhập mô tả');
//            t++;
//        }

//        if ($('#txtDescription').val() == "") {
//            toastr.error('Vui lòng nhập mô tả chi tiết!');
//            t++;
//        }

//        if ($('#txtDescription').val().length >= 4000) {
//            toastr.error('Mô tả chi tiết chỉ tối đa 4.000 kí tự ');
//            t++;
//        }

//        if ($('#grp_176').val() <= 1) {
//            toastr.error('Diện tích phải lớn hơn 1!');
//            t++;
//        }

//        if ($('.dz-preview').size() == 0) {
//            toastr.error('Vui lòng chọn ít nhất một hình!');
//            t++;
//        } else {
//            $('.lblimg').addClass('hidden');
//        }

//        if ($('#ck_13845').prop('checked') == false) {
//            if ($('#grp_1').val() <= 0) {
//                toastr.error('Vui lòng nhập giá!');
//                t++;
//            }

//            if ($('#grp_1').val() > 1000000000) {

//                toastr.error('Giá tiền chưa hợp lệ. Giá tiền nên từ 0 đến 1.000.000');
//                t++;
//            }
//        }

//        if ($('#radioDT2').prop('checked') == true) {
//            if ($('#txtPhone2').val() == '') {
//                toastr.error('Vui lòng nhập số điện thoại 2!');
//                t++;
//            }

//            if ($('#txtPhone2').length > 11) {
//                toastr.error('Số điện thoại tối đa 11 số!');
//                t++;
//            }
//        }

//        if ($('#txtPhone2').val() != "") {
//            if ($('#txtPhone2').length > 11) {

//                toastr.error('Số điện thoại tối đa 11 số!');
//                t++;
//            }
//        }

//        if (t == 0) {
//            sendGaEventTracking('draft ad', 'draft listing but not yet put live', $('#ddlCategory option:selected').text());

//            $('#action_type').val('InsertAdvert');
//            $.post('/Services/LoadNewUi.ashx', $(document.getElementById("ctl01")).serialize(), function (respone) {
//                if (advertId >= 0) {
//                    $('.hidAdvertIdNewui').text(advertId);
//                    UploadImg1();
//                } else {
//                    $('.hidAdvertIdNewui').text(respone.advertID);
//                    UploadImg1();
//                }

//                $('.hdCategoryID').val(respone.categoryID);
//                $('.hdCategoryName').val(respone.categoryName);
//                $.cookie('cate', $('#ddlCategory').val());
//                $.cookie('stateID', $('#ddlGeoState').val());
//                $.cookie('districtID', $('#ddlGeoDistrict').val());
//                $.cookie('subCateID', $('#ddlSubCategory').val());
//                var radio = $("input[type='radio'][name='optionsRadiosInline']:checked").val();
//                $.cookie('radioBan', radio);
//            });
//        }
//    });
 
//});

function ToTop() {
    $('html, body').animate({
        scrollTop: 0
    }, 0);
}

function backtotop() {
    jQuery(window).scroll(function () {
        if (jQuery(this).scrollTop() > 100) {

            jQuery('#back-top').fadeIn();
        } else {
            jQuery('#back-top').fadeOut();
        }
    });

    // scroll body to 0px on click
    jQuery('#back-top a').click(function () {
        jQuery('body, html').stop(false, false).animate({
            scrollTop: 0
        }, 800);
        return false;
    });

    // scroll body to 0px on click
    jQuery('.arrow-scroll a').click(function () {
        jQuery('body, html').stop(false, false).animate({
            scrollTop: 500
        }, 800);
        return false;
    });
}

function eventAfterLoadAttributes() {
    $('#Price').keypress(function (event) {
        var $this = $(this);
        if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
           ((event.which < 48 || event.which > 57) &&
           (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();
        if ((event.which == 46) && (text.indexOf('.') == -1)) {
            setTimeout(function () {
                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                }
            }, 1);
        }

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });

    $("#Price").on('input', function () {
        setTextMoneyAllowDecimal($(this));
    });
}

function setTextMoneyAllowDecimal(elementObject) {
    var trueValue;
    if (elementObject == null || elementObject.val() == "") trueValue = 0;
    else trueValue = elementObject.val().replace(/,/g, '');

    $('#hidXPrice').val(parseFloat(trueValue));
    $('#hidXPriceTemp').val(parseFloat(trueValue));
    var textMoney;
    var displayValue;
    var displayValueInput = trueValue;
    if (parseFloat(trueValue) < 1000) {
        // ok
        textMoney = parseFloat(trueValue) + " Triệu";
    } else {
        var pricePrecision = trueValue.toString().split('.')[0];
        var priceScale = trueValue.toString().split('.')[1];
        displayValue = (parseFloat(pricePrecision) / 1000).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        if (typeof priceScale === "undefined") {
            // ok
            textMoney = displayValue + " Tỷ";
            displayValueInput = pricePrecision.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        } else {
            textMoney = displayValue + '.000' + priceScale + " Tỷ";
            displayValueInput = pricePrecision.replace(/\B(?=(\d{3})+(?!\d))/g, ",") + '.' + priceScale;
        }
    }

    var divPrivatePrice = $('#ck_13845').closest('.row').closest('.col-lg-6');
    if ($('.dividePricePrivate').length == 0) {
        $(divPrivatePrice).before('<div class="clearfix dividePricePrivate"></div>');
        $('.dividePricePrivate').before($('.money-text').remove().clone());
    }
    $('.money-text').text('');
    $('.money-text').text(textMoney);
    $('#hidXPriceDisplay').val(textMoney);
    if (displayValueInput == 0) {
        $('.money-text').addClass('hidden');
    } else {
        $('.money-text').removeClass('hidden');
    }
    if (displayValueInput != null && displayValueInput.length > 1) {
        displayValueInput = displayValueInput.replace(/^0+/, '');
    }
    elementObject.val(displayValueInput);
}

function setTextMoney(elementObject) {
    $(elementObject).parseNumber({ format: "#,###", locale: "vn" });
    $(elementObject).formatNumber({ format: "#,###", locale: "vn" });
    $('#hidXPrice').val($(elementObject).parseNumber({ format: "#,###.00", locale: "vn" }, false));
    $('#hidXPriceTemp').val($(elementObject).val());
    var divPrivatePrice = $('#ck_13845').closest('.row').closest('.col-lg-6');
    if ($('.dividePricePrivate').length == 0) {
        $(divPrivatePrice).before('<div class="clearfix dividePricePrivate"></div>');
        $('.dividePricePrivate').before($('.money-text').remove().clone());
    }

    $('.money-text').text('');
    var textMoney;
    var trueValue;
    var displayValue;
    displayValue = convertPointNumber($(elementObject).val().replace(/[^a-z0-9]/gi, ','));
    trueValue = $(elementObject).parseNumber({ format: "#,###.00", locale: "vn" }, false);

    if (trueValue < 1000) {
        textMoney = displayValue + " Triệu";
    } else {
        textMoney = trueValue / 1000 + " Tỷ";
    }

    $('.money-text').text(textMoney);
    $('#hidXPriceDisplay').val(textMoney);
}


/**
 * Format Money
 * Number.prototype.format(n, x, s, c)
 * 
 * @param integer n: length of decimal
 * @param integer x: length of whole part
 * @param mixed   s: sections delimiter
 * @param mixed   c: decimal delimiter
 */
Number.prototype.format = function (n, x, s, c) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~~n));

    return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
};
