import onvif from "onvif";
import { networkInterfaces } from "os";
import { mainWindow } from "../main";

const username = "admin";
const password = "admin1357";

let networks: any[] = [];
export let cams: any[] = [];
export let _cams: any[] = [];

onvif.Discovery.on("device", function (cam) {
  cam.username = username;
  cam.password = password;
  cam.connect(() => {
    try {
      cam.getStreamUri({ protocol: "RTSP" }, (err: any, stream: any) => {
        cams.push({
          name: cam.hostname,
          type: "rtsp",
          hostname: cam.hostname,
          username: cam.username,
          password: cam.password,
          stream_url: stream.uri,
        });
        mainWindow.webContents.send("discovery", cams);
      });
    } catch (error) {
      console.log(cam.hostname);
    }
  });
});

export const custom_discovery = () => {
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
          { hostname: discovery_ip, username: username, password: password },
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
            _cams.push({
              name: cam.hostname,
              type: "rtsp",
              hostname: cam.hostname,
              username: cam.username,
              password: cam.password,
              stream_url: stream.uri,
            });
            mainWindow.webContents.send("discovery", _cams);
          });
        })
        .catch((ip: string) => {});
    }
  });
};

export const discovery = () => {
  cams = [];
  onvif.Discovery.probe({ timeout: 2000, resolve: true }, () => {
    // 2초 이내로 못찾으면, 커스텀 디스커버리 동작!
    if (!cams.length) custom_discovery();
  });
};
