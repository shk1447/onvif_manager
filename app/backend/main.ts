// const fs = require('fs');
import path from 'path';
// const runtime = require('./runtime');
var edge = require('@nomis51/electron-edge-js');

var helloWorld = edge.func(`
    async (input) => {
        return ".NET Welcomes " + input.ToString();
    }
`);

helloWorld('test', function(err:any, result:any) {
  console.log(result);
})


var dotNetFunction = edge.func('EdgeLib.dll');

console.log('aaa');
dotNetFunction("Test", function(err: any, result: any) {
  if(err) {
    console.log(err);
  }
  console.log(result);
})
//const demo = require('./demo.js')

import {app, BrowserWindow, ipcMain} from 'electron';

console.log(app.getAppPath());
process.env.root_path = path.resolve(__dirname);

ipcMain.handle('exit', async(event, args) => {
  app.exit();
  return false;
})
ipcMain.handle('maximize', async(event, args) => {
  if(mainWindow.isMaximized()) {
    mainWindow.restore()
  } else {
    mainWindow.maximize();
  }
  return mainWindow.isMaximized();
})

ipcMain.handle('minimize', async(event, args) => {
  mainWindow.minimize();
  return true;
})

ipcMain.on('asynchronous-message', (event, arg) => {
    event.sender.send('asynchronous-reply', 'ping')

    console.log(arg)
})

ipcMain.on('synchronous-message', (event, arg) => {
    console.log(arg)  // "ping" 출력
    event.returnValue = 'pong'
})

let mainWindow:BrowserWindow;

function createWindow() {
    mainWindow = new BrowserWindow({
        webPreferences: {
          blinkFeatures: "CSSStickyPosition",
          nodeIntegration:true,
          nodeIntegrationInWorker: false,
          contextIsolation:false,
          webSecurity:false,
          sandbox: false,
          enableRemoteModule: true,
        } as any,
        minWidth:1600,
        minHeight:900,
        width: 1600,
        height: 900,
        kiosk: false,
        fullscreen: false,
        fullscreenable:true,
        resizable:true,
        frame:false,
        show:false
    });

    mainWindow.loadURL('file://' + __dirname + '/index.html');

    mainWindow.webContents.openDevTools();

    mainWindow.once('ready-to-show', () => {
      mainWindow.show();
      mainWindow.webContents.send('asynchronous-reply', '초기 설치 시작');
    })
    mainWindow.on('closed', function() {
        mainWindow = null;
    });
}

app.on('window-all-closed', function() {
  if (process.platform != 'darwin') app.quit();
});

app.on('ready', createWindow);

app.on('activate', function () {
  if (mainWindow === null) createWindow()
})
