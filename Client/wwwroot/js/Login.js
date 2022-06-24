function showLogin() {
    $("#modalForm").modal("toggle");
    let formBody = "";

    formBody += `
<form id="multiForm" class="needs-validation" novalidate>
                
                            <div class="form-row">
                                <div class="col-md-4 mb-3">
                                    <label for="loginEmail">Email</label>
                                    <input type="email" class="form-control" id="loginEmail" placeholder="Email" required>
                                    <div class="valid-feedback">
                                    </div>
                                    <div class="invalid-feedback">
                                        This field is required.
                                    </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-md-4 mb-3">
                                    <label for="loginPassword">Password</label>
                                    <input type="password" class="form-control" id="loginPassword" placeholder="Password" required>
                                    <div class="valid-feedback">
                                    </div>
                                    <div class="invalid-feedback">
                                        This field is required.
                                    </div>
                                </div>
                            </div>
                            <button class="btn btn-primary" type="button" onclick="submitLogin()">Login</button>
</form>`;

    $("#modalFormBody").html(formBody);
    $("#modalTitle").html("Login");
}

function showRegister() {

}

function authMe() {
    let objReq = {
        Email: document.getElementById("floatingInput").value,
        Password: document.getElementById("floatingPassword").value
    };
    $.ajax({
        type: "post",
        url: "../Login/Auth/",
        data: objReq
    }).done((res) => {
        console.log(res);
        switch (res.statusCode) {
            case 200:
                setTimeout(function () {
                    window.location.replace("../Admin/");
                }, 5500);
                Swal.fire({
                    icon: 'success',
                    title: 'Login successful!',
                    html: 'You will be redirected in <span></span> seconds.',
                    timer: 5000,
                    didOpen: () => {
                        timerInterval = setInterval(() => {
                            Swal.getHtmlContainer().querySelector('span')
                                .textContent = (Swal.getTimerLeft() / 1000)
                            .toFixed(0)
                        }, 100);
                    },
                    willClose: () => {
                        clearInterval(timerInterval);
                    }
                }).then(function () {
                    window.location.replace("../Admin/");
                });
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Login failed!',
                    text: res.message,
                })
        }
    }).fail((err) => {
        console.log("Login - Error Log");
        console.log(err);
    });
}

function resetMe()
{
    var formForget = "";

    formForget = `
                                <div class="form-floating mb-3">
                                    <input type="email" class="form-control" id="forgotInput" placeholder="name@example.com" required>
                                    <label for="forgotInput">Email address</label>
                                    <div class="invalid-feedback">
                                        This field is required.
                                    </div>
                                    <div class="valid-feedback">
                                    </div>
                                </div>

                                <div class="d-grid">
                                    <button class="btn btn-lg btn-primary btn-login text-uppercase fw-bold mb-2" type="button" onclick="requestChange()">Request</button>
                                </div>
                `;

    let form = document.getElementById("multiForm");

    form.innerHTML = formForget;

}

function requestChange()
{
    let objReq =
    {
        Email: document.getElementById("forgotInput").value
    }

    $.ajax({
        type: "post",
        url: "../Login/ForgotPassword/",
        data: objReq
    }).done((res) => {
        console.log(res);
        switch (res.statusCode) {
            case 200:
                setTimeout(function () {
                    window.location.replace("../Login/ChangePassword/");
                }, 5500);
                Swal.fire({
                    icon: 'success',
                    title: 'Request submitted!',
                    html: 'Check your email to reset password.'
                }).then(function () {
                    window.location.replace("../Login/ChangePassword/");
                });
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Request failed!',
                    text: res.message,
                })
        }
    }).fail((err) => {
        console.log("Forget Password - Error Log");
        console.log(err);
    });

}

function doReset()
{
    let objReq =
    {
        Email: document.getElementById("resetInput").value,
        OTP: document.getElementById("resetOtp").value,
        NewPassword: document.getElementById("resetPassword").value,
        RetryPassword: document.getElementById("resetRetryPassword").value
    };

    $.ajax({
        type: "post",
        url: "../SubmitChangePassword/",
        data: objReq
    }).done((res) => {
        console.log(res);
        switch (res.statusCode) {
            case 200:
                setTimeout(function () {
                    window.location.replace("../");
                }, 5500);
                Swal.fire({
                    icon: 'success',
                    title: 'Request submitted!',
                    html: 'Password has been changed!'
                }).then(function () {
                    window.location.replace("../");
                });
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Request failed!',
                    text: res.message,
                })
        }
    }).fail((err) => {
        console.log("Change Password - Error Log");
        console.log(err);
    });
}

(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('needs-validation');
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();