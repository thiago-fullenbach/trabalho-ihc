import React from "react";
import './Form.css';

const Form = props => {
    return (
        <form className="Form" {...props}>
            {props.children}
        </form>
    )
}

export default Form;