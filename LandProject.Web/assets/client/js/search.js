﻿LoadLandTypeSearch();
LoadProvinceSearch();

//Loại BDS
function LoadLandTypeSearch() {
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
//Load tỉnh thành
function LoadProvinceSearch() {
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
function LoadDistrictsSearch() {
    GetDistricts($("#Province").val(), function (err, result, msg) {
        $('#District').empty().trigger('change');
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
