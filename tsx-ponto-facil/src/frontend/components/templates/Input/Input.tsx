import './Input.css';
import React from "react";

export default (props: any): JSX.Element => {
    return (
        <div className="input-area">
            {props.label && (
                <label className="label">{props.label}</label>
            )}
            <input className="input form-control" {...props} />
        </div>
    )
}