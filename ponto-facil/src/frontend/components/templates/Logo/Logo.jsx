import "./Logo.css";
import React from "react";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faClock } from "@fortawesome/free-solid-svg-icons";

export default props => {
    return (
        <aside className="logo">
            <FontAwesomeIcon icon={faClock} />
            <h1>Ponto Fácil</h1>
        </aside>
    )
}