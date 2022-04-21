const onvif = require("onvif");
const ffmpeg = require("fluent-ffmpeg");

onvif.Discovery.on("device", function (cam) {
  cam.username = "admin";
  cam.password = "admin1357";
  cam.connect(() => {
    try {
      cam.getStreamUri({ protocol: "RTSP" }, (err, stream) => {
        console.log(stream);
      });
    } catch (error) {
      console.log(cam.hostname);
      // console.log(error);
    }
  });
});
onvif.Discovery.on("error", function (err, xml) {
  // function called as soon as NVT responds, but this library could not parse the response
  console.log("Discovery error " + err);
});
onvif.Discovery.probe();

var test = new onvif.Cam(
  { hostname: "170.101.20.211", username: "admin", password: "admin1357" },
  function (test) {
    // console.log(test);
    this.getStreamUri({ protocol: "RTSP" }, function (err, stream) {
      // console.log(stream);
    });
  }
);
