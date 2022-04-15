import React, { useState } from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './index.css';
import App from './frontend/main/App';
import Login from './frontend/components/view/Pages/Login/Login';

ReactDOM.render(
  <BrowserRouter>
    <Routes>
      <Route exact path="/" element={<Login/>} />
      <Route path="/main/*" element={<App />} />
      <Route exact path="*" element={<Login/>} />
    </Routes>
  </BrowserRouter>
, document.getElementById('root'));
