const path = require("path");
const ffmpeg = require("fluent-ffmpeg");
const { Writable } = require("stream");
var through = require("through2");

// var child_process = require("child_process");
// var test = child_process.spawn("ffmpeg");

// test.stdout.pipe(
//   new Writable({
//     write(chunk, encoding, callback) {
//       console.log(chunk.toString());
//       callback();
//     },
//   })
// );

// test.on("exit", () => {
//   console.log("exit");
// });
// child_process.exec(
//   "ffmpeg -y -re -acodec pcm_s16le -rtsp_transport tcp -i rtsp://admin:admin1357@170.101.20.211:554/stream1 -vcodec copy -af asetrate=22050 -acodec aac -b:a 96k hihihihi.mp4"
// );

var stream = through();

var cmd = ffmpeg("rtsp://admin:admin1357@170.101.20.211:554/stream1")
  .native()
  .inputOption("-rtsp_transport", "tcp")
  .outputOption(
    "-vcodec",
    "copy",
    "-af",
    "asetrate=22050",
    "-acodec",
    "aac",
    "-b:a",
    "96k"
  )
  .output("kkkkkk.mp4");
cmd.run();

// setTimeout(() => {
//   cmd.kill("SIGINT");
// }, 10000);
// var cmd = ffmpeg("rtsp://admin:admin1357@170.101.20.211:554/stream1")
//   .inputOption("-rtsp_transport", "tcp")
//   .videoCodec("copy")
//   .noAudio()
//   .outputFormat("mp4");

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
