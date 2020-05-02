import * as React from "react";
import { HashRouter as Router, Switch, Route, Link } from "react-router-dom";

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
      <nav className="mb-4">
        <ul className="flex">
          <li>
            <Link to="/">Home</Link>
          </li>
          <li className="ml-4">
            <Link to="/admin">Admin</Link>
          </li>
        </ul>
      </nav>
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
