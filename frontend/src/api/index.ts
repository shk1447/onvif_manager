import { Http as _Http } from './Http';
import {
  WebSocketClient as _WebSocketClient,
  WebSocketManager as _WebSocketManager,
} from './WebSocket';
import { EventHandler as _EventHandler } from './EventHandler';

export const Http = _Http;
export class WebSocketClient extends _WebSocketClient {}
export class WebSocketManager extends _WebSocketManager {}
export class EventHandler extends _EventHandler {}
