const { defineConfig } = require('@vue/cli-service');
const NodePolyfillPlugin = require('node-polyfill-webpack-plugin');

module.exports = defineConfig({
  transpileDependencies: true,
  outputDir: '../dist',
  publicPath: './',
  // electron 옵션 ( web 빌드에 대한 분기 필요 )
  configureWebpack: {
    target: 'electron-renderer',
    plugins: [new NodePolyfillPlugin()],
  },
});
