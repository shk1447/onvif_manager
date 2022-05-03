const edge = require("@nomis51/electron-edge-js");
import { EventBus } from "../eventBus";

interface IPayload {
  id: string;
  sync: boolean;
  method: string;
  args: any;
}

interface IEdge {
  controller: any;
}

export class Edge extends EventBus implements IEdge {
  controller: any;
  constructor() {
    super();
    this.controller = edge.func("./modules/SaigeVAD.Edge.dll");
  }

  initialize = () => {
    return new Promise((resolve, reject) => {
      this.controller(
        {
          method: "initialize",
          sync: true,
        },
        (err: any, result: any) => {
          if (err) {
            reject(err);
          } else {
            resolve(result);
          }
        }
      );
    });
  };
}
