import React from "react";
import './NavList.css';

const NavList = props => {
    return (
        <ul className="NavList">
            {props.children}
        </ul>
    )
}

export default NavList;