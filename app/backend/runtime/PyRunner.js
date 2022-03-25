
const path = require('path');
const exec = require('./exec');

const command = process.platform.includes('win') ? 'python' : 'python3';


module.exports = (function() {
    return {
        run:function(filePath, params, callback) {
            // var cmd = command + ' ' + filePath
            // exec.execute(cmd, function(err, stdout, stderr) {
            //     console.log(err, stdout, stderr);
            // })
            var options = [filePath];
            return new Promise((resolve,reject) => {
                exec.run(command,options,{
                    cwd: path.resolve(process.env.root_path, './mlprocess')
                }, params, callback).then((result) => {
                    resolve(result);
                }).catch((err) => {
                    reject(err);
                });
            })
        }
    }
})();
