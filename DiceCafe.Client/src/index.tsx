import * as React from "react";
import { render } from "react-dom";

import { Room } from "./Pages/room";
import { startConnection } from "./connection";

const data = (window as any).preloadedData as RoomViewModel;

async function main() {
  await startConnection(data.room.id);
  render(<Room roomVm={data} />, document.getElementById("root"));
}

main();
