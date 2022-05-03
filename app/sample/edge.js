var edge = require("edge-js");

var dotNetFunction = edge.func("../modules/SaigeVAD.Edge.dll");

dotNetFunction("Test", function (err, result) {
  if (err) {
    console.log(err);
  }
  console.log(result);
});

// dotNetFunction("Test", function (err, result) {
//   if (err) {
//     console.log(err);
//   }
//   console.log(result);
// });

while (true) {}
