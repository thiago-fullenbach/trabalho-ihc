import './Nav.css'
import React from 'react';

import NavLink from '../NavLink/NavLink';
import { faHome, faUserGroup, faWrench, faRightToBracket } from "@fortawesome/free-solid-svg-icons";

export default props => {
    return (
        <aside className="menu-area">
            <nav className="menu">
                <NavLink path="/main/home" icon={faHome} label="Início" />
                <NavLink path="/main/employees" icon={faUserGroup} label="Funcionários" />
                <NavLink path="/main/config" icon={faWrench} label="Configurações" />
                <NavLink path="/" icon={faRightToBracket} label="Sair" />
            </nav>
        </aside>
    )
}