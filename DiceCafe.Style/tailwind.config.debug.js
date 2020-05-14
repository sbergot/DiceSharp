const config = require("./tailwind.config");

module.exports = { ...config, purge: { ...config.purge, enabled: false } };
