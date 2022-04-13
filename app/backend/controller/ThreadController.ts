const edge = require("@nomis51/electron-edge-js");

export class ThreadController {
  constructor() {
    var helloWorld = edge.func(`
      async (input) => {
          return ".NET Welcomes " + input.ToString();
      }
    `);

    helloWorld("test", function (err: any, result: any) {
      console.log("aaa" + result);
    });
  }

  create() {}

  close() {}
}

export default new ThreadController();
