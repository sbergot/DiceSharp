import * as React from "react";

import { HashLink } from "../UI/Components/HashLink";
import { post } from "../http";
import { Button } from "../UI/Components/Button";
import { toast } from "react-toastify";
import { useRoomContext } from "../room-context";
import { Modal } from "../UI/Components/Modal";
import { useLocalStorage } from "../localstorage";

export function Admin() {
  const { room, urls } = useRoomContext();
  const { setConfiguration } = urls;
  const [libraryScript, setLibraryScript] = React.useState(room.libraryScript);
  const isLibrarySaved = libraryScript === room.libraryScript;
  const [discordWebHook, setDiscordWebHook] = React.useState(
    room.discordWebHook
  );
  const isWebHookSaved = discordWebHook === room.discordWebHook;
  const [loadScriptModalActive, setLoadScriptModalActive] = React.useState(
    false
  );
  const [saveScriptModalActive, setSaveScriptModalActive] = React.useState(
    false
  );
  const [deleteScriptModalActive, setDeleteScriptModalActive] = React.useState(
    false
  );
  const [newLocalScriptName, setNewLocalScriptName] = React.useState("");

  const [localScripts, setLocalScripts] = useLocalStorage<
    Record<string, string>
  >("dicecafe.localScripts", {});

  async function saveConfiguration() {
    const response = await post(setConfiguration, {
      libraryScript,
      discordWebHook,
    });
    if (response.status == 200) {
      toast("Room updated successfully", { type: toast.TYPE.SUCCESS });
    } else {
      toast("An error has occured", { type: toast.TYPE.ERROR });
    }
  }

  function loadLocalScript(name: string) {
    setLibraryScript(localScripts[name]);
    setLoadScriptModalActive(false);
  }

  function saveLocalScripts(scripts: Record<string, string>) {
    setLocalScripts(scripts);
    toast("Saved", { type: toast.TYPE.SUCCESS });
  }

  function removeLocalScript(name: string | null) {
    if (name == null) {
      throw new Error("key is null");
    }
    const newScript = { ...localScripts };
    delete newScript[name];
    saveLocalScripts(newScript);
    setDeleteScriptModalActive(false);
  }

  function saveLocalScript(name: string) {
    const newScripts = { ...localScripts, [name]: libraryScript };
    saveLocalScripts(newScripts);
    setNewLocalScriptName("");
    setSaveScriptModalActive(false);
  }

  return (
    <div>
      <Modal active={loadScriptModalActive}>
        Select a local script to load
        <ul className="flex flex-wrap mt-2">
          {Object.keys(localScripts).map((k) => (
            <li>
              <Button
                label={k}
                onclick={() => loadLocalScript(k)}
                className="mr-2 inline-block"
                type="primary"
              />
            </li>
          ))}
        </ul>
        <Button
          onclick={() => setLoadScriptModalActive(false)}
          label="Cancel"
          className="mt-2"
        />
      </Modal>
      <Modal active={deleteScriptModalActive}>
        Select a local script to delete.
        <p className="font-bold">Warning! This operation cannot be undone.</p>
        <ul className="flex flex-wrap mt-2">
          {Object.keys(localScripts).map((k) => (
            <li>
              <Button
                label={k}
                onclick={() => removeLocalScript(k)}
                className="mr-2 inline-block"
                type="danger"
              />
            </li>
          ))}
        </ul>
        <Button
          onclick={() => setDeleteScriptModalActive(false)}
          label="Cancel"
          className="mt-2"
        />
      </Modal>
      <Modal active={saveScriptModalActive}>
        <p>Pick an existing script to overwrite...</p>
        <ul className="flex flex-wrap mt-2">
          {Object.keys(localScripts).map((k) => (
            <li>
              <Button
                label={k}
                onclick={() => saveLocalScript(k)}
                className="mr-2 inline-block"
                type="primary"
              />
            </li>
          ))}
        </ul>
        <p className="my-2">...Or create a new one</p>
        <input
          value={newLocalScriptName}
          onChange={(e) => setNewLocalScriptName(e.target.value)}
          className="input-box mr-2"
          placeholder="entry name"
        />
        <div className="mt-2">
          <Button
            onclick={() => setSaveScriptModalActive(false)}
            label="Cancel"
            className="mr-2"
          />
          <Button
            onclick={() => saveLocalScript(newLocalScriptName)}
            label="Save new"
            type="primary"
          />
        </div>
      </Modal>
      <div className="flex">
        <HashLink href="/" label="Back" className="mr-2" type="link" />
        <Button
          label="Update Room Configuration"
          className="mr-2"
          onclick={saveConfiguration}
          type="primary"
        />
        <Button
          label="Load local script"
          className="mr-2"
          onclick={() => setLoadScriptModalActive(true)}
          type="primary"
        />
        <Button
          label="Save to local script"
          className="mr-2"
          onclick={() => setSaveScriptModalActive(true)}
          type="primary"
        />
        <Button
          label="Delete local script"
          className="mr-2"
          onclick={() => setDeleteScriptModalActive(true)}
          type="danger"
        />
      </div>
      <div className="mt-4">
        <label htmlFor="discordwebhook" className="block text-lg">
          Discord Webhook
          {isWebHookSaved ? (
            <span className="capicon ml-2">&#xe636;</span>
          ) : (
            <span className="ml-2 text-gray-600">(not saved)</span>
          )}
        </label>
        <input
          id="discordwebhook"
          className="input-box text-xs max-w-4xl w-full"
          value={discordWebHook}
          onChange={(e) => setDiscordWebHook(e.target.value)}
        />
      </div>
      <div className="mt-4">
        <label htmlFor="roommacros" className="block text-lg">
          Room macros
          {isLibrarySaved ? (
            <span className="capicon ml-2">&#xe636;</span>
          ) : (
            <span className="ml-2 text-gray-600">(not saved)</span>
          )}
        </label>
        <textarea
          id="roommacros"
          className="input-box"
          cols={80}
          rows={20}
          value={libraryScript}
          onChange={(e) => setLibraryScript(e.target.value)}
        />
      </div>
    </div>
  );
}
