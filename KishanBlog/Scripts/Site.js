
$(document).ready(function () {
    
    $('.upperTab li a').mouseover(function () {
        $(this).addClass('upperTabClr');
        $(this).closest('.glyphicon').css('color', 'gray');
    })
    .mouseout(function () {
        $(this).removeClass('upperTabClr');
        $(this).closest('i').css('color', 'white');
    })

    $('.lowerTabsClr li a').mouseover(function () {
        $(this).addClass('lowerTabsClr');
        $(this).closest('.glyphicon').css('color', 'red');
    })
   .mouseout(function () {
       $(this).removeClass('lowerTabsClr');
       $(this).closest('i').css('color', 'white');
   })

});