import React from 'react'
import "./TemplatePage.css"

import Logo from '../../templates/Logo/Logo'
import Nav from '../../templates/Nav/Nav'
import Routes from '../../../main/Routes'
import Footer from '../../templates/Footer/Footer'

export default props => {
    if(props.sessao == null &&
        props.setSessao == null) {
        console.log("Sessão nula")
    }

    return (
        <div className="tp-page">
            <Logo></Logo>
            <Nav></Nav>
            <Routes />
            <Footer></Footer>
        </div>
    )
}