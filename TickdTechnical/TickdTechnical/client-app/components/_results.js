import React from "react";

export default function Results(props) {
    return (
        <div className="results">
            <p>Successful entries: <span>{props.success}</span></p>
            <p>Failed Entries: <span>{props.failed}</span></p>
            <p>Duplicate Entries: <span>{props.duplicate}</span></p>
        </div>
    )
}