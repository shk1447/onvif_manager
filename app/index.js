// const fs = require('fs');
const path = require('path');
// const runtime = require('./runtime');

//const demo = require('./demo.js')

const {app, BrowserWindow, ipcMain} = require('electron');

console.log(app.getAppPath());
process.env.root_path = path.resolve(__dirname);

ipcMain.on('asynchronous-message', (event, arg) => {
    event.sender.send('asynchronous-reply', 'ping')

    console.log(arg)
})

ipcMain.on('synchronous-message', (event, arg) => {
    console.log(arg)  // "ping" 출력
    event.returnValue = 'pong'
})

let mainWindow;

function createWindow() {
    mainWindow = new BrowserWindow({
        webPreferences: {
          blinkFeatures: 'CSSStickyPosition',
          nodeIntegration:true,
          enableRemoteModule: true
        },
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

    mainWindow.loadURL('file://' + __dirname + '/dist/index.html');

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
