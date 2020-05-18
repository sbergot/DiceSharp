import * as React from "react";
import { toast } from "react-toastify";
import { useRoomContext } from "../room-context";
import { post } from "../http";
import { Button } from "../UI/Components/Button";

export function FunctionList() {
  const { room, urls } = useRoomContext();
  const { functions } = room;
  const { callFunction } = urls;

  return (
    <>
      {Object.keys(functions).length == 0 ? (
        <div>
          No macro defined. Please go to the room administration to define a
          library.
        </div>
      ) : (
        <ul>
          {Object.values(functions).map((f) => {
            return (
              <li className="mt-4">
                <FunctionCall spec={f} callFunction={callFunction} />
              </li>
            );
          })}
        </ul>
      )}
    </>
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
  async function call() {
    for (const arg of spec.arguments) {
      if (!params[arg]) {
        toast("Please fill all parameters", { type: toast.TYPE.ERROR });
        return;
      }
    }
    const response = await post(callFunction, {
      name: spec.name,
      arguments: params,
    });
    if (response.status != 200) {
      toast("An error has occured", { type: toast.TYPE.ERROR });
    }
  }

  return (
    <div className="w-56 mt-2">
      <Button
        label={spec.name}
        onclick={call}
        type="primary"
        className="w-full"
      />
      {spec.arguments.map((arg) => (
        <div className="mt-2 flex justify-between">
          <span>{arg}</span>
          <input
            className="w-20 input-box"
            type="number"
            value={params[arg]}
            onChange={(e) => setParam(arg, e.target.value)}
          />
        </div>
      ))}
    </div>
  );
}
