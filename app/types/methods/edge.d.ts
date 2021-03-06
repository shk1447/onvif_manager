import { EventBus } from "../eventBus";
interface IEdge {
    controller: any;
}
export declare class Edge extends EventBus implements IEdge {
    controller: any;
    constructor();
    initialize: (uuid: string, port: number) => Promise<unknown>;
}
export {};
