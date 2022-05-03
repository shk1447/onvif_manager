import { Application } from "express-ws";
import ws from "ws";

import { Edge } from "../methods/edge";
const EdgeInstance = new Edge();

export class EdgeService {
  constructor(app: Application) {
    app.ws("/method/async", this.asyncMethod);
    app.post("/method/:name", this.postMethod);
  }

  initialize = async () => {
    const result = await EdgeInstance.initialize();
    console.log(result);
  };

  asyncMethod = (ws: ws, req: any) => {
    ws.on("message", (message) => {
      console.log(message);
    });
  };

  postMethod = (req: any, res: any, next: any) => {
    res.status(200).send();
  };
}
