import * as React from "react";
import { getButtonLikeColors } from "../colors";

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
    "btn",
    disabled ? "btn-disabled" : getButtonLikeColors(type),
    className || "",
  ].join(" ");
  return (
    <button className={classes} onClick={onclick} disabled={disabled}>
      {label}
    </button>
  );
}
