import * as React from "react";

export function FunctionList({ room }: RoomProp) {
  const { results } = room;

  return (
    <>
      <ul className="list-disc list-inside">
        {Object.values(results).map((f) => {
          return (
            <li className="mt-4">
              <span className={`font-bold px-2 py-1 rounded-md`}>
                {(f as RollResult).result}
              </span>
            </li>
          );
        })}
      </ul>
    </>
  );
}
