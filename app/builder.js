import fs from 'fs';
import ora from "ora";
import {resolve} from 'path';
import webpack from 'webpack';
import psTree from 'ps-tree';
import taskkill from 'taskkill';

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
        to: resolve('./dist/modules')
      }]
    }),
  ]
};
const spinner = ora('building for production...');
spinner.start();

var child;
var compiler = webpack(config, (err, stats) => {
  if(err) {
    console.log(err);
  }
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
      var command = 'node'
      var args = ['builder.js']
      var options = { cwd: './' }
      var builder = child_process.spawn(command, args, options);
  
      builder.stdout.pipe(new Writable({
        write(chunk, encoding, callback) {
          console.log(chunk.toString());
          callback();
        }
      }));
      builder.on('exit', function () {
        var command = 'electron.cmd'
        var args = ['main.js']
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
      })
    }
  })
});

if (watch) {
  compiler.hooks.beforeCompile.tap({ name: 'app' }, (a, b, c) => {
    if(child) {
      console.log(child.pid)
      psTree(child.pid, function (err, children) {
        const input = children.map(function (p) { return p.PID })
        taskkill(input).then(() => {
          console.log(`Successfully terminated ${input.join(', ')}`);
        })
      });
    }
    spinner.start();
  })
}