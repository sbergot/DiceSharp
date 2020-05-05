export async function post(url: string, data?: object | string) {
  const init: RequestInit = {
    method: "POST",
  };
  if (data) {
    init.headers = {
      Accept: "application/json",
      "Content-Type": "application/json",
    };
    init.body = JSON.stringify(data);
  }
  return await fetch(url, init);
}

export function createUrls(roomId: string): RoomUrls {
  const prefix = `${window.location.origin}/room/${roomId}`;
  const apiprefix = `${window.location.origin}/api/room/${roomId}`;
  return {
    joinUrl: `${prefix}/join`,
    quitUrl: `${prefix}/quit `,
    setLibrary: `${apiprefix}/library`,
    callFunction: `${apiprefix}/run`,
  };
}
