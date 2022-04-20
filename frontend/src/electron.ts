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
  on(channel: string, callback: (event: any, data: any) => void) {
    electron.ipcRenderer.on(channel, callback);
  }
  off(channel: string, callback: (event: any, data: any) => void) {
    electron.ipcRenderer.off(channel, callback);
  }
  emit(channel: string, data: any) {
    electron.ipcRenderer.emit(channel, data);
  }
}

export default new ElectronAPI();
