
    LoadLandType();
    LoadProvince();
    LoadDistricts();
    LoadLandProjectByDistrict();
    LoadCaptchaCode();
    initMap();

    getUserInfo();

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

    $("#AgentName").val(userName);
    $("#AgentAddress").val(userAddress);
    $("#AgentPhone").val('');
    $("#AgentMobilePhone").val(userMobile);
    $("#AgentEmail").val(userEmail);
    $("#UserId").val(userId);
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

var arrImage = [];
var arrTotalImage = [];
var indexImg = 0;

$('body').on("click", '#multi-file', function (e) { e.target.value = null; }); // sự kiện reset input file

$('body').on('change', '#multi-file', function (event) {
    var files = event.target.files;
    var maxSize = parseInt(3 * 1024); // 3MB
    for (var i = 0; i < files.length; i++) {
        var reader = new FileReader();
        //var file = files[i];
        if (files[i].type.indexOf("image") == 0) {
            reader.onload = function (e)
            {
                if ((e.total / 1024).toFixed(0) > maxSize) {
                    toastr.error('kich thuoc qua lon', null, { timeOut: 5000 });
                    return;
                }
                indexImg = parseInt(indexImg + i);
                var fileSrc = e.target.result;
                var fileName = i;
                var temp = '';
                temp += '<div class="photo-item" style="width: 50%; border: 2px solid transparent; box-sizing: padding-box; position: relative">';
                temp += '<img class="img-responsive" style="height: 115px; width: 100 %" src="' + fileSrc + '" data-index="' + indexImg + '" alt="'+fileName+'"/>';
                temp += '<span class="fa fa-remove rmFile" data-index="' + indexImg + '" data-remove-filename="' + fileName + '" style="position: absolute; top: 10px; right: 10px; color: #795548;; cursor: pointer"></span>';
                temp += '</div>';
                $(".img-preview").append(temp);

                var image = {
                    index: parseInt(i),
                    file: e
                }
                arrImage.push(image);
            }
          
        } else {
            toastr.error('Vui lòng chọn ảnh đúng định dạng (*.png | *.gif | *.jpg | *.jpeg)', null, { timeOut: 5000 });
        }
        reader.readAsDataURL(files[i]);

        var image = {
            index: parseInt(i+1),
            file: files[i]
        }
        arrTotalImage.push(image);
        //formData.append('file', files[i] )
    };
       
});
$('body').on('click', '.rmFile', function () {
    var indexImage = $(this).attr('data-index');
    for (var i = 0; i < arrImage.length; i++) {
        if (arrImage[i].index == indexImage) {
            arrImage.splice(i, 1);
            arrTotalImage.splice(i, 1);
            $(this).parent().remove();
            break;
        }
    }


    console.log(arrImage);
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
        $("#TypePlan").select2({
            data: data1
        });
    });

};
function GetLandType(callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/landtype/getall",
        success: function (data) {
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        },
    });
};
function LoadLandCategory() {
    GetLandCategory($("#LandTypeID").val(),function (err, result, msg) {
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
    });

};
function GetLandCategory(lTypeId, callback) {
    if (lTypeId != "" || lTypeId != null) {
    $.ajax({
        type: 'POST',
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
        $("#Province").select2({
            data: data1
        });
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
    });
}
function LoadDistrictsSearch() {
    GetDistricts($("#Province").val(), function (err, result, msg) {
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
        $("#District").select2({
            data: data1
        });
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
        $("#WardID").select2({
            data: data1
        });
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
            "LandTypeID": $("#LandTypeID").val() != "" ?parseInt($("#LandTypeID").val()):0,
            "LandCategoryID": $("#LandCategoryID").val() != "" ? parseInt($("#LandCategoryID").val()) : 0,
            "ProvinceID": $("#ProvinceID").val() != "" ? parseInt($("#ProvinceID").val()) : 0,
            "DistrictID": $("#DistrictID").val() != "" ? parseInt($("#DistrictID").val()) : 0,
            "UserID": 1,  
            "LandNewsScheduleID": 1,
            "AgentID":0,
            "WardID": $("#WardID").val() != "" ? parseInt($("#WardID").val()) : 0,
            "LProjectID": $("#LProjectID").val() != "" ? parseInt($("#LProjectID").val()) : 0,
            "Area": $("#Area").val()!=""?parseInt($("#Area").val()):0,
            "Price": $("#Price").val()!=""?parseFloat($("#Price").val()):0,
            "TotalPrice": $("#TotalPrice").val(),
            "DecimalTotalPrice": $("#DecimalTotalPrice").val()!=""?parseFloat($("#DecimalTotalPrice").val()):0,
            "Unit": $("#Unit").val(),
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
            "Name":  $("#AgentName").val(),
            "Address": $("#AgentAddress").val(),
            "Phone": $("#AgentPhone").val(),
            "Mobile": $("#AgentMobilePhone").val(),
            "Email": $("#AgentEmail").val(),
            "UserId":  $("#UserId").val()
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
                debugger
                if (result.status == true) {
                    alert(result.code);
                    //bootbox.alert({
                    //    title: "Thông báo",
                    //    message: "<span style='padding: 10px'>" + result.code+ "</span>",
                    //    buttons: {
                    //        ok: {
                    //            label: 'OK',
                    //            className: 'btn-success'
                    //        }
                    //    },
                    //    callback: function () { }
                    //});
                    $("#btnSalePlan").attr('disabled', 'disabled');
                    toastr.info("", "Đăng tin thành công", 3000);
                } else {
                    toastr.info("", "Đăng tin thất bại công", 3000);
                }
            },
            error: function (e) {
                toastr.info("", "Đăng tin thất bại công", 3000);
            }
            });
    }
}
