import React from "react";
import { Link } from'react-router-dom'
import './NavLink.css';

const NavLink = props => {
    return (
        <li className="ListItem">
            <Link to={props.path} className="NavLink">
                {props.children}
            </Link>
        </li>
    )
}

export default NavLink;