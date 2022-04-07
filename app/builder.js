import fs from 'fs';
import ora from "ora";
import {resolve} from 'path';
import webpack from 'webpack';

import {program} from 'commander'
program
  .option('--watch')

program.parse();
const { watch } = program.opts();

import child_process from 'child_process';
import { Writable } from 'stream';

import CopyWebpackPlugin from 'copy-webpack-plugin';

var _package = JSON.parse(fs.readFileSync(resolve('./package.json'), 'utf8'));
delete _package.type;
_package.main = "main.js";
_package.devDependencies = {};
_package.dependencies = {
  "electron": "^17.2.0",
  "@nomis51/electron-edge-js": "^17.0.0",
  "edge-js": "^16.6.0",
  "fluent-ffmpeg": "^2.1.2",
  "onvif": "^0.6.5"
}
_package.scripts = {
  "start": "electron ./main.js"
}

var config = {
    watch: watch,
    entry: resolve('./backend/main.ts'),
    target: 'node',
    output: {
        path: resolve('./dist'),
        filename: './main.js'
    },
    resolve: {
      extensions:['.ts', '.js']
    },
    module: {
      rules:[{
        test:/\.ts$/,
        loader:'ts-loader',
        exclude:/node_modules/
      }]
    },
    externals: {
      "electron":"commonjs electron",
      "@nomis51/electron-edge-js": 'commonjs @nomis51/electron-edge-js',
      "fluent-ffmpeg": 'commonjs fluent-ffmpeg',
      "onvif": 'commonjs onvif'
    },
    plugins: [
      new CopyWebpackPlugin({
        patterns:[{
          from: resolve('./modules'),
          to: resolve('../dist/modules')
        }]
      }),
    ]
};
const spinner = ora('building for production...');
spinner.start();

var child;
var compiler = webpack(config, (err, stats) => {
  fs.writeFileSync(resolve('./dist/package.json'), JSON.stringify(_package, null, 2));
  spinner.stop();
  console.log('build complete');
  var installer = child_process.spawn('npm.cmd', ["install"], { cwd: './dist' });
  installer.stdout.pipe(new Writable({
    write(chunk, encoding, callback) {
      console.log(chunk.toString());
      callback();
    }
  }));
  installer.on('exit', () => {
    if(watch) {
      var command = 'npm.cmd'
      var args = ['run', 'start']
      var options = { cwd: './dist' }
      child = child_process.spawn(command, args, options);
  
      child.stdout.pipe(new Writable({
        write(chunk, encoding, callback) {
          console.log(chunk.toString());
          callback();
        }
      }));
      child.on('exit', function () {
        console.log('process exit')
      })
    }
  })
});

if (watch) {
  compiler.hooks.beforeCompile.tap({ name: 'app' }, (a, b, c) => {
    child.kill();
    spinner.start();
  })
}