const bgColor: Record<UIType, string> = {
  primary: "bg-blue-500",
  secondary: "bg-gray-600",
  danger: "bg-red-500",
  link: "bg-white",
};

const bgHoverColor: Record<UIType, string> = {
  primary: "hover:bg-blue-700",
  secondary: "hover:bg-gray-800",
  danger: "hover:bg-red-700",
  link: "",
};

const textColor: Record<UIType, string> = {
  primary: "text-white",
  secondary: "text-white",
  danger: "text-white",
  link: "text-blue-600 hover:underline",
};

export function getButtonLikeColors(mtype?: UIType): string {
  const type = mtype ?? "secondary";
  return `${bgColor[type]} ${bgHoverColor[type]} ${textColor[type]}`;
}
