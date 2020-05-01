import * as React from "react";

import { Link } from "../Components/Link";
import { Button } from "../Components/Button";
import { post } from "../http";
import { getColorBgClass } from "../helpers";

function createUrls(roomId: string): RoomUrls {
  const prefix = `${window.location.origin}/room/${roomId}`;
  const apiprefix = `${window.location.origin}/api/room/${roomId}`;
  return {
    joinUrl: `${prefix}/join`,
    quitUrl: `${prefix}/quit `,
    addComputerUrl: `${apiprefix}/addcomputer`,
    startUrl: `${apiprefix}/start`,
  };
}

interface LobbyProps {
  room: Room;
}

export function Lobby({ room }: LobbyProps) {
  const { joinUrl, quitUrl, startUrl, addComputerUrl } = createUrls(room.id);
  const players = room.users;
  return (
    <div>
      <Link href={quitUrl} label="Quitter" className="mr-2" />
      <Button
        onclick={() => {
          post(startUrl);
        }}
        label="Démarrer"
        className="mr-2"
      />
      <Button
        onclick={() => {
          post(addComputerUrl);
        }}
        label="Ajouter ordinateur"
      />
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
        {Object.values(players).map((p) => {
          return (
            <li className="mt-4">
              <span className={`font-bold px-2 py-1 rounded-md`}>{p.name}</span>
            </li>
          );
        })}
      </ul>
    </div>
  );
}
