import React from "react";
import { Routes, Route } from 'react-router-dom';

import Home from "../components/view/Pages/Home/Home";
import Config from "../components/view/Pages/Config/Config";
import EmployeeCrud from "../components/view/Pages/Employees/EmployeeCrud";
import SessionState from "./SessionState";
import LoadingState from "../components/templates/LoadingModal/LoadingState";

type RoutesProps = {
    sessionState: SessionState
    loadingState: LoadingState
}

export default (props: RoutesProps): JSX.Element => {
    return (
        <Routes>
            <Route path ="/home" element={<Home sessionState={props.sessionState} loadingState={props.loadingState} />}></Route>
            <Route path="/employees" element={<EmployeeCrud sessionState={props.sessionState} loadingState={props.loadingState} />}></Route>
            {/* <Route path="/config" element={<Config />}></Route> */}
            <Route path ="/*" element={<Home sessionState={props.sessionState} loadingState={props.loadingState} />}></Route>
        </Routes>
    )
}


