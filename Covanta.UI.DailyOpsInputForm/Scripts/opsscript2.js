$(document).ready(function () {
    $('TD', '#options').filter(function () { // select all the TDs
        return $(this).text() == '1900'; // keep the ones that have
        // '1900' as their HTML
    }).hide();
});
