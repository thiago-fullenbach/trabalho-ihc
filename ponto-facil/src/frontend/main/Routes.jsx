import React from "react";
import { Routes, Route } from 'react-router-dom';

import Home from "../components/view/Pages/Home/Home";
import Config from "../components/view/Pages/Config/Config";
import EmployeeCrud from "../components/view/Pages/Employees/EmployeeCrud";

export default props => {
    return (
        <Routes>
            <Route exact path ="/home" element={<Home />}></Route>
            <Route path="/employees" element={<EmployeeCrud />}></Route>
            {/* <Route path="/config" element={<Config />}></Route> */}
            <Route exact path ="/*" element={<Home />}></Route>
        </Routes>
    )
}


