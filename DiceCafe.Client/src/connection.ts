import {
  HubConnectionBuilder,
  LogLevel,
  HubConnection,
} from "@microsoft/signalr";

type Updater<T> = (value: T) => void;

class Channel<T> {
  listeners: Updater<T>[] = [];

  subscribe(cb: Updater<T>) {
    this.listeners.push(cb);
  }

  unsubscribe(cb: Updater<T>) {
    this.listeners = this.listeners.filter((c) => c != cb);
  }

  fire(value: T) {
    this.listeners.forEach((cb) => cb(value));
  }
}

export const roomUpdateChannel = new Channel<Room>();

let connection: HubConnection;

export async function startConnection(roomId: string) {
  const startingCon = new HubConnectionBuilder()
    .withUrl("/roomHub")
    .configureLogging(LogLevel.Information)
    .build();

  startingCon.on("Update", (room) => roomUpdateChannel.fire(room));

  await startingCon.start();
  connection = startingCon;
  connection.send("JoinRoom", roomId);
}

export function leaveRoom(roomId: string) {
  if (connection) {
    connection.send("LeaveRoom", roomId);
  }
}
