import * as React from "react";
import { Link } from "react-router-dom";
import { getButtonLikeColors } from "../colors";

interface LinkProps extends ClassProp {
  href: string;
  label: string;
  type?: UIType;
}

export function HashLink({ href, label, className, type }: LinkProps) {
  const classes = ["btn", getButtonLikeColors(type), className || ""].join(" ");
  return (
    <Link className={classes} to={href}>
      {label}
    </Link>
  );
}
