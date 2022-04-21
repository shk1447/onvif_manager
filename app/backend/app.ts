import express from "express";
import expressWebSocket from "express-ws";

let app = express();

// extend express app with app.ws()
let ws = expressWebSocket(app, null, {
  // ws options here
  perMessageDeflate: true,
} as any);

app.listen(9090);

export default ws.app;
