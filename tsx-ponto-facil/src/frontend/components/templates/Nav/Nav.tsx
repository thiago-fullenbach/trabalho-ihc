import './Nav.css'
import React, { useState } from 'react';

import NavLink from '../NavLink/NavLink';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHome, faUserGroup, faWrench, faRightToBracket, faBars } from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from 'react-router-dom';
import SessionState from '../../../main/SessionState';

type NavProps = {
    sessionState: SessionState
}

export default (props: NavProps): JSX.Element => {
    const navigate = useNavigate();
    const [showMenu, setShowMenu] = useState(false);

    const logout = () => {
        sessionStorage.clear()
        props.sessionState.updateLoggedUser(null)
        props.sessionState.updateStringifiedSession(null)
        navigate("/")
    }

    return (
        <aside className="menu-area">
            
            <nav className="menu">
                <button className='menu-toggle' onClick={() => setShowMenu(!showMenu)}>
                    <FontAwesomeIcon icon={faBars} />
                </button>

                <div className={`menu-links ${showMenu ? 'menu-show' : 'menu-hide'}`}>
                    <NavLink path="/main/home" icon={faHome} label="Início" onClick={undefined} />
                    <NavLink path="/main/employees" icon={faUserGroup} label="Funcionários" onClick={undefined} />
                    {/* <NavLink path="/main/config" icon={faWrench} label="Configurações" /> */}
                    <button className='logout-btn' onClick={logout}>
                        <FontAwesomeIcon icon={faRightToBracket} /> Sair
                    </button>
                </div>
            </nav>
        </aside>
    )
}