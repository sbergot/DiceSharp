import * as React from "react";
import { useRoomContext } from "../../room-context";

export function ResultList() {
  const { room } = useRoomContext();
  const { results } = room;

  return (
    <>
      <div className="absolute right-0 bottom-0 p-8 pr-20 max-h-screen h-full">
        <ul className="h-full w-56 flex flex-col justify-end overflow-hidden">
          {Object.values(results).map((f) => {
            return (
              <li className="mt-4 px-2 py-1 rounded-md bg-gray-200">
                {f.results.map((r) => (
                  <Result result={r} />
                ))}
              </li>
            );
          })}
        </ul>
      </div>
    </>
  );
}

function Result({ result }: { result: TaggedResult }) {
  return result.resultType == "Roll" ? (
    <RollResult result={result.result} />
  ) : (
    <PrintResult result={result.result} />
  );
}

function PrintResult({ result }: { result: PrintResult }) {
  return <>{result.value}</>;
}

function RollResult({ result }: { result: RollResult }) {
  var nameDisplay = result.name ? result.name + ": " : "";
  var diceDisplay = result.dices.map((d) => {
    var classes = ["mr-2", d.valid ? "" : "line-through"].join(" ");
    return <span className={classes}>{d.result}</span>;
  });
  return (
    <div>
      {nameDisplay}
      {result.dices.length > 1 ? (
        <>
          {diceDisplay}
          <span className="mr-2">=</span>
        </>
      ) : null}

      {result.result}
    </div>
  );
}
