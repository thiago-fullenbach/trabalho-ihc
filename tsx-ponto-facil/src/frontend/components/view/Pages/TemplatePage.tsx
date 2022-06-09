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
import LoggedUserDTO from '../../../../chamada-api/DTO/DoServidorParaCliente/LoggedUserDTO';
import SessionState from '../../../main/SessionState';
import LoadingState from '../../templates/LoadingModal/LoadingState';

type TemplatePageProps = {
    sessionState: SessionState
}

export default (props: TemplatePageProps): JSX.Element => {
    
    const navigate = useNavigate()
    const [carregando, setCarregando] = React.useState(false)
    const updateCarregando = (carregando: boolean): void => {
        setCarregando(carregando);
    }
    const loadingState = new LoadingState(carregando, updateCarregando)

    useEffect((): void => {
        if(isNullOrEmpty(props.sessionState.loggedUser)) {
            navigate("/")
        }
    }, [props.sessionState.loggedUser])
    return (
        <div className="tp-page">
            <Logo></Logo>
            <Nav sessionState={props.sessionState}></Nav>
            <Routes loadingState={loadingState} sessionState={props.sessionState} />
            <Footer></Footer>
            {carregando && <LoadingModal/> }
        </div>
    )
}