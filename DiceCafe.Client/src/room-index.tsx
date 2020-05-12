import * as React from "react";
import { render } from "react-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import { App } from "./room-app";
import { connect } from "./room-context";

async function main() {
  toast.configure();
  await connect();
  render(<App />, document.getElementById("root"));
}

main();
