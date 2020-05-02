import * as React from "react";

import { Link } from "../Components/Link";
import { createUrls } from "../http";
import { JoinArea } from "./join-area";
import { UserList } from "./user-list";
import { FunctionList } from "./function-list";
import { ResultList } from "./result-list";
import { LibraryEditor } from "./library-editor";

export function Lobby({ room }: RoomProp) {
  const { quitUrl } = createUrls(room.id);
  return (
    <div>
      <Link href={quitUrl} label="Quitter" className="mr-2" />
      <JoinArea room={room} />
      <UserList room={room} />
      <FunctionList room={room} />
      <ResultList room={room} />
      <LibraryEditor room={room} />
    </div>
  );
}
