import * as React from "react";
import { bgColor, bgHoverColor, getButtonLikeColors } from "../colors";

interface ButtonProps extends ClassProp {
  onclick(): void;
  label: string;
  disabled?: boolean;
  type?: UIType;
}

export function Button({
  onclick,
  label,
  disabled,
  className,
  type,
}: ButtonProps) {
  const classes = [
    "inline-block font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline",
    disabled
      ? "bg-gray-500 text-white cursor-not-allowed"
      : getButtonLikeColors(type),
    className || "",
  ].join(" ");
  return (
    <button className={classes} onClick={onclick} disabled={disabled}>
      {label}
    </button>
  );
}
