const child_process = require('child_process');

module.exports = {
    run: function(command,args,options,params,callback) {
        return new Promise((resolve, reject) => {
            let stdout = [];
            let stderr = [];
            const child = child_process.spawn(command,args,options);
            child.stdout.on('data', (data) => {
                // stdout += data.toString().replace(/\r?\n/,'');
                //stdout = stdout.concat(data.toString().replace(/\r?\n/,''))
                if(callback) {
                    data.toString().split(/\r?\n/).forEach(function(d) {
                        callback.apply(null, [d])
                    })
                }
            });
            child.stderr.on('data', (data) => {
                // stderr = stderr.concat(data.toString().split(/\r?\n/))
                if(callback) {
                    data.toString().split(/\r?\n/).forEach(function(d) {
                        callback.apply(null, [d])
                    })
                }
            });
            child.on('error', function(err) {
                console.log(err.toString().split(/\r?\n/))
                //stderr = stderr.concat(err.toString().split(/\r?\n/))
            })
            child.on('close', (code) => {
                if (code === 0) {
                    resolve(stdout);
                } else {
                    reject(stderr);
                }
            });

            if(params) child.stdin.write(params + "\n" )
        })
    },
    execute: function(command) {
        console.log(command);
        child_process.exec(command, function(err, stdout, stderr) {
            console.log(err, stdout, stderr);
        })
    }
}
