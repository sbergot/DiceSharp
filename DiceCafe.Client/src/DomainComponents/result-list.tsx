import * as React from "react";

interface ResultListProps {
  results: ResultGroup[];
}

function formatDate(date: Date): string {
  return `${date.getFullYear()}-${date.getMonth()}-${date.getDate()} ${date.getHours()}:${date.getMinutes()}`;
}

export function ResultList({ results }: ResultListProps) {
  return (
    <ul className="h-full max-h-full w-full flex flex-col justify-end">
      {Object.values(results).map((f) => {
        const datestr = formatDate(new Date(f.created));
        return (
          <li className="mt-4 p-2 rounded-md bg-gray-200 shadow-md">
            <div className="text-xs flex justify-between">
              <div>{datestr}</div> <div>{f.user.name}</div>
            </div>
            {f.results.map((r) => (
              <Result result={r} />
            ))}
          </li>
        );
      })}
    </ul>
  );
}

function Result({ result }: { result: TaggedResult }) {
  switch (result.resultType) {
    case "Roll":
      return <RollResult result={result.result} />;
    case "Print":
      return <PrintResult result={result.result} />;
    case "Value":
      return <ValueResult result={result.result} />;
    case "Dice":
      return <DiceResult result={result.result} />;
  }
}

function ValueResult({ result }: { result: ValueResult }) {
  if (result.name == null) {
    return null;
  }
  return (
    <div>
      {result.name}: {result.result}
    </div>
  );
}

function DiceResult({ result }: { result: DiceResult }) {
  const nameDisplay = result.name ? result.name + ": " : "";
  const diceDisplay = result.dices.map((d) => {
    return <span className="mr-2">{d.result}</span>;
  });
  const rollDescription = formatDescription(result.description);
  return (
    <div>
      {nameDisplay}
      {rollDescription} &#8594; {diceDisplay}
    </div>
  );
}

function PrintResult({ result }: { result: PrintResult }) {
  return <div>{result.value}</div>;
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
