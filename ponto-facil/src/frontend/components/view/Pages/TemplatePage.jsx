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
import LoadingModal from '../../templates/LoadingModal/LoadingModal';

export default props => {
    const navigate = useNavigate()
    const signedInUser = getSessionStorageOrDefault('user')

    const [carregando, setCarregando] = React.useState(false)

    useEffect(() => {
        if(isNullOrEmpty(signedInUser)) {
            navigate("/")
        }
    }, [signedInUser])

    

    return (
        <div className="tp-page">
            <Logo></Logo>
            <Nav></Nav>
            <Routes carregando={carregando} setCarregando={setCarregando} />
            <Footer></Footer>
            {carregando && <LoadingModal/> }
        </div>
    )
}