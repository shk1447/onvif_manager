export interface IEventBus {
  handlers: any;
  on: any;
  off: any;
  emit: any;
}

export class EventBus implements IEventBus {
  handlers: any;

  constructor() {
    this.handlers = {};
  }

  on = (evt: string, func: (...args: any) => void) => {
    this.handlers[evt] = this.handlers[evt] || [];
    this.handlers[evt].push(func);
  };
  off = (evt: string, func: (...args: any) => void) => {
    var handler = this.handlers[evt];
    if (handler) {
      for (var i = 0; i < handler.length; i++) {
        if (handler[i] === func) {
          handler.splice(i, 1);
          return;
        }
      }
    }
  };
  emit = (evt: string, args: any, _this?: any) => {
    if (this.handlers[evt]) {
      for (var i = 0; i < this.handlers[evt].length; i++) {
        try {
          this.handlers[evt][i].apply(_this ?? null, args);
        } catch (err) {
          console.log(
            "common.events.emit error: [" + evt + "] " + err.toString()
          );
          console.log(err);
        }
      }
    }
  };
}
