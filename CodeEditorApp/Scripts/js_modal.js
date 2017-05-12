/*      --START--   VILLI ER AÐ VINNA HÉR   --START--   */

/* File Tree START */
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
        };

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
$('#tree2').treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });

$(document).ready(function(){
    $("#remove-member").click(function(){
        var input = document.getElementById('membertoremove').value;
        $.ajax({
            url: 'Project/RemoveMemberIfInProject',
            type: 'POST',
            contentType: 'application/json;',
            data: JSON.stringify({ email: input, projectID: @Model.ID}),
            success: function (valid)
            {
                if(valid)
                {
                    //show that id is valid
                }
                else
                {
                    //show that id is not valid
                }
            }
        });
    });

    $('#delete-file-button').on('click', function() {
        DeleteFile();
    });

    $('#file-item>a').on('click', function () {
        SaveFileContent();

        $('#file-item>a').each(function () {
            $(this).css("color", "#369");
        });
        $(this).css('color', 'black');
        var ID = $(this).parent().attr('alt');
                               
        console.log(ID);
        $.ajax({
            url: "/Project/OpenFile",
            method: "POST",
            data: { fileID : ID },
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            beforeSend:function () {

            },
            success: function (responseData) {
                console.log(responseData);
                console.log(responseData.Content);
                UpdateEditor(responseData.Content, responseData.FileType.Extension, responseData.ID);
            }

        });



        $.ajax({
            url:"/Project/SaveFile",
            method: "POST",
            data: { fileID : documentID, fileContent : content },
            dataType: "json",
            beforeeSend:function () {
                cosole.log("DocumentID: "+documentID);
            },
            success: function () {
                console.log("fucking success man!")
            }
        });
        //return false;
    });

});