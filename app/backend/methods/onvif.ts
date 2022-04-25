import { EventBus } from "../eventBus";
import onvif from "onvif";
import { networkInterfaces } from "os";
import { WindowManager } from "./electron";

interface IOnvif {
  username: string;
  password: string;
}

export class Onvif extends EventBus implements IOnvif {
  cams: any[];
  username: string;
  password: string;

  constructor(username: string, password: string) {
    super();
    this.cams = [];
    this.username = username;
    this.password = password;

    onvif.Discovery.on("device", function (cam) {
      cam.username = username;
      cam.password = password;
      cam.connect(() => {
        try {
          cam.getStreamUri({ protocol: "RTSP" }, (err: any, stream: any) => {
            this.cams.push({
              name: cam.hostname,
              type: "rtsp",
              hostname: cam.hostname,
              username: cam.username,
              password: cam.password,
              stream_url: stream.uri,
            });
            this.emit("discovery", [this.cams]);
          });
        } catch (error) {
          console.log(cam.hostname);
        }
      });
    });
  }

  discovery = () => {
    this.cams = [];
    onvif.Discovery.probe({ timeout: 2000, resolve: true }, () => {});
  };

  custom_discovery = () => {
    let networks: any[] = [
      {
        address: "170.101.20.36",
        family: "IPv4",
        internal: false,
      },
    ];
    this.cams = [];
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

      const tryCam = (ip: string) => {
        return new Promise((resolve, reject) => {
          var cam = new onvif.Cam(
            {
              hostname: discovery_ip,
              username: this.username,
              password: this.password,
            },
            (err: any) => {
              if (!err) resolve(cam);
              else reject(ip);
            }
          );
        });
      };

      for (var i = 1; i <= 255; i++) {
        var discovery_ip =
          ipv4_arr[0] + "." + ipv4_arr[1] + "." + ipv4_arr[2] + "." + i;

        tryCam(discovery_ip)
          .then((cam: any) => {
            cam.getStreamUri({ protocol: "RTSP" }, (err: any, stream: any) => {
              this.cams.push({
                name: cam.hostname,
                type: "rtsp",
                hostname: cam.hostname,
                username: cam.username,
                password: cam.password,
                stream_url: stream.uri,
              });
              this.emit("discovery", [this.cams]);
            });
          })
          .catch((ip: string) => {});
      }
    });
  };
}
