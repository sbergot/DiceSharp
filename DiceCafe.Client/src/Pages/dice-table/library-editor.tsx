import * as React from "react";
import { createUrls, post } from "../../http";
import { Button } from "../../Components/Button";

export function LibraryEditor({ room }: RoomProp) {
  const { setLibrary } = createUrls(room.id);
  const [libraryScript, setLibrayScript] = React.useState("");

  return (
    <>
      <textarea
        value={libraryScript}
        onChange={(e) => setLibrayScript(e.target.value)}
      />
      <Button label="Sauver" onclick={() => post(setLibrary, libraryScript)} />
    </>
  );
}
