$(document).ready(function() {
    var url = window.location.href;

    $('.image-1').on('click',function(){
        $('.image-selected').val(url + "images/landscape.jpg");
    });

    $('.image-2').on('click',function(){
        $('.image-selected').val(url + "images/people.jpg");
    });

    $('.image-3').on('click',function(){
        $('.image-selected').val(url + "images/pizza.jpg");
    });

    $('.image-4').on('click',function(){
        $('.image-selected').val(url + "images/car.jpg");
    });
});