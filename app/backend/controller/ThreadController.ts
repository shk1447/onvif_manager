const edge = require("@nomis51/electron-edge-js");

export class ThreadController {
  constructor() {}

  create() {
    console.log("test");
    var helloWorld = edge.func("./modules/SaigeVAD.Edge.dll");

    helloWorld("test", function (err: any, result: any) {
      if (err) console.log(err);
      console.log("aaa" + result);
    });
  }

  close() {}
}

export default new ThreadController();
