import React from 'react'
import "./TemplatePage.css"
import { useNavigate } from 'react-router-dom';
import { getSessionStorageOrDefault } from '../../../../utils/useSessionStorage';

import Logo from '../../templates/Logo/Logo'
import Nav from '../../templates/Nav/Nav'
import Routes from '../../../main/Routes'
import Footer from '../../templates/Footer/Footer'
import { useEffect } from 'react';
import { isNullOrEmpty } from '../../../../utils/valid';

export default props => {
    const navigate = useNavigate()
    const signedInUser = getSessionStorageOrDefault('user')

    useEffect(() => {
        if(isNullOrEmpty(signedInUser)) {
            navigate("/")
        }
    }, [signedInUser])

    return (
        <div className="tp-page">
            <Logo></Logo>
            <Nav></Nav>
            <Routes />
            <Footer></Footer>
        </div>
    )
}