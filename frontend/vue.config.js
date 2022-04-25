const webpack = require('webpack');
const { defineConfig } = require('@vue/cli-service');
const NodePolyfillPlugin = require('node-polyfill-webpack-plugin');

const config = {
  transpileDependencies: true,
  outputDir: '../dist',
  publicPath: './',
};

if (process.env.MODE == 'electron') {
  config['configureWebpack'] = {
    target: 'electron-renderer',
    plugins: [new NodePolyfillPlugin()],
  };
} else {
  config['configureWebpack'] = {
    resolve: {
      fallback: {
        fs: false,
        path: false,
        crypto: false,
      },
    },
  };
  config['devServer'] = {
    proxy: {
      '/discovery': {
        target: 'http://localhost:9090',
        ws: true,
        changeOrigin: true,
      },
      '/rtsp': {
        target: 'http://localhost:9090',
        ws: true,
        changeOrigin: true,
      },
    },
  };
}

module.exports = defineConfig(config);
