var landnews = {
    ID: "0",
    Title: "",
    Alias: "",
    Description: "",
    Image: "",
    Code: "",
    Address: "",
    LandTypeID: "",
    LandTypeName: "",
    LandCategoryID: "",
    ProvinceID: "",
    DistrictID: "",
    UserID: "",
    LandNewsScheduleID: "",
    AgentID: "0",
    WardID: "",
    LProjectID: "",
    Area: "",
    Price: "",
    TotalPrice: "",
    DecimalTotalPrice: "",
    Unit: "",
    Facade: "",
    Entry: "",
    LegalStatus: "",
    HouseDirection: "",
    BalconyDirection: "",
    NumberFloor: "",
    NumberBedroom: "",
    NumberWC: "",
    Furniture: "",
    LatiLongTude: "",
    Convenient: "",
    Environment: "",
    IsDelete: false,
    IsPublished: false,
    IsSale: false,
    Status: false,
    TypeExchange: ""
};

var agent = {
    ID: "0",
    Name: "",
    Address: "",
    Phone1: "",
    Phone2: "",
    Phone3: "",
    PhoneShow: "",
    Email: "",
    UserId: ""
};

var arr_convenient = [
    "Hồ bơi", "Máy giặt", "Wifi", "Sân vườn", "Đầy đủ nội thất", "Điều hòa", "Ban công / Sân thượng", "Phòng tập gym", "Internet cáp quang"
];
var arr_environment = [
    "Siêu thị", "Trường học", "Gần biển", "Bệnh viện", "Công viên", "Gần chùa", "Giao thông công cộng", "Khu an ninh", "Gần nhà thờ"
];

var arr_checked_convenient = [];
var arr_checked_environment = [];

var typeExchange = 1;

var landID = $("#landNewsIdViewbag").val();
$(document).ready(function () {

    // user session
    getUserInfo();

    if (landID != "") {
        getLandNews();
    } else {
        LoadLandType(1);
        initMap();

        // template môi trường và tiện nghi
        renderConvenient();
        renderEnvironment();
    }

    LoadProvince();
    LoadLandCategory();
    LoadDistricts();

    eventAfterLoadAttributes();
    LoadCaptchaCode();

    $('body').on('change', '#radioBan', function () {
        LoadLandType(1);
        LoadLandCategory();
        typeExchange = 1;
    })
    
    $('body').on('change', '#radioChothue', function () {
        LoadLandType(2);
        LoadLandCategory();
        typeExchange = 2;
    })

    $('body').on('change', '#radioDT1', function () {
               $('#hdValueDT').val(1);
    })
    $('body').on('change', '#radioDT2', function () {
        $('#hdValueDT').val(2);
    })
    $('body').on('click', 'input.chk_convenient', function () {
        if ($(this).prop('checked') == false) {
            var itemChecked = $(this).prop('value');// lấy value của input đc check
            $.each(arr_checked_convenient, function (index, item) {
                if (item === itemChecked) { // kiểm tra value được check có trong mảng không.
                    index = arr_checked_convenient.indexOf(itemChecked);// nếu có lấy index của value đó.
                    arr_checked_convenient.splice(index, 1);// remove theo vị trí trong mảng.
                }
            })
            console.log('refresh ds :   ' + arr_checked_convenient + '      bỏ id:' + $(this).val());//bật f12
        }
        else {
            arr_checked_convenient.push($(this).val());
            console.log('refresh ds :   ' + arr_checked_convenient + '      them id:' + $(this).val());//bật f12
        }
    })
    $('body').on('click', 'input.chk_environment', function () {
        if ($(this).prop('checked') == false) {
            var itemChecked = $(this).prop('value');// lấy value của input đc check
            $.each(arr_checked_environment, function (index, item) {
                if (item === itemChecked) { // kiểm tra value được check có trong mảng không.
                    index = arr_checked_environment.indexOf(itemChecked);// nếu có lấy index của value đó.
                    arr_checked_environment.splice(index, 1);// remove theo vị trí trong mảng.
                }
            })
            console.log('refresh ds :   ' + arr_checked_environment + '      bỏ id:' + $(this).val());//bật f12
        }
        else {
            arr_checked_environment.push($(this).val());
            console.log('refresh ds :   ' + arr_checked_environment + '      them id:' + $(this).val());//bật f12
        }
    })
    $('body').on('click', 'input.chk_price', function () {
        if ($(this).prop('checked') == false) {
            $("#Price").attr("disabled", false)
            $('.money-text').text('');
        } else {
            $("#Price").val(0);
            $("#Price").attr("disabled", true)
            $('.money-text').text('Thỏa thuận');
        }
    })
})

