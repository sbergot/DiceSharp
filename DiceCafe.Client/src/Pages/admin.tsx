import * as React from "react";

import { HashLink } from "../Components/HashLink";
import { createUrls, post } from "../http";
import { Button } from "../Components/Button";
import { toast } from "react-toastify";
import { useRoomContext } from "../room-context";

export function Admin() {
  const { room } = useRoomContext();
  const { setLibrary } = createUrls(room.id);
  const [libraryScript, setLibrayScript] = React.useState(room.libraryScript);

  async function saveLibrary() {
    const response = await post(setLibrary, libraryScript);
    if (response.status == 200) {
      toast("Script saved successfully", { type: toast.TYPE.SUCCESS });
    } else {
      toast("An error has occured", { type: toast.TYPE.ERROR });
    }
  }

  return (
    <div>
      <div className="flex">
        <HashLink href="/" label="Back" className="mr-2" />
        <Button label="Save" onclick={saveLibrary} />
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
