import React, { useState } from "react";
import Form from "../Generic/Form/Form";
import Input from "../Generic/Input/Input";
import Button from "../Generic/Button/Button";
import Card from "../Generic/Card/Card";
import Title from "../Generic/Title/Title";
import './Login.css';

const Login = () => {
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const submitForm = () => {
        console.log(login)
        console.log(password)
    }

    const handleSubmit = e => {
        e.preventDefault();
        submitForm();
    }

    return (
        <div className="Login">
            <Card>
                <Title>Ponto Fácil</Title>
                <Form onSubmit={handleSubmit} type="post">
                    <div className="inputGroup">
                        <Input name="login" placeholder="Usuário" label="Login" 
                            value={login} onChange={e => setLogin(e.target.value)}/>
                        <Input type="password" name="pwd" placeholder="Senha" label="Senha"
                            value={password} onChange={e => setPassword(e.target.value)}/>
                    </div>
                    <Button type="submit">Entrar</Button>
                    <Button>Cadastre-se</Button>
                </Form>
            </Card>
        </div>
    )
}

export default Login;