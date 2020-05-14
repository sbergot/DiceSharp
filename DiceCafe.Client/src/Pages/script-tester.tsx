import * as React from "react";

import { ResultList } from "../DomainComponents/result-list";
import { Button } from "../UI/Components/Button";
import { post } from "../http";

const defaultScript = `dice $a<- roll 8D6 (exp);
int $successes <- aggregate $a (>4, count);
int $failures <- aggregate $a (<3, count);
int $result <- calc "result" $successes - $failures;
match $result ((<1, "failure"), (<3, "success"), (default, "critical success"));`;

export function ScriptTester() {
  const [libraryScript, setLibrayScript] = React.useState(defaultScript);
  const [results, setResults] = React.useState<ResultGroup[]>([]);

  async function runScripts() {
    const response = await post("/api/runscript", libraryScript);
    const newres: ResultGroup = await response.json();
    setResults((results) => [...results, newres]);
  }

  return (
    <div className="mt-4">
      <p>
        Dice Café is a dice room platform. You can use it to manage all your
        dice rolls in an online rpg session. It uses DiceScript, a simple
        language for dice roll scripting.
      </p>
      <h1 className="text-3xl font-bold">Try it live!</h1>
      <div className="flex justify-start items-stretch pb-4 overflow-hidden">
        <div className="">
          <textarea
            className="mt-4 input-box"
            cols={80}
            rows={10}
            value={libraryScript}
            onChange={(e) => setLibrayScript(e.target.value)}
          />
        </div>
        <div className="relative max-w-sm w-full ml-4">
          <div className="absolute right-0 bottom-0 h-full w-full">
            <ResultList results={results} />
          </div>
        </div>
      </div>
      <Button label="run script" onclick={runScripts} className="mt-2" />
      <p className="mt-4">
        For more information on DiceScript, please check the{" "}
        <a
          href="/dicescript_reference.html"
          target="_blank"
          className="btn-link"
        >
          documentation
        </a>
      </p>
    </div>
  );
}
