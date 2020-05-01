type UserId = string;

interface RoomViewModel {
  room: Room;
  playerId: UserId;
}

interface Room {
  id: string;
  users: Record<UserId, User>;
  lastUpdate: string;
}

type RoomMode = "Starting" | "InProgress" | "Ended";

type Color =
  | "Red"
  | "Orange"
  | "Yellow"
  | "Green"
  | "Teal"
  | "Blue"
  | "Indigo"
  | "Purple"
  | "Pink";

type PlayerType = "Human" | "Computer";

interface User {
  id: UserId;
  name: string;
}

interface RoomUrls {
  joinUrl: string;
  quitUrl: string;
  startUrl: string;
  addComputerUrl: string;
}
