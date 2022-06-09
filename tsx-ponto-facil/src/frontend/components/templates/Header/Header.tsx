import "./Header.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React from "react";
import { IconDefinition } from "@fortawesome/fontawesome-svg-core";

type HeaderProps = {
    icon: IconDefinition
    title: string
    subtitle: string
}

export default (props: HeaderProps): JSX.Element => {
    return (
        <header className="header d-none d-sm-flex flex-column">
            <h1 className="mt-3">
                <FontAwesomeIcon icon={props.icon} /> {props.title}
            </h1>
            <p className="lead text-muted">{props.subtitle}</p>
        </header>
    )
}