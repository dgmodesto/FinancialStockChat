var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Disable send button until connection is established
$("#sendMessage").prop('disabled', true);

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "";;
    var li = document.createElement("li");

    var sender = $("#sender").val();


    if (user.includes(sender)) {
        var encodedMsg = msg + " - ( " + user + " )";
        
        li.className = 'list-group-item list-group-item-info d-flex justify-content-end';
    }
    else if (user.includes('bot')) {
        var encodedMsg = "( " + user + " ) - " + msg;
        li.className = 'list-group-item list-group-item-danger';
    } else {
        var encodedMsg = "( " + user + " ) - " + msg;
        li.className = 'list-group-item list-group-item-warning';
    }


    li.textContent = encodedMsg;
    $("#messagesList").append(li);
});

connection.start().then(function () {
    $("#sendMessage").prop('disabled', false);
}).catch(function (err) {
    return console.error(err.toString());
});


$("#sendMessage").click(function () {

    var sender = $("#sender").val();
    var receiver = $("#receiver").val();
    var message = $("#message").val();


    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg + " - ( " + sender + " )";
    var li = document.createElement("li");
    li.className = 'list-group-item list-group-item-info d-flex justify-content-end';


    li.textContent = encodedMsg;
    $("#messagesList").append(li);

    if (receiver != "") {
        //send to a user
        connection.invoke("SendMessageToGroup", sender, receiver, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        //send to all
        connection.invoke("SendMessage", sender, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    $("#message").val("");

    event.preventDefault();
});


//$(document.getElementById("chat").animate({ scrollTop: $("#chat").scrollTop() }, 400);

$('#chat').click(function () {
    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
    return false;
});



