import { Application } from "express-ws";
import ws from "ws";
export declare class EdgeService {
    edges: any;
    constructor(app: Application);
    initialize: (port: number) => Promise<void>;
    responseMethod: (ws: ws, req: any) => void;
    asyncMethod: (ws: ws, req: any) => void;
    postMethod: (req: any, res: any, next: any) => void;
}
