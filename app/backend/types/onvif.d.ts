declare module "onvif" {
  interface EventMap {
    device: (cam: any, rinfo: any, xml: any) => void;
    error: (err: any, xml: any) => void;
  }

  interface IDiscovery {
    on: <Name extends keyof EventMap>(
      event: Name,
      func: EventMap[Name]
    ) => void;
    probe: (options: any, callback: any) => void;
  }
  interface IOnvif {
    Discovery: IDiscovery;
    Cam: any;
  }
  const onvif: IOnvif;
  export default onvif;
}
