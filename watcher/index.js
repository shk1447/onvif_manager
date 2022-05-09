const fs = require("fs");
const path = require("path");

const electron = require("electron");

const express = require("express");
const portscanner = require("portscanner");
const expressWs = require("express-ws");
const child_process = require("child_process");
const stream = require("stream");

const config = require("./configure");
const logger = require("./logger");

var tray;
const watches = {};

const buildMenu = (watchers) => {
  var items = Object.keys(watchers).map((name) => {
    return {
      label: name,
      submenu: [
        {
          label: "PORT NUMBER",
          sublabel: watchers[name].port.toString(),
          enabled: false,
        },
        {
          label: "PROCESS ID",
          sublabel: watchers[name].process.pid.toString(),
          enabled: false,
        },
      ],
    };
  });

  items.push({
    label: "Exit",
    click: () => {
      electron.app.quit();
    },
  });
  console.log(items);
  const menu = electron.Menu.buildFromTemplate(items);

  tray.setContextMenu(menu);
};

electron.app.on("window-all-closed", () => {
  console.log("test");
});

electron.app.on("ready", () => {
  const app = express();
  app.use(express.json());

  const ws = expressWs(app, null, {});

  ws.app.get("/status", (req, res, next) => {
    res.status(200).send(
      Object.keys(watches).map((name) => {
        return {
          pid: watches[name].process
            ? watches[name].process.pid
            : "Not Working",
          name: name,
          port: watches[name].port,
          status: watches[name].status,
        };
      })
    );
  });

  ws.app.post("/watch", (req, res, next) => {
    const name = req.body.name
      ? req.body.name
      : "inference_app_" + (Object.keys(watches).length + 1);
    const port = req.body.port ? req.body.port : config.port;

    portscanner.findAPortNotInUse(port, (err, _port) => {
      watches[name] = {
        port: _port,
        status: false,
      };
      const child = child_process.spawn(
        config.app_path,
        ["-p", watches[name].port, "-ip", "0.0.0.0", "-s", name],
        {
          cwd: path.dirname(config.app_path),
        }
      );
      child.on("spawn", () => {
        console.log("success");
        watches[name]["process"] = child;
        watches[name]["status"] = true;
        buildMenu(watches);

        res.status(200).send(
          Object.keys(watches).map((name) => {
            return {
              pid: child.pid,
              name: name,
              port: watches[name].port,
              status: watches[name].status,
            };
          })
        );
      });
      child.on("error", () => {
        child.kill();
      });

      child.stdout.pipe(
        new stream.Writable({
          write(chunk, encoding, callback) {
            if (chunk) logger.info(chunk.toString().replace(/(\r\n)/g, ""));
            callback();
          },
        })
      );

      child.on("exit", function () {
        console.log("child exit");
        delete watches[name];
        buildMenu(watches);
      });
    });
  });

  ws.app.ws(`/watch/:name`, (ws, req) => {
    console.log(req.params.name);
    ws.on("message", (message) => {
      console.log(message);
    });
  });

  portscanner.findAPortNotInUse(config.port, (err, port) => {
    config.port = port;
    app.listen(port, "0.0.0.0", () => {
      console.log("listen port : ", port);

      tray = new electron.Tray(path.join(__dirname, "./favicon.ico"));
      tray.setToolTip("Saige Watcher Listening : " + port);

      buildMenu(watches);
    });
  });
});
