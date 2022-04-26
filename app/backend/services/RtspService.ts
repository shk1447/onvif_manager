import ffmpeg from "fluent-ffmpeg";
import webSocketStream from "websocket-stream/stream";
import { Application } from "express-ws";
import ws from "ws";

// 향후 Onvif 생성자도 api로 변경 필요.
import { Onvif } from "../methods/onvif";
const OnvifInstance = new Onvif("admin", "admin1357");

export class RtspService {
  constructor(app: Application) {
    app.ws(`/rtsp/stream/:name`, this.stream);
    app.ws(`/rtsp/discovery`, this.discovery);
  }

  discovery = (ws: ws, req: any) => {
    ws.on("message", (message) => {
      OnvifInstance.custom_discovery();
    });
    ws.binaryType = "arraybuffer";
    OnvifInstance.on("discovery", (cams: any) => {
      ws.send(JSON.stringify(cams));
    });
  };

  stream = (ws: ws, req: any) => {
    const name = req.params.name;
    const cam = OnvifInstance.cams.find((cam: any) => cam.name === name);
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
      ws.addEventListener("close", () => {
        ws.close();
        ws.terminate();
      });
      ws.addEventListener("error", () => {
        console.log("error");
      });

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
