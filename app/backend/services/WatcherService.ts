import { Application } from "express-ws";
import ws from "ws";

// 향후 Onvif 생성자도 api로 변경 필요.
import { Watcher } from "../methods/watcher";
const WatcherInstance = new Watcher(5000);

export class WatcherService {
  constructor(app: Application) {
    app.ws("/watcher/discovery", this.discovery);
    app.post("/watcher", this.postWatcher);
  }

  postWatcher = async (req: any, res: any, next: any) => {
    const result = await WatcherInstance.post(
      req.body.watcher,
      req.body.name,
      req.body.port
    );
    res.status(200).send(result);
  };

  discovery = (ws: ws, req: any) => {
    ws.on("message", (message: string) => {
      WatcherInstance.discovery();
      console.log(message);
    });
    WatcherInstance.on("discovery", (watchers: any, apps: any) => {
      console.log(watchers);
      ws.send(JSON.stringify({ watchers: watchers, apps: apps }));
    });
  };
}
