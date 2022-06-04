var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Disable send button until connection is established
$("#sendMessage").prop('disabled', true);

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");

    var sender = $("#sender").val();


    if (encodedMsg.includes(sender)) {
        li.className = 'list-group-item list-group-item-info d-flex justify-content-end';
    }
    else if (encodedMsg.includes('bot')) {
        li.className = 'list-group-item list-group-item-danger';
    } else {
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
    var encodedMsg = sender + " says " + msg;
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


    event.preventDefault();

});


//$(document.getElementById("chat").animate({ scrollTop: $("#chat").scrollTop() }, 400);

$('#chat').click(function () {
    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
    return false;
});