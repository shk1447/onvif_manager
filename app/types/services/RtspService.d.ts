import { Application } from "express-ws";
import ws from "ws";
export declare class RtspService {
    constructor(app: Application);
    discovery: (ws: ws, req: any) => void;
    stream: (ws: ws, req: any) => void;
}
