export function post(url: string, data?: object | string) {
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
  fetch(url, init);
}
