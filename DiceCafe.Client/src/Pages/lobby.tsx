import * as React from "react";

import { JoinArea } from "./join-area";
import { UserList } from "./user-list";
import { FunctionList } from "./function-list";
import { ResultList } from "./result-list";
import { LibraryEditor } from "./library-editor";

export function Lobby({ room }: RoomProp) {
  return (
    <div>
      <JoinArea room={room} />
      <UserList room={room} />
      <div className="flex mt-4">
        <div className="w-full max-w-xs">
          <FunctionList room={room} />
        </div>
        <div className="ml-4">
          <ResultList room={room} />
        </div>
      </div>
      <LibraryEditor room={room} />
    </div>
  );
}
