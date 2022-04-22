import ffmpeg from "fluent-ffmpeg";
import webSocketStream from "websocket-stream/stream";
import { cams } from "../methods/onvif";
import { Application } from "express-ws";

export class RtspService {
  constructor(app: Application) {
    app.ws(`/rtsp/:name`, this.stream);
  }

  stream = (ws: any, req: any) => {
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

      try {
        var cmd = ffmpeg(input_uri)
          .outputFormat("flv")
          .outputFps(25)
          .videoCodec("copy")
          .noAudio();

        cmd.pipe(stream);
      } catch (error) {
        console.log(error);
      }
    }
  };
}
