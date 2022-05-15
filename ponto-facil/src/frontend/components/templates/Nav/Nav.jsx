import './Nav.css'
import React, { useState } from 'react';

import NavLink from '../NavLink/NavLink';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHome, faUserGroup, faWrench, faRightToBracket, faLocation, faBars } from "@fortawesome/free-solid-svg-icons";

export default props => {
    const [showMenu, setShowMenu] = useState(true);

    return (
        <aside className="menu-area">
            <nav className="menu">
                <NavLink path="/main/home" icon={faHome} label="Início" />
                <NavLink path="/main/employees" icon={faUserGroup} label="Funcionários" />
                <NavLink path="/main/locations" icon={faLocation} label="Locais" />
                <NavLink path="/main/config" icon={faWrench} label="Configurações" />
                <NavLink path="/" icon={faRightToBracket} label="Sair" />
            </nav>
        </aside>
    )
}