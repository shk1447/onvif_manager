import { EventBus } from "../eventBus";
interface IWatcher {
    port: number;
}
export declare class Watcher extends EventBus implements IWatcher {
    apps: any[];
    watchers: any[];
    port: number;
    constructor(port: number);
    post: (watcher: any, name: string, port: number) => Promise<boolean>;
    discovery: () => void;
}
export {};
