// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var report = {
    init: function () {
        //var date = new Date();
        //var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        //var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        //document.getElementById('monthFilter').valueAsDate = addDays(firstDay, 1);
        $('#btnBenefit').on('click', function () {
            $('#mdbenefit').modal('show');
            report.tblBenefit();
        });
        $('.benefit').on('click', function () {
            var id = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Report?handler=ProfileBonus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: { id: id, month: $('#monthFilter').val() },
                beforeSend: function (xhr) {
                },
                success: function (res) {
                    if (res.status) {
                        $('#mdPayment').modal('show');
                        $('#hdfprofileId').val(id);
                        $('#full-name').val(res.data.fullName);
                        $('#subCount').val(res.subCount);
                    } else {
                        alert("Không tồn tại thông tin tài khoản này.");
                    }
                },
            });
        });
        $('._view').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var month = $('#monthFilter').val();
            window.location.href = "/Profile?id=" + id + "&month=" + month;
            //$.ajax({
            //    type: "GET",
            //    url: "/Profile?handler=ProfileInfo",
            //    data: {
            //        id: id,
            //        month: $('#monthFilter').val()
            //    },
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    beforeSend: function (xhr) {
            //    },
            //    success: function (res) {
            //        if (res.status) {
            //            $('#mdInfo').modal('show');
            //            $('#Rfcode').val(res.data.referBy);
            //            $('#fullname').val(res.data.fullName);
            //            $('#AWcode').val(res.data.amwayCode);
            //            $('#phonenumber').val(res.data.phone);
            //            $('#totalMember').val(res.subCount);
            //            $('#totalProfit').val(res.totalProfit);
            //            var bank = "số tài khoản: " + res.data.bankAccount + " ngân hàng: " + res.data.bankName;
            //            $('#bankAcc').val(bank);
            //        } else {
            //            alert("Không tồn tại thông tin tài khoản này.");
            //        }
            //    },
            //});
        });
    },
    tblBenefit: function () {
        var objTable = $("#tblbenefit");
        objTable.bootstrapTable('destroy');
        objTable.bootstrapTable({
            method: 'get',
            url: '/Report?handler=BenefitList',
            queryParams: function (p) {
                var param = $.extend(true, {
                    month: $('#monthFilter').val(),
                    limit: p.limit,
                    offset: p.offset
                }, p);
                return param;
            },
            striped: true,
            sidePagination: 'server',
            pagination: true,
            paginationVAlign: 'bottom',
            search: false,
            pageSize: 10,
            pageList: [10, 50, 100],
            columns: [

                {
                    field: "id",
                    title: "Id",
                    align: 'left',
                    valign: 'left',

                },
                {
                    field: "fullName",
                    title: "Họ tên",
                    align: 'left',
                    valign: 'left',
                },
                {
                    field: "",
                    title: "Ảnh hoá đơn",
                    align: 'left',
                    valign: 'left',
                    formatter: function (value, row, index) {
                        var html = "";
                        if (row.path != '') {
                            html += '<a href="' + row.imgPath + '" target="_blank"> <img src="' + row.imgPath + '" width="80" height="80"/> </a>';
                        }
                        return html;
                    }
                },
                {
                    title: "Ngày tri ân",
                    align: 'left',
                    valign: 'left',
                    formatter: function (value, row, index) {
                        var html = '';
                        html = site.formatDate(row.payDate);
                        return html;
                    }
                },
            ],
            onLoadSuccess: function (data) {
                if (data.status = false) {
                    toastr.error(data.msg);
                }
            },
        })
    }
}
$(document).ready(function () {
    report.init();
});

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}