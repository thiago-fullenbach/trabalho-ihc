import './Login.css';
import React, { ChangeEvent, useState } from "react";

import Card from '../../../templates/Card/Card';
import Input from '../../../templates/Input/Input';
import Button from '../../../templates/Button/Button';
import MsgDialog from '../../../templates/MsgDialog/MsgDialog';
import { faClock } from '@fortawesome/free-solid-svg-icons';

import ApiUtil from '../../../../../chamada-api/ApiUtil';

import { useNavigate } from 'react-router-dom';
import { getSessionStorageOrDefault, useSessionStorage } from '../../../../../utils/useSessionStorage';
import { useEffect } from 'react';
import { isNullOrEmpty } from '../../../../../utils/valid';
import LoadingModal from '../../../templates/LoadingModal/LoadingModal';
import { mountMessage } from '../../../../../utils/formatting';
import SessionState from '../../../../main/SessionState';
import { setSimpleState } from '../../../../../utils/stateUtil';
import { message } from '../../../../modelo/message';

type LoginProps = {
    sessionState: SessionState
}

export default (props: LoginProps): JSX.Element => {
    const navigate = useNavigate()

    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')
    const [msgError, setMsgError] = useState('')
    const [carregando, setCarregando] = useState(false)

    useEffect((): void => {
        if(!isNullOrEmpty(props.sessionState.loggedUser)) {
            navigate("/main/home")
        }
    }, [props.sessionState.loggedUser])

    const handleSubmit = (e: Event): void => {
        e.preventDefault();
        setCarregando(true); 
        (async (): Promise<void> => {
            let r = await ApiUtil.loginAsync(props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, userName, password)
            setCarregando(false)
            if (r.status === 200) {
                // console.log(getSessionStorageOrDefault('session', null))
                // console.log(getSessionStorageOrDefault('user', null))
                navigate("/main/home")
            } else {
                setMsgError(r.body !== undefined ? mountMessage(r.body) : message.serverError)
            }
        })()
    }

    return (
        <div className="login">
            <Card title="Ponto Fácil" icon={faClock}>
                <form action="post" className="login-form">
                    <Input label="Usuário" type="text" value={userName} onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSimpleState(setUserName, e.target.value) }
                        placeholder="E-mail ou usuário" />
                    <Input label="Senha" type="password" value={password} onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSimpleState(setPassword, e.target.value) } 
                        placeholder="Senha"/>
                    <div className="button-group">
                        <Button onClick={handleSubmit} type="submit">Acessar</Button>
                    </div>
                </form>
            </Card>
            { msgError && 
                <MsgDialog typeAlert="danger" msgType="Erro no Login" msg={msgError}/>
            }
            { carregando && <LoadingModal/> }
            
        </div>
    )
}