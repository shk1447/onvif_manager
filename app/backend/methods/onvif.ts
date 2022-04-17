import onvif from "onvif";
import ffmpeg from "fluent-ffmpeg";

const discovery = () => {};

var test = new onvif.Cam(
  { hostname: "0.0.0.0", username: "test", password: "test" },
  () => {}
);
