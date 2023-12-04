// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var site = {
    init: function () {
        $('#logout').on('click', function () {
            $('#frmlogout').submit();
        });
        $("#resetcaptcha").on('click', function () {
            site.resetCaptchaImage();
        });
        $('.cls-verify').on('click', function () {
            var id = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Profile?handler=ProfileInfo&id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function (xhr) {
                    
                },
                success: function (res) {
                    if (res.status) {
                        $("#mdConfirm").modal('show');
                        $('#coderefer').val(res.data.referBy);
                        $('#full-name').val(res.data.fullName);
                        $('#invoice').attr('src', res.data.pathInvoice);
                        $('#btnSave').on('click', function () {
                            if (confirm("Bạn có chắc chắn xác nhận tài khoản này không?") == true) {
                                $.ajax({
                                    type: "POST",
                                    url: "/Profile?handler=ProfileInfoVerify&id=" + id,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (res) {
                                        if (res.status) {
                                            alert(res.msg);
                                            window.location.reload();
                                        } else {
                                            alert(res.msg);
                                        }
                                    },
                                });
                            } 
                        });
                    } else {
                        alert("Không tồn tại thông tin tài khoản này.");
                    }
                },
            });
        });
        $("#btnInvoice").on('click', function () {
            $("#mdInvoice").modal('show');
        });
        $('.view').on('click', function () {
            var id = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Profile?handler=ProfileInfo&id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function (xhr) {
                },
                success: function (res) {
                    if (res.status) {
                        $('#mdInfo').modal('show');
                        $('#Rfcode').val(res.data.referBy);
                        $('#fullname').val(res.data.fullName);
                        $('#AWcode').val(res.data.amwayCode);
                        $('#phonenumber').val(res.data.phone);
                        $('#subCount').val(res.subCount);
                    } else {
                        alert("Không tồn tại thông tin tài khoản này.");
                    }
                },
            });
        });
        $('#benefit').on('click', function () {
            var id = $(this).data('id');
            var benefit = $('#hdfBenefit').val();
            var bonus = $('#hdfBonus').val();
            var total = benefit + bonus;
            console.log(total);
            $('#mdPayment').modal('show');
            $('#hdfprofileId').val(id);
            $('#txtbenefit').val(total);
        });
    },
    resetCaptchaImage: function () {
        $.ajax({
            type: "POST",
            url: "/Index?handler=ResetCaptcha&captname=" + $('#img-captcha').attr('src'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (res) {
                var anh = $('#img-captcha').attr('src', "/" + res.name);
                return anh;
            },
        });
    },
    formatDate: function (date) {
        if (date == undefined)
            return 'N/A';
        var currentTime = new Date(date);
        //var currentTime = new Date(parseInt(dateString));
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var date = day + "/" + month + "/" + year;
        return date;
    }
}
$(document).ready(function () {
    site.init();
});