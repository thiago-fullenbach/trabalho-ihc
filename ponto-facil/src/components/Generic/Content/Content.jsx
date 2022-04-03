import React from "react";
import Home from '../../View/Content/Home';
import Config from '../../View/Content/Config';
import { Routes, Route } from "react-router-dom";
import './Content.css'
import Funcionarios from "../../View/Content/Funcionarios";
import RegistroPonto from "../../View/Content/RegistroPonto";

const Content = () => {
    return (
        <main className="Content">
            <Routes>
                <Route exact path="/home" element={<Home />}></Route>
                <Route exact path="/funcionarios" element={<Funcionarios/>}></Route>
                <Route exact path="/registrar-ponto" element={<RegistroPonto />}></Route>
                <Route exact path="/config" element={<Config />}></Route>
            </Routes>
        </main>
    )
}

export default Content;