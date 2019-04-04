// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(document).ready(function () {
    var pathname = window.location.pathname;
    console.log(pathname);
    var links = $('ul.flex-column .nav-link');
    links.map((index, link) => {
        if (link.getAttribute('href') == pathname) {
            $(link).toggleClass('text-dark');
            $(link.parentElement).addClass('bg-white');
        }
    });
});