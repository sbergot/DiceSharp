import * as React from "react";
import { HashRouter as Router, Switch, Route } from "react-router-dom";

import { DiceTable } from "./dice-table";
import { roomUpdateChannel } from "../connection";
import { Admin } from "./admin";

interface RoomProps {
  roomVm: RoomViewModel;
}

export function Room({ roomVm }: RoomProps) {
  const [room, setRoom] = React.useState(roomVm.room);

  React.useEffect(() => {
    roomUpdateChannel.subscribe(setRoom);
    return () => {
      roomUpdateChannel.unsubscribe(setRoom);
    };
  }, []);

  return (
    <Router>
      <Switch>
        <Route path="/admin">
          <Admin room={room} />
        </Route>
        <Route>
          <DiceTable room={room} />
        </Route>
      </Switch>
    </Router>
  );
}
