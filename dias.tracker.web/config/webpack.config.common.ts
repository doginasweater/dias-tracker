import path from "path";
import webpack from "webpack";

const config: webpack.Configuration = {
  entry: {
    app: "./src/index.tsx",
  },
  resolve: {
    extensions: [ ".tsx", ".ts", ".jsx", ".js" ],
  },
  output: {
    path: path.resolve("./dist"),
    filename: "[name].js",
    publicPath: "/",
  },
  module: {
    rules: [
      {
        test: /\.(ts|tsx)$/,
        use: {
          loader: "ts-loader",
          options: {
            transpileOnly: true,
          },
        },
      },
      {
        test: /\.(png|svg|jpg|jpeg|gif|ico)$/,
        use: "file-loader",
      },
      {
        test: /\.(woff|woff2|eot|ttf|otf)$/,
        use: "file-loader",
      },
      {
        test: /\.scss$/,
        use: [
          "style-loader",
          "css-loader",
          {
            loader: "postcss-loader",
            options: {
              plugins: [
                require("autoprefixer"),
              ],
            },
          },
          "sass-loader",
        ],
      },
    ],
  },
  plugins: [ ],
};

export default config;
