import electron from "electron";

import { IElectronAPI } from "./shims-tsx";
class ElectronAPI implements IElectronAPI {
  maximize() {
    electron.ipcRenderer.invoke("maximize", {}).then((result) => {
      console.log(result);
    });
  }
  minimize() {
    electron.ipcRenderer.invoke("minimize", {}).then((result) => {
      console.log(result);
    });
  }
  exit() {
    electron.ipcRenderer.invoke("exit", {}).then((result) => {
      console.log(result);
    });
    console.log(electron);
  }
}

export default new ElectronAPI();
