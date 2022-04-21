const path = require("path");
const ffmpeg = require("fluent-ffmpeg");
const { Writable } = require("stream");

var cmd = ffmpeg("rtsp://admin:admin1357@170.101.20.211:554/stream1")
  .outputFormat("flv")
  .outputFps(25)
  .audioCodec("copy")
  .videoCodec("copy")
  .pipe();
cmd.on("data", (chunk) => {
  console.log("ffmpeg just wrote " + chunk.length + " bytes");
});

// console.log(cmd);
// ffmpeg(path.resolve(__dirname, "./dental_sample.mp4"))
//   .on("start", () => {
//     console.log("start");
//   })
//   .on("end", () => {
//     console.log("end");
//   })
//   .size('1920x1080')
//   .autopad()
//   .fps(25)
//   .save('./output.mp4');f

//   ffmpeg.ffprobe(path.resolve(__dirname, "./output.mp4"), function(err, metadata) {
//     console.log(metadata);
//   })

// var ffstream = command.pipe();
// ffstream.on("data", (chunk) => {
//   console.log("ffmpeg just wrote " + chunk.length + " bytes");
// });
// ffstream.on("end", () => {
//   console.log("end2");
// });

// setInterval(() => {}, 1000);
