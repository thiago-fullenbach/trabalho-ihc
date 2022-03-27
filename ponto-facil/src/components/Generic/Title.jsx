import React from "react";
import './Title.css';

const Title = props => {
    return (
        <h1 className="Title">{props.children}</h1>
    )
}

export default Title;