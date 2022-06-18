import './Nav.css'
import React, { useState } from 'react';

import NavLink from '../NavLink/NavLink';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHome, faUserGroup, faWrench, faRightToBracket, faBars, faCheck } from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from 'react-router-dom';
import SessionState from '../../../main/SessionState';

type NavProps = {
    sessionState: SessionState
    setShowMenu: (showmenu: boolean) => void
    showMenu: boolean
}

export default (props: NavProps): JSX.Element => {
    const navigate = useNavigate();

    const logout = () => {
        sessionStorage.clear()
        props.sessionState.updateLoggedUser(null)
        props.sessionState.updateStringifiedSession(null)
        navigate("/")
    }

    return (
        <aside className="menu-area">
            
            <nav className="menu">

                <div className={`menu-links ${props.showMenu ? 'menu-show' : 'menu-hide'}`}>
                    <NavLink path="/main/home" icon={faHome} label="Início" onClick={undefined} />
                    <NavLink path="/main/employees" icon={faUserGroup} label="Funcionários" onClick={undefined} />
                    <NavLink path="/main/presences" icon={faCheck} label="Presenças" onClick={undefined} />
                    {/* <NavLink path="/main/config" icon={faWrench} label="Configurações" /> */}
                </div>
                <div className='d-flex jutify-content-center'>
                    <div className='d-flex'>
                        <span className='text-white align-self-center ps-2'>Logado como {props.sessionState.loggedUser?.Nome.split(' ')[0] || '???'}</span>
                        <button className='logout-btn ms-2' onClick={logout}>
                            <FontAwesomeIcon icon={faRightToBracket} /> Sair
                        </button>
                    </div>
                </div>
            </nav>
        </aside>
    )
}