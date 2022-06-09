import './Card.css';
import React from "react";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';

type CardProps = {
    title: string
    icon: IconDefinition
    children: React.ReactNode
}

export default (props: CardProps): JSX.Element => {
    return (
        <div className="card">
            <h1 className="card-title">
                <FontAwesomeIcon icon={props.icon} />
                {props.title}
            </h1>
            {props.children}
        </div>
    )
}