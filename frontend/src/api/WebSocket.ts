import { EventHandler } from './EventHandler';

export class WebSocketClient extends EventHandler {
  url: string;
  ws: WebSocket;
  callback: any;
  constructor(url: string, callback: any) {
    super();
    this.url = url;
    this.callback = callback;
    this.ws = new WebSocket(url);
    this.ws.onopen = this.onOpen;
    this.ws.onmessage = this.onMessge;
    this.ws.onclose = this.onClose;
  }

  onOpen = (args: any) => {
    this.callback(true, this);
  };
  onClose = (args: any) => {
    this.callback(false, this);
  };
  onMessge = (args: any) => {
    try {
      const data = JSON.parse(args.data);
      this.emit('data', [data]);
    } catch (error) {
      console.log(error);
    }
  };

  close = () => {
    this.ws.close();
  };
}
