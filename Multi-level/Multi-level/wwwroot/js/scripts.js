/*!
* Start Bootstrap - Shop Homepage v5.0.6 (https://startbootstrap.com/template/shop-homepage)
* Copyright 2013-2023 Start Bootstrap
* Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-shop-homepage/blob/master/LICENSE)
*/
// This file is intentionally blank
// Use this file to add JavaScript to your project

$.fn.extend({
    treed: function (o) {

        //var openedClass = 'bi-caret-down';
        //var closedClass = 'bi-caret-right';
        var openedClass = '';
        var closedClass = '';

        if (typeof o != 'undefined') {
            if (typeof o.openedClass != 'undefined') {
                openedClass = o.openedClass;
            }
            if (typeof o.closedClass != 'undefined') {
                closedClass = o.closedClass;
            }
        };

        //initialize each of the top levels
        var tree = $(this);
        tree.addClass("tree");
        //tree.find('li').has("ul").each(function () {
        //    var branch = $(this); //li with children ul
        //    branch.prepend("<i class='indicator bi " + closedClass + "'></i>");
        //    branch.addClass('branch');
        //    branch.on('click', function (e) {
        //        if (this == e.target) {
        //            var icon = $(this).children('i:first');
        //            icon.toggleClass(openedClass + " " + closedClass);
        //            $(this).children().children().toggle();
        //        }
        //    })
        //    $(this).children().children().toggle();
        //    branch.children().children().toggle();
        //});
        //fire event from the dynamically added icon
        tree.find('.branch .indicator').each(function () {
            $(this).on('click', function () {
                $(this).closest('li').click();
            });
        });
        //fire event to open branch if the li contains an anchor instead of text
        tree.find('.branch>a').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
        //fire event to open branch if the li contains a button instead of text
        tree.find('.branch>button').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
    }
});

//Initialization of treeviews

$('#tree1').treed();

$('#tree2').treed({ openedClass: 'bi-folder2-open', closedClass: 'bi-folder2' });

$('#tree3').treed({ openedClass: 'bi-people-fill', closedClass: 'bi-person' });
