import * as React from "react";
import { useRoomContext } from "../../room-context";

export function ResultList() {
  const { room } = useRoomContext();
  const { results } = room;

  return (
    <>
      <div className="absolute right-0 bottom-0 p-8 pr-20 max-h-screen h-full max-w-md w-full">
        <ul className="h-full w-full flex flex-col justify-end overflow-hidden p-4">
          {Object.values(results).map((f) => {
            return (
              <li className="mt-4 px-2 py-1 rounded-md bg-gray-200 shadow-md">
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
  const nameDisplay = result.name ? result.name + ": " : "";
  const diceDisplay = result.dices.map((d) => {
    const classes = ["mr-2", d.valid ? "" : "line-through"].join(" ");
    return <span className={classes}>{d.result}</span>;
  });
  const rollDescription = formatDescription(result.description);
  return (
    <div>
      {nameDisplay}
      {rollDescription} &#8594;{" "}
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

function formatDescription(description: RollDescription) {
  const number = description.number > 1 ? description.number.toString() : "";
  let bonus = "";
  if (description.bonus > 0) {
    bonus = `+${description.bonus}`;
  }
  if (description.bonus < 0) {
    bonus = description.bonus.toString();
  }
  return `${number}D${description.faces}${bonus}`;
}
