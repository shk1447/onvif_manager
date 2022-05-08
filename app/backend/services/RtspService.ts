import ffmpeg from "fluent-ffmpeg";
import webSocketStream from "websocket-stream/stream";
import { Application } from "express-ws";
import ws from "ws";
import { Writable } from "stream";
import moment from "moment";
import fs from "fs";
import { exec, spawn, ChildProcessWithoutNullStreams } from "child_process";

// 향후 Onvif 생성자도 api로 변경 필요.
import { Onvif } from "../methods/onvif";
const OnvifInstance = new Onvif("admin", "admin1357");

export class RtspService {
  constructor(app: Application) {
    app.ws(`/rtsp/stream/:name`, this.stream);
    app.ws(`/rtsp/discovery`, this.discovery);
    app.ws(`/rtsp/image`, this.imageStream);
    app.ws(`/rtsp/record/:name`, this.record);
  }

  discovery = (ws: ws, req: any) => {
    ws.on("message", (message) => {
      OnvifInstance.discovery();
    });
    OnvifInstance.on("discovery", (cams: any) => {
      ws.send(JSON.stringify(cams));
    });
  };

  record = (ws: ws, req: any) => {
    const name = req.params.name;
    const cam = OnvifInstance.cams.find((cam: any) => cam.name === name);
    if (cam) {
      var input_uri = cam.stream_url.replace(
        "rtsp://",
        `rtsp://${cam.username}:${cam.password}@`
      );
      try {
        var process: ChildProcessWithoutNullStreams;
        ws.on("message", (message) => {
          if (message.toString() == "start") {
            const unixtime = moment().unix();
            process = spawn(
              "ffmpeg",
              [
                "-re",
                "-acodec",
                "pcm_s16le",
                "-rtsp_transport",
                "tcp",
                "-i",
                input_uri,
                "-vcodec",
                "copy",
                "-af",
                "asetrate=22050",
                "-acodec",
                "aac",
                "-b:a",
                "96k",
                `./${name}_${unixtime}.mp4`,
              ],
              {
                cwd: "./",
              }
            );
            process.stdout.on("data", function (data) {
              console.log(data.toString());
            });

            process.stderr.on("data", function (data) {
              console.log(data.toString());
            });

            process.on("exit", function () {
              process = undefined;
            });
          } else if (message.toString() == "stop") {
            if (process) {
              process.stdin.cork();
              process.stdin.write("q");
              process.stdin.uncork();
            }
          }
        });
      } catch (error) {
        console.log(error);
      }
    }
  };

  imageStream = (ws: ws, req: any) => {
    ws.on("message", (message) => {
      console.log(message.toString("base64").length);
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
