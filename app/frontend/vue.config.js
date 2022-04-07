const { defineConfig } = require("@vue/cli-service");
const NodePolyfillPlugin = require("node-polyfill-webpack-plugin");

module.exports = defineConfig({
  transpileDependencies: true,
  outputDir: "../dist",
  publicPath: "./",
  // electron 옵션
  configureWebpack: {
    target: "electron-renderer",
    plugins: [new NodePolyfillPlugin()],
  },
});
