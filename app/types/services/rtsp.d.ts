import { Application } from "express-ws";
export declare class RtspService {
    constructor(app: Application);
    stream: (ws: any, req: any) => void;
}
