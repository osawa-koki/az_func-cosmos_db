import React, { useState } from "react";
import { Button, CloseButton } from "react-bootstrap";
import { Form } from "react-bootstrap";
import User from "../interface/User";

type Props = {
  close_func: () => void;
  user?: User;
  // eslint-disable-next-line no-unused-vars
  update_func: (user: User) => void;
};

export function EditComponent({ close_func, user, update_func }: Props) {

  const [name, setName] = useState<string>(user.name);
  const [profession, setProfession] = useState<string>(user.profession);
  const [age, setAge] = useState<number>(user.age);

  const Update = async () => {
    const _user: User = {
      id: user.id,
      name: name,
      profession: profession,
      age: age,
    };
    update_func(_user);
    close_func();
  };

  return (
    <div id="EditBg">
      <div id="EditFg">
        <h1>Edit</h1>
        <CloseButton onClick={() => {close_func()}} />
        <Form>
          <Form.Group className="mt-3">
            <Form.Label>Name</Form.Label>
            <Form.Control type="text" placeholder="Enter name" value={name} onInput={(e) => {setName(e.currentTarget.value)}} />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>Profession</Form.Label>
            <Form.Control type="text" placeholder="Enter profession" value={profession} onInput={(e) => {setProfession(e.currentTarget.value)}} />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>Age</Form.Label>
            <Form.Control type="number" min={0} max={100} placeholder="Enter age" value={age} onInput={(e) => {setAge(parseInt(e.currentTarget.value))}} />
          </Form.Group>
          <Button variant="primary" className="d-block mx-auto mt-3" onClick={Update}>Update</Button>
        </Form>
      </div>
    </div>
  );
}
