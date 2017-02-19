
//Функции для динамического изменения временной отметки

function DateCarer()
{
    var messages = $('li .mes');
    for(i = 0; i < messages.length; i++)
    {
        id = messages[i].attr('id');
        console.log(id);
        TimeSpanChanger(id);
    }
}

function TimeSpanChanger(id) {
    var today = new Date();

    var date = $('#' + id).attr('data'); //getting message date
    var diffMs = (today - date); // milliseconds between now & message date

    $('#' + id + ' .timeSpan').html("<span class='glyphicon glyphicon-time'></span>" + GetTimeSpan());
    //Debug purposes
    console.log($('#' + id + ' .timeSpan'));

    function inDays() {
        return Math.round(diffMs / 86400000); // days
    }

    function inHours() {
        return Math.round((diffMs % 86400000) / 3600000); // hours
    }

    function inMins() {
        return Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes
    }

    function GetTimeSpan()
    {
        var time = "";
        if (inMins() < 2)
        {
            time = "Just now";
        }
        else if (inHours() < 1)
        {
            time = inMins() + " mins ago";
        }
        else if (date.getUTCDate() != today.getUTCDate())
        {
            time =  inHours() + "Yesterday";
        }
        else if(inHours() < 24)
        {
            time = inHours() + " hours ago";
        }
        else
        {
            var month = date.getUTCMonth() + 1; //months from 1-12
            var day = date.getUTCDate();
            var year = date.getUTCFullYear();

            time = day + "/" + month + "/" + year;
        }


        return time;
    }



}