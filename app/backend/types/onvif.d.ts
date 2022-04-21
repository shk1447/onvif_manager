declare module "websocket-stream/stream" {
  const test: any;
  export default test;
}

declare module "onvif" {
  interface EventMap {
    device: (cam: ICam, rinfo: any, xml: any) => void;
    error: (err: any, xml: any) => void;
  }

  interface ICamInfo {
    useSecure?: boolean;
    secureOpts?: any;
    hostname: string;
    username: string;
    password: string;
    port?: string;
  }

  interface ICam extends ICamInfo {
    new (camInfo: ICamInfo, callback: (err: any) => void): any;
    connect: (callback: () => void) => void;
    getSnapshotUri: (options: any, callback: any) => void;
    getStreamUri: (options: any, callback: any) => void;
    getStatus: (options: any, callback: any) => void;
  }

  interface IDiscovery {
    on: <Name extends keyof EventMap>(
      event: Name,
      func: EventMap[Name]
    ) => void;
    probe: (options?: any, callback?: (err: any, cams: ICam[]) => void) => void;
  }
  interface IOnvif {
    Discovery: IDiscovery;
    Cam: ICam;
  }
  const onvif: IOnvif;
  export default onvif;
}
