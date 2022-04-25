export interface IEventBus {
    handlers: any;
    on: any;
    off: any;
    emit: any;
}
export declare class EventBus implements IEventBus {
    handlers: any;
    constructor();
    on: (evt: string, func: (...args: any) => void) => void;
    off: (evt: string, func: (...args: any) => void) => void;
    emit: (evt: string, args: any, _this?: any) => void;
}
