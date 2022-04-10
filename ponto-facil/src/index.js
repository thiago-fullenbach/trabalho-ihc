import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './index.css';
import App from './frontend/main/App';
import Access from './frontend/main/Access';

ReactDOM.render(
<BrowserRouter>
  <Routes>
    <Route exact path="/" element={<App />} />
    <Route path="/access/*" element={<Access />} />
    <Route exact path="*" element={<App />} />
  </Routes>
</BrowserRouter>
, document.getElementById('root'));
