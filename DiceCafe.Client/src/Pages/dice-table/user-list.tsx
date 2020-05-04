import * as React from "react";

export function UserList({ room }: RoomProp) {
  const { users } = room;

  return (
    <>
      <ul className="">
        {Object.values(users).map((p) => {
          return (
            <li className="mt-4">
              <span className={`font-bold px-2 py-1 rounded-md`}>{p.name}</span>
            </li>
          );
        })}
      </ul>
    </>
  );
}