function renderConvenient() {
    var html = ''
    $.each(arr_convenient, function (index, value) {
        html += ' <div class="col-lg-4">';
        html += '<div class="checkbox">';
        html += '<label>';
        html += '<input type="checkbox" style="margin-top: 2px;" value="' + value + '" class="chk_convenient">' + value + '';
        html += '</label>';
        html += '</div>';
        html += '</div>';
    })
    $("#temp_convenient").empty().append(html);
}

function renderEnvironment() {
    var html = ''
    $.each(arr_environment, function (index, value) {
        html += ' <div class="col-lg-4">';
        html += '<div class="checkbox">';
        html += '<label>';
        html += '<input type="checkbox" style="margin-top: 2px;" value="' + value + '" class="chk_environment">' + value + '';
        html += '</label>';
        html += '</div>';
        html += '</div>';
    })
    $("#temp_environment").empty().append(html);
}

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

    $('#txtPhone1').val(userMobile);

}

//init map
function initLatiLongMap(latitude, longitude) {
    var posVietNam = { lat: 16.4498, lng: 107.5624 };
    var zoomSize = 5;
    if (latitude && longitude) {
        posVietNam = { lat: parseFloat(latitude), lng: parseFloat(longitude) };
        zoomSize = 18;
    }
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: zoomSize,
        center: posVietNam
    });
    marker = new google.maps.Marker({
        map: map,
        draggable: true,
        animation: google.maps.Animation.DROP,
        position: posVietNam
    });
    google.maps.event.addListener(marker, 'dragend', function (event) {
        document.getElementById("Latitude").value = this.getPosition().lat();
        document.getElementById("Longitude").value = this.getPosition().lng();
    });
    //autocomplete = new google.maps.places.Autocomplete(
    //    (document.getElementById('address')), { types: ['geocode'] });
    var input = document.getElementById('Address');
    autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.addListener('place_changed', fillInAddress);
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
                    temp += '<div class="photo-item" style="padding:5px; border: 2px solid transparent; box-sizing: padding-box; position: relative">';
                    temp += '<img class="img-responsive" style="height: 115px; width: 100 %" src="' + fileSrc + '" data-index="' + index + '" alt="' + fileName + '"/>';
                    temp += '<span class="rmFile" data-id="0" data-index="' + index + '" data-remove-filename="' + fileName + '" style="top: 10px; right: 10px; color: #795548;; cursor: pointer;color:red">X</span>';
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
    };
});
$('body').on('click', '.rmFile', function () {
    var element = $(this);
    var indexImage = $(this).attr('data-index');
    var idImage = $(this).attr('data-id');
    if (idImage != 0) {
        var param = {
            fileID: idImage
        }
        param = JSON.stringify(param);
        var svr1 = new AjaxCall("api/file/delete", param);
        svr1.callServicePOST(function (data) {
            if (data) {
                element.parent().remove();
            }
        });
    } else {
        for (var i = 0; i < arrImageAsUrl.length; i++) {
            if (arrImageAsUrl[i].index == indexImage) {
                arrImageAsUrl.splice(i, 1);
                arrTotalImage.splice(i, 1);
                $(this).parent().remove();
                break;
            }
        }
    }
});

