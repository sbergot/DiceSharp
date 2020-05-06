import * as React from "react";

import { Link } from "../../Components/Link";
import { Button } from "../../Components/Button";
import { Modal } from "../../Components/Modal";
import { HashLink } from "../../Components/HashLink";
import { useRoomContext } from "../../room-context";

export function JoinArea() {
  const { room, isCreator, urls } = useRoomContext();
  const { joinUrl, quitUrl } = urls;
  const [joinModalOpened, setJoinModalOpened] = React.useState(false);

  return (
    <>
      <Button
        onclick={() => setJoinModalOpened(true)}
        label="Invite"
        className="mr-2"
      />
      {isCreator ? (
        <HashLink href="/admin" label="Admin" className="mr-2" />
      ) : null}
      <Link href={quitUrl} label="Quit" className="mr-2" />

      <Modal active={joinModalOpened}>
        <p>
          Other users can join this room by clicking on this url:{" "}
          <a
            className="underline text-blue-400 visited:text-blue-800"
            href={joinUrl}
          >
            {joinUrl}
          </a>
        </p>
        <p>
          They can also click on "Join room" and provide the following code:{" "}
          <strong>{room.id}</strong>
        </p>
        <Button
          className="mt-4"
          onclick={() => setJoinModalOpened(false)}
          label="Close"
        />
      </Modal>
    </>
  );
}
