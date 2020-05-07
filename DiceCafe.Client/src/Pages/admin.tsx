import * as React from "react";

import { HashLink } from "../UI/Components/HashLink";
import { post } from "../http";
import { Button } from "../UI/Components/Button";
import { toast } from "react-toastify";
import { useRoomContext } from "../room-context";

export function Admin() {
  const { room, urls } = useRoomContext();
  const { setLibrary } = urls;
  const [libraryScript, setLibrayScript] = React.useState(room.libraryScript);

  async function saveLibrary() {
    const response = await post(setLibrary, libraryScript);
    if (response.status == 200) {
      toast("Room updated successfully", { type: toast.TYPE.SUCCESS });
    } else {
      toast("An error has occured", { type: toast.TYPE.ERROR });
    }
  }

  return (
    <div>
      <div className="flex">
        <HashLink href="/" label="Back" className="mr-2" type="secondary" />
        <Button label="Set Room Script" onclick={saveLibrary} type="primary" />
      </div>
      <textarea
        className="mt-4 p-2 border-solid border-4 border-gray-600"
        cols={80}
        rows={20}
        value={libraryScript}
        onChange={(e) => setLibrayScript(e.target.value)}
      />
    </div>
  );
}
