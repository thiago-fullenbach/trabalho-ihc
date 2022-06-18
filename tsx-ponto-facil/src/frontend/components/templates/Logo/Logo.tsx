import "./Logo.css";
import React from "react";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faBars, faClock } from "@fortawesome/free-solid-svg-icons";

type LogoProps = {
    setShowMenu: (showmenu: boolean) => void
    showMenu: boolean
}

export default (props: LogoProps): JSX.Element => {
    return (
        <aside className="logo d-flex justify-content-between align-items-center">
            <button className='menu-toggle p-3' onClick={() => props.setShowMenu(!props.showMenu)}>
                <FontAwesomeIcon icon={faBars} />
            </button>
            <div className='d-flex align-items-center'>
                <FontAwesomeIcon icon={faClock} />
                <h1>Ponto Fácil</h1>
            </div>
            <button className='menu-toggle p-3 invisible'>
                <FontAwesomeIcon icon={faBars} />
            </button>
        </aside>
    )
}