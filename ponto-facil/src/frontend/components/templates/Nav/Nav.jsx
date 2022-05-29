import './Nav.css'
import React, { useState } from 'react';

import NavLink from '../NavLink/NavLink';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHome, faUserGroup, faWrench, faRightToBracket, faBars } from "@fortawesome/free-solid-svg-icons";

export default props => {
    const [showMenu, setShowMenu] = useState(false);

    return (
        <aside className="menu-area">
            
            <nav className="menu">
                <button className='menu-toggle' onClick={() => setShowMenu(!showMenu)}>
                    <FontAwesomeIcon icon={faBars} />
                </button>

                <div className={`menu-links ${showMenu ? 'menu-show' : 'menu-hide'}`}>
                    <NavLink path="/main/home" icon={faHome} label="Início" />
                    <NavLink path="/main/employees" icon={faUserGroup} label="Funcionários" />
                    <NavLink path="/main/config" icon={faWrench} label="Configurações" />
                    <NavLink path="/" icon={faRightToBracket} label="Sair" />
                </div>
            </nav>
        </aside>
    )
}