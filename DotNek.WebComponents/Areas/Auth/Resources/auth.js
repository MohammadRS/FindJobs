var googleAuthId = '$0$';
var facebookAppId = '$1$';
var culture = '$2$';
var applicantProfile = '$3$';
var companyProfile = '$4$';
var typeApplicant = '$5$';
var typeCompany = `$6$`;
var LoginWithEmail = `$7$`;
var SendEmailValidateUrl = `$8$`;
var LoginWithGoogle = `$9$`;
var LoginWithFacebook = `$10$`;
var EmailNotValid = `$11$`;
var VerifyCodeNotValid = `$12$`;
var colorAqua = '$13$';
var colorTeal = '$14$';
colorAqua = '#' + colorAqua;
colorTeal = '#' + colorTeal;
if (document.querySelector(".loginbox_open") != null) {
    document.querySelector(".loginbox_open").addEventListener("click",
        (function () {
            document.querySelector('#loginbox').style.display = 'block';
        }));
}
if (document.querySelector(".loginbox_close") != null) {
    document.querySelector(".loginbox_close").addEventListener("click",
        (function () {
            document.querySelector('#loginbox').style.display = 'none';
        }));
}



function OpenTab(evt, divName, color) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");


    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(divName).style.display = "block";
    document.getElementById(divName).style.visibility = "visible";
    evt.currentTarget.className += " active";
}

window.onload = function () {
    google.accounts.id.initialize({
        client_id: googleAuthId,
        callback: handleCredentialResponse
    });
    google.accounts.id.renderButton(
        document.getElementById("buttonDiv"),
        { theme: "filled_blue", size: "large", width: "300" }  // customization attributes
    );
    //LOGINBOX
    emailregx = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

    //LOGIN/SIGNUP
    document.querySelector('#loginbox #email').onchange = function () {
        document.querySelector('#email').style.backgroundColor = '#FFFFFF';
        document.querySelector('#verifyCode').value = '';
    };
    document.querySelector('#loginbox #verifyCode').onchange = function () {
        document.querySelector('#verifyCode').style.backgroundColor = '#FFFFFF';
    };
    document.querySelector("#loginbox #verifyBtn").onclick = function () {
        isvalid = true;
        if (emailregx.test(document.querySelector('#email').value) == false) { document.querySelector('#email').style.backgroundColor = colorTeal; isvalid = false; } else document.querySelector('#email').style.backgroundColor = '#FFFFFF';
        if (isvalid) {
            grecaptcha.execute();
        } else {
            document.getElementById("errorMessage").innerText = EmailNotValid;
        }
    };

    document.querySelector("#loginbox #loginBtn").onclick = function () {
        var isvalid = true;
        if (emailregx.test(document.querySelector('#email').value) == false) { document.querySelector('#email').style.backgroundColor = colorAqua; isvalid = false; } else document.querySelector('#email').style.backgroundColor = '#FFFFFF';
        if (document.querySelector('#verifyCode').value == '') { document.querySelector('#verifyCode').style.backgroundColor = co; isvalid = false; } else document.querySelector('#verifyCode').style.backgroundColor = '#FFFFFF';
        if (isvalid) {
            document.querySelector('#loginBtn').disabled = true;
            document.querySelector('#email').disabled = true;
            document.querySelector('#verifyCode').disabled = true;
            var type = document.querySelector(".tablinks.active").getAttribute("tabType");
            fetch(LoginWithEmail, {
                method: 'Post',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    type: type,
                    Email: document.querySelector('#email').value,
                    Code: document.querySelector('#verifyCode').value
                })
            })

                .then(response => response.json())
                .then(data => {
                    if (data.data.messageCode == 0) {
                        localStorage.setItem("AuthToken", data.data.token);
                        document.getElementById("loginbox").style.display = "none";
                      
                        if (type == typeApplicant)
                            window.location.href = applicantProfile;
                        if (type == typeCompany)
                            window.location.href = companyProfile;
                        window.open(document.querySelector('#prev_link').textContent = '_self');
                    }
                    if (data.data.messageCode == 4) {
                        document.getElementById("errorMessage").innerText = VerifyCodeNotValid;
                        document.querySelector('#email').disabled = false;
                        document.querySelector('#verifyCode').disabled = false;
                        document.querySelector('#verifyCode').style.backgroundColor = colorTeal;
                        document.querySelector('#loginBtn').disabled = false;
                    }
                })

                .catch((err) => {
                    document.querySelector('#email').disabled = false;
                    document.querySelector('#verifyCode').disabled = false;
                    document.querySelector('#verifyCode').style.backgroundColor = colorTeal;
                    document.querySelector('#loginBtn').disabled = false;
                })
        }
    };
}


function EmailVerifyCallback(token) {
    document.getElementById("errorMessage").innerText = "";
    var type = document.querySelector(".tablinks.active").getAttribute("tabType");
    const verifyBtn = document.querySelector('#verifyBtn');
    verifyBtn.disabled = true;
    verifyBtn.style.backgroundColor = colorTeal;
    document.querySelector('#verifyCode').disabled = false;
    fetch(SendEmailValidateUrl, {
        method: 'Post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Type: type,
            Email: document.querySelector('#email').value,
            Captcha: token
        })
    })

        .then(response => response.json())
        .then(data => {
            if (data.messageCode == 0) {

                let t = 59;
                document.querySelector('#loginBtn').disabled = false;
                var text = verifyBtn.value;
                verifyBtn.style.backgroundColor = colorAqua;
                const interval = setInterval(function () {
                    t -= 1;
                    verifyBtn.value = "00:" + ('0' + t).slice(-2)
                    if (t < 1) {
                        clearInterval(interval)
                        verifyBtn.disabled = false;
                        verifyBtn.style.backgroundColor = '#225566';
                        verifyBtn.value = text;
                    }
                }, 1000);
            }
            else {
                document.querySelector('#verifyCode').disabled = true;
                document.querySelector('#verifyBtn').displayed = false;
                document.querySelector('#loginBtn').disabled = true;
            }

        }).catch((err) => { console.log(err) })
    grecaptcha.reset();
}




function handleCredentialResponse(response) {
    if (response != null) {
        fetch(LoginWithGoogle + "?token=" + response.credential, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.data.messageCode == 0) {
                    localStorage.setItem("AuthToken", data.data.token);
                    document.getElementById("loginbox").style.visibility = "hidden";
                    debugger;
                    console.log(data.data);
                    window.location.href = applicantProfile;
                }
            })

            .catch(err => console.log(err))
    }
}






//LOGIN WITH FACEBOOK 
function onFacebookSignIn(response) {
    if (response.authResponse) {
        FB.getLoginStatus(function (response) {
            var facebook_token = response.authResponse.accessToken;

            fetch(LoginWithFacebook + "?FacebookToken=" + facebook_token, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.data.messageCode == 0) {
                        localStorage.setItem("AuthToken", data.data.token);
                        document.getElementById("loginbox").style.visibility = "hidden";
                        window.location.href = applicantProfile;
                    }
                })

                .catch(err => console.log(err));
        })


    }
}


if (document.querySelector('.loginbox_open') != null) {
    document.querySelector('.loginbox_open').addEventListener('click', function () {
        if (document.querySelector(".zmenu") != null) document.querySelector(".zmenu").classList.remove("zmenu--active");
        this.classList.remove("zmenu__btn--active");
        if (document.querySelector(".zmenu__nav") != null) document.querySelector(".zmenu__nav").classList.remove("zmenu__nav--active");
        if (document.querySelector(".zmenu__btn--active") != null) document.querySelector(".zmenu__btn--active").classList.remove("zmenu__btn--active");
    });
}


