import React from "react";
import { CloseButton } from "react-bootstrap";
import User from "../interface/User";

type Props = {
  delete_func: () => void;
  user?: User;
};

export function EditComponent({ delete_func }: Props) {

  return (
    <div>
      <h1>Edit</h1>
      <CloseButton onClick={() => {delete_func()}} />
    </div>
  );
}
