import './Access.css';
import React from 'react';

import { Routes, Route } from 'react-router-dom';
import Login from '../components/view/Pages/Login/Login';
import Signup from '../components/view/Pages/Singup/Signup';

export default props => {
    const [sessao, setSessao] = React.useState({});

    return (
        <div className="access">
            <Routes>
                <Route path="/login" element={<Login enviarSessao={sessao} enviarSetSessao={setSessao}></Login>}></Route>
                <Route path="/signup" element={<Signup enviarSessao={sessao} enviarSetSessao={setSessao}></Signup>}></Route>
            </Routes>
        </div>
    )
}