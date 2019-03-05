// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(document).ready(function () {
    var pathname = window.location.pathname;
    console.log(pathname);
    var tabs = $('.nav-tabs .nav-link');
    tabs.map((index, tab) => {
        if (tab.getAttribute('href') == pathname) {
            $(tab).addClass('active');
        }
    });
    $("#submitButton").click(function (event) {
        var username = $("input#username").val()
        var password = $("input#password").val();
        console.log({ username, password });
        $.ajax({
            type: "POST",
            url: "../api/Users/Login",
            data: { username, password },
            dataType: "json",
            success: function (result) {
                console.log(result);
            },
            error: function (result) {
                console.log(result);
            }
        });
        event.preventDefault();
    });
});