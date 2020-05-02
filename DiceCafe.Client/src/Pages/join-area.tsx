import * as React from "react";

import { createUrls } from "../http";

export function JoinArea({ room }: RoomProp) {
  const { joinUrl } = createUrls(room.id);

  return (
    <>
      <p>
        Les autres joueurs peuvent rejoindre cette salle en allant sur cette
        url:{" "}
        <a
          className="underline text-blue-400 visited:text-blue-800"
          href={joinUrl}
        >
          {joinUrl}
        </a>
      </p>
      <p>
        Il est aussi possible de cliquer sur "Rejoindre une salle" et de fournir
        le code: <strong>{room.id}</strong>
      </p>
    </>
  );
}
