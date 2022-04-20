import { app, BrowserWindow, ipcMain } from "electron";

// var dotNetFunction = edge.func(path.resolve(app.getAppPath(), './modules/EdgeLib.dll'));
console.log("aaaaa");
// console.log('aaa');
// dotNetFunction("Test", function(err: any, result: any) {
//   if(err) {
//     console.log(err);
//   }
//   console.log(result);
// })
//const demo = require('./demo.js')
import WindowController from "./controller/WindowController";
console.log(WindowController);

import ThreadController from "./controller/ThreadController";
console.log(ThreadController);
process.env.app_path = app.getAppPath();

import onvif from "onvif";

ipcMain.handle("discovery", async (event, args) => {
  console.log("testestestset");
  onvif.Discovery.on("device", function (cam) {
    cam.username = "admin";
    cam.password = "admin1357";
    cam.connect(() => {
      try {
        cam.getStreamUri({ protocol: "RTSP" }, (err: any, stream: any) => {
          console.log(stream);
        });
      } catch (error) {
        console.log(cam.hostname);
        // console.log(error);
      }
    });
  });
  onvif.Discovery.probe({}, () => {
    console.log("end");
  });
  return false;
});

ipcMain.handle("exit", async (event, args) => {
  app.exit();
  return false;
});

ipcMain.handle("maximize", async (event, args) => {
  if (mainWindow.isMaximized()) {
    mainWindow.restore();
  } else {
    mainWindow.maximize();
  }
  return mainWindow.isMaximized();
});

ipcMain.handle("minimize", async (event, args) => {
  mainWindow.minimize();
  return true;
});

ipcMain.handle("createWindow", async (event, args) => {
  createWindow(args.path, {
    webPreferences: {
      blinkFeatures: "CSSStickyPosition",
      nodeIntegration: true,
      nodeIntegrationInWorker: false,
      contextIsolation: false,
      webSecurity: false,
      sandbox: false,
      enableRemoteModule: true,
    } as any,
    minWidth: 1600,
    minHeight: 900,
    width: 1600,
    height: 900,
    kiosk: false,
    fullscreen: false,
    fullscreenable: true,
    resizable: true,
    frame: false,
    show: false,
  });
  return true;
});

ipcMain.on("asynchronous-message", (event, arg) => {
  event.sender.send("asynchronous-reply", "ping");

  console.log(arg);
});

ipcMain.on("synchronous-message", (event, arg) => {
  console.log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
  console.log(arg); // "ping" 출력
  event.returnValue = "pong";
});

let mainWindow: BrowserWindow;

const createWindow = (
  path: string,
  options: Electron.BrowserWindowConstructorOptions
) => {
  var _window = new BrowserWindow(options);

  _window.loadURL("file://" + __dirname + "/index.html#" + path);
  _window.webContents.openDevTools();

  _window.once("ready-to-show", () => {
    _window.show();
    console.log("show!!!");
    setTimeout(() => {
      mainWindow.webContents.send("test", "test");
    }, 1000);
  });
  _window.on("closed", function () {
    _window = null;
  });
  return _window;
};

app.on("window-all-closed", function () {
  if (process.platform != "darwin") app.quit();
});

app.on("ready", () => {
  mainWindow = createWindow("/", {
    webPreferences: {
      blinkFeatures: "CSSStickyPosition",
      nodeIntegration: true,
      nodeIntegrationInWorker: false,
      contextIsolation: false,
      webSecurity: false,
      sandbox: false,
      enableRemoteModule: true,
    } as any,
    minWidth: 1600,
    minHeight: 900,
    width: 1600,
    height: 900,
    kiosk: false,
    fullscreen: false,
    fullscreenable: true,
    resizable: true,
    frame: false,
    show: false,
  });
});
