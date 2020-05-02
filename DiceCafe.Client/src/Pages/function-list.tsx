import * as React from "react";
import { Button } from "../Components/Button";
import { createUrls, post } from "../http";

export function FunctionList({ room }: RoomProp) {
  const { functions } = room;
  const { callFunction } = createUrls(room.id);

  return (
    <>
      <p>functions</p>
      <ul>
        {Object.values(functions).map((f) => {
          return (
            <li className="mt-4">
              <Button
                label={f.name}
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
