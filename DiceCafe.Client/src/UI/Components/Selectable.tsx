import * as React from "react";

interface SelectableProps extends ClassProp {
  onclick(): void;
  label: string;
  selected?: boolean;
}

export function Selectable({
  onclick,
  label,
  className,
  selected,
}: SelectableProps) {
  const classes = [
    "font-bold py-2 px-4 rounded cursor-pointer border-solid border hover:shadow-md focus:outline-none",
    selected ? "border-gray-600 shadow-md" : "",
    className || "",
  ].join(" ");
  return (
    <div className={classes} onClick={onclick}>
      {label}
    </div>
  );
}
