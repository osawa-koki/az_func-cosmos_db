import React, { useState } from "react";

import { Button, Alert, Form, Table } from 'react-bootstrap';
import { EditComponent } from "../components/Edit";
import Layout from "../components/Layout";
import User from "../interface/User";
import setting from "../setting";

export default function InsertPage() {

  const [id, setId] = useState<string>('');
  const [name, setName] = useState<string>('');
  const [profession, setProfession] = useState<string>('');
  const [age, setAge] = useState<number>();
  const [state, setState] = useState<['primary' | 'danger' | 'info' | 'secondary', string]>(['secondary', '']);
  const [users, setUsers] = useState<User[]>([]);
  const [edit_mode, setEditMode] = useState<boolean>(false);
  const [edit_user, setEditUser] = useState<User>();

  const Search = async () => {
    const condition = {};
    if (id.length > 0) {
      condition['id'] = id;
    }
    if (name.length > 0) {
      condition['name'] = name;
    }
    if (profession.length > 0) {
      condition['profession'] = profession;
    }
    if (isNaN(age) === false) {
      condition['age'] = age;
    }
    const query = Object.keys(condition).map((key) => {
      return `${key}=${condition[key]}`;
    }).join('&');
    setState(['info', 'Searching...']);
    await new Promise((resolve) => setTimeout(resolve, setting.smallWaitingTime));
    const res = await fetch(`${setting.apiPath}/Select?${query}`);
    if (res.ok === false) {
      setState(['danger', 'Search failed...']);
      await new Promise((resolve) => setTimeout(resolve, setting.waitingTime));
      setState(['secondary', '']);
      return;
    }
    const users = await res.json();
    setUsers(users);
    setState(['primary', 'Search success!']);
    await new Promise((resolve) => setTimeout(resolve, setting.waitingTime));
    setState(['secondary', '']);
  };

  const Delete = async (id: string) => {
    setState(['info', 'Deleting...']);
    await new Promise((resolve) => setTimeout(resolve, setting.smallWaitingTime));
    const res = await fetch(`${setting.apiPath}/users/${id}`, {
      method: 'DELETE',
    });
    if (res.ok === false) {
      setState(['danger', 'Delete failed...']);
      await new Promise((resolve) => setTimeout(resolve, setting.waitingTime));
      setState(['secondary', '']);
      return;
    }
    setState(['primary', 'Delete success!']);
    setUsers(users.filter((user) => {
      return user.id !== id;
    }));
    await new Promise((resolve) => setTimeout(resolve, setting.waitingTime));
    setState(['secondary', '']);
  };

  const Edit = async (id: string) => {
    setEditMode(true);
    setEditUser(users.find((user) => {
      return user.id === id;
    }));
  };

  const Update = async (user: User) => {
    setState(['info', 'Updating...']);
    await new Promise((resolve) => setTimeout(resolve, setting.smallWaitingTime));
    const res = await fetch(`${setting.apiPath}/users/${user.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(user),
    });
    if (res.ok === false) {
      setState(['danger', 'Update failed...']);
      await new Promise((resolve) => setTimeout(resolve, setting.waitingTime));
      setState(['secondary', '']);
      return;
    }
    setState(['primary', 'Update success!']);
    setUsers(users.map((_user) => {
      if (user.id === _user.id) {
        return user;
      }
      return _user;
    }));
    await new Promise((resolve) => setTimeout(resolve, setting.waitingTime));
    setState(['secondary', '']);
  }

  return (
    <Layout>
      <div id="Contact">
        <h1>🦀 Make Action! 🦀</h1>
        <h2>🐪 Condition</h2>
        <Form>
          <Form.Group className="mt-3">
            <Form.Label>🐋 Name</Form.Label>
            <Form.Control type="text" placeholder="Enter name" value={name} onInput={(e) => {setName(e.currentTarget.value)}} />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>🐋 Id</Form.Label>
            <Form.Control type="text" placeholder="Enter id" value={id} onInput={(e) => {setId(e.currentTarget.value)}} />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>🐋 Profession</Form.Label>
            <Form.Control type="text" placeholder="Enter profession" value={profession} onInput={(e) => {setProfession(e.currentTarget.value)}} />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>🐋 Age</Form.Label>
            <Form.Control type="number" min={0} max={100} placeholder="Enter age" value={age} onInput={(e) => {setAge(parseInt(e.currentTarget.value))}} />
          </Form.Group>
          <Button variant="primary" className="d-block mx-auto mt-3" onClick={Search}>Sarch 🌸</Button>
        </Form>
        <Alert variant={state[0]} className="mt-3" style={{minHeight: '5rem'}}>{state[1]}</Alert>
        <Table striped bordered hover>
          <thead>
            <tr className="text-center">
              <th>🐋 Name</th>
              <th>🐋 Profession</th>
              <th>🐋 Age</th>
              <th>🐋 Edit</th>
              <th>🐋 Delete</th>
            </tr>
          </thead>
          <tbody>
            {
              users.map((user) => {
                return (
                  <tr key={user.id}>
                    <td>{user.name}</td>
                    <td>{user.profession}</td>
                    <td>{user.age}</td>
                    <td><Button variant="success" size="sm" className="d-block m-auto" onClick={() => {Edit(user.id)}}>Edit</Button></td>
                    <td><Button variant="danger" size="sm" onClick={() => {Delete(user.id)}} className="d-block m-auto">Delete</Button></td>
                  </tr>
                )
              })
            }
          </tbody>
        </Table>
        {
          edit_mode && <EditComponent close_func={() => {setEditMode(false)}} update_func={Update} user={edit_user} />
        }
      </div>
    </Layout>
  );
};
