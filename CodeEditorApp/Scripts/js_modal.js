

/* ========================================================================== *
*                                                                             *
*                   File and Folder Tree                                      *
*                                                                             *
*  ========================================================================== */
$.fn.extend({
    treed: function (o) {

        var openedClass = 'glyphicon-minus-sign';
        var closedClass = 'glyphicon-plus-sign';

        if (typeof o != 'undefined') {
            if (typeof o.openedClass != 'undefined') {
                openedClass = o.openedClass;
            }
            if (typeof o.closedClass != 'undefined') {
                closedClass = o.closedClass;
            }
        }

        //initialize each of the top levels
        var tree = $(this);
        tree.addClass("tree");
        tree.find('li').has("ul").each(function () {
            var branch = $(this); //li with children ul
            branch.prepend("<i class='indicator glyphicon " + closedClass + "'></i>");
            branch.addClass('branch');
            branch.on('click', function (e) {
                if (this == e.target) {
                    var icon = $(this).children('i:first');
                    icon.toggleClass(openedClass + " " + closedClass);
                    $(this).children().children().toggle();
                }
            })
            branch.children().children().toggle();
        });

        tree.find('#file-item').each(function () {
            $(this).prepend("<i class='indicator glyphicon glyphicon-file'></i>");
        });

        tree.find('#file-item>a').each(function () {
            $(this).on('click', function () {
                tree.find('#file-item>a').each(function () {
                    $(this).css("color", "#369");
                });
                $(this).css('color', 'black');
            });
        });
        //fire event from the dynamically added icon
        tree.find('.branch .indicator').each(function () {
            $(this).on('click', function () {
                $(this).closest('li').click();
            });
        });

        tree.find('.branch>a').each(function () {
            $(this).click(function (e) {
                e.preventDefault();
            });

            /*
                var url = $(this).attr("href");

                
                    var vars = {};
                    var hashes = url.split("?")[1];
                    var hash = hashes.split("=");

                    for (var i = 0; i < hash.length; i++) {
                        params = hash[i].split("=");
                        vars[params[0]] = params[1];
                    }
                    return vars;
            */

            $(this).dblclick(function (e) {
                window.location.replace($(this).attr("href"));
            });
        });
    }
});



//Initialization of treeviews
$('#tree1').treed();
$('#tree2').treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
$('#tree3').treed({ openedClass: 'glyphicon-chevron-right', closedClass: 'glyphicon-chevron-down' });

/***************************       File Tree END         ***************************/



/* ========================================================================== *
*                                                                             *
*                   Document ON ready                                         *
*                                                                             *
*  ========================================================================== */

// On document Ready commands
$(document).ready(function () {

    $("#btn1").click(function () {
        $("#test1").text("Hello world!");
    });


    $(function () {
        $('form').on('submit', function () {        // form er class yfir formið "on submit"
            var form = $(this);                     // búa til variable form undir "þessarri" fyrirspurn
            $.ajax({
                url: form.attr('action'),           // attr er í raun að draga gildi einhvers ákveðinns t.d. action í form
                data: form.serialize(),             // serialize tekur öll gildi og nöfn innsláttar í formi í stað þess að harðkóða einhver gildi
                method: 'POST',
                success: function (returnData) {    // Þegar Protocol fær success indicator hendir hann út nýju falli með inntöku
                    console.log(returnData);
                }
            });
        });
    });

    // CreateProject-Form
    $("#AddFile").click(function () {
        $("#AddedFiles").prepend(
            "<p>New File Here!</p> ");
    });

    // CreateProject-Form
    $("#AddFolder").click(function () {
        $("AddedFolders").prepend("<p>Folder Added!  </p>");
    });
});

/***************************       Document ON ready END         ***************************/