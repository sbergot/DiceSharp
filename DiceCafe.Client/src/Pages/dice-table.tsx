import * as React from "react";

import { JoinArea } from "./dice-table/join-area";
import { UserList } from "./dice-table/user-list";
import { FunctionList } from "./dice-table/function-list";
import { ResultList } from "./dice-table/result-list";

export function DiceTable({ room }: RoomProp) {
  return (
    <div>
      <JoinArea room={room} />
      <UserList room={room} />
      <div className="flex mt-4">
        <div className="w-24">
          <FunctionList room={room} />
        </div>
        <div className="ml-4">
          <ResultList room={room} />
        </div>
      </div>
    </div>
  );
}
