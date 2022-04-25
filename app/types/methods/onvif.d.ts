import { EventBus } from "../eventBus";
interface IOnvif {
    username: string;
    password: string;
}
export declare class Onvif extends EventBus implements IOnvif {
    cams: any[];
    username: string;
    password: string;
    constructor(username: string, password: string);
    discovery: () => void;
    custom_discovery: () => void;
}
export {};
