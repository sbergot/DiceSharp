import * as React from "react";
import { Link } from "react-router-dom";
import { bgColor, bgHoverColor, getButtonLikeColors } from "../colors";

interface LinkProps extends ClassProp {
  href: string;
  label: string;
  type?: UIType;
}

export function HashLink({ href, label, className, type }: LinkProps) {
  const classes = [
    "inline-block font-bold py-2 px-4 rounded",
    getButtonLikeColors(type),
    className || "",
  ].join(" ");
  return (
    <Link className={classes} to={href}>
      {label}
    </Link>
  );
}
