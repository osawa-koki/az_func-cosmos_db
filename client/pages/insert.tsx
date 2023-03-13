import React from "react";

import { Button, Alert, Form } from 'react-bootstrap';
import Layout from "../components/Layout";
import setting from "../setting";

export default function InsertPage() {

  const [name, setName] = React.useState<string>('');
  const [profession, setProfession] = React.useState<string>('');
  const [age, setAge] = React.useState<number>(0);
  const [state, setState] = React.useState<['primary' | 'danger' | 'info' | 'secondary', string]>(['secondary', '']);

  const Send = async () => {
    setState(['info', 'Sending...']);
    await new Promise((resolve) => setTimeout(resolve, setting.smallWaitingTime));
    const res = await fetch(`${setting.apiPath}/Insert`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        name: name,
        profession: profession,
        age: age
      })
    });
    if (res.ok) {
      setState(['primary', 'Insert success!']);
    } else {
      setState(['danger', 'Insert failed...']);
    }
    await new Promise((resolve) => setTimeout(resolve, setting.waitingTime));
    setState(['secondary', '']);
  };

  const IsValid_Age = () => {
    return age >= 0 && age <= 100;
  };

  const IsValid_Name = () => {
    return name.length > 0;
  };

  const IsValid_Profession = () => {
    return profession.length > 0;
  };

  const IsValid = () => {
    return IsValid_Age() && IsValid_Name() && IsValid_Profession();
  };

  return (
    <Layout>
      <div id="Contact">
        <h1>🦀 Make Action! 🦀</h1>
        <h2>🐪 Insert</h2>
        <Form>
          <Form.Group className="mt-3">
            <Form.Label>🐋 Name</Form.Label>
            <Form.Control type="text" placeholder="Enter name" value={name} onInput={(e) => {setName(e.currentTarget.value)}} />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>🐋 Profession</Form.Label>
            <Form.Control type="text" placeholder="Enter profession" value={profession} onInput={(e) => {setProfession(e.currentTarget.value)}} />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>🐋 Age</Form.Label>
            <Form.Control type="number" min={0} max={100} placeholder="Enter age" value={age} onInput={(e) => {setAge(parseInt(e.currentTarget.value))}} />
          </Form.Group>
          <Button variant="primary" className="d-block mx-auto mt-3" onClick={Send} disabled={IsValid() === false}>Insert 📨</Button>
        </Form>
        {
          IsValid() === false &&
          <Alert variant="danger" className="mt-3" style={{minHeight: '5rem'}}>
            <ul style={{margin: 0}}>
              {IsValid_Name() === false && <li>Name is invalid.</li>}
              {IsValid_Profession() === false && <li>Profession is invalid.</li>}
              {IsValid_Age() === false && <li>Age is invalid.</li>}
            </ul>
          </Alert>
        }
        <Alert variant={state[0]} className="mt-5" style={{minHeight: '5rem'}}>{state[1]}</Alert>
      </div>
    </Layout>
  );
};
