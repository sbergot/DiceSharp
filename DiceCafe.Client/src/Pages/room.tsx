import * as React from "react";
import { HashRouter as Router, Switch, Route } from "react-router-dom";

import { DiceTable } from "./dice-table";
import { Admin } from "./admin";
import { RoomContextListener } from "../room-context";

export function Room() {
  return (
    <RoomContextListener>
      <Router>
        <Switch>
          <Route path="/admin">
            <Admin />
          </Route>
          <Route>
            <DiceTable />
          </Route>
        </Switch>
      </Router>
    </RoomContextListener>
  );
}
