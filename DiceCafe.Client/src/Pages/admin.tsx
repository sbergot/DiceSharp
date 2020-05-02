import * as React from "react";

import { LibraryEditor } from "./dice-table/library-editor";

export function Admin({ room }: RoomProp) {
  return (
    <div>
      <LibraryEditor room={room} />
    </div>
  );
}
