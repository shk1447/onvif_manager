import electron from 'electron';

import { IElectronAPI } from './shims-tsx';
class ElectronAPI implements IElectronAPI {
  showDialog(path: string) {
    return electron.ipcRenderer.invoke('showDialog', {
      path: path,
    });
  }
  discovery() {
    electron.ipcRenderer.invoke('discovery', {}).then(result => {
      console.log(result);
    });
  }
  createWindow(path: string) {
    electron.ipcRenderer
      .invoke('createWindow', {
        path: path,
      })
      .then(result => {
        console.log(result);
      });
  }
  maximize(path: string) {
    return electron.ipcRenderer.invoke('maximize', {
      path: path,
    });
  }
  minimize(path: string) {
    electron.ipcRenderer.invoke('minimize', { path: path }).then(result => {
      console.log(result);
    });
  }
  exit(path: string) {
    electron.ipcRenderer.invoke('exit', { path: path }).then(result => {
      console.log(result);
    });
  }
  on(channel: string, callback: (event: any, data: any) => void) {
    electron.ipcRenderer.on(channel, callback);
  }
  off(channel: string, callback: (event: any, data: any) => void) {
    electron.ipcRenderer.off(channel, callback);
  }
  send(channel: string, data: any) {
    electron.ipcRenderer.send(channel, data);
  }
}

export default new ElectronAPI();
