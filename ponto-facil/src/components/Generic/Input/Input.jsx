import React from "react";
import './Input.css'

const Input = props => {
    return (
        <div className="inputDiv">
            { props.label ? (
                <label htmlFor={props.name}>{props.label}</label>
            ) : null }
            
            <input {...props} />
        </div>
    )
}

export default Input;