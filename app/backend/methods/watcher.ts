import { EventBus } from "../eventBus";
import { networkInterfaces } from "os";
import axios from "axios";
interface IWatcher {
  port: number;
}

export class Watcher extends EventBus implements IWatcher {
  apps: any[];
  watchers: any[];
  port: number;
  constructor(port: number) {
    super();
    this.apps = [];
    this.watchers = [];
    this.port = port;
  }

  post = async (watcher: any, name: string, port: number) => {
    let ret = false;
    const exists = this.apps.find((app: any) => {
      return app.ip == watcher.ip && app.name == name;
    });
    if (!exists) {
      const url = `http://${watcher.ip}:${watcher.port}/watch`;
      const res = await axios.post(url, { name: name, port: port });

      ret = true;
    }
    return ret;
  };

  discovery = () => {
    this.watchers = [];
    this.apps = [];
    let networks: any[] = [];
    const interfaces = networkInterfaces();
    Object.keys(interfaces).forEach((name: string) => {
      networks = networks.concat(
        interfaces[name].filter((item: any) => {
          return item.family === "IPv4" && !item.internal;
        })
      );
    });

    networks.forEach((item) => {
      var ipv4_arr = item.address.split(".");

      const tryHttp = (ip: string) => {
        var url = `http://${ip}:${this.port}/status`;
        axios
          .get(url)
          .then((res) => {
            console.log(res.data);
            this.apps = this.apps.concat(
              res.data.map((app: any) => {
                return {
                  pid: app.pid,
                  ip: ip,
                  name: app.name,
                  port: app.port,
                  status: app.status,
                };
              })
            );
            this.watchers.push({
              ip: ip,
              port: this.port,
            });
            this.emit("discovery", [this.watchers, this.apps]);
          })
          .catch((err) => {
            //console.log(err);
          });
      };

      for (var i = 1; i <= 255; i++) {
        var discovery_ip =
          ipv4_arr[0] + "." + ipv4_arr[1] + "." + ipv4_arr[2] + "." + i;

        tryHttp(discovery_ip);
      }
    });
  };
}
