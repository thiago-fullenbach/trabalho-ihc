import './Card.css';
import React from "react";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export default props => {
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