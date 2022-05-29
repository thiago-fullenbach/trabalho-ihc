import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css'
import React from 'react'
import { Route, Routes } from 'react-router-dom';

import Login from '../components/view/Pages/Login/Login';
import TemplatePage from '../components/view/Pages/TemplatePage'

export default props => {

    const [sessao, setSessao] = React.useState({});

    return (
        <Routes>
            <Route exact path="/" element={<Login enviarSessao={sessao} enviarSetSessao={setSessao} />} />
            <Route path="/main/*" element={<TemplatePage enviarSessao={sessao} enviarSetSessao={setSessao} />} />
            <Route exact path="*" element={<Login enviarSessao={sessao} enviarSetSessao={setSessao} />} />
        </Routes>
    )
}
    