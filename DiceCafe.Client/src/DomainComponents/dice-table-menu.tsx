import * as React from "react";

import { Link } from "../UI/Components/Link";
import { Button } from "../UI/Components/Button";
import { Modal } from "../UI/Components/Modal";
import { HashLink } from "../UI/Components/HashLink";
import { useRoomContext } from "../room-context";

export function DiceTableMenu() {
  const { room, isCreator, urls } = useRoomContext();
  const { joinUrl, quitUrl } = urls;
  const [joinModalOpened, setJoinModalOpened] = React.useState(false);

  return (
    <>
      <Link href={quitUrl} label="Quit" className="mr-2" type="danger" />
      <Button
        onclick={() => setJoinModalOpened(true)}
        label="Invite"
        className="mr-2"
      />
      {isCreator ? (
        <HashLink
          href="/admin"
          label="Room admin"
          className="mr-2"
          type="link"
        />
      ) : null}

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
          type="secondary"
        />
      </Modal>
    </>
  );
}
