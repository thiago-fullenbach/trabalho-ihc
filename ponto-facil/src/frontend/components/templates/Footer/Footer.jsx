import './Footer.css'
import React from 'react'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHeart } from "@fortawesome/free-solid-svg-icons";

export default props => {
    return (
        <footer className="footer">
            <span>
                Desenvolvido com <FontAwesomeIcon icon={faHeart} className="text-danger" /> pelo melhor grupo de IHC.
            </span>
        </footer>
    )
}