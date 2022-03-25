
const path = require('path');
const exec = require('./exec');

const command = process.platform.includes('win') ? './Scripts/pip' : './bin/pip';


module.exports = (function() {
    return {
        upgrade: function() {
            return exec.run(command, ['install', 'pip', '--upgrade'], {
                cwd: path.resolve(process.env.root_path, './mlprocess')
            })
        },
        list: function() {
            return exec.run(command, ['list', '--format', 'json'], {
                cwd: path.resolve(process.env.root_path, './mlprocess')
            })
        },
        install:function(module_name, callback) {
            var args = ['install'];
            if(Array.isArray(module_name)) args = args.concat(module_name)
            else args.push(module_name)

            return exec.run(command, args, {
                cwd: path.resolve(process.env.root_path, './mlprocess')
            }, null, callback)
        },
        uninstall:function(module_name,callback) {
            var args = ['uninstall', '-y'];
            if(Array.isArray(module_name)) args = args.concat(module_name)
            else args.push(module_name)

            return exec.run(command, args, {
                cwd: path.resolve(process.env.root_path, './mlprocess')
            },null, callback)
        }
    }
})()
