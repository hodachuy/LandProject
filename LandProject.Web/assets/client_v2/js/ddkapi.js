var mbndiframefeatureList = {
    init: function () {
        //null != document.getElementById("mbnd_id_feature_list") && jQuery.ajax({
        //    url: mbndApi.lroot + "/mbndapi.php?mbndrequest=listFeature&v=" + mbndApi.version
        //}).done(function (n) {
        //    console.log(n)
        //    $("#mbnd_id_feature_list").html(n.HTML)
        //})
    }
},
    mbndiframefeature = {
        init: function () {
            null != document.getElementById("mbnd_id_feature") && jQuery.ajax({
                url: mbndApi.lroot + "/mbndapi.php?mbndrequest=featureDetail&v=" + mbndApi.version
            }).done(function (n) {
                $("#mbnd_id_feature").html(n.HTML)
            })
        }
    },
    mbndiframevidoeo = {
        init: function () {
            null != document.getElementById("mbnd_id_video") && $.ajax({
                url: mbndApi.lroot + "/mbndapi.php?mbndrequest=video&v" + mbndApi.version
            }).done(function (n) {
                $("#mbnd_id_video").html(n.HTML)
            })
        }
    },
    mbndFengshui = {
        init: function () {
            null != document.getElementById("mbnd_id_fengshui") && jQuery.ajax({
                url: mbndApi.lroot + "/mbndapi.php?mbndrequest=fengshui&v=" + mbndApi.version
            }).done(function (n) {
                $("#mbnd_id_fengshui").html(n.HTML)
            })
        },
        getDataFengshui: function () {
            if (null != document.getElementById("mbnd_id_fengshui")) {
                var n = "aaa" + $("#mbnd_form_fengshui").serialize();
                if (n.indexOf("mbnd_fengshui_year=0") > 0) return void alert("Vui lĂ²ng chá»n nÄƒm sinh");
                if (n.indexOf("&mbnd_fengshui_orientation=0") > 0) return void alert("Vui lĂ²ng chá»n hÆ°á»›ng");
                if (n.indexOf("&mbnd_fengshui_gender") <= 0) return void alert("Vui lĂ²ng chá»n giá»›i tĂ­nh");
                $(".mbnd_waiting_fengshui").show(), $.post(mbndApi.lroot + "/mbndapi.php?mbndrequest=fengshuipost&v=" + mbndApi.version, $("#mbnd_form_fengshui").serialize()).done(function (n) {
                    if (n.HTML.length > 0) {
                        null != document.getElementById("mbnd-fengshui-container_id") && $("#mbnd-fengshui-container_id").html(n.HTML), null != document.getElementById("mbnd_fengshui_imgshow_id") && $("#mbnd_fengshui_imgshow_id").attr("src", $("div.divBody div p img").attr("src")), mbndFengshui.data = [];
                        var i = "";
                        $("div.mbnd-fengshui-container div.divBody > div").each(function (n) {
                            switch (n) {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                    i += '<div class="mbnd_fengshui_person">', i += '     <span class="mbnd_fengshui_person_title">', i += this.childNodes[1].textContent, i += "     </span>", i += '     <span class="mbnd_fengshui_person_content">', i += this.childNodes[2].textContent, i += "     </span>", i += "</div>";
                                    break;
                                case 6:
                                    break;
                                case 7:
                                    i += '<div class="mbnd_fengshui_orientation_good">', i += '     <span class="mbnd_fengshui_orientation_good_title">', i += "           H&#432;&#7899;ng t&#7889;t", i += "     </span>", i += '     <span class="mbnd_fengshui_orientation_good_content">', i += this.children[2].innerHTML, i += "     </span>", i += "</div>";
                                    break;
                                case 8:
                                    i += '<div class="mbnd_fengshui_orientation_not_good">', i += '     <span class="mbnd_fengshui_orientation_not_good_title">', i += "           H&#432;&#7899;ng x&#7845;u ", i += "     </span>", i += '     <span class="mbnd_fengshui_orientation_not_good_content">', i += this.children[2].innerHTML, i += "     </span>", i += "</div>";
                                    break;
                                case 9:
                                    $(".mbnd_fengshui_refre_source").html(this.innerHTML)
                            }
                        }), i.length > 0 && (i = "<span class='mbnd_fengshui_person_home'>Gia Ch&#7911;</span>" + i, $("div.mbnd-fengshui-container").html(i)), $(".mbnd_waiting_fengshui").hide(), $("#mbnd_fengshui_id_show").modal("show")
                    }
                }), $(window).on("keyup", function (n) {
                    27 === (n.keyCode || n.which) && $(".mbnd_fengshui_button_close_top").click()
                })
            }
        }
    },
    mbndCalendarEvent = {
        init: function () {
            null != document.getElementById("mbnd_id_calendar_event") && jQuery.ajax({
                url: mbndApi.lroot + "/mbndapi.php?mbndrequest=calendarevent&v=" + mbndApi.version
            }).done(function (n) {
                $("#mbnd_id_calendar_event").html(n.HTML)
            })
        }
    },
    mbndApi = {
        version: "000a2957e1ed090c3687df3cd7c4a948VersionNew",
        lroot: "http://static.muabannhadat.vn",
        initMbndApi: function () {
            mbndiframefeature.init(),
            mbndiframevidoeo.init(),
            mbndFengshui.init(),
            mbndCalendarEvent.init(),
            mbndiframefeatureList.init(),
            $("div.col-md-8 > div.panel  div.cussrow div.listing-zone div.listing-tag-a div.listing-tag a").length > 0 && $("div.col-md-8 > div.panel  div.cussrow div.listing-zone div.listing-tag-a div.listing-tag a").each(function (n) {
                var i = $.trim($(this).attr("href"));
                (i = i.replace(/(\r\n|\n|\r)/gm, "")).indexOf("/du-an-vip/") > 0 && $(this).attr("onclick", 'mbndApi.clickTrackingProject("' + window.encodeURIComponent(i) + '");')
            })
        },
        clickTrackingArticle: function (n) {
            $.ajax({
                url: "http://static.muabannhadat.vn/mbndapi.php",
                data: {
                    mbndrequest: "checkTrackingArticle",
                    article_id: n
                },
                type: "POST",
                dataType: "json",
                error: function () { },
                success: function (n) { }
            })
        },
        clickTrackingProject: function (n) {
            $.ajax({
                url: "http://static.muabannhadat.vn/mbndapi.php",
                data: {
                    mbndrequest: "checkTrackingProject",
                    project_id: n
                },
                type: "POST",
                dataType: "json",
                error: function () { },
                success: function (n) { }
            })
        }
    };