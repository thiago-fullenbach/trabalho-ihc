import './Login.css';
import React, { useState } from "react";

import Card from '../../../templates/Card/Card';
import Input from '../../../templates/Input/Input';
import Button from '../../../templates/Button/Button';
import { faClock } from '@fortawesome/free-solid-svg-icons';
import { useNavigate } from 'react-router-dom';

export default props => {
    const navigate = useNavigate();
    const [user, setUser] = useState("");
    const [pwd, setPwd] = useState("");

    const handleSubmit = e => {
        e.preventDefault();
        console.log("Validar Usuário e Senha")
    }

    return (
        <div className="login">
            <Card title="Ponto Fácil" icon={faClock}>
                <form action="post" className="login-form">
                    <Input label="Usuário" type="text" value={user} onChange={e => setUser(e.target.value)}
                        placeholder="E-mail ou usuário" />
                    <Input label="Senha" type="password" value={pwd} onChange={e => setPwd(e.target.value)} 
                        placeholder="Senha"/>

                    <div className="button-group">
                        <Button onClick={handleSubmit} type="submit">Acessar</Button>
                        <Button onClick={() => navigate('/access/signup')}>Cadastrar-se</Button>
                    </div>
                </form>
            </Card>
        </div>
    )
}