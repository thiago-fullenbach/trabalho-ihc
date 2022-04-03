import React from "react";
import Home from '../../View/Content/Home';
import Config from '../../View/Content/Config';
import { Routes, Route } from "react-router-dom";
import './Content.css'

const Content = () => {
    return (
        <main className="Content">
            <Routes>
                <Route exact path="/home" element={<Home />}></Route>
                <Route exact path="/config" element={<Config />}></Route>
            </Routes>
        </main>
    )
}

export default Content;