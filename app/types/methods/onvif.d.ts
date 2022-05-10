import { EventBus } from "../eventBus";
interface IOnvif {
    username: string;
    password: string;
    discoveryPort: string;
}
export declare class Onvif extends EventBus implements IOnvif {
    cams: any[];
    username: string;
    password: string;
    discoveryPort: string;
    constructor(username: string, password: string, discoveryPort?: string);
    discovery: () => void;
    custom_discovery: () => void;
}
export {};
