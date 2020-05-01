import * as React from "react";

interface LinkProps extends ClassProp {
  href: string;
  label: string;
}

export function Link({ href, label, className }: LinkProps) {
  const classes = [
    "inline-block bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline",
    className || ""
  ].join(" ");
  return (
    <a className={classes} href={href}>
      {label}
    </a>
  );
}
