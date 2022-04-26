import { Application } from "express-ws";
import ws from "ws";
export declare class WatcherService {
    constructor(app: Application);
    postWatcher: (req: any, res: any, next: any) => Promise<void>;
    discovery: (ws: ws, req: any) => void;
}
