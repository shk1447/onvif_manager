import { Application } from "express-ws";
import ws from "ws";
export declare class EdgeService {
    constructor(app: Application);
    initialize: () => Promise<void>;
    asyncMethod: (ws: ws, req: any) => void;
    postMethod: (req: any, res: any, next: any) => void;
}
