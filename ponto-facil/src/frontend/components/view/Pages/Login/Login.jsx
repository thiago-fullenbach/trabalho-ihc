import './Login.css';
import React, { useState } from "react";

import Card from '../../../templates/Card/Card';
import Input from '../../../templates/Input/Input';
import Button from '../../../templates/Button/Button';
import MsgDialog from '../../../templates/MsgDialog/MsgDialog';
import { faClock } from '@fortawesome/free-solid-svg-icons';

import ApiUtil from '../../../../../chamada-api/ApiUtil';

import { useNavigate } from 'react-router-dom';
import { useSessionStorage } from '../../../../../utils/useSessionStorage';

export default props => {
    const navigate = useNavigate()

    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')
    const [msgError, setMsgError] = useState('')

    const [sessao, setSessao] = useSessionStorage('session', {});

    const handleSubmit = e => {
        e.preventDefault();
        (async () => {
            let r = await ApiUtil.RespostaDoServidor_submitEntrarAsync(setSessao, userName, password)
            if(r.int_status === 404) {
                setMsgError("Login ou Senha inválidos")
            } else {
                setSessao(r.SessaoDTO_sessao)
                navigate("/main/home")
            }
        })()
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
            { msgError && 
                <MsgDialog typeAlert="danger" msgType="Erro no Login" msg={msgError}/>
            }
            
        </div>
    )
}