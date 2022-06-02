import React from "react";
import { Routes, Route } from 'react-router-dom';

import Home from "../components/view/Pages/Home/Home";
import Config from "../components/view/Pages/Config/Config";
import EmployeeCrud from "../components/view/Pages/Employees/EmployeeCrud";

export default props => {
    return (
        <Routes>
            <Route exact path ="/home" element={<Home carregando={props.carregando} setCarregando={props.setCarregando} />}></Route>
            <Route path="/employees" element={<EmployeeCrud carregando={props.carregando} setCarregando={props.setCarregando} />}></Route>
            {/* <Route path="/config" element={<Config />}></Route> */}
            <Route exact path ="/*" element={<Home carregando={props.carregando} setCarregando={props.setCarregando} />}></Route>
        </Routes>
    )
}


