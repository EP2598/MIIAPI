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

function submitLogin() {
    let objReq = {
        Email: document.getElementById("loginEmail").value,
        Password: document.getElementById("loginPassword").value
    };

    $.ajax({
        type: "post",
        url: "../Login/Auth/",
        data: objReq
    }).done((res) => {
        switch (res.statusCode) {
            case 200:
                window.location.replace("../Admin/")
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: res.message,
                })
        }
    }).fail((err) => {
        console.log(err);
    });
}