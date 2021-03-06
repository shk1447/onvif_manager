import express from "express";
import expressWebSocket from "express-ws";
import { EdgeService } from "./services/EdgeService";
import { RtspService } from "./services/RtspService";
import { WatcherService } from "./services/WatcherService";
import portscanner from "portscanner";

let app = express();

app.use("/", express.static(__dirname));
app.use(express.json());

// extend express app with app.ws()
let ws = expressWebSocket(app, null, {
  // ws options here
  wsOptions: {
    perMessageDeflate: false,
  },
});

const wss = ws.getWss();
// wss.on("connection", (ws, req) => {
//   ws.on("message", (message) => {
//     console.log(message);
//   });
// });

new RtspService(ws.app);
new WatcherService(ws.app);
var edgeService = new EdgeService(ws.app);

export default new Promise((resolve, reject) => {
  portscanner.findAPortNotInUse(9090, (err, port) => {
    app.listen(port, "0.0.0.0", async () => {
      console.log("listen port : ", port);
      await edgeService.initialize(port);
      resolve(port);
    });
  });
});
