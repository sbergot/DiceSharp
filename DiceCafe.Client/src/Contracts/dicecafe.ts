type UserId = string;

interface RoomViewModel {
  room: Room;
  userId: string;
}

interface Room {
  id: string;
  creator: User;
  users: User[];
  libraryScript: string;
  discordWebHook: string;
  results: ResultGroup[];
  functions: FunctionSpec[];
}

interface User {
  id: UserId;
  name: string;
}

interface RoomUrls {
  joinUrl: string;
  quitUrl: string;
  setConfiguration: string;
  callFunction: string;
}
