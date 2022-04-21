import ffmpeg from "fluent-ffmpeg";
import { mainWindow } from "../main";
import app from "../app";
export const stream_cams: any = {};
import webSocketStream from "websocket-stream/stream";

export const start = (cam: any) => {
  app.ws(`/rtsp/${cam.name}`, (ws, req) => {
    //
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
  });
};

export const stop = (cam: any) => {
  if (stream_cams[cam.name]) {
    console.log("test");
    stream_cams[cam.name].kill("SIGINT");
    // delete stream_cams[cam.name];
  }
};
