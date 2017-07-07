<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="EchartTest.WebForm2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <title></title>
    <script src="js/jquery-1.7.min.js"></script>
    <script src="js/sockjs-0.3.min.js"></script>
    <script src="js/stomp.js"></script>
    <link href="main.css" rel="stylesheet" />

</head><body lang="en">
   

    <div id="first" class="box">
      <h2>Received</h2>
      <div></div>
      <form><input autocomplete="off" placeholder="Type here..."></input></form>
    </div>

    <div id="second" class="box">
      <h2>Logs</h2>
      <div></div>
    </div>
  
    <script type="text/javascript">
        function openmq() {
            var ws = new SockJS('http://192.168.1.235:15674/stomp');
            var client = Stomp.over(ws);
            client.heartbeat.outgoing = 0;
            client.heartbeat.incoming = 0;
            client.send('EHL_MONITOR_QUEUE', { 'reply-to': '/temp-queue/foo' }, new Date());
            var on_connect = function (x) {
                id = client.subscribe("EHL_MONITOR_QUEUE", function (m) {
                    var reversedText = m.body.split("").reverse().join("");
                    client.send(m.headers['reply-to'], { "content-type": "text/plain" }, reversedText);
                });
            };
            var on_error = function () {
                console.log('error');
            };
            client.connect('admin', 'admin', on_connect, on_error, '/');
        }
    </script>
    <script>
        var ws = new SockJS('http://192.168.1.235:15674/stomp');
        var client = Stomp.over(ws);
        client.heartbeat.outgoing = 0;
        client.heartbeat.incoming = 0;
        client.debug = function (e) {
            //$('#second div').append($("<code>").text(e));
            try {
                //if (e == "Whoops! Lost connection to undefined") {
                //    openmq();
                //}
                //else
                    $('#second div').append($("<code>").text(e));
            }
            catch (err)
            { }
        };
        // default receive callback to get message from temporary queues
        client.onreceive = function (m) {
            
            //$('#first div').append($("<code>").text(m.body));
            client.send('EHL_MONITOR_QUEUE', { 'reply-to': '/temp-queue/foo' }, new Date());
        }

        var on_connect = function (x) {
            id = client.subscribe("EHL_MONITOR_QUEUE", function (m) {
                var reversedText = m.body.split("").reverse().join("");
                client.send(m.headers['reply-to'], { "content-type": "text/plain" }, reversedText);
            });
        };
        var on_error = function () {
            console.log('error');
        };
        client.connect('admin', 'admin', on_connect, on_error, '/');

        //$('#first form').submit(function () {
        //    var text = $('#first form input').val();
        //    if (text) {
        //        client.send('EHL_MONITOR_QUEUE', { 'reply-to': '/temp-queue/foo' }, text);
        //        $('#first form input').val("");
        //    }
        //    return false;
        //});
    </script>
</body></html>
