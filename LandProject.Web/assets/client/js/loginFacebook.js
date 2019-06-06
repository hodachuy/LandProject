
        //set khi custome rieng
        function checkLoginFbDialog() {
            FB.login(function (response) {
                statusChangeCallback(response);
            }, {
                //scope: 'manage_pages, pages_show_list, pages_messaging, pages_messaging_subscriptions,email',//pages_messaging,pages_messaging_phone_number,pages_messaging_subscriptions
                scope : 'public_profile,email',
                return_scopes: true
            });
        }


        // This is called with the results from from FB.getLoginStatus().
        function statusChangeCallback(response) {
            console.log('statusChangeCallback');
            console.log(response);
            // The response object is returned with a status field that lets the
            // app know the current login status of the person.
            // Full docs on the response object can be found in the documentation
            // for FB.getLoginStatus().
            if (response.status === 'connected') {
                // Logged into your app and Facebook.
                getUserInfo();
            } else {
                // The person is not logged into your app or we are unable to tell.
                console.log('Please login')             
            }
        }

        // This function is called when someone finishes with the Login
        // Button.  See the onlogin handler attached to it in the sample
        // code below.
        function checkLoginState() {
            FB.getLoginStatus(function (response) {
                statusChangeCallback(response);
            });
        }

        // init auto login
        window.fbAsyncInit = function () {
            FB.init({
                appId: '410230363104302', //FB App ID
                cookie: true,  //enable cookies to allow the server to access the session
                xfbml: true,  //parse social plugins on this page
                version: 'v3.2' //use this graph api version 3.2
            });

            // Now that we've initialized the JavaScript SDK, we call
            // FB.getLoginStatus().  This function gets the state of the
            // person visiting this page and can return one of three states to
            // the callback you provide.  They can be:
            //
            // 1. Logged into your app ('connected')
            // 2. Logged into Facebook, but not your app ('not_authorized')
            // 3. Not logged into Facebook and can't tell if they are logged into
            //    your app or not.
            //
            // These three cases are handled in the callback function.

            //FB.getLoginStatus(function (response) {
            //    statusChangeCallback(response);
            //});

        };

        // Load the SDK asynchronously
        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "https://connect.facebook.net/en_US/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));

        // Here we run a very simple test of the Graph API after login is
        // successful.  See statusChangeCallback() for when this call is made.
        function getUserInfo() {
            FB.api('/me', { fields: 'name,id,email,picture,accounts' }, function (response) {
                console.log(response)
                console.log('Successful login for: ' + response.name);
                // response account bao gồm các thông tin về trang page_id,accesstoken page
                var user = {
                    UserName: response.name,
                    Email: response.email,
                    Avatar:response.picture.data.url
                }
                login(user)
            });
        }

        function login(user) {
            var block = $('body');
            user = JSON.stringify(user);
            $.ajax({
                type: 'POST',
                url: _Host + "Account/LoginSocial",
                data: user,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //async: this._serviceAsync,
                success: function (response) {
                    if (response.status) {
                        window.location.href = "/thanh-vien/ho-so.html";
                    }
                },
                error: function (error) {                  
                    console.log(error)
                },
                beforeSend: function () {
                    $(block).block({
                        message: '<i class="fa fa-spinner fa-pulse spinner"></i>',
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
                }
            });
        }
