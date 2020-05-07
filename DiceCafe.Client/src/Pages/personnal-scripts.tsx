import * as React from "react";

import { HashLink } from "../UI/Components/HashLink";
import { Button } from "../UI/Components/Button";
import { toast } from "react-toastify";
import { useRoomContext } from "../room-context";
import { Modal } from "../UI/Components/Modal";
import { Selectable } from "../UI/Components/Selectable";
import { useLocalStorage } from "../localstorage";
import { post } from "../http";

export function PersonnalScript() {
  const { room, urls, isCreator } = useRoomContext();
  const { setLibrary } = urls;
  const { libraryScript } = room;
  const [saveNewOpened, setSaveNewOpened] = React.useState(false);
  const [deleteConfirmOpened, setDeleteConfirmOpened] = React.useState(false);
  const [newName, setNewName] = React.useState("");
  const [selectedKey, setSelectedKey] = React.useState<string | null>(null);
  const [localScripts, setLocalScripts] = useLocalStorage<
    Record<string, string>
  >("dicecafe.localScripts", {});

  function save(scripts: Record<string, string>) {
    setLocalScripts(scripts);
    toast("Saved", { type: toast.TYPE.SUCCESS });
  }

  function saveTo(key: string) {
    const newScripts = { ...localScripts, [key]: libraryScript };
    save(newScripts);
  }

  function remove(key: string | null) {
    if (key == null) {
      throw new Error("key is null");
    }
    const newScript = { ...localScripts };
    delete newScript[key];
    save(newScript);
    setDeleteConfirmOpened(false);
  }

  function saveNew() {
    saveTo(newName);
    setSaveNewOpened(false);
  }

  async function saveLibrary() {
    if (selectedKey == null) {
      throw new Error("key is null");
    }
    const response = await post(setLibrary, localScripts[selectedKey]);
    if (response.status == 200) {
      toast("Room updated successfully", { type: toast.TYPE.SUCCESS });
    } else {
      toast("An error has occured", { type: toast.TYPE.ERROR });
    }
  }

  return (
    <div>
      <Modal active={saveNewOpened}>
        <input
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
          className="input-box mr-2"
          placeholder="entry name"
        />
        <Button
          onclick={() => setSaveNewOpened(false)}
          label="Cancel"
          className="mr-2"
        />
        <Button onclick={saveNew} label="Save" type="primary" />
      </Modal>
      <Modal active={deleteConfirmOpened}>
        <p>Are you sure that you want to delete "{selectedKey}"?</p>
        <Button
          onclick={() => setDeleteConfirmOpened(false)}
          label="Cancel"
          className="mr-2"
        />
        <Button
          onclick={() => remove(selectedKey)}
          label="Confirm"
          type="danger"
        />
      </Modal>
      <HashLink href="/" label="Back" className="mr-2" type="link" />
      <Button
        onclick={() => setSaveNewOpened(true)}
        label="Save room script to new"
        type="primary"
        className="mr-2"
      />
      {selectedKey != null ? (
        <>
          {isCreator ? (
            <Button
              label="Set as room script"
              onclick={saveLibrary}
              className="mr-2"
              type="primary"
            />
          ) : null}
          <Button
            label="Overwrite with room script"
            onclick={() => saveTo(selectedKey)}
            className="mr-2"
            type="primary"
          />
          <Button
            label="Delete"
            onclick={() => setDeleteConfirmOpened(true)}
            type="danger"
          />
        </>
      ) : null}
      <div className="flex mt-8">
        <div className="mr-8">
          <ul>
            {Object.keys(localScripts).map((k) => (
              <li>
                <Selectable
                  label={k}
                  onclick={() => setSelectedKey(k)}
                  selected={k === selectedKey}
                  className="mt-2 inline-block"
                />
              </li>
            ))}
          </ul>
        </div>
        <div>
          {selectedKey != null ? (
            <pre className="ml-4">{localScripts[selectedKey]}</pre>
          ) : null}
        </div>
      </div>
    </div>
  );
}
