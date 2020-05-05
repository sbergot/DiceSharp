import * as React from "react";
import { roomUpdateChannel, startConnection } from "./connection";

interface RoomGlobalContext {
  room: Room;
  isCreator: boolean;
}

const roomVm = (window as any).preloadedData as RoomViewModel;
const { userId, room } = roomVm;
const creatorId = room.creator.id;
const isCreator = userId == creatorId;
const RoomContext = React.createContext<RoomGlobalContext>({ room, isCreator });

export async function connect() {
  await startConnection(roomVm.room.id);
}

export function RoomContextListener({ children }: ChildrenProp) {
  const [room, setRoom] = React.useState(roomVm.room);

  React.useEffect(() => {
    roomUpdateChannel.subscribe(setRoom);
    return () => {
      roomUpdateChannel.unsubscribe(setRoom);
    };
  }, []);

  const contextVal: RoomGlobalContext = { room, isCreator };

  return (
    <RoomContext.Provider value={contextVal}>{children}</RoomContext.Provider>
  );
}

export function useRoomContext() {
  return React.useContext(RoomContext);
}
