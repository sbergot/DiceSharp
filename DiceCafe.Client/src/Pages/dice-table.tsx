import * as React from "react";

import { JoinArea } from "./dice-table/join-area";
import { UserList } from "./dice-table/user-list";
import { FunctionList } from "./dice-table/function-list";
import { ResultList } from "./dice-table/result-list";
import { useRoomContext } from "../room-context";

export function DiceTable() {
  const { room, isCreator } = useRoomContext();
  return (
    <div className="flex mt-4">
      <div className="w-full max-w-xs">
        <JoinArea room={room} isCreator={isCreator} />
        <UserList room={room} />
        <FunctionList room={room} />
      </div>
      <div className="ml-4">
        <ResultList room={room} />
      </div>
    </div>
  );
}
