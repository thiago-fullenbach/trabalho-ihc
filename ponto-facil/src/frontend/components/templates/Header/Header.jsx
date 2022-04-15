import "./Header.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React from "react";

export default props => {
    return (
        <header className="header d-none d-sm-flex flex-column">
            <h1 className="mt-3">
                <FontAwesomeIcon icon={props.icon} /> {props.title}
            </h1>
            <p className="lead text-muted">{props.subtitle}</p>
        </header>
    )
}