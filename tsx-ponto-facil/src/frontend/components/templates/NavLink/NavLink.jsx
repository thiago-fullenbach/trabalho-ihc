import './NavLink.css';
import React from "react";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { Link } from 'react-router-dom';

export default props => {
    return (
        <Link to={props.path}>
            <FontAwesomeIcon icon={props.icon} onClick={props.onClick} /> {props.label}
        </Link>
    )
}