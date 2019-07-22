import webpack from "webpack";

import webpackMerge from "webpack-merge";
import common from "./webpack.config.common";

const config: webpack.Configuration = webpackMerge(common, {
  mode: "development",
  devtool: "inline-source-map",
  devServer: {
    contentBase: "./dist",
    hot: true,
    historyApiFallback: true,
  },
  plugins: [
    new webpack.HotModuleReplacementPlugin(),
  ],
});

export default config;
