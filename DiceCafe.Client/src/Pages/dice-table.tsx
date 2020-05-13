import * as React from "react";

import { DiceTableMenu } from "../DomainComponents/dice-table-menu";
import { UserList } from "../DomainComponents/user-list";
import { FunctionList } from "../DomainComponents/function-list";
import { ResultList } from "../DomainComponents/result-list";
import { useRoomContext } from "../room-context";

export function DiceTable() {
  const { room } = useRoomContext();
  const { results } = room;

  return (
    <>
      <DiceTableMenu />
      <div className="w-full max-w-xs">
        <UserList />
        <FunctionList />
      </div>
      <div className="absolute right-0 bottom-0 p-8 pr-20 overflow-hidden max-h-screen h-full max-w-md w-full">
        <ResultList results={results} />
      </div>
    </>
  );
}
