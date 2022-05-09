import { resolve } from "path";
import { app, BrowserWindow, ipcMain, dialog } from "electron";
import unhandled from "electron-unhandled";

unhandled({
  logger: (err: Error) => {
    if (err) console.log("occured unhandled error!", err);
  },
  showDialog: false,
  reportButton: (error) => {
    console.log("Report Button Initialized");
  },
});

import service from "./app";
import { WindowManager } from "./methods/electron";
process.env.app_path = app.getAppPath();

ipcMain.handle("showDialog", async (event, args) => {
  return dialog.showOpenDialogSync(null, {});
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
  WindowManager.create(
    args.path,
    {
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
    },
    parseInt(process.env.port)
  );
  return true;
});

app.on("window-all-closed", function () {
  if (process.platform != "darwin") app.quit();
});

app.on("ready", () => {
  service.then((port: number) => {
    process.env.port = port.toString();
    WindowManager.create(
      "/",
      {
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
      },
      port
    );
  });
});
