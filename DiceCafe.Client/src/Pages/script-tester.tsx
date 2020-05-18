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
    <div className="mt-2">
      <div className="flex">
        <div className="text-sm max-w-md mr-8">
          <h1 className="text-2xl font-bold mt-4">What is Dice Café?</h1>
          <p>
            Dice Café is a dice room platform. You can use it to manage all your
            dice rolls in an online rpg session. It uses DiceScript, a simple
            language for dice roll scripting.
          </p>
          <p>
            A DM needs a few minutes to write scripts for a system. During the
            game players can click on buttons to trigger rolls. The results are
            shared with every player in the room.
          </p>
        </div>
        <div className="text-sm max-w-lg">
          <h1 className="text-2xl font-bold mt-4">How to use</h1>
          <ol className="list-decimal list-inside">
            <li>First you need to create a room.</li>
            <li>
              Write macros for your room. Each macro contains a script an
              optionally some parameters.
            </li>
            <li>
              Invite players to the room by sharing the link or the room code.
            </li>
            <li>
              Each macro will appear as a button. Clicking on the button will
              run the script. The result of the roll will be displayed for all
              member present in the rooms.
            </li>
            <li>
              Optionally provide a discord webhook in the room administration.
              If you do so the results will also be published on the discord
              room.
            </li>
          </ol>
        </div>
      </div>
      <h1 className="text-2xl font-bold mt-2">Try it live!</h1>
      <p className="text-sm">
        You can test your script with the form below. For more information on
        DiceScript, please check the{" "}
        <a
          href="/dicescript_reference.html"
          target="_blank"
          className="btn-link"
        >
          documentation
        </a>
      </p>
      <div className="flex justify-start items-stretch pb-4 overflow-hidden">
        <div className="">
          <textarea
            className="mt-4 input-box"
            cols={80}
            rows={8}
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
      <Button label="run script" onclick={runScripts} />
      <p className="mt-4"></p>
    </div>
  );
}
