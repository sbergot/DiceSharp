import * as React from "react";

import { Menu } from "./dice-table/menu";
import { UserList } from "./dice-table/user-list";
import { FunctionList } from "./dice-table/function-list";
import { ResultList } from "./dice-table/result-list";

export function DiceTable() {
  return (
    <>
      <Menu />
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
