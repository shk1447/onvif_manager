const path = require("path");
const ffmpeg = require("fluent-ffmpeg");
const { Writable } = require("stream");

var command = ffmpeg(path.resolve(__dirname, "./dental_sample.mp4"))
  .on("start", () => {
    console.log("start");
  })
  .on("end", () => {
    console.log("end");
  })
  .seekInput(10)
  .duration(10)
  .outputFormat("flv")
  .videoCodec("copy");

var ffstream = command.pipe();
ffstream.on("data", function (chunk) {
  console.log("ffmpeg just wrote " + chunk.length + " bytes");
});

// setInterval(() => {}, 1000);
