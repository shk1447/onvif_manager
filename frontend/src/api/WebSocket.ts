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

export interface IWebSocketManager {
  host: string;
  port: string;
  clients: Record<string, WebSocketClient>;
  socket: (path: string) => Promise<WebSocketClient>;
}

export class WebSocketManager
  extends EventHandler
  implements IWebSocketManager
{
  host: string;
  port: string;
  clients: Record<string, WebSocketClient>;
  constructor(host: string, port: string) {
    super();
    this.host = host;
    this.port = port;
    this.clients = {};
  }

  socket = (path: string): Promise<WebSocketClient> => {
    return new Promise((resolve, reject) => {
      try {
        if (this.clients[path]) {
          resolve(this.clients[path]);
        } else {
          new WebSocketClient(
            `ws://${this.host}:${this.port}${path}`,
            (state: boolean, client: WebSocketClient) => {
              if (state) {
                this.clients[path] = client;
                resolve(this.clients[path]);
              } else {
                this.clients[path].close();
                delete this.clients[path];
              }
            },
          );
        }
      } catch (error) {
        reject(error);
      }
    });
  };
  close = (path: string) => {
    if (this.clients[path]) {
      this.clients[path].close();
      delete this.clients[path];
    }
  };
  dispose = () => {
    Object.keys(this.clients).forEach((path: string) => {
      this.clients[path].close();
      delete this.clients[path];
    });
  };
}
