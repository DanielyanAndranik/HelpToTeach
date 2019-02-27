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
});