const fs = require("fs");
const path = require("path");
const { program } = require("commander");

program.option("-c, --conf [conf]", "set config", "./resources/config.json");

program.parse();

const options = program.opts();
var relative_path = path.relative(__dirname, "./");
process.env.conf_path = path.resolve(__dirname, relative_path, options.conf);
config = JSON.parse(fs.readFileSync(process.env.conf_path, "utf8"));

module.exports = config;
