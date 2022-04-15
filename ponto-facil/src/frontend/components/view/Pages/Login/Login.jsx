import './Login.css';
import React, { useState } from "react";

import Card from '../../../templates/Card/Card';
import Input from '../../../templates/Input/Input';
import Button from '../../../templates/Button/Button';
import { faClock } from '@fortawesome/free-solid-svg-icons';

export default props => {
    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')

    const handleSubmit = e => {
        e.preventDefault();

        console.log(userName)
        console.log(password)
    }

    return (
        <div className="login">
            <Card title="Ponto Fácil" icon={faClock}>
                <form action="post" className="login-form">
                    <Input label="Usuário" type="text" value={userName} onChange={e => setUserName(e.target.value)}
                        placeholder="E-mail ou usuário" />
                    <Input label="Senha" type="password" value={password} onChange={e => setPassword(e.target.value)} 
                        placeholder="Senha"/>

                    <div className="button-group">
                        <Button onClick={handleSubmit} type="submit">Acessar</Button>
                    </div>
                </form>
            </Card>
        </div>
    )
}