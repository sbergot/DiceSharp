import * as React from "react";
import { render } from "react-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { ScriptTester } from "./Pages/script-tester";

async function main() {
  toast.configure();
  render(<ScriptTester />, document.getElementById("root"));
}

main();
