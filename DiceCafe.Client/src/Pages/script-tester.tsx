import * as React from "react";

import { ResultList } from "../DomainComponents/result-list";
import { Button } from "../UI/Components/Button";
import { post } from "../http";

const defaultScript = `dice $a<- roll 8D6 (exp);
int $successes <- aggregate $a (>4, count);
int $failures <- aggregate $a (<3, count);
int $result <- calc $successes - $failures;
match $result ((<0, "failure"), (<3, "success"), (default, "critical success"));`;

export function ScriptTester() {
  const [libraryScript, setLibrayScript] = React.useState(defaultScript);
  const [results, setResults] = React.useState<ResultGroup[]>([]);

  async function runScripts() {
    const response = await post("/api/runscript", libraryScript);
    const newres: ResultGroup = await response.json();
    setResults((results) => [...results, newres]);
  }

  return (
    <>
      <div className="flex mt-4">
        <div className="">
          <textarea
            className="mt-4 input-box"
            cols={80}
            rows={20}
            value={libraryScript}
            onChange={(e) => setLibrayScript(e.target.value)}
          />
        </div>
        <div className="absolute right-0 bottom-0 p-8 pr-20 max-h-screen h-full max-w-md w-full">
          <ResultList results={results} />
        </div>
      </div>
      <Button label="run" onclick={runScripts} />
    </>
  );
}
