var edge = require('edge-js');

var helloWorld = edge.func(`
    async (input) => {
        return ".NET Welcomes " + input.ToString();
    }
`);

helloWorld('test', function(erro, result) {
  console.log(result);
})


var dotNetFunction = edge.func('EdgeTest.dll');

console.log('aaa');
dotNetFunction("Test", function(err, result) {
  if(err) {
    console.log(err);
  }
  console.log(result);
})

while(true) {

}