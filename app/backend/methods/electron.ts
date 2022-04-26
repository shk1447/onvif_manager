import { app, BrowserWindow } from "electron";
interface IWindowManager {
  windows: { [path: string]: BrowserWindow };
  create: (
    path: string,
    options: Electron.BrowserWindowConstructorOptions
  ) => BrowserWindow;
  minimize: (path: string) => boolean;
  maximize: (path: string) => boolean;
  exit: (path: string) => boolean;
}

class _WindowManager implements IWindowManager {
  windows: any;
  constructor() {
    this.windows = {};
  }
  create = (
    path: string,
    options: Electron.BrowserWindowConstructorOptions,
    port?: number
  ) => {
    if (this.windows[path]) return this.windows[path];
    var _window = new BrowserWindow(options);

    _window.loadURL(
      "file://" + __dirname + "/index.html#" + path + `?port=${port}`
    );
    _window.webContents.openDevTools();

    _window.once("ready-to-show", () => {
      _window.show();
    });
    _window.on("closed", function () {
      _window = null;
    });
    this.windows[path] = _window;
    return _window;
  };
  minimize = (path: string) => {
    this.windows[path].minimize();
    return true;
  };
  maximize = (path: string) => {
    if (this.windows[path].isMaximized()) {
      this.windows[path].restore();
    } else {
      this.windows[path].maximize();
    }
    return this.windows[path].isMaximized();
  };
  exit = (path: string) => {
    if (path === "/") {
      app.exit();
    } else {
      this.windows[path].close();
      delete this.windows[path];
    }
    return true;
  };
}
export const WindowManager = new _WindowManager();
