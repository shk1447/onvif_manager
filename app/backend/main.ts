import { resolve } from "path";
import { app, BrowserWindow, ipcMain } from "electron";
import unhandled from "electron-unhandled";

unhandled({
  logger: (err: Error) => {
    if (err) console.log("occured unhandled error!");
  },
  showDialog: false,
  reportButton: (error) => {
    console.log("Report Button Initialized");
  },
});

import "./app";

import { WindowManager } from "./methods/electron";

import WindowController from "./controller/WindowController";
console.log(WindowController);

import ThreadController from "./controller/ThreadController";
console.log(ThreadController);

import { discovery } from "./methods/onvif";
import { start, stop } from "./methods/ffmpeg";

process.env.app_path = app.getAppPath();

ipcMain.on("rtsp/start", async (event, args) => {
  start(args);
});

ipcMain.on("rtsp/stop", async (event, args) => {
  stop(args);
});

ipcMain.handle("discovery", async (event, args) => {
  discovery();
  return false;
});

ipcMain.handle("exit", async (event, args) => {
  WindowManager.exit(args.path);
  return false;
});

ipcMain.handle("maximize", async (event, args) => {
  return WindowManager.maximize(args.path);
});

ipcMain.handle("minimize", async (event, args) => {
  WindowManager.minimize(args.path);
  return true;
});

ipcMain.handle("createWindow", async (event, args) => {
  WindowManager.create(args.path, {
    webPreferences: {
      blinkFeatures: "CSSStickyPosition",
      nodeIntegration: true,
      nodeIntegrationInWorker: false,
      contextIsolation: false,
      webSecurity: false,
      sandbox: false,
      enableRemoteModule: true,
    } as any,
    minWidth: 640,
    minHeight: 480,
    width: 720,
    height: 640,
    kiosk: false,
    fullscreen: false,
    fullscreenable: true,
    resizable: true,
    frame: false,
    show: false,
  });
  return true;
});

app.on("window-all-closed", function () {
  if (process.platform != "darwin") app.quit();
});

app.on("ready", () => {
  WindowManager.create("/", {
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
    icon: resolve("../favicon.ico"),
  });
});
