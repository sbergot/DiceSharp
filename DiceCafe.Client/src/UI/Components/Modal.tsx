import * as React from "react";

interface ModalProps extends ChildrenProp {
  active: boolean;
}

export function Modal({ active, children }: ModalProps) {
  const [reduced, setReduced] = React.useState(false);

  React.useEffect(() => {
    window.document.body.classList.toggle("modal-active", active);
    setReduced(false);
    return () => {
      window.document.body.classList.toggle("modal-active", false);
    };
  }, [active]);

  const modalClasses = [
    "opacity-ease fixed w-full h-full top-0 left-0 flex items-center justify-center z-10",
    active ? "" : "opacity-0 pointer-events-none",
  ].join(" ");

  const containerClasses = [
    "bg-white md:max-w-md rounded shadow-lg z-50 overflow-y-auto",
    reduced ? "absolute right-0 bottom-0 mb-4 mr-4" : "mx-auto relative",
  ].join(" ");

  const overlayClasses = [
    "opacity-ease absolute w-full h-full bg-gray-900",
    reduced ? "opacity-25" : "opacity-50",
  ].join(" ");

  return (
    <div className={modalClasses}>
      <div className={overlayClasses}></div>
      <div className={containerClasses}>
        <div className="py-4 text-left pl-6 pr-12">{children}</div>
      </div>
    </div>
  );
}
