

$(document).ready(function () {

    var news = $("#news");
    var galery = $("#galery");

    galery.owlCarousel({
        loop: true,
        margin: 20,
        nav: false,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 3
            },
            1000: {
                items: 4
            }
        }
    })

    news.owlCarousel({
        loop: true,
        dots: false,
        margin: 20,
        nav: true,
        navText: ['<i class=" text-muted far fa-arrow-alt-circle-left fa-3x" aria-hidden="true"></i>', '<i class=" text-muted far fa-arrow-alt-circle-right fa-3x" aria-hidden="true"></i>'],
        
        responsive: {
            0: {
                items: 1,
                nav: false,
              
               
            },
            600: {
                items: 1,
                 //nav: false

            },
            1000: {
                items: 1
            }
        }
    })

});




