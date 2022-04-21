import electron from 'electron';

import { IElectronAPI } from './shims-tsx';
class ElectronAPI implements IElectronAPI {
  discovery() {
    console.log('aaa');
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
  maximize() {
    return electron.ipcRenderer.invoke('maximize', {});
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
