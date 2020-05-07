const colorMap: Record<UIType, string> = {
  primary: "btn-primary",
  secondary: "btn-secondary",
  danger: "btn-danger",
  link: "btn-link",
};

export function getButtonLikeColors(mtype?: UIType): string {
  const type = mtype ?? "secondary";
  return colorMap[type];
}
