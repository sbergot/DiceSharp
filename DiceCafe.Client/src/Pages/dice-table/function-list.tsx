import * as React from "react";
import { Button } from "../../UI/Components/Button";
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
  const [warnDisplayed, setWarnDisplayed] = React.useState(false);
  function setParam(name: string, value: string) {
    setParams((p) => ({ ...p, [name]: parseInt(value) }));
  }
  function call() {
    for (const arg of spec.arguments) {
      if (!params[arg]) {
        setWarnDisplayed(true);
        return;
      }
    }
    setWarnDisplayed(false);
    post(callFunction, { name: spec.name, arguments: params });
  }

  return (
    <div className="w-56 mt-2">
      <Button label={spec.name} onclick={call} type="main" className="w-full" />
      {spec.arguments.map((arg) => (
        <div className="mt-2 flex justify-between">
          <span>{arg}</span>
          <input
            className="w-20 p-1 border-solid border-4 border-gray-600"
            type="number"
            value={params[arg]}
            onChange={(e) => setParam(arg, e.target.value)}
          />
        </div>
      ))}
      {warnDisplayed ? (
        <div className="h-4 text-red-400">Please fill all parameters</div>
      ) : (
        <div className="h-4"></div>
      )}
    </div>
  );
}
