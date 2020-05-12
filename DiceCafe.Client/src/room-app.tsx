import * as React from "react";
import { HashRouter as Router, Switch, Route } from "react-router-dom";

import { DiceTable } from "./Pages/dice-table";
import { Admin } from "./Pages/admin";
import { RoomContextListener } from "./room-context";
import { PersonnalScript } from "./Pages/personnal-scripts";
import { ScriptTester } from "./Pages/script-tester";

export function App() {
  return (
    <RoomContextListener>
      <Router>
        <Switch>
          <Route path="/admin">
            <Admin />
          </Route>
          <Route path="/personnal-scripts">
            <PersonnalScript />
          </Route>
          <Route path="/script-tester">
            <ScriptTester />
          </Route>
          <Route>
            <DiceTable />
          </Route>
        </Switch>
      </Router>
    </RoomContextListener>
  );
}
