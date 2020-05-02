import * as React from "react";
import { Button } from "../Components/Button";
import { createUrls, post } from "../http";

export function ResultList({ room }: RoomProp) {
  const { functions } = room;
  const { callFunction } = createUrls(room.id);

  return (
    <>
      <p>functions</p>
      <ul className="list-disc list-inside">
        {Object.values(functions).map((f) => {
          return (
            <li className="mt-4">
              <span className={`font-bold px-2 py-1 rounded-md`}>{f.name}</span>
              <Button
                label="Lancer"
                onclick={() =>
                  post(callFunction, { name: f.name, arguments: {} })
                }
              />
            </li>
          );
        })}
      </ul>
    </>
  );
}
