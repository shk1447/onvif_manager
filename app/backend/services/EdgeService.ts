import { v4 as uuidv4 } from "uuid";
import { Application } from "express-ws";
import ws from "ws";

import { Edge } from "../methods/edge";
const EdgeInstance = new Edge();

export class EdgeService {
  edges: any;
  constructor(app: Application) {
    this.edges = {};
    app.ws("/edge/resp/:guid", this.responseMethod);
    app.ws("/edge/async/:guid", this.asyncMethod);
    app.post("/edge/exec/:name", this.postMethod);
    app.delete("/edge/exit/:name", this.postMethod);
  }

  initialize = async (port: number) => {
    const uuid = uuidv4();
    const result = await EdgeInstance.initialize(uuid, port);
    console.log(result);
  };

  responseMethod = (ws: ws, req: any) => {
    const guid = req.params.guid;
    this.edges[guid] = ws;
  };

  asyncMethod = (ws: ws, req: any) => {
    // 비동기 실행 후 리스폰스 대기.
    ws.on("message", (message) => {
      if (this.edges[req.params.guid]) {
        this.edges[req.params.guid].send(message);
      }
    });
  };

  postMethod = (req: any, res: any, next: any) => {
    /*
    1. guid 생성
    2. {
      guid: _guid,
      method: _method,
      args: _args
    }
    3. edge에서 해당 method 및 guid로 웹소켓 연결
      - guid로 dictionary 생성하여 쓰레드 관리
    4. guid를 포함하여, response로 전달
    
    5. browser에서 해당 guid로 웹소켓 연결하여 비동기 응답 기다림.
    */
    const uuid = req.query.uuid ? req.query.uuid : uuidv4();
    const method = req.params.name;
    EdgeInstance.controller(
      {
        uuid: uuid,
        method: method,
        args: req.body,
      },
      (err: any, result: any) => {
        res.status(200).send(result);
      }
    );
  };
}
