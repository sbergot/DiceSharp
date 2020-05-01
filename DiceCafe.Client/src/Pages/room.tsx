import * as React from "react";

import { Lobby } from "./lobby";
import { roomUpdateChannel } from "../connection";

interface RoomProps {
  roomVm: RoomViewModel;
}

export function Room({ roomVm }: RoomProps) {
  const [room, setRoom] = React.useState(roomVm.room);

  React.useEffect(() => {
    roomUpdateChannel.subscribe(setRoom);
    return () => {
      roomUpdateChannel.unsubscribe(setRoom);
    };
  }, []);

  return <Lobby room={room} />;
}
