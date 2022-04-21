import onvif from "onvif";
import { networkInterfaces } from "os";
import { mainWindow } from "../main";

const username = "admin";
const password = "admin1357";

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

  for (var i = 1; i <= 255; i++) {
    var discovery_ip =
      ipv4_arr[0] + "." + ipv4_arr[1] + "." + ipv4_arr[2] + "." + i;
    new onvif.Cam(
      { hostname: discovery_ip, username: username, password: password },
      () => {}
    );
  }
});

let cams: any[] = [];

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

export const discovery = () => {
  cams = [];
  onvif.Discovery.probe({ timeout: 5000, resolve: true }, () => {
    console.log(cams);
  });
};
