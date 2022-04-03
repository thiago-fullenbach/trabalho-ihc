import React from "react";
import NavLink from "../Navegation/NavLink";
import NavList from "../Navegation/NavList";
import './Menu.css'

const Menu = () => {
    return (
        <nav className="Menu">
            <NavList>
                <NavLink path="/main/home">Home</NavLink>
                <NavLink path="/main/config">Configurações</NavLink>
            </NavList>
        </nav>
    )
}
export default Menu;