//Loại BDS
function LoadLandType(typeExchange) {
    $('#LandTypeID').empty().trigger('change');
    console.log(typeExchange)
    if (typeExchange == 1) {
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
    } else {
        GetLandTypeRent(function (err, result, msg) {
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
function GetLandTypeSale(callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/landtype/getalltypesale",
        success: function (data) {
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        },
    });
};
function GetLandTypeRent(callback) {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/landtype/getalltyperent",
        success: function (data) {
            callback(false, data, null);
        },
        error: function (xhr, error) {
            callback(true, null, error);
        },
    });
};
function LoadLandCategory() {
    $('#LandCategoryID').empty().trigger('change');
    if ($("#LandTypeID").val() != null) {
        GetLandCategory($("#LandTypeID").val(), function (err, result, msg) {
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
    } else {
        var data = {
            id: 0,
            text: '-- Loại --',
        };

        var newOption = new Option(data.text, data.id, false, false);
        $('#LandCategoryID').append(newOption).trigger('change');
    }


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

        var totalPrice = price * 1000000;
        strFirstTotalPrice = replaceCommas(totalPrice.toString());
        var strTotalPrice = strFirstTotalPrice + " VNĐ";
    }
    else if (Unit == "2") //Tỷ
    {
        var totalPrice = price * 1000000000;
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
        var totalPrice = area * price * 1000000;
        strFirstTotalPrice = replaceCommas(totalPrice.toString());
        var strTotalPrice = strFirstTotalPrice + "VNĐ";
    }
    $("#TotalPrice").val(strTotalPrice);
    $("#DecimalTotalPrice").val(totalPrice);
}

function CheckValid() {
    if ($('#chk_price').prop('checked') == false) {
        if ($('#Price').val() <= 0) {
            toastr.error('Vui lòng nhập giá!');
            return false;
        }
        if ($('#Price').val() > 1000000000) {
            toastr.error('Giá tiền chưa hợp lệ. Giá tiền hợp lệ nên từ 0 đến 1.000.000');
            return false;
        }
    }
    if ($('#radioDT2').prop('checked') == true) {
        if ($('#txtPhone2').val() == '') {

            toastr.error('Vui lòng nhập số điện thoại 2!');
            return false;
        }
        if ($('#txtPhone2').length > 11) {

            toastr.error('Số điện thoại tối đa 11 số!');
            return false;
        }
    }

    if ($('#txtPhone2').val() != "") {
        if ($('#txtPhone2').length > 11) {
            toastr.error('Số điện thoại tối đa 11 số!');
            return false;
        }
    }

    if ($("#Title").val() == "") {
        //$("#Title").parent().find(".errorMessage").html("Vui lòng nhập tiêu đề!");
        //$("#Title").parent().find(".errorMessage").show();
        $("#Title").focus();
        toastr.error("", "Vui lòng nhập tiêu đề", 30000);
        return false;
    }
    else if ($("#LandTypeID").val() == 0) {
        $("#LandTypeID").focus();
        toastr.error("", "Vui lòng chọn hình thức", 30000);
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
    } else if ($("#Captcha").val() == "") {
        $("#Captcha").focus();
        toastr.error("", "Vui lòng nhập mã captcha ", 30000);
        return false;
    } else {
        return true;
    }
}

function hideErrorMessage(e) {
    if ($(e).val() != "" || $(e).val() != 0) {
        $(e).parent().find(".errorMessage").hide();
    }
}

function getLandNews() {
    $.ajax({
        type: 'GET',
        data: {},
        contentType: 'application/json',
        dataType: "json",
        url: _Host + "api/landnews/getbyid?landNewsID=" + landID,
        success: function (data) {
            console.log(data)
            if (data.TypeExchange == 1) {
                $("#radioBan").prop("checked", true);
                $("#radioChothue").prop("checked", false);
                LoadLandType(1);
            } else {
                $("#radioChothue").prop("checked", true);
                $("#radioBan").prop("checked", false);
                LoadLandType(2);
            }
            setTimeout(function () {
                $('#LandTypeID option[value="' + data.LandTypeID + '"]').attr('selected', 'selected');
                $('#LandTypeID').select2().trigger('change');;
                LoadLandCategory();
                setTimeout(function () {
                    $('#LandCategoryID option[value="' + data.LandCategoryID + '"]').attr('selected', 'selected');
                    $('#LandCategoryID').select2({});
                }, 1000)
            }, 2000)

            setTimeout(function () {
                $('#ProvinceID option[value="' + data.ProvinceID + '"]').attr('selected', 'selected');
                $('#ProvinceID').select2().trigger('change');
                setTimeout(function () {
                    $('#DistrictID option[value="' + data.DistrictID + '"]').attr('selected', 'selected');
                    $('#DistrictID').select2().trigger('change');
                }, 1000)
                setTimeout(function () {
                    $('#WardID option[value="' + data.WardID + '"]').attr('selected', 'selected');
                    $('#WardID').select2().trigger('change');
                }, 2000)
            }, 3000)

            $("#Address").val(data.Address);
            if (data.LatiLongTude != '') {
                var arrLatLong = data.LatiLongTude.split(',');
                initLatiLongMap(arrLatLong[0], arrLatLong[1]);
            } else {
                initMap();
            }

            $("#LProjectID").val(data.LProjectID);

            $("#Address").val(data.Address);
            $("#Title").val(data.Title);
            $("#Description").html(data.Description.replace(/<br \/>/g, "&#13;&#10;"));
            $("#Area").val(data.Area);
            $("#Price").val(data.Price);

            setTextMoneyAllowDecimal($("#Price"));

            $("#Facade").val(data.Facade);
            $("#Entry").val(data.Entry);
            $("#LegalStatus").val(data.LegalStatus);
            $("#HouseDirection").val(data.HouseDirection);
            $("#BalconyDirection").val(data.BalconyDirection);
            $("#NumberFloor").val(data.NumberFloor);
            $("#NumberBedroom").val(data.NumberBedroom);
            $("#NumberWC").val(data.NumberWC);
            $("#Furniture").val(data.Furniture);
            $("#LatiLongTude").val(data.LatiLongTude);

            $("#txtPhone1").val(data.Agent.Phone1);
            $("#txtPhone2").val(data.Agent.Phone2);


            if (data.Convenient != '') {
                var arrConvenient = data.Convenient.split(',');
                $.each(arrConvenient, function (index, value) {
                    arr_checked_convenient.push(value);
                })
                var html = ''
                $.each(arr_convenient, function (index, value) {                  
                    html += ' <div class="col-lg-4">';
                    html += '<div class="checkbox">';
                    html += '<label>';
                    var arraycontainsturtles = (arrConvenient.indexOf(value) > -1);
                    if (arraycontainsturtles) {
                        html += '<input type="checkbox" checked style="margin-top: 2px;" value="' + value + '" class="chk_convenient">' + value + '';
                    } else {
                        html += '<input type="checkbox" style="margin-top: 2px;" value="' + value + '" class="chk_convenient">' + value + '';
                    }
                    html += '</label>';
                    html += '</div>';
                    html += '</div>';
                })              
                $("#temp_convenient").empty().append(html);
            }
            else {
                renderConvenient();
            }

            if (data.Environment != '') {
                var arrEnvironment = data.Environment.split(',');
                $.each(arrEnvironment, function (index, value) {
                    arr_checked_environment.push(value);
                })
                var html = ''
                $.each(arr_environment, function (index, value) {
                    html += ' <div class="col-lg-4">';
                    html += '<div class="checkbox">';
                    html += '<label>';
                    var arraycontainsturtles = (arrEnvironment.indexOf(value) > -1);
                    if (arraycontainsturtles) {
                        html += '<input type="checkbox" checked style="margin-top: 2px;" value="' + value + '" class="chk_environment">' + value + '';
                    } else {
                        html += '<input type="checkbox" style="margin-top: 2px;" value="' + value + '" class="chk_environment">' + value + '';
                    }
                    html += '</label>';
                    html += '</div>';
                    html += '</div>';
                })
                $("#temp_environment").empty().append(html);
            } else {
                renderEnvironment();
            }

            landnews.ID = data.ID;
            landnews.TypeExchange = data.TypeExchange;
            landnews.Image = data.Image;
            landnews.Code = data.Code;
            landnews.LandNewsScheduleID = data.LandNewsScheduleID;
            landnews.AgentID = data.AgentID;
            landnews.Status = data.Status;
            landnews.IsPublished = data.IsPublished;
            landnews.IsDelete = data.IsDelete;
            agent.ID = data.AgentID;

            if (data.LandFiles.length != 0) {
                var temp = '';
                $.each(data.LandFiles, function (index,value) {
                    temp += '<div class="photo-item" style="padding:5px; border: 2px solid transparent; box-sizing: padding-box; position: relative">';
                    temp += '<img class="img-responsive" style="height: 115px; width: 100 %" src="' + _Host + 'fileman/Uploads/LandNews/' + value.FileName + '" data-index="' + index + '" alt="' + value.FileName + '"/>';
                    temp += '<span class="rmFile" data-id="'+value.ID+'" data-index="' + index + '" data-remove-filename="' + value.FileName + '" style="top: 10px; right: 10px; color: #795548;; cursor: pointer;color:red">X</span>';
                    temp += '</div>';
                })
                $(".img-preview").append(temp);
            }

        },
        error: function (xhr, error) {

        }
    });
}

//Lưu thông tin bán dự án
function SubmitSalePlan() {
    if (CheckValid() == true) {
        landnews.Title = $("#Title").val();
        landnews.Alias = new commonService().getSeoTitle($('#Title').val());
        landnews.Description = $("#Description").val().replace(/\r?\n/g, '<br />');
        landnews.Address = $("#Address").val();
        landnews.LandTypeID = $("#LandTypeID").val() != "" ? parseInt($("#LandTypeID").val()) : 0;
        landnews.LandTypeName = $("#LandTypeID option:selected").text();
        landnews.LandCategoryID = $("#LandCategoryID").val() != "" ? parseInt($("#LandCategoryID").val()) : 0;
        landnews.ProvinceID = $("#ProvinceID").val() != "" ? parseInt($("#ProvinceID").val()) : 0;
        landnews.DistrictID = $("#DistrictID").val() != "" ? parseInt($("#DistrictID").val()) : 0;
        landnews.UserID = $("#UserId").val() != "" ? $("#UserId").val() : "";
        landnews.WardID = $("#WardID").val() != "" ? parseInt($("#WardID").val()) : 0;
        landnews.LProjectID = $("#LProjectID").val() != "" ? parseInt($("#LProjectID").val()) : 0;
        landnews.Area = $("#Area").val() != "" ? parseInt($("#Area").val()) : 0;
        landnews.Price = $("#Price").val() != "" ? parseFloat(numberReplaceCommas($("#Price").val())) : 0;
        landnews.TotalPrice = $("#Price").val() != 0 ? $('.money-text').html() : "Thỏa thuận";
        landnews.DecimalTotalPrice = $("#Price").val() != "" ? parseFloat(numberReplaceCommas($("#Price").val())) : 0;
        landnews.Unit = $("#Unit option:selected").text();//$("#Unit").val(),
        landnews.Facade = $("#Facade").val() != "" ? parseInt($("#Facade").val()) : 0;
        landnews.Entry = $("#Entry").val() != "" ? parseInt($("#Entry").val()) : 0;
        landnews.LegalStatus = $("#LegalStatus").val();
        landnews.HouseDirection = $("#HouseDirection").val();
        landnews.BalconyDirection = $("#BalconyDirection").val();
        landnews.NumberFloor = $("#NumberFloor").val() != "" ? parseInt($("#NumberFloor").val()) : 0;
        landnews.NumberBedroom = $("#NumberBedroom").val() != "" ? parseInt($("#NumberBedroom").val()) : 0;
        landnews.NumberWC = $("#NumberWC").val() != "" ? parseInt($("#NumberWC").val()) : 0;
        landnews.Furniture = $("#Furniture").val();
        landnews.LatiLongTude = $("#LatiLongTude").val();
        landnews.Convenient = arr_checked_convenient.toString();
        landnews.Environment = arr_checked_environment.toString();

        if(landnews.ID == 0){
            landnews.TypeExchange = typeExchange;
            landnews.Image = "";
            landnews.Code = "";
            landnews.LandNewsScheduleID = 2;
            landnews.AgentID = 0;
            landnews.Status = false;
            landnews.IsPublished = false;
        }

        agent.Name = $("#UserName").val(),
        agent.Address = $("#UserAddress").val(),
        agent.Phone1 = $("#UserMobile").val(),
        agent.Phone2 = $("#txtPhone2").val(),
        agent.Phone3 = "",
        agent.PhoneShow = $("#hdValueDT").val() == 1 ? $("#UserMobile").val() : $("#txtPhone2").val(),
        agent.Email = $("#UserEmail").val(),
        agent.UserId = $("#UserId").val()

        //File hình ảnh
        var formData = new FormData();

        $.each(arrTotalImage, function (index, value) {
            formData.append('file' + index, value.file);
        });
        formData.append('captcha', JSON.stringify($("#Captcha").val()));
        formData.append('landnews', JSON.stringify(landnews));
        formData.append('agent', JSON.stringify(agent));

        $.ajax({
            url: _Host + "PostingNews2/Create",
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            dataType: "json",
            success: function (result) {
                if (result.status == true) {
                    //alert(result.code);
                    if (landnews.ID == 0) {
                        bootbox.confirm({
                            title: "Thông báo",
                            message: "Mã bài tin của bạn là: " + result.code,
                            buttons: {
                                cancel: {
                                    label: 'Đóng',
                                    className: 'btn-primary'
                                },
                                confirm: {
                                    label: 'Quản lý tin',
                                    className: 'btn-success'
                                }
                            },
                            callback: function (result) {
                                window.location = '/thanh-vien/tin-dang.html';
                            }
                        });
                        $("#btnSalePlan").attr('disabled', 'disabled');
                        toastr.info("", "Đăng tin thành công", 3000);

                    } else {
                        bootbox.confirm({
                            title: "Thông báo",
                            message: "Chỉnh sửa thành công.Mã bài tin của bạn là: " + result.code,
                            buttons: {
                                cancel: {
                                    label: 'Đóng',
                                    className: 'btn-primary'
                                }
                            },
                            callback: function (result) {
                                window.location = '/thanh-vien/tin-dang.html';
                            }
                        });
                        $("#btnSalePlan").attr('disabled', 'disabled');
                        toastr.info("", "Chính sửa tin thành công", 3000);
                    }


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


commonService = function () {
    this.getSeoTitle = function (input) {
        if (input == undefined || input == '')
            return '';
        //Đổi chữ hoa thành chữ thường
        var slug = input.toLowerCase();

        //Đổi ký tự có dấu thành không dấu
        slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
        slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
        slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
        slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
        slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
        slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
        slug = slug.replace(/đ/gi, 'd');
        //Xóa các ký tự đặt biệt
        slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
        //Đổi khoảng trắng thành ký tự gạch ngang
        slug = slug.replace(/ /gi, "-");
        //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
        //Phòng trường hợp người nhập vào quá nhiều ký tự trắng
        slug = slug.replace(/\-\-\-\-\-/gi, '-');
        slug = slug.replace(/\-\-\-\-/gi, '-');
        slug = slug.replace(/\-\-\-/gi, '-');
        slug = slug.replace(/\-\-/gi, '-');
        //Xóa các ký tự gạch ngang ở đầu và cuối
        slug = '@' + slug + '@';
        slug = slug.replace(/\@\-|\-\@|\@/gi, '');

        return slug;
    }
}
function CheckEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function formatNumber(yourNumber) {
    if (yourNumber) {
        var components = yourNumber.toString().split(".");
        components[0] = components[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return components.join(".");
    } else {
        return "0";
    }
}
function numberReplaceCommas(x) {
    return x.toString().replace(/\,/g, '');
}
function testDecimals(currentVal) {
    var count;
    currentVal.match(/\./g) === null ? count = 0 : count = currentVal.match(/\./g);
    return count;
}
function replaceCommas(yourNumber) {
    var components = yourNumber.toString().split(".");
    if (components.length === 1)
        components[0] = yourNumber;
    components[0] = components[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    if (components.length === 2)
        components[1] = components[1].replace(/\D/g, "");
    return components.join(".");
}

//Bỏ dấu tiếng việt
function stringToSlug(str) {
    // remove accents
    var from = "àáãảạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệđùúủũụưừứửữựòóỏõọôồốổỗộơờớởỡợìíỉĩịäëïîöüûñç",
        to = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeduuuuuuuuuuuoooooooooooooooooiiiiiaeiiouunc";
    for (var i = 0, l = from.length; i < l; i++) {
        str = str.replace(RegExp(from[i], "gi"), to[i]);
    }

    str = str.toLowerCase()
        .trim()
        .replace(/[^a-z0-9\-]/g, '-')
        .replace(/-+/g, '-');
    return str;
}