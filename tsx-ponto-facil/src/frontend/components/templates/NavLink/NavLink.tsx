import './NavLink.css';
import React from "react";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { Link, To } from 'react-router-dom';
import { IconProp } from '@fortawesome/fontawesome-svg-core';

type NavLinkProps = {
    path: string
    icon: IconProp
    label: string
    onClick: React.MouseEventHandler<SVGSVGElement> | undefined
}

export default (props: NavLinkProps): JSX.Element => {
    return (
        <Link to={props.path}>
            <FontAwesomeIcon icon={props.icon} onClick={props.onClick} /> {props.label}
        </Link>
    )
}