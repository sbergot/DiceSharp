const path = require("path");

module.exports = {
  entry: {
    room: "./src/room-index.tsx",
    "script-tester": "./src/script-tester-index.tsx",
  },

  output: {
    path: path.resolve(__dirname, "..", "DiceCafe.WebApp", "wwwroot", "dist"),
  },

  mode: "production",

  devtool: "source-map",

  resolve: {
    extensions: [".js", ".ts", ".tsx"],
  },

  module: {
    rules: [
      {
        test: /\.ts(x?)$/,
        exclude: /node_modules/,
        use: [
          {
            loader: "ts-loader",
          },
        ],
      },
      // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
      {
        enforce: "pre",
        test: /\.js$/,
        loader: "source-map-loader",
      },
      {
        test: /\.css$/,
        use: ["style-loader", "css-loader"],
      },
    ],
  },

  externals: {
    react: "React",
    "react-dom": "ReactDOM",
    "@microsoft/signalr": "signalR",
  },
};
