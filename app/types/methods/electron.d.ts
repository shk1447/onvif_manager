import { BrowserWindow } from "electron";
interface IWindowManager {
    windows: {
        [path: string]: BrowserWindow;
    };
    create: (path: string, options: Electron.BrowserWindowConstructorOptions) => BrowserWindow;
    minimize: (path: string) => boolean;
    maximize: (path: string) => boolean;
    exit: (path: string) => boolean;
}
declare class _WindowManager implements IWindowManager {
    windows: any;
    constructor();
    create: (path: string, options: Electron.BrowserWindowConstructorOptions) => any;
    minimize: (path: string) => boolean;
    maximize: (path: string) => any;
    exit: (path: string) => boolean;
}
export declare const WindowManager: _WindowManager;
export {};
