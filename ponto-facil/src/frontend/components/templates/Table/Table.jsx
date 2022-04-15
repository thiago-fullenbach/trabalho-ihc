import './Table.css';
import React from "react";

export default props => {
    return (
        <table className="table mt4">
            <thead>
                <tr>
                    {props.headings.map((heading, index) => <th key={index}>{heading}</th>)}
                </tr>
            </thead>
            <tbody>
                {props.children}
            </tbody>
        </table>
    )
}