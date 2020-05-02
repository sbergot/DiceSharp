import * as React from "react";
import { render } from "react-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import { Room } from "./Pages/room";
import { startConnection } from "./connection";

const data = (window as any).preloadedData as RoomViewModel;
toast.configure();

async function main() {
  await startConnection(data.room.id);
  render(<Room roomVm={data} />, document.getElementById("root"));
}

main();
