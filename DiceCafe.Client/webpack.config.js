const path = require("path");

module.exports = {
  entry: "./src/index.tsx",

  output: {
    path: path.resolve(__dirname, "..", "WebApp", "wwwroot", "dist")
  },

  mode: "production",

  devtool: "source-map",

  resolve: {
    extensions: [".ts", ".tsx"]
  },

  module: {
    rules: [
      {
        test: /\.ts(x?)$/,
        exclude: /node_modules/,
        use: [
          {
            loader: "ts-loader"
          }
        ]
      },
      // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
      {
        enforce: "pre",
        test: /\.js$/,
        loader: "source-map-loader"
      }
    ]
  },

  externals: {
    react: "React",
    "react-dom": "ReactDOM",
    "@microsoft/signalr": "signalR"
  }
};
