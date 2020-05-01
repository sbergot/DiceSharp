import * as React from "react";

import { Link } from "../Components/Link";
import { Button } from "../Components/Button";
import { post } from "../http";

function createUrls(roomId: string): RoomUrls {
  const prefix = `${window.location.origin}/room/${roomId}`;
  const apiprefix = `${window.location.origin}/api/room/${roomId}`;
  return {
    joinUrl: `${prefix}/join`,
    quitUrl: `${prefix}/quit `,
    setLibrary: `${apiprefix}/library`,
    callFunction: `${apiprefix}/run`,
  };
}

interface LobbyProps {
  room: Room;
}

export function Lobby({ room }: LobbyProps) {
  const { joinUrl, quitUrl, setLibrary } = createUrls(room.id);
  const { users } = room;
  const [libraryScript, setLibrayScript] = React.useState("");
  return (
    <div>
      <Link href={quitUrl} label="Quitter" className="mr-2" />
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
      <p>Joueurs présents dans cette salle:</p>
      <ul className="list-disc list-inside">
        {Object.values(users).map((p) => {
          return (
            <li className="mt-4">
              <span className={`font-bold px-2 py-1 rounded-md`}>{p.name}</span>
            </li>
          );
        })}
      </ul>
      <textarea
        value={libraryScript}
        onChange={(e) => setLibrayScript(e.target.value)}
      />
    </div>
  );
}
