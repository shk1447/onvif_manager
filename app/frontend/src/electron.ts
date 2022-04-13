import electron from 'electron';

import { IElectronAPI } from './shims-tsx';
class ElectronAPI implements IElectronAPI {
  createWindow(path: string) {
    electron.ipcRenderer
      .invoke('createWindow', {
        path: path,
      })
      .then(result => {
        console.log(result);
      });
  }
  maximize() {
    electron.ipcRenderer.invoke('maximize', {}).then(result => {
      console.log(result);
    });
  }
  minimize() {
    electron.ipcRenderer.invoke('minimize', {}).then(result => {
      console.log(result);
    });
  }
  exit() {
    electron.ipcRenderer.invoke('exit', {}).then(result => {
      console.log(result);
    });
  }
}

export default new ElectronAPI();
