import './Access.css';
import React from 'react';

import { Routes, Route } from 'react-router-dom';
import Login from '../components/view/Pages/Login/Login';
import Signup from '../components/view/Pages/Singup/Signup';

export default props => {
    return (
        <div className="access">
            <Routes>
                <Route path="/login" element={<Login></Login>}></Route>
                <Route path="/signup" element={<Signup></Signup>}></Route>
            </Routes>
        </div>
    )
}