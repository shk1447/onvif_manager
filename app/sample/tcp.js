var net = require("net");
var guid = require("guid");
console.log(guid);
var watson = function (message, data) {};

module.exports = function (port, cluster) {
  var clients = [];
  var cluster_node = [];
  function connect_client(cluster) {
    console.log("test");
    var cluster = cluster;
    var client = net.connect(
      { port: cluster.port, host: cluster.host },
      function () {
        console.log(cluster.host + ":" + cluster.port + " 접속 성공!");
        cluster_node.push(cluster.host + ":" + cluster.port);
        console.log(cluster_node);
        var data = "Hello, client!  Here's some metadata!22222";
        var _guid = guid.raw();
        var buf = Buffer.from(
          `{"s":"Normal","guid":"${_guid}","sreq":true,"exp":"2022-04-26T15:43:46.15343+09:00","len":${data.length},"md":{"Request":"Initialization"}}\r\n\r\n${data}`,
          "utf-8"
        );
        console.log(buf);
        client.write(buf);
      }
    );

    client.on("data", function (data) {
      var message = "";
      var watson = {};
      var str = Buffer.from(data).toString("utf8", 0, data.length);
      console.log(str);
      var resp = str.split(`\r\n\r\n`);
      try {
        switch (resp.length) {
          case 2: {
            watson = JSON.parse(resp[0]);
            message = resp[1];
            break;
          }
          case 1: {
            message = resp[0];
            break;
          }
        }
        console.log(watson);
        console.log(message);
      } catch (error) {
        console.log(error);
        console.log(str);
      }

      // var buf = Buffer.from(`{"s":"Normal","len":37,"md":{"foo":"bar"}}\r\n\r\nHello, client!  Here's some metadata!`, 'utf-8');
      // console.log(buf)
      // client.write(data);
    });
    client.on("end", function (data) {
      console.log(data);
    });
    client.on("error", function (error) {
      console.log("error : " + error);
    });
    client.on("close", function (data) {
      console.log("server disconnected");
      var destroy_index = cluster_node.findIndex((node) => {
        return node === cluster.host + ":" + cluster.port;
      });
      if (destroy_index >= 0) {
        cluster_node.splice(destroy_index, 1);
      }
    });
  }

  connect_client({ port: 1234, host: "localhost" });

  /*
    var server = net.createServer()

    server.on('error', function(err) {
        console.log('err : ' + err);
    })

    server.on('connection', function(socket) {
        clients.push(socket);
        console.log('Buffer size : ' + socket.bufferSize);

        console.log('---------server details -----------------');

        var address = server.address();
        var port = address.port;
        var family = address.family;
        var ipaddr = address.address;
        console.log('Server is listening at port' + port);
        console.log('Server ip :' + ipaddr);
        console.log('Server is IP4/IP6 : ' + family);

        var lport = socket.localPort;
        var laddr = socket.localAddress;
        console.log('Server is listening at LOCAL port' + lport);
        console.log('Server LOCAL ip :' + laddr);

        console.log('------------remote client info --------------');

        var rport = socket.remotePort;
        var raddr = socket.remoteAddress;
        var rfamily = socket.remoteFamily;

        console.log('REMOTE Socket is listening at port' + rport);
        console.log('REMOTE Socket ip :' + raddr);
        console.log('REMOTE Socket is IP4/IP6 : ' + rfamily);

        console.log('--------------------------------------------')
        socket.on('data', function(data){
            try {
                var recv_json = JSON.parse(data.toString());
                if(recv_json.type === "addCluster" && !cluster_node.includes(socket.remoteAddress + ":" + recv_json.data)) {
                    connect_client({port:recv_json.data, host:socket.remoteAddress});
                    broadcast(JSON.stringify({type:"addCluster",data:{port:recv_json.data, host:socket.remoteAddress}}), socket);
                }
            } catch (error) {
                console.log('recv error : ', error);
            }
        });
        socket.on('error', function(error){
            console.log('error :' + error);
        });
        socket.on('close', function() {
            console.log('client disconnected')
            var destroy_index = clients.findIndex((sock) => {return sock === socket});
            if(destroy_index >= 0) {
                clients.splice(destroy_index,1);
            }
        })

        function broadcast(message, sender) {
            clients.forEach(function (client) {
                // Don't want to send it to sender
                if (client === sender) return;
                client.write(message);
            });
        }
    })

    server.listen(port,'0.0.0.0', function(){
        console.log('listening on ' + port);
        if(cluster) {
            connect_client(cluster);
        }
    })
    */
  // 서버 접근
};
