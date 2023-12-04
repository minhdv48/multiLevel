// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var invoice = {
    init: function () {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        document.getElementById('startDateFilter').valueAsDate = addDays(firstDay,1);
        var tdate = addDays(new Date(), 10);
        document.getElementById('endDateFilter').valueAsDate = tdate;
        setTimeout(function () { invoice.tblemployeelave(); }, 1000);
        $('#btnBenefit').on('click', function () {
            $('#mdbenefit').modal('show');
            invoice.tblBenefit();
        });
        $('#btnFilterStatus').on('click', function () {
            invoice.tblemployeelave();
        });
    },
    tblemployeelave: function () {
        var objTable = $("#tblInvoice");
        objTable.bootstrapTable('destroy');
        objTable.bootstrapTable({
            method: 'get',
            url: '/Invoice?handler=InvoiceInfo',
            queryParams: function (p) {
                var param = $.extend(true, {
                    startDate: $('#startDateFilter').val(),
                    endDate: $('#endDateFilter').val(),
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
                    title: "Ngày hoá đơn",
                    align: 'left',
                    valign: 'left',
                    formatter: function (value, row, index) {
                        var html = '';
                        html = site.formatDate(row.dateInvoice);
                        return html;
                    }
                },
                {
                    field: "fullName",
                    title: "Họ tên",
                    align: 'left',
                    valign: 'left',
                },
                {
                    field: "invoiceCode",
                    title: "Số hoá đơn",
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
                            const myArray = row.path.split(";");
                            for (var i = 1; i < myArray.length; i++) {
                                html += '<a href="' + myArray[i] + '" target="_blank"> <img src="' + myArray[i] + '" width="80" height="80"/> </a>';
                            }
                        }
                        return html;
                    }
                },
            ],
            onLoadSuccess: function (data) {
                if (data.status = false) {
                    alert(data.msg);
                }
            },
        })
    },
    tblBenefit: function () {
        var objTable = $("#tblbenefit");
        objTable.bootstrapTable('destroy');
        objTable.bootstrapTable({
            method: 'get',
            url: '/Invoice?handler=BenefitList',
            queryParams: function (p) {
                var param = $.extend(true, {
                    startDate: $('#startDateFilter').val(),
                    endDate: $('#endDateFilter').val(),
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
                    field: "profileId",
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
                    field: "isPay",
                    title: "Tình trạng",
                    align: 'left',
                    valign: 'left',
                    formatter: function (value, row, index) {
                        if (row.isPay != null && row.isPay == false) {
                            return 'Chưa thanh toán';
                        }
                        if (row.isPay != '' && row.isPay == true) {
                            return 'Đã thanh toán';
                        }
                        return 'N/A';
                    }
                },
                {
                    field: "",
                    title: "Hành động",
                    align: 'left',
                    valign: 'left',
                    formatter: function (value, row, index) {
                        var html = '';
                        if (row.isPay != null && row.isPay == false) {
                            return '<a class="payment" data-id="' + row.profileId + '"><i class="bi bi-credit-card"></i></a>';
                        }
                        return html;
                    },
                    events: {
                        'click .payment': function (e, value, row, index) {
                            if (confirm('Bạn muốn chắc chắn tri ân cho người này?')) {
                                $.ajax({
                                    type: 'post',
                                    url: '/Invoice?handler=Benefit',
                                    data: {
                                        profileId: row.profileId,
                                        startDate: $('#startDateFilter').val(),
                                        endDate: $('#endDateFilter').val(),
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (rp) {
                                        if (rp.status) {
                                            toastr.success(rp.message);
                                            objTable.bootstrapTable("refresh");

                                        } else {
                                            toastr.error(rp.message);
                                        }

                                    }
                                });
                            };
                        }
                    }
                },
            ],
            onLoadSuccess: function (data) {
                if (data.status = false) {
                    alert(data.msg);
                }
            },
        })
    }
}
$(document).ready(function () {
    invoice.init();
});

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}