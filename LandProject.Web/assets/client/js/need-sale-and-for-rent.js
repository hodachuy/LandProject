
LoadProvince();
LoadDistrictID();
initMap();
LoadCaptchaCode();


function LoadCaptchaCode() {
    var url = _Host + 'PostingNews/CaptchaIndex'; // _Host trong file jquery.js
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


$('#multi-file').change(function () {
    var el = $('.img-input[data-id="cover"] .img-preview div');

    // Danh sách hình ảnh mới thêm
    var t = 0;
    for (var i = 0; i < el.length; i++) {
        if (($(el[i]).find('.img-responsive').attr('src')).indexOf('base64') > -1) {
            t++;
        }
    }
    if (checkFileExtensionImg($('#multi-file').val())) {
        var mess = CheckCoverImage(this);

        if (mess) {
            countPlaceImg = 0;
            toastr.error(mess, null, { timeOut: 5000 });
        }
        else {
            getListBase64Image(this)
                .then((listData) => {
                    loadPreviewImage(listData);
                });
        }
    } else {
        countPlaceImg = 0;
        toastr.error('Vui lòng chọn ảnh đúng định dạng (*.png | *.gif | *.jpg | *.jpeg)', null, { timeOut: 5000 });
    }
});


//Đăng tin
function SubmitBuyPlan() {
    toastr.info("", "Đăng tin Bán thành công", 3000);
}
function LoadProvince() {

    var result = {
        "citys": [
            { "ID": 48, "Name": "Phan Thiết" },
            { "ID": 3, "Name": "Hà Nội" },
            { "ID": 4, "Name": "Đà Nẵng" },
            { "ID": 38, "Name": "Đồng Tháp" },
            { "ID": 5, "Name": "Hồ Chí Minh" },
            { "ID": 30, "Name": "Cà Mau" },
            { "ID": 7, "Name": "Lâm Đồng" },
            { "ID": 11, "Name": "Quảng Ninh" },
            { "ID": 12, "Name": "Đồng Nai" },
            { "ID": 13, "Name": "Bình Thuận" },
            { "ID": 15, "Name": "Cần Thơ" },
            { "ID": 26, "Name": "Hải Phòng" },
            { "ID": 34, "Name": "Quảng Nam" },
            { "ID": 36, "Name": "Quảng Trị" },
            { "ID": 37, "Name": "Quảng Bình" },
            { "ID": 39, "Name": "Bình Định" },
            { "ID": 40, "Name": "Nghệ An" },
            { "ID": 41, "Name": "Khánh Hòa" },
            { "ID": 42, "Name": "Bến Tre" },
            { "ID": 43, "Name": "Ninh Bình" },
            { "ID": 44, "Name": "Quảng Ngãi" },
            { "ID": 45, "Name": "Đắk Lắk" },
            { "ID": 46, "Name": "Nam Định" },
            { "ID": 47, "Name": "Thái Nguyên" }
        ]
    };
    var data1 = $.map(result.citys, function (obj) {
        var newObje = {};
        newObje.id = obj.ID;
        newObje.text = obj.Name;
        return newObje;
    });
    data1.unshift({
        id: 0,
        text: '-- Chọn tỉnh thành --',
    });
    $("#ProvinceID").select2({
        data: data1
    });
    $("#Province").select2({
        data: data1
    });

};
function LoadDistrictID() {

    var result = {
        "citys": [
            { "ID": 1, "Name": "Quận 1" },
            { "ID": 3, "Name": "Quận 2" },
            { "ID": 4, "Name": "Quận 3" },
            { "ID": 38, "Name": "Quận 4" },
            { "ID": 5, "Name": "Quận 5" },
            { "ID": 30, "Name": "Quận 6" },
            { "ID": 7, "Name": "Quận 7" },
            { "ID": 11, "Name": "Quận 8" },
            { "ID": 12, "Name": "Quận 9" },
            { "ID": 13, "Name": "Quận 10" },
            { "ID": 15, "Name": "Quận 11" },
            { "ID": 26, "Name": "Quận 12" },
            { "ID": 34, "Name": "Quận Bình Thạnh" },
            { "ID": 36, "Name": "Quận Hóc Môn" },
        ]
    };
    var data1 = $.map(result.citys, function (obj) {
        var newObje = {};
        newObje.id = obj.ID;
        newObje.text = obj.Name;
        return newObje;
    });
    data1.unshift({
        id: 0,
        text: '-- Chọn Quận/Huyện --',
    });
    $("#DistrictID").select2({
        data: data1
    });
    $("#District").select2({
        data: data1
    });

};

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


//Lưu thông tin bán dự án
//Đăng tin
function SubmitSalePlan() {
    if (CheckValid() == true) {
        toastr.info("", "Đăng tin thành công", 3000);
    }
}

function CheckValid() {
    if ($("#Title").val() == "") {
        $("#Title").parent().find(".errorMessage").html("Vui lòng nhập tiêu đề!");
        $("#Title").parent().find(".errorMessage").show();
        $("#Title").focus();
        return false;
    }
    else if ($("#PlandTypeID").val() == 0) {
        $("#PlandTypeID").parent().find(".errorMessage").html("Vui lòng nhập tiêu đề!");
        $("#PlandTypeID").parent().find(".errorMessage").show();
        $("#PlandTypeID").focus();
        return false;
    } else if ($("#LandCategoryID").val() == 0) {
        $("#LandCategoryID").parent().find(".errorMessage").html("Vui lòng nhập tiêu đề!");
        $("#LandCategoryID").parent().find(".errorMessage").show();
        $("#LandCategoryID").focus();
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
