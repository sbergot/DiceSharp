import * as React from "react";
import { useRoomContext } from "../room-context";

export function UserList() {
  const { room } = useRoomContext();
  const { users } = room;

  return (
    <>
      {Object.values(users).map((p) => {
        return (
          <span className={`font-bold px-2 py-1 rounded-md`}>{p.name}</span>
        );
      })}
    </>
  );
}
