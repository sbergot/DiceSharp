import * as React from "react";

interface ButtonProps extends ClassProp {
  onclick(): void;
  label: string;
  disabled?: boolean;
}

export function Button({ onclick, label, disabled, className }: ButtonProps) {
  const classes = [
    "inline-block text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline",
    disabled
      ? "bg-gray-500 cursor-not-allowed"
      : "bg-blue-500 hover:bg-blue-700",
    className || ""
  ].join(" ");
  return (
    <button className={classes} onClick={onclick} disabled={disabled}>
      {label}
    </button>
  );
}
