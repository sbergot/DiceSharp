import * as React from "react";
import { getButtonLikeColors } from "../colors";

interface LinkProps extends ClassProp {
  href: string;
  label: string;
  type?: UIType;
}

export function Link({ href, label, className, type }: LinkProps) {
  const classes = [
    "inline-block font-bold py-2 px-4 rounded focus:outline-none",
    getButtonLikeColors(type),
    className || "",
  ].join(" ");
  return (
    <a className={classes} href={href}>
      {label}
    </a>
  );
}