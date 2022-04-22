import app from "../app";
import ffmpeg from "fluent-ffmpeg";
import webSocketStream from "websocket-stream/stream";
import { cams } from "../methods/onvif";
app.ws(`/rtsp/:name`, (ws, req) => {
  const name = req.params.name;
  const cam = cams.find((cam) => cam.name === name);
  if (cam) {
    const stream = webSocketStream(
      ws,
      {
        binary: true,
        browserBufferTimeout: 1000000,
      },
      {
        browserBufferTimeout: 1000000,
      }
    );
    var input_uri = cam.stream_url.replace(
      "rtsp://",
      `rtsp://${cam.username}:${cam.password}@`
    );

    var cmd = ffmpeg(input_uri)
      .inputOption("-rtsp_transport", "tcp", "-buffer_size", "102400")
      .outputFormat("flv")
      .outputFps(25)
      .videoCodec("copy")
      .noAudio();
    cmd.pipe(stream);
  }
});
