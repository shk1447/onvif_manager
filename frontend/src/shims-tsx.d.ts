import Vue, { VNode } from 'vue';

interface IElectronAPI {
  showDialog: (path: string) => Promise<any>;
  createWindow: (path: string) => void;
  maximize: (path: string) => Promise<any>;
  minimize: (path: string) => void;
  exit: (path: string) => void;
  discovery: () => void;
  on: (channel: string, callback: (event: any, data: any) => void) => void;
  off: (channel: string, callback: (event: any, data: any) => void) => void;
  send: (channel: string, data: any) => void;
}

declare global {
  namespace JSX {
    interface Element extends VNode {}
    interface ElementClass extends Vue {}
    interface IntrinsicElements {
      [elem: string]: any;
    }
  }
}

declare module 'vue/types/vue' {
  // 3. Declare augmentation for Vue
  interface Vue {
    $electron: IElectronAPI;
  }
}
declare module 'mux.js' {
  const muxjs: any;
  export default muxjs;
}
