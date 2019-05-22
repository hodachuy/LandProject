var startDateEx;
var endDateEx;
var startDate = "";
var endDate = "";
var yearStart = "..";
var monthStart = "..";
var dateStart = "..";
var yearEnd = "..";
var monthEnd = "..";
var dateEnd = "..";
var _idchart = "#chart";
var today = new Date();
var dd = today.getDate();
var mm = today.getMonth() + 1;
var yyyy = today.getFullYear();

if (dd < 10) {
    dd = '0' + dd
}

if (mm < 10) {
    mm = '0' + mm
}
today = dd + '/' + mm + '/' + yyyy;

$(document).ready(function () {
    startDate = yyyy + '-' + mm + '-' + '01';
    endDate = kendo.toString(new Date(yyyy, mm, 0), "yyyy-MM-dd")
    LoadChart("days");
    LoadDatePicker('txtTungay', startDate);
    LoadDatePicker('txtDenngay', endDate);
})
$(".box").bind("change", refresh);

function refresh() {
    var chart = $("#chart").data("kendoChart"),
        series = chart.options.series,
        categoryAxis = chart.options.categoryAxis,
        baseUnitInputs = $("input:radio[name=baseUnit]"),
        aggregateInputs = $("input:radio[name=aggregate]"),
        typeInput = $("input:radio[name=seriesType]");


    for (var i = 0, length = series.length; i < length; i++) {
        series[i].aggregate = aggregateInputs.filter(":checked").val();
        series[i].type = typeInput.filter(":checked").val();
    };


    categoryAxis.baseUnit = baseUnitInputs.filter(":checked").val();

    chart.refresh();
}
SearchForDate = function () {
    $('#showDate').empty();
    $('#dateNow').empty();
    $('#dateNow').hide();
    startDate = kendo.toString($("#txtTungay").data("kendoDatePicker").value(), "yyyy-MM-dd");
    endDate = kendo.toString($("#txtDenngay").data("kendoDatePicker").value(), "yyyy-MM-dd");
    if (startDate != null) {
        yearStart = startDate.substring(0, 4)
        monthStart = startDate.substring(5, 7)
        dateStart = startDate.substring(8, 10)
    }
    if (endDate != null) {
        yearEnd = endDate.substring(0, 4)
        monthEnd = endDate.substring(5, 7)
        dateEnd = endDate.substring(8, 10)
    }
    $('#showTitle').show();
    $('#showDate').show();
    $('#dateNow').show();

    $('#showDate').append('(Từ ngày ' + dateStart + '/' + monthStart + '/' + yearStart + ' đến ngày ' + dateEnd + '/' + monthEnd + '/' + yearEnd + ')');

    if (startDate == null && endDate == null) {
        $('#showTitle').hide();
        $('#showDate').hide();
        LoadChart("days");
    }
    else {
        InitChart(_idchart, new DataSource().MasterDatasource("" + _Host + "api/visitor/getstatistic", startDate, endDate), "days")
    }
}

LoadChart = function (typeView) {
    InitChart(_idchart, new DataSource().MasterDatasource("" + _Host + "api/visitor/getstatistic", startDate, endDate), typeView)
}

DataSource = function () {
    this.MasterDatasource = function (url, startDate, endDate) {
        var total = 0;
        var data = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    //console.log(options.data);
                    var params ={
                    		StartDate: startDate,
							EndDate: endDate
                    }
                    params = JSON.stringify(params);
                    $.ajax({
                        url: url,
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data:params,                        
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
                        if(data.Items != null)
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
                model: { // ép lại kiểu để hiển thị bên chart
                    fields: {
                        Date: { type: "date" },
                        CountVisitors: { type: "number" }
                    }
                }
            },
            pageSize: 2147483647,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        });
        return data;
    }
}


function InitChart(id, DataSource, typeViewDate) {
    var html = '<p>THỐNG KÊ LƯỢT KHÁCH TRUY CẬP TẠI CỔNG</p>';
    $(id).kendoChart({
        //title: {
        //    //text: html,
        //    template: '<p>THỐNG KÊ LƯỢT KHÁCH TRUY CẬP TẠI CỔNG</p>'
        //},
        dataSource: (DataSource != undefined ? DataSource : []),
        legend: {
            position: "bottom"
        },
        chartArea: {
            background: ""
        },
        seriesDefaults: {
            type: "column",//mặc định
            style: "smooth",
            labels: {
                visible: true,
                background: "transparent",
            }
        },
        series: [{
            //type: "column",
            //startAngle: 150,
            name: "Lượt truy cập",
            field: "CountVisitors",
            categoryField: "Date",
            aggregate: "sum"
        }],
        valueAxis: {
            labels: {
                format: "{0}"
            },
            line: {
                visible: false
            },
        },
        categoryAxis: {
            baseUnit: typeViewDate,
            majorGridLines: {
                visible: false
            },
            labels:
                   {
                       rotation: "auto",
                       dateFormats:
                       {
                           minutes: "HH:mm",
                           hours: "HH:mm",
                           days: "dd/MM/yyyy",
                           months: "MM/yyyy",
                           years: "yyyy"
                       }
                   }
        },
        tooltip: {
            visible: true,
            format: "{0}",
            template: "#= series.name #: #= value #"
        }
    });
}