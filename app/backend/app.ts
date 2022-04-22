import express from "express";
import expressWebSocket from "express-ws";
import { RtspService } from "./services/rtsp";

let app = express();

// extend express app with app.ws()
let ws = expressWebSocket(app, null, {
  // ws options here
  perMessageDeflate: true,
} as any);

new RtspService(ws.app);

app.listen(9090);
