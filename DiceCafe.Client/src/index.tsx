import * as React from "react";
import { render } from "react-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import { Room } from "./Pages/room";
import { connect } from "./room-context";

async function main() {
  toast.configure();
  await connect();
  render(<Room />, document.getElementById("root"));
}

main();
