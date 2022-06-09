import './Button.css';
import React from "react";

export default (props: any): JSX.Element => {
    return (
        <button className='button btn btn-primary' {...props}>{props.children}</button>
    )
}