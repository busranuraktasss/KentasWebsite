$(document).ready(function () {
    $("#menu_link li a").click(function (e) {
        let link = $(e.currentTarget).attr('href').slice(1);
        if (link.charAt(0) == "#") {
            event.preventDefault();
            var link_pos = $(`${link}`).position().top;
            $('html, body').animate({ scrollTop: link_pos }, 500);
        }
    });
});
