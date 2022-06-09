import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css'
import React from 'react'
import { Route, Routes } from 'react-router-dom';

import Login from '../components/view/Pages/Login/Login';
import TemplatePage from '../components/view/Pages/TemplatePage'
import { EmptyStatement } from 'typescript';
import { useSessionStorage } from '../../utils/useSessionStorage';
import LoggedUserDTO from '../../chamada-api/DTO/DoServidorParaCliente/LoggedUserDTO';
import SessionState from './SessionState';

export default (_: {}): JSX.Element => {

    const [sessao, setSessao] = useSessionStorage<string | null>('session', null);
    const [usuarioLogado, setUsuarioLogado] = useSessionStorage<LoggedUserDTO | null>('user', null);

    const updateSessao = (sessao: string | null): void => {
        setSessao(sessao);
    }
    const updateUsuarioLogado = (usuarioLogado: LoggedUserDTO | null): void => {
        setUsuarioLogado(usuarioLogado);
    }
    const sessionState = new SessionState(sessao, updateSessao, usuarioLogado, updateUsuarioLogado)
    const userIsLogged = (): boolean => {
        return (usuarioLogado !== null && usuarioLogado.Id !== 0)
    }
    if (userIsLogged()) {
        return (
            <Routes>
                <Route path="/main/*" element={<TemplatePage sessionState={sessionState} />} />
            </Routes>
        )
    } else {
        return (
            <Routes>
                <Route path="/*" element={<Login sessionState={sessionState} />} />
            </Routes>
        )
    }
}
    