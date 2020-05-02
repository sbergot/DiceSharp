type UserId = string;

interface RoomViewModel {
  room: Room;
  userId: string;
}

interface Room {
  id: string;
  users: User[];
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
  setLibrary: string;
  callFunction: string;
}
