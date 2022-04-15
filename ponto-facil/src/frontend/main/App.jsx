import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css'
import React from 'react'

import Logo from '../components/templates/Logo/Logo'
import Nav from '../components/templates/Nav/Nav'
import Routes from './Routes';
import Footer from '../components/templates/Footer/Footer'

export default props => {

    return (
        <div className="app">
            <Logo></Logo>
            <Nav></Nav>
            <Routes />
            <Footer></Footer>
        </div>
    )
}
    