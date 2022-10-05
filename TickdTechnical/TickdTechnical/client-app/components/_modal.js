import React from "react";

export default function Modal(props) {
    return (
        <div className="modal">
            <div className="modal__content">
                <h3 className="modal__title">{props.title}</h3>
                <p className="modal__copy">{props.copy}</p>
                <a className="modal__button" onClick={props.closeError}>{props.btnText}</a>
            </div>
        </div>
    )
}