import * as React from "react";

import { JoinArea } from "./dice-table/join-area";
import { UserList } from "./dice-table/user-list";
import { FunctionList } from "./dice-table/function-list";
import { ResultList } from "./dice-table/result-list";

export function DiceTable() {
  return (
    <>
      <JoinArea />
      <div className="flex mt-4">
        <div className="w-full max-w-xs">
          <UserList />
          <FunctionList />
        </div>
        <div>
          <ResultList />
        </div>
      </div>
    </>
  );
}
