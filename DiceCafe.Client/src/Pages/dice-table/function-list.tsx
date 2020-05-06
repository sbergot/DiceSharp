import * as React from "react";
import { Button } from "../../Components/Button";
import { post } from "../../http";
import { useRoomContext } from "../../room-context";

export function FunctionList() {
  const { room, urls } = useRoomContext();
  const { functions } = room;
  const { callFunction } = urls;

  return (
    <ul>
      {Object.values(functions).map((f) => {
        return (
          <li className="mt-4">
            <FunctionCall spec={f} callFunction={callFunction} />
          </li>
        );
      })}
    </ul>
  );
}

export function FunctionCall({
  spec,
  callFunction,
}: {
  spec: FunctionSpec;
  callFunction: string;
}) {
  const [params, setParams] = React.useState<Record<string, number>>({});
  function setParam(name: string, value: string) {
    setParams((p) => ({ ...p, [name]: parseInt(value) }));
  }

  return (
    <>
      <Button
        label={spec.name}
        onclick={() =>
          post(callFunction, { name: spec.name, arguments: params })
        }
      />
      {spec.arguments.map((arg) => (
        <div className="mt-2">
          <span className="mr-2 inline-block w-24">{arg}</span>
          <input
            className="border-solid border-4 border-gray-600 inline-block"
            type="number"
            value={params[arg]}
            onChange={(e) => setParam(arg, e.target.value)}
          />
        </div>
      ))}
    </>
  );
}
