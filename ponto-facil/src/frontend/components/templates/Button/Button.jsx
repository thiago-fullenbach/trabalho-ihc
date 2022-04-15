import './Button.css';
import React from "react";

export default props => {
    return (
        <button className='button btn btn-primary' {...props}>{props.children}</button>
    )
}