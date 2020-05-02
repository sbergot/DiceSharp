import * as React from "react";
import { Link } from "react-router-dom";

interface LinkProps extends ClassProp {
  href: string;
  label: string;
}

export function HashLink({ href, label, className }: LinkProps) {
  const classes = [
    "inline-block bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline",
    className || "",
  ].join(" ");
  return (
    <Link className={classes} to={href}>
      {label}
    </Link>
  );
}
