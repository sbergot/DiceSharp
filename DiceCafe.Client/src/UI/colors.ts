export const bgColor: Record<UIType, string> = {
  main: "bg-blue-500",
  secondary: "bg-gray-600",
  danger: "bg-red-500",
};

export const bgHoverColor: Record<UIType, string> = {
  main: "hover:bg-blue-700",
  secondary: "hover:bg-gray-800",
  danger: "hover:bg-red-700",
};

export function getButtonLikeColors(mtype?: UIType): string {
  const type = mtype ?? "secondary";
  return `${bgColor[type]} ${bgHoverColor[type]} text-white`;
}
